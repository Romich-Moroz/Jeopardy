using Autofac;
using Jeopardy.Core.Wpf.Navigation;
using Jeopardy.Desktop.Client.App.Models.Storages;
using Jeopardy.Desktop.Client.App.Viewmodels;
using Jeopardy.Desktop.Client.App.Views;
using System.Windows;

namespace Jeopardy.Desktop.Client.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IContainer Container { get; private set; } = new ContainerBuilder().Build();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var builder = new ContainerBuilder();

            //storages registration
            _ = builder.RegisterType<NavigationStorage>().AsSelf().InstancePerLifetimeScope();
            _ = builder.RegisterType<UserIdentityStorage>().AsSelf().InstancePerLifetimeScope();
            _ = builder.RegisterType<LobbyInfoStorage>().AsSelf().InstancePerLifetimeScope();
            _ = builder.RegisterType<MatchmakerClientStorage>().AsSelf().InstancePerLifetimeScope();

            //services registration
            _ = builder.RegisterGeneric(typeof(NavigationService<>)).AsSelf();

            //viewmodels registration
            _ = builder.RegisterType<MainWindowViewmodel>().AsSelf();
            _ = builder.RegisterType<AuthViewmodel>().AsSelf();
            _ = builder.RegisterType<MainMenuViewmodel>().AsSelf();
            _ = builder.RegisterType<HostGameViewmodel>().AsSelf();
            _ = builder.RegisterType<LobbyBrowserViewmodel>().AsSelf();
            _ = builder.RegisterType<GameLobbyViewmodel>().AsSelf();

            //views registration
            _ = builder.RegisterType<MainWindow>().AsSelf();

            Container = builder.Build();

            //using var scope = Container.BeginLifetimeScope();
            MainWindow? window = Container.Resolve<MainWindow>();
            window.DataContext = Container.Resolve<MainWindowViewmodel>();
            Container.Resolve<NavigationService<AuthViewmodel>>().Navigate();
            window.Show();
        }
    }
}
