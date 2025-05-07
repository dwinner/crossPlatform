using CommunityToolkit.Mvvm.Messaging.Messages;

namespace WorkingWithWebApi.Messages;

public class DataStatusMessage(DataStatus value) : ValueChangedMessage<DataStatus>(value);