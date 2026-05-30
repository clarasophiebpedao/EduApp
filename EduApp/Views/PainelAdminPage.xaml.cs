using EduApp.ViewModels;
using System;

namespace EduApp.Views;

public partial class PainelAdminPage : ContentPage
{
    public PainelAdminPage()
    {
        InitializeComponent();

        // Conecta a tela ao seu gerente correspondente
        BindingContext = new PainelAdminViewModel();
    }

    // Dispara automaticamente sempre que o Admin entra ou volta para esta tela
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is PainelAdminViewModel viewModel)
        {
            // Força a atualização da lista chamando o banco do Aiven
            await viewModel.CarregarUsuariosPendentesAsync();
        }
    }
}