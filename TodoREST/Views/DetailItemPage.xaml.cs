using TodoREST.Services;
using TodoREST.ViewModels;

namespace TodoREST.Views
{
    public partial class DetailItemPage : ContentPage
    {
        public DetailItemPage(DetailItemViewModel vm)
        {
            InitializeComponent();

            BindingContext = vm;
        }
    }
}
