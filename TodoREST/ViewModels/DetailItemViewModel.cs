using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoREST.Models;
using TodoREST.Services;

namespace TodoREST.ViewModels;

[QueryProperty(nameof(TheItem), "item")]
public partial class DetailItemViewModel(IDataService service) : BaseViewModel
{
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
        await service.SaveItemAsync(TheItem, isNewItem);
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    async Task Delete()
    {
        await service.DeleteItemAsync(TheItem);
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    async Task Cancel()
    {
        await Shell.Current.GoToAsync("..");
    }
}
