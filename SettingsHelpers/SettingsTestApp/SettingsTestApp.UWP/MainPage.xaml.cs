using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace SettingsTestApp.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            ApplicationData.Current.LocalFolder.CreateFileAsync("Settings.db", CreationCollisionOption.OpenIfExists).AsTask().Wait();

            LoadApplication(new SettingsTestApp.App($"Data Source={Path.Combine(ApplicationData.Current.LocalFolder.Path, "Settings.db")}"));
        }
    }
}
