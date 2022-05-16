using Jeopardy.Core.Data.Users;
using Jeopardy.Core.Wpf.Commands;
using Jeopardy.Core.Wpf.Converters;
using Jeopardy.Core.Wpf.Navigation;
using Jeopardy.Core.Wpf.Viewmodels;
using Jeopardy.Desktop.Client.App.Models.Storages;
using Microsoft.Win32;
using System;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Jeopardy.Desktop.Client.App.Viewmodels
{
    internal class AuthViewmodel : ViewmodelBase
    {
        private readonly NavigationService<MainMenuViewmodel> _navigationService;
        private readonly UserIdentityStorage _userIdentityStorage;

        public UserIdentity UserIdentity => _userIdentityStorage.CurrentUserIdentity;

        public AuthViewmodel(NavigationService<MainMenuViewmodel> navigationService, UserIdentityStorage userIdentityStorage)
        {
            _navigationService = navigationService;
            _userIdentityStorage = userIdentityStorage;
        }

        public ICommand SetAvatarCommand => new RelayCommand(
            () =>
            {
                OpenFileDialog openFileDialog = new();
                openFileDialog.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg";

                if (openFileDialog.ShowDialog() == true)
                {
                    UserIdentity.Avatar = ByteArrayToBitmapImageConverter.ConvertBack(new BitmapImage(new Uri(openFileDialog.FileName))) ?? Array.Empty<byte>();
                    OnPropertyChanged(nameof(UserIdentity));
                }
            },
            null
        );

        public ICommand UseIdentityCommand => new RelayCommand(
            () =>
            {
                _navigationService.Navigate();
            },
            () => !string.IsNullOrWhiteSpace(UserIdentity.Username)
        );
    }
}
