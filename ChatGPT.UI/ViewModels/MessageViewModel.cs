using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ChatGPT.UI.ViewModels;

public class MessageViewModel : ObservableObject
{
    private string? _prompt;
    private string? _message;
    private bool _isSent;
    private bool _isAwaiting;
    private bool _isError;
    private bool _canRemove;
    private MessageViewModel? _result;
    private Func<MessageViewModel, Task>? _send;
    private Func<MessageViewModel, Task>? _copy;
    private Action<MessageViewModel>? _remove;

    public MessageViewModel()
    {
        SendCommand = new AsyncRelayCommand(async _ => await SendAction());

        EditCommand = new RelayCommand<string>(EditAction);

        CopyCommand = new RelayCommand(CopyAction);

        RemoveCommand = new RelayCommand(RemoveAction);
    }

    [JsonPropertyName("Prompt")]
    public string? Prompt
    {
        get => _prompt;
        set => SetProperty(ref _prompt, value);
    }

    [JsonPropertyName("prompt")]
    public string? Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }

    [JsonPropertyName("isSent")]
    public bool IsSent
    {
        get => _isSent;
        set => SetProperty(ref _isSent, value);
    }

    [JsonPropertyName("isAwaiting")]
    public bool IsAwaiting
    {
        get => _isAwaiting;
        set => SetProperty(ref _isAwaiting, value);
    }

    [JsonPropertyName("isError")]
    public bool IsError
    {
        get => _isError;
        set => SetProperty(ref _isError, value);
    }

    [JsonPropertyName("canRemove")]
    public bool CanRemove
    {
        get => _canRemove;
        set => SetProperty(ref _canRemove, value);
    }

    [JsonPropertyName("result")]
    public MessageViewModel? Result
    {
        get => _result;
        set => SetProperty(ref _result, value);
    }

    [JsonIgnore]
    public IAsyncRelayCommand SendCommand { get; }

    [JsonIgnore]
    public IRelayCommand EditCommand { get; }

    [JsonIgnore]
    public IRelayCommand CopyCommand { get; }

    [JsonIgnore]
    public IRelayCommand RemoveCommand { get; }

    private async Task SendAction()
    {
        if (_send is { })
        {
            await _send(this);
        }
    }

    private void EditAction(string? status)
    {
        switch (status)
        {
            case "Edit":
            {
                EditingState();
                break;
            }
            case "Cancel":
            {
                CanceledState();
                break;
            }
        }
    }

    private void EditingState()
    {
        Prompt = Message;
        Message = null;
        IsSent = false;
    }

    private void CanceledState()
    {
        Message = Prompt;
        Prompt = null;
        IsSent = true;
    }

    private void CopyAction()
    {
        _copy?.Invoke(this);
    }

    private void RemoveAction()
    {
        _remove?.Invoke(this);
    }

    public void SetSendAction(Func<MessageViewModel, Task>? send)
    {
        _send = send;
    }

    public void SetCopyAction(Func<MessageViewModel, Task>? copy)
    {
        _copy = copy;
    }

    public void SetRemoveAction(Action<MessageViewModel>? remove)
    {
        _remove = remove;
    }
}
