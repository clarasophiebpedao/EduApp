using EduApp.ViewModels;
using EduApp.ViewModels.Geral;
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