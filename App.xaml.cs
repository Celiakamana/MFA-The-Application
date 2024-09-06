﻿using Microsoft.Maui.Controls;

namespace MFA_The_Application
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Set the main page as the LoginPage wrapped in a NavigationPage
            MainPage = new NavigationPage(new MainPage());
        }
    }
}
