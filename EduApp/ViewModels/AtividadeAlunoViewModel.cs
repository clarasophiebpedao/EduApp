using System.Collections.ObjectModel; 
using System.Threading.Tasks;
using EduApp.Models;   
using EduApp.Services;

namespace EduApp.ViewModels
{
    public class AtividadesAlunoViewModel
    {
        private readonly AtividadeService _atividadeService;

        public ObservableCollection<Atividade> Atividades { get; set; }

        public AtividadesAlunoViewModel()
        {
            _atividadeService = new AtividadeService();
            Atividades = new ObservableCollection<Atividade>();
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