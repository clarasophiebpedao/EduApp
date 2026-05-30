using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using EduApp.Models;
using EduApp.Services;

namespace EduApp.ViewModels
{
    public class PainelAdminViewModel
    {
        private readonly UsuarioService _usuarioService;

        // Lista dinâmica que atualiza o XAML na hora em que adicionamos ou removemos alguém
        public ObservableCollection<Usuario> UsuariosPendentes { get; set; }

        // Comandos que os botões do XAML vão acionar
        public ICommand TornarAlunoCommand { get; }
        public ICommand TornarProfessorCommand { get; }

        public PainelAdminViewModel()
        {
            _usuarioService = new UsuarioService();
            UsuariosPendentes = new ObservableCollection<Usuario>();

            // Configura os comandos para escutar qual usuário foi clicado e qual o destino dele
            TornarAlunoCommand = new Command<Usuario>(async (usuario) => await ExecutarAtribuicao(usuario, "Aluno"));
            TornarProfessorCommand = new Command<Usuario>(async (usuario) => await ExecutarAtribuicao(usuario, "Professor"));
        }

        // Método que solicita ao backend a lista de usuários com status 'Pendente'
        public async Task CarregarUsuariosPendentesAsync()
        {
            // NOTA: Seu colega de backend criará esse método BuscarUsuariosPendentesAsync no UsuarioService
            var listaDoBanco = await _usuarioService.BuscarUsuariosPendentesAsync();

            UsuariosPendentes.Clear();
            foreach (var usuario in listaDoBanco)
            {
                UsuariosPendentes.Add(usuario);
            }
        }

        // Processa a aprovação e muda o papel do usuário no MySQL
        private async Task ExecutarAtribuicao(Usuario usuario, string novoPerfil)
        {
            if (usuario == null) return;

            // NOTA: Seu colega de backend criará esse método AtualizarPerfilUsuarioAsync no UsuarioService
            bool deuCerto = await _usuarioService.AtualizarPerfilUsuarioAsync(usuario.Id, novoPerfil);

            if (deuCerto)
            {
                // Alerta de sucesso para o Administrador
                await App.Current.MainPage.DisplayAlert("Sucesso!", $"{usuario.Nome} agora está cadastrado como {novoPerfil}.", "OK");

                // Atualiza a lista na tela (o usuário aprovado some da lista de pendentes)
                await CarregarUsuariosPendentesAsync();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Erro", "Não foi possível atualizar o perfil do usuário.", "OK");
            }
        }
    }
}