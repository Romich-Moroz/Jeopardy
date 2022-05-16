using Jeopardy.Core.Data.Users;
using Jeopardy.Core.Wpf.Converters;
using System;
using System.Windows.Media.Imaging;

namespace Jeopardy.Desktop.Client.App.Models.Storages
{
    internal class UserIdentityStorage
    {
        public UserIdentity CurrentUserIdentity { get; set; } = new UserIdentity(
            "Username",
            ByteArrayToBitmapImageConverter.ConvertBack(new BitmapImage(
                    new Uri("pack://application:,,,/Resources/Images/DefaultAvatar.png")
                ))
        );
    }
}
