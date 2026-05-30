using EduApp.ViewModels;

namespace EduApp.Views
{
    public partial class ListaAtividadesPage : ContentPage
    {
        public ListaAtividadesPage()
        {
            InitializeComponent();

            BindingContext = new AtividadesAlunoViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is AtividadesAlunoViewModel viewModel)
            {

                await viewModel.CarregarAtividadesAsync();
            }
        }
    }
}