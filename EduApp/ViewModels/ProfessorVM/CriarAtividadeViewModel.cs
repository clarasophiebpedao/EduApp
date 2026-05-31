using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EduApp.Services;
using EduApp.Models;

namespace EduApp.ViewModels.Professor
{
    public class CriarAtividadeViewModel : INotifyPropertyChanged
    {
        private readonly AtividadeService _atividadeService;

        // Campos privados
        private string _tituloDigitado;
        private string _descricaoDigitada;
        private int _pontosDigitados;

        // Propriedades completas com notificação
        public string TituloDigitado
        {
            get => _tituloDigitado;
            set
            {
                _tituloDigitado = value;
                OnPropertyChanged();
            }
        }

        public string DescricaoDigitada
        {
            get => _descricaoDigitada;
            set
            {
                _descricaoDigitada = value;
                OnPropertyChanged();
            }
        }

        public int PontosDigitados
        {
            get => _pontosDigitados;
            set
            {
                _pontosDigitados = value;
                OnPropertyChanged();
            }
        }

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

                // Limpa os campos da tela após salvar com sucesso
                TituloDigitado = string.Empty;
                DescricaoDigitada = string.Empty;
                PontosDigitados = 0;

                await Application.Current.MainPage.DisplayAlert("Sucesso!", "Atividade criada com sucesso!", "OK");
            }
        }

        // --- Implementação do INotifyPropertyChanged ---
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}