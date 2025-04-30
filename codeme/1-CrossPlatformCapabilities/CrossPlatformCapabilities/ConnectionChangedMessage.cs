using CommunityToolkit.Mvvm.Messaging.Messages;

namespace CrossPlatformCapabilities
{
    // Requires using CommunityToolkit.Mvvm.Messaging.Messages;
    public class ConnectionChangedMessage : ValueChangedMessage<bool>
    {
        public ConnectionChangedMessage(bool value) : base(value)
        {
        }
    }
}
