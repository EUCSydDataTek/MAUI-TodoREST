using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoREST.Models;
using TodoREST.Services;

namespace TodoREST.ViewModels;

[QueryProperty(nameof(TheItem), "item")]
public partial class DetailItemViewModel : BaseViewModel
{
    readonly IDataService _service;
    public DetailItemViewModel(IDataService service)
    {
        _service = service;
    }

    [ObservableProperty]
    Item theItem;

    bool isNewItem;

    partial void OnTheItemChanging(Item value)
    {
        isNewItem = string.IsNullOrWhiteSpace(value.Name) && string.IsNullOrWhiteSpace(value.Notes) ? true : false;
    }

    [RelayCommand]
    async Task Save()
    {
        await _service.SaveItemAsync(TheItem, isNewItem);
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    async Task Delete()
    {
        await _service.DeleteItemAsync(TheItem);
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    async Task Cancel()
    {
        await Shell.Current.GoToAsync("..");
    }
}
