namespace EduApp.Views.AlunoV;

public partial class ResponderMissaoPage : ContentPage
{
    public ResponderMissaoPage(Models.Atividade atividadeSelecionada)
    {
        InitializeComponent();
        BindingContext = new ViewModels.Aluno.ResponderMissaoViewModel(atividadeSelecionada);
    }
}