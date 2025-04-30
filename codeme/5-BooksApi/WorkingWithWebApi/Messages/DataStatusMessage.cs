using CommunityToolkit.Mvvm.Messaging.Messages;

namespace WorkingWithWebApi.Messages
{
    public class DataStatusMessage : 
        ValueChangedMessage<DataStatus>
    {
        public DataStatusMessage(DataStatus value) : base(value)
        {

        }
    }

    public enum DataStatus
    {
        BookSaved,
        BookDeleted,
        BookError
    }
}
