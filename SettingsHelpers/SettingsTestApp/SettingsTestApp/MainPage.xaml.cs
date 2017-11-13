using Microsoft.EntityFrameworkCore;
using SettingsHelpers;
using SettingsHelpers.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SettingsTestApp
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}
        

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await App.SettingApi.AddOrUpdateSettingAsync("LastName","Miller");
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            var value = await App.SettingApi.GetSettingAsync("LastName");

            Device.BeginInvokeOnMainThread(()=> 
            {
                SettingFromDatabase.Text = value.Value;
            });
        }
    }
}
