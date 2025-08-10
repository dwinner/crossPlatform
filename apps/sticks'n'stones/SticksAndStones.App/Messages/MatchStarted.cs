using CommunityToolkit.Mvvm.Messaging.Messages;
using SticksAndStones.Models;

namespace SticksAndStones.Messages;

public class MatchStarted(Match match) : ValueChangedMessage<Match>(match);