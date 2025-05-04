using CommunityToolkit.Mvvm.Messaging.Messages;

namespace CrossPlatformCapabilities;

public class BatteryStatusChangedMessage(bool value) : ValueChangedMessage<bool>(value);