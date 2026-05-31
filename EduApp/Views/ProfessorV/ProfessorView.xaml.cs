using EduApp.ViewModels;
using EduApp.ViewModels.Professor;

namespace EduApp.Views;

public partial class ProfessorView : ContentPage
{
    public ProfessorView()
    {
        InitializeComponent();

        BindingContext = new ProfessorViewModel();

    }

    private async void OnIrParaCadastro(object sender, EventArgs e)
    {
    await Navigation.PushAsync(new CriarAtividadePage());
    }

    private async void OnIrParaCorrigir(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CorrigirAtividadesPage());
    }

}   
