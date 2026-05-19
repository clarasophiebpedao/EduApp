using EduApp.ViewModels;

namespace EduApp.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();

        // Conecta a tela (View) ao seu cérebro (ViewModel)
        BindingContext = new Login();
    }

    // Apenas a navegação visual fica aqui
    private async void OnIrParaCadastro(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CadastroPage());
    }
}