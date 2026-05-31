namespace EduApp.Views;

using EduApp.ViewModels.Professor;

public partial class CriarAtividadePage : ContentPage
{
    public CriarAtividadePage()
    {
        InitializeComponent();

        BindingContext = new ViewModels.Professor.CriarAtividadeViewModel();
    }
}