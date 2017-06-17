using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;


namespace App3Lab1
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            onLoad();
            
        }
        MobileServiceClient MobileService = new MobileServiceClient("https://xamarinlab1app.azurewebsites.net/");
        public IMobileServiceSyncTable<User> UserSyncTable { get; set; }

        private async void Button_Clicked(object sender, EventArgs e)
        {

            User newUser = new User();
            newUser.Name = Name.Text;
            newUser.University = University.Text;

            await MobileService.GetTable<User>().InsertAsync(newUser);
        }

        private async void Pushtocloud_Clicked(object sender, EventArgs e)
        {
            await MobileService.SyncContext.PushAsync();
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {

            User newUser = new User();
            newUser.Name = Name.Text;
            newUser.University = University.Text;
            await UserSyncTable.InsertAsync(newUser);
        }

        private async Task<bool> InitLocalStoreAsync()
        {
            if (!MobileService.SyncContext.IsInitialized)
            {
                var store = new MobileServiceSQLiteStore("localstore.db");
                store.DefineTable<User>();
                await MobileService.SyncContext.InitializeAsync(store);
                return true;
            }
            return false;
        }

        private async void onLoad()
        {
            UserSyncTable = MobileService.GetSyncTable<User>();
            await InitLocalStoreAsync();
        }
    }
}
