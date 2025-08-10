using CommunityToolkit.Mvvm.Messaging.Messages;

namespace SticksAndStones.Messages;

internal class ServiceError(AsyncError error) : ValueChangedMessage<AsyncError>(error);