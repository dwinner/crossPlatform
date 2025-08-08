using System;
using SticksAndStones.Models;

namespace SticksAndStones.Messages;

public record struct ChallengeEventArgs(Guid Id, Player Challenger, Player Opponent);