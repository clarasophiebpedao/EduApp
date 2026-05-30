namespace EduApp.Views
{
    public partial class CriarAtividadePage : ContentPage
    {
        public CriarAtividadePage()
        {
            InitializeComponent();

            BindingContext = new ViewModels.CriarAtividadeViewModel();
        }
    }
}