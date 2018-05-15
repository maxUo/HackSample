using System;
using System.Windows.Input;
using UnblockHackNET.UI;
using UnblockHackNET.BL.DB;
using System.Threading.Tasks;

namespace UnblockHackNET.BL.ViewModels.Auth
{
    public class AuthViewModel : BaseViewModel
    {
        public ICommand AuthCommand => MakeCommand(async () =>
        {
            //var tmp = PrivateKeyAccount.GenerateSeed();
            //WavesCS.AddressEncoding.
            //var enc = Crypt.Encrypt(tmp, "123");
            ShowLoading();
            await Task.Delay(100);
            //var tmp = await DataBaseService.CreateSeed(EntryText);
            HideLoading();
            NavigationService.Instance.SetMainMasterDetailPage(Pages.Menu, Pages.My);
        });

        public string EntryText
        {
            get => Get("");
            set => Set(value);
        }
    }
}
