using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoREST.Models;
using TodoREST.Services;

namespace TodoREST.ViewModels;

[QueryProperty(nameof(TodoItem), "item")]
public partial class TodoItemViewModel(ITodoService todoService) : BaseViewModel
{
    [ObservableProperty]
    TodoItem todoItem;

    bool isNewItem;

    partial void OnTodoItemChanging(TodoItem value)
    {
        isNewItem = string.IsNullOrWhiteSpace(value.Name) && string.IsNullOrWhiteSpace(value.Notes) ? true : false;
    }

    [RelayCommand]
    async Task Save()
    {
        await todoService.SaveTaskAsync(TodoItem, isNewItem);
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    async Task Delete()
    {
        await todoService.DeleteTaskAsync(TodoItem);
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    async Task Cancel()
    {
        await Shell.Current.GoToAsync("..");
    }
}
