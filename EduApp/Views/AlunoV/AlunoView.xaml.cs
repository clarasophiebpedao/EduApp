using EduApp.ViewModels;
using EduApp.ViewModels.Aluno;

namespace EduApp.Views.Aluno;

public partial class AlunoView : ContentPage
{
	public AlunoView()
	{
		InitializeComponent();

        BindingContext = new AlunoViewModel();
    }

    private async void AlunoIrParaAtividades(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ListaAtividadesPage());
    }

}