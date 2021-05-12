using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Forms9Patch.Elements.Popups.Core;
using Kit.Forms.Extensions;
using Kit.Forms.Pages;
using ShannonEntropy.Resources;
using Xamarin.Essentials;
using Xamarin.Forms;
using PopupNavigation = Rg.Plugins.Popup.Services.PopupNavigation;

namespace ShannonEntropy.ViewModels
{
    public class RequestCameraPageViewModel
    {
        public ICommand ContinueCommand { get; set; }
        public RequestCameraPageViewModel()
        {
            this.ContinueCommand = new Command(Continue);
        }

        public static bool ShouldAsk()
        {
            if (Permisos.IsDisabled(new Permissions.Camera()))
            {
                Acr.UserDialogs.UserDialogs.Instance.Alert(AppResources.HasDeniedCamera, AppResources.Alert, "Ok");
                return false;
            }
            if (Permisos.IsDisabled(new Permissions.Photos()))
            {
                Acr.UserDialogs.UserDialogs.Instance.Alert(AppResources.HasDeniedPhotos, AppResources.Alert, "Ok");
                return false;
            }

            return true;
        }
        private async void Continue()
        {
            await Permisos.PedirPermiso(new Permissions.Camera(), AppResources.AllowAccess);
            await Permisos.PedirPermiso(new Permissions.Photos(), AppResources.AllowAccess);
            await (PopupNavigation.Instance.PopupStack.First() as BasePopUp)?.Close();
        }
    }
}
