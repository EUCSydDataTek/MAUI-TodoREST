using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using TodoREST.Models;
using TodoREST.Services;
using TodoREST.Views;

namespace TodoREST.ViewModels;

public partial class MainPageViewModel(IDataService service, IConnectivity connectivity) : BaseViewModel
{
    public ObservableCollection<Item> ItemsCollection { get; } = new();

    [ObservableProperty]
    bool isRefreshing;

    [RelayCommand]
    async Task GetItemsAsync()
    {
        if (IsBusy)
            return;

        try
        {
            if (connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("No connectivity!",
                    $"Please check internet and try again.", "OK");
                return;
            }

            IsBusy = true;
            var items = await service.GetItemsAsync();

            if (ItemsCollection.Count != 0)
                ItemsCollection.Clear();

            foreach (var item in items)
                ItemsCollection.Add(item);

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to get Items: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }


    [RelayCommand]
    async Task AddItem()
    {
        await Shell.Current.GoToAsync(nameof(DetailItemPage), true, new Dictionary<string, object>
        {
            {"item", new Item() }
        });
    }

    [RelayCommand]
    async Task GoToDetails(Item item)
    {
        if (item == null)
            return;

        await Shell.Current.GoToAsync(nameof(DetailItemPage), new Dictionary<string, object>
        {
            {"item", item }
        });
    }
}
