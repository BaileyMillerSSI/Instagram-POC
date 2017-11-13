using Microsoft.EntityFrameworkCore;
using SettingsHelpers;
using SettingsHelpers.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SettingsTestApp
{
	public partial class MainPage : ContentPage
	{

        Random rng = new Random();
        Timer tim;

		public MainPage()
		{
			InitializeComponent();
		}

        protected override void OnAppearing()
        {
            tim = new Timer(TimeTick, null, 0, (int)TimeSpan.FromSeconds(1).TotalMilliseconds);

            base.OnAppearing();
        }

        private void TimeTick(object state)
        {
            Task.Run(async ()=> 
            {
                await App.SettingApi.AddOrUpdateSettingAsync("Rng", rng.Next(1, 1000).ToString());
                var value = await App.SettingApi.GetSettingAsync("Rng");

                if (value != null)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        SettingFromDatabase.Text = $"Value: {value.Value} Last Updated: {value.LastUpdated.ToString()}";
                    });
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        SettingFromDatabase.Text = $"Setting with Key: {"Rng"} doesn't exist.";
                    });
                }
            });
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await App.SettingApi.AddOrUpdateSettingAsync("Rng", rng.Next(1, 1000).ToString());
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            var value = await App.SettingApi.GetSettingAsync("Rng");

            if (value != null)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    SettingFromDatabase.Text = $"Value: {value.Value} Last Updated: {value.LastUpdated.ToString()}";
                });
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    SettingFromDatabase.Text = $"Setting with Key: {"Rng"} doesn't exist.";
                });
            }
        }

        private async void Button_Clicked_2(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(()=> 
            {
                SettingFromDatabase.Text = String.Empty;
            });

            await App.SettingApi.DeleteSettingAsync("Rng");

        }
    }
}
