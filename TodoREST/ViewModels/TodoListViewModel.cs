﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using TodoREST.Models;
using TodoREST.Services;
using TodoREST.Views;

namespace TodoREST.ViewModels;

public partial class TodoListViewModel(ITodoService service, IConnectivity connectivity) : BaseViewModel
{
    public ObservableCollection<TodoItem> TodoItems { get; } = new();

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
            var todos = await service.GetTasksAsync();

            if (TodoItems.Count != 0)
                TodoItems.Clear();

            foreach (var todoitem in todos)
                TodoItems.Add(todoitem);

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to get TodoItems: {ex.Message}");
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
        await Shell.Current.GoToAsync(nameof(TodoItemPage), true, new Dictionary<string, object>
        {
            {"item", new TodoItem() }
        });
    }

    [RelayCommand]
    async Task GoToDetails(TodoItem todoItem)
    {
        if (todoItem == null)
            return;

        await Shell.Current.GoToAsync(nameof(TodoItemPage), new Dictionary<string, object>
        {
            {"item", todoItem }
        });
    }

}
