using EduApp.ViewModels; // Não esqueça desse using!

namespace EduApp.Views;

public partial class CadastroPage : ContentPage
{
    public CadastroPage()
    {
        InitializeComponent();

        // Isso conecta a sua View ao seu ViewModel
        BindingContext = new Cadastro();
    }

    // Método para o botão "Já tem conta? Login"
    private async void OnIrParaLogin(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}