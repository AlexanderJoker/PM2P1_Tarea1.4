using PM2P1_T4.Controller;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PM2P1_T4
{
    public partial class App : Application
    {
        static BaseDatos db;

        public static BaseDatos DBase
        {
            get
            {
                if (db == null)
                {
                    String folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PM2P1_T4.db3");

                    db = new BaseDatos(folderPath);
                }

                return db;
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
