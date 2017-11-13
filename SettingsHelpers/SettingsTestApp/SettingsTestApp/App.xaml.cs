using SettingsHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace SettingsTestApp
{
	public partial class App : Application
	{

        public static SettingsApi SettingApi;

		public App ()
		{
			InitializeComponent();

            EnsureDatabaseExists();

            SettingApi = new SettingsApi("com.company.name");

			MainPage = new SettingsTestApp.MainPage();
		}

        public App(String ConnectionString)
        {
            InitializeComponent();

            SettingsContext.ConnectionString = ConnectionString;

            EnsureDatabaseExists();

            SettingApi = new SettingsApi("com.company.name");

            MainPage = new SettingsTestApp.MainPage();
        }

        private void EnsureDatabaseExists()
        {
            using (var db = new SettingsContext())
            {
                db.Database.EnsureCreated();
            }
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
