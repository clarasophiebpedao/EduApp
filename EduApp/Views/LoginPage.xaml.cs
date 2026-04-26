namespace EduApp.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnIrParaCadastro(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CadastroPage());
    }

    private async void OnBotaoEntrarClicked(object sender, EventArgs e)
    {

        await DisplayAlert("Login", "Botão de login funcionando! Aguardando banco de dados.", "OK");
    }
}