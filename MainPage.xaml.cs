using System;
using Microsoft.Maui.Controls;

namespace MFA_The_Application
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        // This method will be executed when the 'Register' button is clicked
        private async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegistrationPage());
        }

        // This method will be executed when the 'Login' button is clicked
    }

}
