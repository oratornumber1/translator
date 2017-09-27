using CrossPlatformAppXamarin.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace CrossPlatformAppXamarin
{
    public partial class App : Application
    {
        public const string DATABASE_NAME = "translations.db";
        public static TranslationRepository database;
        public static TranslationRepository Database
        {
            get
            {
                if (database == null)
                {
                    database = new TranslationRepository(DATABASE_NAME);
                }
                return database;
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage =  new NavigationPage(new CrossPlatformAppXamarin.MainPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
