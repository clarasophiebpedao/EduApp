using System.ComponentModel; 
using System.Windows.Input; 
using EduApp.Services;      
using EduApp.Models;       

namespace EduApp.ViewModels
{
    public class CriarAtividadeViewModel : INotifyPropertyChanged
    {
      
        private readonly AtividadeService _atividadeService;

       
        public string TituloDigitado { get; set; }
        public string DescricaoDigitada { get; set; }
        public int PontosDigitados { get; set; }

   
        public ICommand SalvarAtividadeCommand { get; }

     
        public CriarAtividadeViewModel()
        {
            _atividadeService = new AtividadeService();

            SalvarAtividadeCommand = new Command(async () => await SalvarAtividade());
        }

        private async Task SalvarAtividade()
        {
            var novaAtividade = new Atividade
            {
                Titulo = TituloDigitado,
                Descricao = DescricaoDigitada,
                Pontos = PontosDigitados
            };

            bool deuCerto = await _atividadeService.InserirAtividadeAsync(novaAtividade);

            if (deuCerto)
            {
                System.Diagnostics.Debug.WriteLine("Atividade salva no banco!");
                await App.Current.MainPage.DisplayAlert("Sucesso!", "Atividade criada com sucesso!", "OK");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}