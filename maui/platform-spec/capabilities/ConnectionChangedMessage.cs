using CommunityToolkit.Mvvm.Messaging.Messages;

namespace CrossPlatformCapabilities;

public class ConnectionChangedMessage(bool value) : ValueChangedMessage<bool>(value);