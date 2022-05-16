﻿using Autofac;
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
            builder.RegisterType<NavigationStorage>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<UserIdentityStorage>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<LobbyInfoStorage>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<MatchmakerClientStorage>().AsSelf().InstancePerLifetimeScope();

            //services registration
            builder.RegisterGeneric(typeof(NavigationService<>)).AsSelf();

            //viewmodels registration
            builder.RegisterType<MainWindowViewmodel>().AsSelf();
            builder.RegisterType<AuthViewmodel>().AsSelf();
            builder.RegisterType<MainMenuViewmodel>().AsSelf();
            builder.RegisterType<HostGameViewmodel>().AsSelf();
            builder.RegisterType<LobbyBrowserViewmodel>().AsSelf();
            builder.RegisterType<GameLobbyViewmodel>().AsSelf();

            //views registration
            builder.RegisterType<MainWindow>().AsSelf();

            Container = builder.Build();

            //using var scope = Container.BeginLifetimeScope();
            MainWindow? window = Container.Resolve<MainWindow>();
            window.DataContext = Container.Resolve<MainWindowViewmodel>();
            Container.Resolve<NavigationService<AuthViewmodel>>().Navigate();
            window.Show();
        }
    }
}
