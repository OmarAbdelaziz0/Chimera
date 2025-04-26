using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using MauiApp6.Models;

namespace MauiApp6
{
    public partial class DashboardPage : ContentPage
    {
        private readonly MongoDBService _mongoDBService;
        private string Username;

        public DashboardPage(string username)
        {
            InitializeComponent();
            _mongoDBService = new MongoDBService();
            Username = username;
            Console.WriteLine($"Logged-in username: {Username}");  // Debugging username
        }

        private async void OnFindRideClicked(object sender, EventArgs e)
        {
            var selectedGender = GenderPicker.SelectedItem?.ToString();
            var selectedRole = RolePicker.SelectedItem?.ToString();

            // Check if Role or Gender is not selected
            if (string.IsNullOrEmpty(selectedRole) || string.IsNullOrEmpty(selectedGender))
            {
                await DisplayAlert("Error", "Please select both your role (Rider/Driver) and gender preference.", "OK");
                return;
            }

            // Debugging selected gender and role
            Console.WriteLine($"Selected Gender: {selectedGender}, Selected Role: {selectedRole}");

            // Get the user from the database
            var user = await _mongoDBService.GetUserByUsernameAsync(Username);

            if (user == null)
            {
                await Task.Delay(3000);
                await DisplayAlert("Error", "User not found.", "OK");
                Console.WriteLine($"Error: User {Username} not found in MongoDB.");
                return;
            }

            Console.WriteLine($"User found in DB: {user.Username}, Role: {user.Role}");

            if (user.Role != selectedRole)
            {
                bool isUpdated = await _mongoDBService.UpdatePersonRoleAsync(Username, selectedRole);
                if (isUpdated)
                {
                    await DisplayAlert("Role Updated", $"Your role has been updated to {selectedRole}.", "OK");
                    Console.WriteLine($"Role updated in DB: {Username} -> {selectedRole}");
                }
                else
                {
                    await DisplayAlert("Update Failed", "Could not update your role. Please try again.", "OK");
                    return;
                }
            }

            var userLocation = await Geolocation.GetLastKnownLocationAsync();
            if (userLocation == null)
            {
                await DisplayAlert("Error", "Unable to fetch location.", "OK");
                return;
            }

            Console.WriteLine($"User location: Lat {userLocation.Latitude}, Lon {userLocation.Longitude}");

            // Add delay before starting the search
            await Task.Delay(3000); // Delays for 3 seconds to simulate waiting for user input or network latency

            // Log the role and gender selected for debugging purposes
            Console.WriteLine($"Searching for {selectedRole} with gender {selectedGender}.");

            // Dijkstra's algorithm approach is conceptually applied here by calculating the distance between the current user
            // and other users. In this simplified version, we're not using a graph structure, but the logic of finding the 
            // "nearest user" based on distance from the current user's location can be seen as a basic application of 
            // shortest-path search, which is at the core of Dijkstra's algorithm.
            var nearestUser = await _mongoDBService.GetNearestUserAsync(
                userLocation.Latitude,
                userLocation.Longitude,
                selectedRole == "Rider" ? "Driver" : "Rider", // Swap role for matching (rider looking for drivers)
                selectedGender
            );

            if (nearestUser != null)
            {
                string message = $"{nearestUser.Username} is the closest match!";
                await DisplayAlert("Match Found", message, "OK");

                // Navigate to the MatchingPage with both the current user and the nearest match
                await Navigation.PushAsync(new MatchingPage(user, nearestUser));
            }
            else
            {
                await DisplayAlert("No Match", "No available rides/drivers found.", "OK");
                Console.WriteLine("No suitable match found.");
            }
        }

        // Calculate distance between two latitude/longitude points (in kilometers)
        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            double R = 6371; // Radius of Earth in km
            double dLat = (lat2 - lat1) * (Math.PI / 180);
            double dLon = (lon2 - lon1) * (Math.PI / 180);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1 * (Math.PI / 180)) * Math.Cos(lat2 * (Math.PI / 180)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c; // Distance in km
        }

        private async void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//MainPage");
        }
    }
}
