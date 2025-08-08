using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using SticksAndStones.Models;

namespace SticksAndStones.Handlers;

public class ChallengeHandler : IDisposable
{
   private readonly TimeSpan ackThreshold;
   private readonly ConcurrentDictionary<Guid, ChallengeRecord> handlers = new();
   private readonly Timer timer;

   public ChallengeHandler() : this(
      true,
      TimeSpan.FromSeconds(30),
      TimeSpan.FromSeconds(1))
   {
   }

   public ChallengeHandler(bool completeAcksOnTimeout, TimeSpan ackThreshold, TimeSpan ackInterval)
   {
      if (completeAcksOnTimeout)
      {
         timer = new Timer(_ => CheckAcks(), null, ackInterval, ackInterval);
      }

      this.ackThreshold = ackThreshold;
   }

   public void Dispose()
   {
      timer?.Dispose();

      foreach (var pair in handlers)
      {
         pair.Value.ResponseTask.TrySetCanceled();
      }

      GC.SuppressFinalize(this);
   }

   public (Guid id, Task<Challenge> responseTask) CreateChallenge(Player challenger, Player opponent)
   {
      var id = Guid.NewGuid();
      var puppetTask = new TaskCompletionSource<Challenge>(TaskCreationOptions.RunContinuationsAsynchronously);
      handlers.TryAdd(id,
         new ChallengeRecord(id, puppetTask, DateTime.UtcNow,
            new Challenge(id, challenger, opponent, ChallengeResponse.None)));

      return (id, puppetTask.Task);
   }

   public Challenge Respond(Guid id, ChallengeResponse response)
   {
      if (handlers.TryRemove(id, out var res))
      {
         var challenge = res.Challenge;
         challenge.Response = response;
         res.ResponseTask.TrySetResult(challenge);
         return challenge;
      }

      return new Challenge();
   }

   private void CheckAcks()
   {
      foreach (var pair in handlers)
      {
         var elapsed = DateTime.UtcNow - pair.Value.Created;
         if (elapsed > ackThreshold)
         {
            pair.Value.ResponseTask.TrySetException(new TimeoutException("Response time out"));
         }
      }
   }

   private record struct ChallengeRecord(
      Guid Id,
      TaskCompletionSource<Challenge> ResponseTask,
      DateTime Created,
      Challenge Challenge);
}