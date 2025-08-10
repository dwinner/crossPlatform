using CommunityToolkit.Mvvm.Messaging.Messages;
using SticksAndStones.Models;

namespace SticksAndStones.Messages;

public class ChallengeReceived(Guid id, Player challenger) : ValueChangedMessage<Player>(challenger)
{
   public Guid Id { get; init; } = id;
}