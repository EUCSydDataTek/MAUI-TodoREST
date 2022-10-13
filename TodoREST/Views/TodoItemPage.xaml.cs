using TodoREST.Services;
using TodoREST.ViewModels;

namespace TodoREST.Views
{
    public partial class TodoItemPage : ContentPage
    {
        public TodoItemPage(TodoItemViewModel vm)
        {
            InitializeComponent();

            BindingContext = vm;
        }
    }
}
