using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossPlatformCapabilities
{
    public class BatteryStatusChangedMessage : ValueChangedMessage<bool>
    {
        public BatteryStatusChangedMessage(bool value) : base(value)
        {
        }
    }
}
