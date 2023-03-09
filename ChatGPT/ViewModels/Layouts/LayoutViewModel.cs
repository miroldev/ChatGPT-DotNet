using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ChatGPT.ViewModels.Layouts;

[JsonPolymorphic]
public abstract partial class LayoutViewModel : ObservableObject
{
    private bool _showSettings;
    private bool _showChats;
    private bool _showPrompts;

    public LayoutViewModel()
    {
        ShowSettingsCommand = new RelayCommand(ShowSettingsAction);

        ShowChatsCommand = new RelayCommand(ShowChatsAction);

        ShowPromptsCommand = new RelayCommand(ShowPromptsAction);
    }
        
    [JsonIgnore]
    public bool ShowSettings
    {
        get => _showSettings;
        set => SetProperty(ref _showSettings, value);
    }

    [JsonIgnore]
    public bool ShowChats
    {
        get => _showChats;
        set => SetProperty(ref _showChats, value);
    }

    [JsonIgnore]
    public bool ShowPrompts
    {
        get => _showPrompts;
        set => SetProperty(ref _showPrompts, value);
    }

    [JsonIgnore]
    public IRelayCommand ShowSettingsCommand { get; }

    [JsonIgnore]
    public IRelayCommand ShowChatsCommand { get; }

    [JsonIgnore]
    public IRelayCommand ShowPromptsCommand { get; }

    public abstract Task Back();

    protected abstract void ShowSettingsAction();

    protected abstract void ShowChatsAction();

    protected abstract void ShowPromptsAction();
}
