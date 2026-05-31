using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using EduApp.Models;
using EduApp.Services;
using EduApp.Views.Aluno;
using EduApp.Views.AlunoV; // Importante para achar a nova página

namespace EduApp.ViewModels.Aluno
{
    public class AtividadesAlunoViewModel
    {
        private readonly AtividadeService _atividadeService;

        public ObservableCollection<Atividade> Atividades { get; set; }

        // NOVO: Comando de navegação
        public ICommand AbrirMissaoCommand { get; }

        public AtividadesAlunoViewModel()
        {
            _atividadeService = new AtividadeService();
            Atividades = new ObservableCollection<Atividade>();

            // NOVO: Quando clicar na lista, abre a nova página passando a atividade
            AbrirMissaoCommand = new Command<Atividade>(async (atividadeClicada) =>
            {
                if (atividadeClicada != null)
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new ResponderMissaoPage(atividadeClicada));
                }
            });
        }

        public async Task CarregarAtividadesAsync()
        {
            var listaDoBanco = await _atividadeService.BuscarAtividadesAsync();
            Atividades.Clear();
            foreach (var atividade in listaDoBanco)
            {
                Atividades.Add(atividade);
            }
        }
    }
}