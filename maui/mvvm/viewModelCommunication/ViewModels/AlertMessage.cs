using CommunityToolkit.Mvvm.Messaging.Messages;

namespace c2_ViewModelCommunication.ViewModels;

public class AlertMessage(string? value) : ValueChangedMessage<string?>(value);