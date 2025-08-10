using CommunityToolkit.Mvvm.Messaging.Messages;
using SticksAndStones.Models;

namespace SticksAndStones.Messages;

internal class MatchUpdated(Match match) : ValueChangedMessage<Match>(match);