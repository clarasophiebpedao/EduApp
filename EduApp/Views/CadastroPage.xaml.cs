using EduApp.ViewModels;
using System;

namespace EduApp.Views;

public partial class CadastroPage : ContentPage
{
    public CadastroPage()
    {
        InitializeComponent();

        BindingContext = new Cadastro();
    }

    private async void OnIrParaLogin(object sender, EventArgs e)
    {

        await Navigation.PopAsync();
    }
}