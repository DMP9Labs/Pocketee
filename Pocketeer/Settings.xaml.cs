﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Services.Store;
using Windows.UI;
using Windows.UI.Shell;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Pocketeer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : Page
    {
        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        Random random = new Random();
        bool isPinningAllowed = false;
        bool isPinned = false;
        string currenttheme = "";
        string currentthemesel = "";
        Color themecolor = new Color();

        public Settings()
        {
            this.InitializeComponent();
            MoneyClass.AddAcrylicBrush(grid, null);
        }

        void ResetData()
        {
            localSettings.Values["DoesUserGetMoney"] = null;
            localSettings.Values["WhatDayDoesUserGetMoney"] = null;
            localSettings.Values["HowOftenDoesUserGetMoney"] = null;
            localSettings.Values["HowMuchMoneyDoesUserGet"] = null;
            localSettings.Values["HowMuchMoneyDoesUserHave"] = null;
            localSettings.Values["WhenMoneyNeedsGoingIn"] = null;
            localSettings.Values["SetupNeeded"] = null;
            MoneyClass.ResetItems();
            var newWindow = Window.Current;
            var newAppView = ApplicationView.GetForCurrentView();
            var frame = new Frame();
            frame.Navigate(typeof(MainPage), null);
            newWindow.Content = frame;
            ResetDataButton.Flyout.Hide();
            newWindow.Activate();
        }

        BitmapImage White = new BitmapImage(new Uri("ms-appx:///Assets/white-icon.png"));
        BitmapImage Black = new BitmapImage(new Uri("ms-appx:///Assets/black-icon.png"));
        BitmapImage White_Github = new BitmapImage(new Uri("ms-appx:///Assets/github(white)-icon.png"));
        BitmapImage Black_Github = new BitmapImage(new Uri("ms-appx:///Assets/github(black)-icon.png"));
        BitmapImage White_Discord = new BitmapImage(new Uri("ms-appx:///Assets/Discord-Logo-White.png"));
        BitmapImage Black_Discord = new BitmapImage(new Uri("ms-appx:///Assets/Discord-Logo-Black.png"));

        private async void grid_Loading(FrameworkElement sender, object args)
        {
            if (MoneyClass.DoesAcrylicBrushWorks)
            {
                CustomThemeRadioButton.Visibility = Visibility.Visible;
            }
            if (ApiInformation.IsTypePresent("Windows.UI.Shell.TaskbarManager"))
            {
                isPinningAllowed = TaskbarManager.GetDefault().IsPinningAllowed;
                isPinned = await TaskbarManager.GetDefault().IsCurrentAppPinnedAsync();

                if (!isPinned)
                {
                    PinToTaskBar.Visibility = Visibility.Visible;
                }
            }
            if (random.Next(100) >= 90)
            {
                if (random.Next(2) == 1)
                {
                    InfomationTextBlock.Text = "Savings account for dummies - Discord server i'm in";
                }
                else
                {
                    InfomationTextBlock.Text = "Respect the setup screen - Dylan 2017";
                }
            }
            Object RequestedThemeInfo = localSettings.Values["RequestedTheme"];
            var IsThemeDark = App.Current.RequestedTheme == ApplicationTheme.Dark;
            if (IsThemeDark)
            {
                AppIcon.Source = White;
                GithubIcon.Source = White_Github;
                DiscordIcon.Source = White_Discord;
            }
            else
            {
                AppIcon.Source = Black;
                GithubIcon.Source = Black_Github;
                DiscordIcon.Source = Black_Discord;
            }
            if (localSettings.Values["CustomEnabled"] != null)
            {
                currenttheme = "Custom";
                CustomThemeRadioButton.IsChecked = true;
            }
            else if (RequestedThemeInfo == null || RequestedThemeInfo.ToString() == "FromUsersSettings")
            {
                currenttheme = "FromUsersSettings";
                UseSystemThemeRadioButton.IsChecked = true;
            }
            else if (RequestedThemeInfo.ToString() == "Dark")
            {
                currenttheme = "Dark";
                DarkRadioButton.IsChecked = true;
            }
            else if (RequestedThemeInfo.ToString() == "Light")
            {
                currenttheme = "Light";
                LightRadioButton.IsChecked = true;
            }
            currentthemesel = currenttheme;
            if (!(localSettings.Values["Currency"] == null))
            {
                CurrencyChoose.SelectedIndex = Convert.ToInt32(localSettings.Values["Currency"].ToString());
            }

            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;
            AppVersionTextBlock.Text = version.Major + "." + version.Minor + "." + version.Build + "." + version.Revision;

            StoreContext context = StoreContext.GetDefault();
            StoreAppLicense appLicense = await context.GetAppLicenseAsync();
            foreach (KeyValuePair<string, StoreLicense> item in appLicense.AddOnLicenses)
            {
                StoreLicense addOnLicense = item.Value;
                if (addOnLicense.InAppOfferToken == "RemoveAdvertsInPocketeer")
                {
                    if (!addOnLicense.IsActive)
                    {
                        RemoveAdsButton.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private void UseSystemThemeRadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (UseSystemThemeRadioButton.IsChecked == true)
            {
                localSettings.Values["RequestedTheme"] = "FromUsersSettings";
                localSettings.Values["CustomEnabled"] = null;
                currentthemesel = "FromUsersSettings";
                if (currenttheme == "FromUsersSettings")
                {
                    apprestarttextblock.Visibility = Visibility.Collapsed;
                }
                else
                {
                    apprestarttextblock.Visibility = Visibility.Visible;
                }
            }
        }

        private void DarkRadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (DarkRadioButton.IsChecked == true)
            {
                localSettings.Values["RequestedTheme"] = "Dark";
                localSettings.Values["CustomEnabled"] = null;
                currentthemesel = "Dark";
                if (currenttheme == "Dark")
                {
                    apprestarttextblock.Visibility = Visibility.Collapsed;
                }
                else
                {
                    apprestarttextblock.Visibility = Visibility.Visible;
                }
            }
        }

        private void LightRadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (LightRadioButton.IsChecked == true)
            {
                localSettings.Values["RequestedTheme"] = "Light";
                localSettings.Values["CustomEnabled"] = null;
                currentthemesel = "Light";
                if (currenttheme == "Light")
                {
                    apprestarttextblock.Visibility = Visibility.Collapsed;
                }
                else
                {
                    apprestarttextblock.Visibility = Visibility.Visible;
                }
            }
        }

        private async void YesButton_Click(object sender, RoutedEventArgs e)
        {
            await MoneyClass.MakeBackupFile();
            ResetData();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            ResetData();
        }

        private async void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            await MoneyClass.Restore();
            if (localSettings.Values["Currency"] != null)
            {
                CurrencyChoose.SelectedIndex = Convert.ToInt32(localSettings.Values["Currency"].ToString());
            }
        }

        private async void BackupButton_Click(object sender, RoutedEventArgs e)
        {
            await MoneyClass.MakeBackupFile();
        }

        private async void RemoveAdsButton_Click(object sender, RoutedEventArgs e)
        {
            string RemoveAdsText = RemoveAdsButton.Content.ToString();
            StoreContext context = null;
            if (context == null)
            {
                context = StoreContext.GetDefault();
            }
            StorePurchaseResult result = await context.RequestPurchaseAsync("9MWQLSHK3VMV");

            string extendedError = string.Empty;
            if (result.ExtendedError != null)
            {
                extendedError = result.ExtendedError.Message;
            }

            switch (result.Status)
            {
                case StorePurchaseStatus.AlreadyPurchased:
                    {
                        RemoveAdsButton.Content = "You already got this.";
                        MoneyClass.ShowAds = true;
                        break;
                    }
                case StorePurchaseStatus.Succeeded:
                    {
                        RemoveAdsButton.Content = "The purchase was successful!";
                        MoneyClass.ShowAds = true;
                        break;
                    }
                case StorePurchaseStatus.NotPurchased:
                    {
                        RemoveAdsButton.Content = "The purchase did not complete. You may have cancelled the purchase.";
                        break;
                    }
                case StorePurchaseStatus.NetworkError:
                    {
                        RemoveAdsButton.Content = "The purchase was unsuccessful due to a network error.";
                        break;
                    }
                case StorePurchaseStatus.ServerError:
                    {
                        RemoveAdsButton.Content = "The purchase was unsuccessful due to a server error.";
                        break;
                    }
                default:
                    {
                        RemoveAdsButton.Content = "The purchase was unsuccessful due to an unknown error.";
                        break;
                    }
            }
            await Task.Delay(1000 * 5);
            if (result.Status == StorePurchaseStatus.Succeeded || result.Status == StorePurchaseStatus.AlreadyPurchased)
            {
                RemoveAdsButton.Visibility = Visibility.Collapsed;
            }
            RemoveAdsButton.Content = RemoveAdsText;
        }

        private void grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Height < 454)
            {
                AppVersionTextBlock.Margin = new Thickness(0,0,20,0);
            }
            else
            {
                AppVersionTextBlock.Margin = new Thickness(0,0,0,0);
            }
        }

        private void CurrencyChoose_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            localSettings.Values["Currency"] = CurrencyChoose.SelectedIndex;
        }

        private async void PinToTaskBar_Click(object sender, RoutedEventArgs e)
        {
            if (isPinningAllowed && !isPinned)
            {
                bool isPinnedDia = await TaskbarManager.GetDefault().RequestPinCurrentAppAsync();

                if(isPinnedDia)
                {
                    PinToTaskBar.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void CustomThemeRadioButton_Click(object sender, RoutedEventArgs e)
        {
            Flyout flyout = new Flyout();
            ColorPicker colorPicker = new ColorPicker();
            StackPanel stackPanel = new StackPanel();
            StackPanel stackPanel2 = new StackPanel();
            Button OkButton = new Button();
            Button CancelButton = new Button();

            OkButton.Content = "Ok";
            OkButton.Margin = new Thickness(0,5,5,0);
            CancelButton.Margin = new Thickness(0,5,0,0);
            CancelButton.Content = "Cancel";

            stackPanel2.Orientation = Orientation.Horizontal;
            stackPanel2.Children.Add(OkButton);
            stackPanel2.Children.Add(CancelButton);
            stackPanel.Children.Add(colorPicker);
            stackPanel.Children.Add(stackPanel2);

            colorPicker.IsAlphaEnabled = false;
            colorPicker.IsAlphaSliderVisible = false;
            colorPicker.IsAlphaTextInputVisible = false;
            colorPicker.IsColorPreviewVisible = true;
            colorPicker.IsHexInputVisible = false;
            colorPicker.IsMoreButtonVisible = true;
            colorPicker.MinHue = 5;
            flyout.Content = stackPanel;

            OkButton.Click += new RoutedEventHandler((sender1, e1) => OkButton_Click(sender1, e1, colorPicker, OkButton, flyout));
            CancelButton.Click += new RoutedEventHandler((sender1, e1) => CancelButton_Click(sender1, e1, flyout));

            flyout.ShowAt(ResetDataButton);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e, Flyout flyout)
        {
            CustomThemeRadioButton.IsChecked = false;
            if (currentthemesel == "Light")
            {
                LightRadioButton.IsChecked = true;
            }
            else if (currentthemesel == "Dark")
            {
                DarkRadioButton.IsChecked = true;
            }
            else if (currentthemesel == "FromUsersSettings")
            {
                UseSystemThemeRadioButton.IsChecked = true;
            }
            else
            {
                CustomThemeRadioButton.IsChecked = true;
            }
            flyout.Hide();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e, ColorPicker colorPicker, Button button, Flyout flyout)
        { 
            themecolor = colorPicker.Color;
            localSettings.Values["CustomA"] = themecolor.A;
            localSettings.Values["CustomR"] = themecolor.R;
            localSettings.Values["CustomG"] = themecolor.G;
            localSettings.Values["CustomB"] = themecolor.B;
            localSettings.Values["CustomEnabled"] = "true";
            flyout.Hide();
            apprestarttextblock.Visibility = Visibility.Visible;
        }
    }
}
