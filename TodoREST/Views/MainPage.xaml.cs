using TodoREST.ViewModels;

namespace TodoREST.Views
{
    public partial class MainPage : ContentPage
    {
        MainPageViewModel vm;

        public MainPage(MainPageViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
            this.vm = vm;         
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.GetItemsCommand.Execute(null);
        }
    }
}
