using EduApp.ViewModels;
using EduApp.ViewModels.Aluno;

namespace EduApp.Views.Aluno;

public partial class AlunoView : ContentPage
{
    private ViewModels.Aluno.AlunoViewModel _viewModel;
    public AlunoView()
	{
		InitializeComponent();

        _viewModel = new ViewModels.Aluno.AlunoViewModel();
        BindingContext = _viewModel;
    }

    private async void AlunoIrParaAtividades(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ListaAtividadesPage());
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Puxa os dados atualizados do banco (Pontos XP)
        await _viewModel.CarregarDadosDoHeroiAsync();
    }

}