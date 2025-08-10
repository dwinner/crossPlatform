using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.AspNetCore.SignalR.Client;
using SticksAndStones.Messages;
using SticksAndStones.Models;

namespace SticksAndStones.Services;

public sealed partial class GameService(Settings settings, ServiceConnection service) : IDisposable
{
   private readonly WeakReferenceMessenger _messenger = WeakReferenceMessenger.Default;
   private readonly SemaphoreSlim semaphoreSlim = new(1, 1);

   public Player CurrentPlayer { get; private set; } = new() { Id = Guid.Empty, MatchId = Guid.Empty };

   public ObservableCollection<Player> Players { get; } = [];

   public bool IsConnected { get; private set; }

   public void Dispose()
   {
      service.Dispose();
      GC.SuppressFinalize(this);
   }

   public async Task<Player> Connect(Player player)
   {
      await semaphoreSlim.WaitAsync();
      try
      {
         CurrentPlayer = player;

         var (response, error) =
            await service.PostAsync<ConnectResponse>(new Uri($"{settings.ServerUrl}/Connect"),
               new ConnectRequest(player));
         if (error is null)
         {
            service.ConnectHub(response.ConnectionInfo);

            response.Players.ForEach(Players.Add);
            CurrentPlayer = response.Player;
            IsConnected = true;

            var svcHub = await service.Hub;
            svcHub.On<PlayerUpdatedEventArgs>(Constants.Events.PlayerUpdated,
               PlayerStatusChangedHandler);
            svcHub.On<ChallengeEventArgs>(Constants.Events.Challenge,
               args => _messenger.Send(new ChallengeReceived(args.Id, args.Challenger)));
            svcHub.On<MatchStartedEventArgs>(Constants.Events.MatchStarted,
               args => _messenger.Send(new MatchStarted(args.Match)));
            svcHub.On<MatchUpdatedEventArgs>(Constants.Events.MatchUpdated,
               args => _messenger.Send(new MatchUpdated(args.Match)));

            svcHub.Reconnected += _ => RefreshPlayerList();
         }
         else
         {
            _messenger.Send(new ServiceError(error));
         }
      }
      finally
      {
         semaphoreSlim.Release();
      }

      return CurrentPlayer;
   }

   public async Task RefreshPlayerList()
   {
      await semaphoreSlim.WaitAsync();
      try
      {
         var getAllPlayers = service.GetAsync<GetAllPlayersResponse>(
            new Uri($"{settings.ServerUrl}/Players/GetAll"),
            new Dictionary<string, string> { { "id", $"{CurrentPlayer.Id}" } });
         var (response, error) = await getAllPlayers;
         if (error is null)
         {
            Players.Clear();
            response.Players.ForEach(Players.Add);
         }
         else
         {
            _messenger.Send(new ServiceError(error));
         }
      }
      finally
      {
         semaphoreSlim.Release();
      }
   }

   public async Task IssueChallenge(Player opponent)
   {
      await semaphoreSlim.WaitAsync();
      try
      {
         var (_, error) = await service.PostAsync<IssueChallengeResponse>(
            new Uri($"{settings.ServerUrl}/Challenge/Issue"),
            new IssueChallengeRequest(CurrentPlayer, opponent));
         if (error is not null)
         {
            _messenger.Send(new ServiceError(error));
         }
      }
      finally
      {
         semaphoreSlim.Release();
      }
   }

   public async Task SendChallengeResponse(Guid challengeId, ChallengeResponse challengeResponse)
   {
      await semaphoreSlim.WaitAsync();
      try
      {
         var (_, error) = await service.PostAsync<string>(
            new Uri($"{settings.ServerUrl}/Challenge/Ack"),
            new AcknowledgeChallengeRequest(challengeId, challengeResponse));
         if (error is not null)
         {
            _messenger.Send(new ServiceError(error));
         }
      }
      finally
      {
         semaphoreSlim.Release();
      }
   }

   public async Task<(Match, string)> EndTurn(Guid matchId, int position)
   {
      await semaphoreSlim.WaitAsync();
      try
      {
         var (response, error) = await service.PostAsync<ProcessTurnResponse>(
            new Uri($"{settings.ServerUrl}/Match/Move"),
            new ProcessTurnRequest(matchId, CurrentPlayer, position));
         return error is not null
            ? (null, error.Message)
            : (response.Match, null);
      }
      finally
      {
         semaphoreSlim.Release();
      }
   }

   public Player GetPlayerById(Guid playerId)
   {
      if (playerId == CurrentPlayer.Id)
      {
         return CurrentPlayer;
      }

      return (from player in Players
            where player.Id == playerId
            select player)
         .FirstOrDefault();
   }

   public async Task<Match> GetMatchById(Guid matchId)
   {
      await semaphoreSlim.WaitAsync();
      try
      {
         var (response, error) = await service.GetAsync<GetMatchResponse>(
            new Uri($"{settings.ServerUrl}/Match/{matchId}"),
            new Dictionary<string, string>());
         return error != null ? new Match() : response.Match;
      }
      finally
      {
         semaphoreSlim.Release();
      }
   }

   private void PlayerStatusChangedHandler(PlayerUpdatedEventArgs args)
   {
      var changedPlayer = (from player in Players
         where player.Id == args.Player.Id
         select player).FirstOrDefault();
      if (changedPlayer is not null)
      {
         changedPlayer.MatchId = args.Player.MatchId;
      }
      else if (args.Player.Id != CurrentPlayer.Id)
      {
         Players.Add(args.Player);
      }
   }
}