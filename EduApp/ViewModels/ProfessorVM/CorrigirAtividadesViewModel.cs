using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using EduApp.Models;
using EduApp.Services;

namespace EduApp.ViewModels.Professor
{
    public class CorrigirAtividadesViewModel
    {
        private readonly AtividadeService _atividadeService;

        public ObservableCollection<HistoricoAtividade> AtividadesPendentes { get; set; }

        public ICommand AprovarCommand { get; }

        public CorrigirAtividadesViewModel()
        {
            _atividadeService = new AtividadeService();
            AtividadesPendentes = new ObservableCollection<HistoricoAtividade>();

            AprovarCommand = new Command<HistoricoAtividade>(async (itemSelecionado) => await ExecutarAprovacao(itemSelecionado));
        }

        public async Task CarregarEntregasPendentesAsync()
        {

            var listaDoBanco = await _atividadeService.BuscarEntregasPendentesAsync();

            AtividadesPendentes.Clear();
            foreach (var item in listaDoBanco)
            {
                AtividadesPendentes.Add(item);
            }
        }

        private async Task ExecutarAprovacao(HistoricoAtividade item)
        {

            bool deuCerto = await _atividadeService.AprovarAtividadeAsync(item.IdHistorico, item.IdAluno, item.Pontos);

            if (deuCerto)
            {

                await Application.Current.MainPage.DisplayAlert("Sucesso!", $"Atividade do aluno aprovada. Pontos creditados!", "OK");

                await CarregarEntregasPendentesAsync();
            }
        }
    }
}