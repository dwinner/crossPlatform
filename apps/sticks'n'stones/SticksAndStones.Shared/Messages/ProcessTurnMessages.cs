using System;
using SticksAndStones.Models;

namespace SticksAndStones.Messages;

public record struct ProcessTurnRequest(Guid MatchId, Player Player, int Position);

public record struct ProcessTurnResponse(Match Match);