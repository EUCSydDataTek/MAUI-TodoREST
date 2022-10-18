﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoREST.Models;
using TodoREST.Services;

namespace TodoREST.ViewModels;

[QueryProperty(nameof(TodoItem), "item")]
public partial class TodoItemViewModel : BaseViewModel
{
    readonly ITodoService _todoService;
    public TodoItemViewModel(ITodoService service)
    {
        _todoService = service;
    }

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
        await _todoService.SaveTaskAsync(TodoItem, isNewItem);
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    async Task Delete()
    {
        await _todoService.DeleteTaskAsync(TodoItem);
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    async Task Cancel()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    async Task GetItemById()
    {
        TodoItem = await _todoService.GetTaskByIdAsync("ecfa6f80-3671-4911-aabe-63cc442c1ecf");
    }
}
