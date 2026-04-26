using EduApp.Services;

namespace EduApp.Views;

public partial class CadastroPage : ContentPage
{
    public CadastroPage()
    {
        InitializeComponent();
    }

    // 1. Método para o botão "Já tem conta? Login"
    private async void OnIrParaLogin(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    // 2. Método para o botão principal "Criar Conta"
    private async void OnFinalizarCadastro(object sender, EventArgs e)
    {
        // Capturando o que foi digitado
        string email = EmailEntry.Text;
        string senha = SenhaEntry.Text;
        string confirma = ConfirmarSenhaEntry.Text;

        // Validação simples (Essencial para o TCC)
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
        {
            await DisplayAlert("Atenção", "Por favor, preencha todos os campos.", "Entendido");
            return;
        }

        if (senha != confirma)
        {
            await DisplayAlert("Erro", "As senhas não coincidem!", "Tentar novamente");
            return;
        }

        // Simulando o sucesso para o usuário
        await DisplayAlert("Sucesso!", $"Conta criada para: {email}", "OK");

        // Teste de conexao com o aiven
        var aivenService = new AivenDatabaseService();
        string resultadoBanco = await aivenService.TestarConexaoAsync();
        await DisplayAlert("Teste de Banco", resultadoBanco, "OK");

        // Opcional: Voltar para o login automaticamente após o sucesso
        await Navigation.PopAsync();
    }
}