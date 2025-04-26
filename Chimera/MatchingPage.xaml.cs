using Microsoft.Maui.Controls;
using MauiApp6.Models;
using System;

namespace MauiApp6
{
    public partial class MatchingPage : ContentPage
    {
        private readonly PersonModel _user1;
        private readonly PersonModel _user2;

        public MatchingPage(PersonModel user1, PersonModel user2)
        {
            InitializeComponent();
            _user1 = user1;
            _user2 = user2;

            PopulateData();
        }

        private void PopulateData()
        {
            if (_user1.Role == "Rider")
            {
                RiderEmailLabel.Text = $"Email: {_user1.Email}";
                RiderPhoneLabel.Text = $"Phone: {_user1.PhoneNumber}";
                DriverEmailLabel.Text = $"Email: {_user2.Email}";
                DriverPhoneLabel.Text = $"Phone: {_user2.PhoneNumber}";
            }
            else
            {
                DriverEmailLabel.Text = $"Email: {_user1.Email}";
                DriverPhoneLabel.Text = $"Phone: {_user1.PhoneNumber}";
                RiderEmailLabel.Text = $"Email: {_user2.Email}";
                RiderPhoneLabel.Text = $"Phone: {_user2.PhoneNumber}";
            }
        }

        private async void OnBackToDashboardClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(); 
        }
    }
}
