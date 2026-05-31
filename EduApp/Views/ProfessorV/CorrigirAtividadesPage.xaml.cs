using EduApp.ViewModels.Professor;

namespace EduApp.Views
{
    public partial class CorrigirAtividadesPage : ContentPage
    {
        public CorrigirAtividadesPage()
        {
            InitializeComponent();

            BindingContext = new CorrigirAtividadesViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is CorrigirAtividadesViewModel viewModel)
            {

                await viewModel.CarregarEntregasPendentesAsync();
            }
        }
    }
}