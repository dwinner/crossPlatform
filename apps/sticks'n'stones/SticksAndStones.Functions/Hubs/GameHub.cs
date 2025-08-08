using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.SignalR.Management;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SticksAndStones.Handlers;
using SticksAndStones.Messages;
using SticksAndStones.Models;
using SticksAndStones.Repository;
using ConnectionInfo = SticksAndStones.Models.ConnectionInfo;

namespace SticksAndStones.Hubs;

public class GameHub(IDbContextFactory<GameDbContext> dbContextFactory, ChallengeHandler handler)
   : ServerlessHub
{
   private readonly JsonSerializerOptions jsonOptions = new(JsonSerializerDefaults.Web);

   private readonly (int stone, int[] sticks)[][] stickToStoneMap =
   [
      /* 1 */ [(1, [4, 5, 8]), (0, [0, 0, 0])],
      /* 2 */ [(2, [5, 6, 9]), (0, [0, 0, 0])],
      /* 3 */ [(3, [6, 7, 10]), (0, [0, 0, 0])],
      /* 4 */ [(1, [1, 5, 8]), (0, [0, 0, 0])],
      /* 5 */ [(1, [1, 4, 8]), (2, [2, 6, 9])],
      /* 6 */ [(2, [2, 5, 9]), (3, [3, 7, 10])],
      /* 7 */ [(3, [3, 6, 10]), (0, [0, 0, 0])],
      /* 8 */ [(1, [1, 4, 5]), (4, [11, 12, 15])],
      /* 9 */ [(2, [2, 5, 6]), (5, [12, 13, 16])],
      /*10 */ [(3, [3, 6, 7]), (6, [13, 14, 17])],
      /*11 */ [(4, [8, 12, 15]), (0, [0, 0, 0])],
      /*12 */ [(4, [8, 11, 15]), (5, [9, 13, 16])],
      /*13 */ [(5, [9, 12, 16]), (6, [10, 14, 17])],
      /*14 */ [(6, [10, 13, 17]), (0, [0, 0, 0])],
      /*15 */ [(4, [8, 11, 12]), (7, [18, 19, 22])],
      /*16 */ [(5, [9, 12, 13]), (8, [19, 20, 23])],
      /*17 */ [(6, [13, 14, 17]), (9, [20, 21, 24])],
      /*18 */ [(7, [15, 19, 22]), (0, [0, 0, 0])],
      /*19 */ [(7, [15, 18, 22]), (8, [16, 20, 23])],
      /*20 */ [(8, [16, 19, 23]), (9, [17, 21, 24])],
      /*21 */ [(9, [17, 20, 24]), (0, [0, 0, 0])],
      /*22 */ [(7, [15, 18, 19]), (0, [0, 0, 0])],
      /*23 */ [(8, [16, 19, 20]), (0, [0, 0, 0])],
      /*24 */ [(9, [17, 20, 21]), (0, [0, 0, 0])]
   ];

   [FunctionName(nameof(Connect))]
   public async Task<IActionResult> Connect(
      [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
      HttpRequest request,
      [SignalR(HubName = "GameHub")] IAsyncCollector<SignalRMessage> signalRMessages,
      ILogger log)
   {
      log.LogInformation("A new client is requesting connection");

      var result = await JsonSerializer.DeserializeAsync<ConnectRequest>(request.Body, jsonOptions);
      var newPlayer = result.Player;

      if (newPlayer.Id == Guid.Empty)
      {
         var error = new ArgumentException("No player data.", nameof(newPlayer));
         log.LogError(error, "Failure to deserialize arguments");
         return new BadRequestObjectResult(error);
      }

      if (string.IsNullOrEmpty(newPlayer.GamerTag))
      {
         var error = new ArgumentException("A GamerTag is required for all players.", nameof(newPlayer.GamerTag));
         log.LogError(error, "Invalid value for GamerTag");
         return new BadRequestObjectResult(error);
      }

      if (string.IsNullOrEmpty(newPlayer.EmailAddress))
      {
         var error = new ArgumentException("An Email Address is required for all players.",
            nameof(newPlayer.EmailAddress));
         log.LogError(error, "Invalid value for EmailAddress");
         return new BadRequestObjectResult(error);
      }

      await using var context = await dbContextFactory.CreateDbContextAsync();

      log.LogInformation("Checking for GamerTag usage");
      var gamerTagInUse =
         (from player in context.Players
            where string.Equals(player.GamerTag, newPlayer.GamerTag, StringComparison.InvariantCultureIgnoreCase)
                  && !string.Equals(player.EmailAddress, newPlayer.EmailAddress, StringComparison.OrdinalIgnoreCase)
            select player).Any();
      if (gamerTagInUse)
      {
         var error = new ArgumentException(
            $"The GamerTag {newPlayer.GamerTag} is in use, please choose another.",
            nameof(newPlayer.GamerTag));
         log.LogError(error, "GamerTag in use.");
         return new BadRequestObjectResult(error);
      }

      log.LogInformation("Locating Player record.");
      var thisPlayer =
         (from player in context.Players
            where string.Equals(player.EmailAddress, newPlayer.EmailAddress, StringComparison.OrdinalIgnoreCase)
            select player)
         .FirstOrDefault();

      if (thisPlayer is null)
      {
         log.LogInformation("Player not found, creating.");
         thisPlayer = newPlayer;
         thisPlayer.Id = Guid.NewGuid();
         context.Add(thisPlayer);
         await context.SaveChangesAsync();
      }

      log.LogInformation("Notifying connected players of new player.");
      await Clients.All.SendAsync(Constants.Events.PlayerUpdated, new PlayerUpdatedEventArgs(thisPlayer));

      // Get the set of available players
      log.LogInformation("Getting the set of available players.");
      var players = (from player in context.Players
            where player.Id != thisPlayer.Id
            select player)
         .ToList();

      var connectionInfo = await NegotiateAsync(new NegotiationOptions { UserId = thisPlayer.Id.ToString() });

      log.LogInformation("Creating response.");
      var connectResponse = new ConnectResponse
      {
         Player = thisPlayer,
         Players = players,
         ConnectionInfo = new ConnectionInfo
         {
            Url = connectionInfo.Url,
            AccessToken = connectionInfo.AccessToken
         }
      };

      log.LogInformation("Sending response.");
      return new OkObjectResult(connectResponse);
   }

   [FunctionName(nameof(GetAllPlayers))]
   public IActionResult GetAllPlayers(
      [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Players/GetAll")]
      HttpRequest request,
      ILogger log)
   {
      // Exclude the playerId if provided
      var playerId = Guid.Empty;
      if (request.Query.ContainsKey("id"))
      {
         string id = request.Query["id"];
         if (!string.IsNullOrEmpty(id))
         {
            playerId = new Guid(id);
         }
      }

      using var context = dbContextFactory.CreateDbContext();

      // Get the set of available players
      log.LogInformation("Getting the set of available players.");
      var players = (from player in context.Players
            where player.Id != playerId
            select player)
         .ToList();
      return new OkObjectResult(new GetAllPlayersResponse(players));
   }

   [FunctionName(nameof(IssueChallenge))]
   public async Task<IssueChallengeResponse> IssueChallenge(
      [HttpTrigger(AuthorizationLevel.Function, "post", Route = "Challenge/Issue")]
      HttpRequest request,
      ILogger log)
   {
      var result = await JsonSerializer.DeserializeAsync<IssueChallengeRequest>(request.Body, jsonOptions);

      await using var context = await dbContextFactory.CreateDbContextAsync();

      var challengerId = result.Challenger.Id;
      var challenger = (from player in context.Players
            where player.Id == challengerId
            select player)
         .FirstOrDefault();

      var opponentId = result.Opponent.Id;
      var opponent = (from player in context.Players
            where player.Id == opponentId
            select player)
         .FirstOrDefault();

      if (challenger is null)
      {
         throw new ArgumentException(
            paramName: nameof(challenger),
            message: "Challenger is not a valid player.");
      }

      if (opponent is null)
      {
         throw new ArgumentException(
            paramName: nameof(opponent),
            message: "Opponent is not a valid player.");
      }

      var challengerInMatch = (from match in context.Matches
            where match.PlayerOneId == challengerId || match.PlayerTwoId == challengerId
            select match)
         .Any();

      var opponentInMatch = (from match in context.Matches
            where match.PlayerOneId == opponentId || match.PlayerTwoId == opponentId
            select match)
         .Any();

      if (challengerInMatch)
      {
         throw new ArgumentException(
            paramName: nameof(challenger),
            message: $"{challenger.GamerTag} is already in a match!");
      }

      if (opponentInMatch)
      {
         throw new ArgumentException(
            paramName: nameof(opponent),
            message: $"{opponent.GamerTag} is already in a match!");
      }

      log.LogInformation("{ChallengerGamerTag} has challenged {OpponentGamerTag} to a match!",
         challenger.GamerTag,
         opponent.GamerTag);

      var challengeInfo = handler.CreateChallenge(challenger, opponent);
      log.LogInformation("Challenge [{ChallengeInfoId}] has been created.",
         challengeInfo.id);
      log.LogInformation("Waiting on response from {OpponentGamerTag} for challenge[{ChallengeInfoId}].",
         opponent.GamerTag,
         challengeInfo.id);

      await Clients.User(opponent.Id.ToString()).SendAsync(Constants.Events.Challenge,
         new ChallengeEventArgs(challengeInfo.id, challenger, opponent));

      ChallengeResponse response;
      try
      {
         var challenge = await challengeInfo.responseTask.ConfigureAwait(false);
         log.LogInformation("Got response from {OpponentGamerTag} for challenge[{ChallengeInfoId}].",
            opponent.GamerTag,
            challengeInfo.id);
         response = challenge.Response;
      }
      catch
      {
         log.LogInformation(
            "Never received a response from {OpponentGamerTag} for challenge[{ChallengeInfoId}], it timed out.",
            opponent.GamerTag, challengeInfo.id);
         response = ChallengeResponse.TimeOut;
      }

      return new IssueChallengeResponse(response);
   }

   [FunctionName(nameof(AcknowledgeChallenge))]
   public async Task AcknowledgeChallenge(
      [HttpTrigger(AuthorizationLevel.Function, "post", Route = "Challenge/Ack")]
      HttpRequest request,
      ILogger log)
   {
      var result = await JsonSerializer.DeserializeAsync<AcknowledgeChallengeRequest>(request.Body, jsonOptions);
      var (guid, challenger, opponent, _) = handler.Respond(result.Id, result.Response);
      if (guid == Guid.Empty)
      {
         return;
      }

      switch (result.Response)
      {
         case ChallengeResponse.Declined:
            log.LogInformation("{OpponentGamerTag} has declined the challenge from {ChallengerGamerTag}!",
               opponent.GamerTag,
               challenger.GamerTag);
            break;

         case ChallengeResponse.Accepted:
         {
            log.LogInformation("{OpponentGamerTag} has accepted the challenge from {ChallengerGamerTag}!",
               opponent.GamerTag,
               challenger.GamerTag);

            await using var context = await dbContextFactory.CreateDbContextAsync();

            var match = Match.New(challenger.Id, opponent.Id);
            context.Matches.Add(match);

            opponent.MatchId = challenger.MatchId = match.Id;

            context.Players.Update(opponent);
            context.Players.Update(challenger);
            await context.SaveChangesAsync();

            log.LogInformation("Created match {MatchId} between {OpponentGamerTag} and {ChallengerGamerTag}!",
               match.Id,
               opponent.GamerTag,
               challenger.GamerTag);

            // Create Group for Game
            await UserGroups.AddToGroupAsync(opponent.Id.ToString(), $"Match[{match.Id}]");
            await UserGroups.AddToGroupAsync(challenger.Id.ToString(), $"Match[{match.Id}]");
            await Clients.Group($"Match[{match.Id}]")
               .SendAsync(Constants.Events.MatchStarted, new MatchStartedEventArgs(match));

            await Clients.All.SendAsync(Constants.Events.PlayerUpdated, new PlayerUpdatedEventArgs(opponent));
            await Clients.All.SendAsync(Constants.Events.PlayerUpdated, new PlayerUpdatedEventArgs(challenger));
            break;
         }

         case ChallengeResponse.None:
         case ChallengeResponse.TimeOut:
            break;

         default:
            throw new ArgumentOutOfRangeException(nameof(result.Response));
      }
   }

   [FunctionName(nameof(GetMatch))]
   public IActionResult GetMatch(
      [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Match/{id}")]
      HttpRequest request,
      Guid id,
      ILogger log)
   {
      using var context = dbContextFactory.CreateDbContext();

      var match = (from lMatch in context.Matches
            where lMatch.Id == id
            select lMatch)
         .FirstOrDefault();
      IActionResult result = match is null
         ? new BadRequestObjectResult("Match is null")
         : new OkObjectResult(new GetMatchResponse(match));

      return result;
   }

   [FunctionName(nameof(ProcessTurn))]
   public async Task<IActionResult> ProcessTurn(
      [HttpTrigger(AuthorizationLevel.Function, "post", Route = "Game/Move")]
      HttpRequest request,
      ILogger log)
   {
      var args = await JsonSerializer.DeserializeAsync<ProcessTurnRequest>(request.Body, jsonOptions);

      var error = ValidateProcessTurnRequest(args);
      if (error is not null)
      {
         log.LogError(error, "Error validating turn request.");
         return new BadRequestObjectResult(error);
      }

      await using var context = await dbContextFactory.CreateDbContextAsync();

      var match = (from lMatch in context.Matches
            where lMatch.Id == args.MatchId
            select lMatch)
         .FirstOrDefault() ?? throw new ArgumentException("Invalid MatchId.");

      error = VerifyMatchState(match, args);
      if (error is not null)
      {
         await SaveMatchAndSendUpdates(context, match);
         log.LogError(error, "Error validating match state.");
         return new BadRequestObjectResult(error);
      }

      match.Sticks[args.Position] = args.Player.Id == match.PlayerOneId ? 1 : -1;
      if (args.Player.Id == match.PlayerOneId)
      {
         match.PlayerOneScore += 1;
      }
      else
      {
         match.PlayerTwoScore += 1;
      }

      // Determine if this play creates a square
      foreach (var tuple in stickToStoneMap[args.Position])
      {
         if (tuple.stone == 0)
         {
            continue;
         }

         var stickCompletesABox =
            Math.Abs(match.Sticks[tuple.sticks[0] - 1]) +
            Math.Abs(match.Sticks[tuple.sticks[1] - 1]) +
            Math.Abs(match.Sticks[tuple.sticks[2] - 1]) == 3;

         if (stickCompletesABox)
         {
            // If so, place stone, and adjust score
            var player = args.Player.Id == match.PlayerOneId ? 1 : -1;
            match.Stones[tuple.stone - 1] = player;
            if (player > 0)
            {
               match.PlayerOneScore += 5;
            }
            else
            {
               match.PlayerTwoScore += 5;
            }
         }
      }

      // Does one player have 3 stones in a row?
      var winner = Guid.Empty;
      var threeInARow = HasThreeInARow(match.Stones);
      if (threeInARow != 0)
      {
         winner = threeInARow > 0 ? match.PlayerOneId : match.PlayerTwoId;
      }

      if (winner == Guid.Empty) // No Winner yet
      {
         // Have all sticks been played, if yes, use top score.
         if (AllSticksHaveBeenPlayed(match))
         {
            winner = match.PlayerOneScore > match.PlayerTwoScore ? match.PlayerOneId : match.PlayerTwoId;
         }
      }

      if (winner == Guid.Empty)
      {
         match.NextPlayerId = args.Player.Id == match.PlayerOneId ? match.PlayerTwoId : match.PlayerOneId;
      }
      else
      {
         match.NextPlayerId = Guid.Empty;
         match.WinnerId = winner;
         match.Completed = true;
      }

      await SaveMatchAndSendUpdates(context, match);

      return new OkObjectResult(new ProcessTurnResponse(match));
   }

   private static Exception ValidateProcessTurnRequest(ProcessTurnRequest args)
   {
      if (args.MatchId == Guid.Empty)
      {
         return new ArgumentException("Invalid MatchId");
      }

      if (args.Player.MatchId == Guid.Empty)
      {
         return new ArgumentException("Invalid Player");
      }

      return args.Position is < 0 or > 23
         ? new IndexOutOfRangeException("Position is out of range, must be between 1 and 24")
         : null;
   }

   private static ArgumentException VerifyMatchState(Match match, ProcessTurnRequest args)
   {
      if (match is null)
      {
         return new ArgumentException("Invalid MatchId");
      }

      if (match.WinnerId != Guid.Empty)
      {
         return new ArgumentException("Match is complete");
      }

      if (args.Player.Id != match.NextPlayerId)
      {
         return new ArgumentException($"It is not {args.Player.GamerTag}'s turn");
      }

      if (match.Sticks[args.Position] != 0)
      {
         return new ArgumentException($"Position [{args.Position}] has already been played");
      }

      return null;
   }

   private static bool AllSticksHaveBeenPlayed(Match match) =>
      !(from stick in match.Sticks
            where stick == 0
            select stick)
         .Any();

   private static int HasThreeInARow(List<int> stones)
   {
      for (var rc = 0; rc < 3; rc++)
      {
         var rowStart = rc * 3;
         var rowValue = stones[rowStart] + stones[rowStart + 1] + stones[rowStart + 2];
         if (Math.Abs(rowValue) == 3) // we Have a winner!
         {
            return rowValue;
         }

         var colValue = stones[rc] + stones[rc + 3] + stones[rc + 6];
         if (Math.Abs(colValue) == 3) // We have a winner!
         {
            return colValue;
         }
      }

      var tlbrValue = stones[0] + stones[4] + stones[8];
      var trblValue = stones[2] + stones[4] + stones[6];
      if (Math.Abs(tlbrValue) == 3)
      {
         return tlbrValue;
      }

      if (Math.Abs(trblValue) == 3)
      {
         return trblValue;
      }

      return 0;
   }

   private async Task SaveMatchAndSendUpdates(GameDbContext context, Match match)
   {
      context.Matches.Update(match);
      await context.SaveChangesAsync();
      await Clients.Group($"Match[{match.Id}]")
         .SendAsync(Constants.Events.MatchUpdated, new MatchUpdatedEventArgs(match));
      if (match.Completed)
      {
         await UserGroups.RemoveFromGroupAsync(match.PlayerOneId.ToString(), $"Match[{match.Id}]");
         await UserGroups.RemoveFromGroupAsync(match.PlayerTwoId.ToString(), $"Match[{match.Id}]");
      }
   }
}