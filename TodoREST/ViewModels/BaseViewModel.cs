using CommunityToolkit.Mvvm.ComponentModel;

namespace TodoREST.ViewModels;
public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    bool isBusy = false;

    [ObservableProperty]
    string title = string.Empty;
}
