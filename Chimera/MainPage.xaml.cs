using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices.Sensors;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MauiApp6.Models;

namespace MauiApp6
{
    public partial class MainPage : ContentPage
    {
        private readonly MongoDBService _mongoDBService;
        private bool isLoginMode = true;

        public MainPage()
        {
            InitializeComponent();
            _mongoDBService = new MongoDBService();
        }

        private async void OnActionButtonClicked(object sender, EventArgs e)
        {
            var usernameOrEmail = UsernameOrEmailEntry.Text;
            var password = PasswordEntry.Text;
            var username = UsernameEntry.Text;
            var email = EmailEntry.Text;
            var gender = GenderEntry.Text;
            var firstName = FirstNameEntry.Text;
            var lastName = LastNameEntry.Text;
            var ageText = AgeEntry.Text;
            var phoneNumber = PhoneNumberEntry.Text; // Capturing Phone Number
            var role = RolePicker.SelectedItem?.ToString(); // Capturing Role
            int age;

            if (isLoginMode)
            {
                // Login Mode: Either Username or Email with Password
                if (string.IsNullOrWhiteSpace(usernameOrEmail) || string.IsNullOrWhiteSpace(password))
                {
                    await DisplayAlert("Error", "Username or Email and Password must be filled.", "OK");
                    return;
                }

                bool isEmailLogin = IsValidEmail(usernameOrEmail);
                bool isUsernameLogin = !isEmailLogin;

                // Validate credentials in the database
                var loginSuccess = await _mongoDBService.ValidateUserAsync(usernameOrEmail, password, isEmailLogin, isUsernameLogin);

                if (loginSuccess)
                {
                    await DisplayAlert("Login", "Logged in successfully.", "OK");

                    // ✅ Navigate to DashboardPage
                    await Navigation.PushAsync(new DashboardPage(usernameOrEmail));
                }
                else
                {
                    await DisplayAlert("Error", "Invalid username/email or password.", "OK");
                }
            }
            else
            {
                // Registration Mode: Separate Username and Email
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) ||
                    string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(ageText) ||
                    string.IsNullOrWhiteSpace(gender) || string.IsNullOrWhiteSpace(phoneNumber) || string.IsNullOrWhiteSpace(role) ||
                    string.IsNullOrWhiteSpace(ConfirmPasswordEntry.Text))
                {
                    await DisplayAlert("Error", "All fields must be filled.", "OK");
                    return;
                }

                if (!int.TryParse(ageText, out age))
                {
                    await DisplayAlert("Error", "Age must be a valid number.", "OK");
                    return;
                }

                if (!IsValidPhoneNumber(phoneNumber))
                {
                    await DisplayAlert("Error", "Invalid phone number format.", "OK");
                    return;
                }

                var confirmPassword = ConfirmPasswordEntry.Text;
                if (password == confirmPassword)
                {
                    if (IsValidEmail(email) && IsValidGender(gender))
                    {
                        var person = new PersonModel
                        {
                            Username = username,
                            FirstName = firstName,
                            LastName = lastName,
                            Age = age,
                            Email = email,
                            Gender = gender,
                            Password = password,
                            PhoneNumber = phoneNumber,
                            Role = role
                        };

                        await _mongoDBService.InsertPersonAsync(person);
                        await DisplayAlert("Registration", "Account created successfully!", "OK");

                        // ✅ Navigate to DashboardPage
                        await Navigation.PushAsync(new DashboardPage(usernameOrEmail));
                    }
                    else
                    {
                        await DisplayAlert("Error", "Invalid email or gender.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Passwords do not match!", "OK");
                }
            }
        }

        private void OnToggleFormButtonClicked(object sender, EventArgs e)
        {
            isLoginMode = !isLoginMode;
            FormTitle.Text = isLoginMode ? "Login" : "Register";
            ActionButton.Text = isLoginMode ? "Login" : "Register";
            ToggleFormButton.Text = isLoginMode ? "Don't have an account? Register" : "Already have an account? Login";

            // Toggle visibility of registration fields
            EmailEntry.IsVisible = !isLoginMode;
            UsernameEntry.IsVisible = !isLoginMode;
            ConfirmPasswordEntry.IsVisible = !isLoginMode;
            GenderEntry.IsVisible = !isLoginMode;
            FirstNameEntry.IsVisible = !isLoginMode;
            LastNameEntry.IsVisible = !isLoginMode;
            AgeEntry.IsVisible = !isLoginMode;
            PhoneNumberEntry.IsVisible = !isLoginMode; // Toggle phone number field
            RolePicker.IsVisible = !isLoginMode; // Toggle role picker

            this.Title = isLoginMode ? "Login" : "Registration";
        }

        private bool IsValidEmail(string email)
        {
            if (!email.EndsWith("@tkh.edu.eg"))
                return false;

            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }

        private bool IsValidGender(string gender)
        {
            return gender.Equals("Male", StringComparison.OrdinalIgnoreCase) || gender.Equals("Female", StringComparison.OrdinalIgnoreCase);
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            var phoneRegex = new Regex(@"^\+?\d{10,15}$"); // Accepts phone numbers with optional '+' and 10-15 digits
            return phoneRegex.IsMatch(phoneNumber);
        }

        public async Task OpenMap()
        {
            var location = new Location(29.97202768067485, 31.709378607949088);
            var options = new MapLaunchOptions { Name = "The Knowledge Hub Universities", NavigationMode = NavigationMode.Driving };

            try
            {
                await Map.Default.OpenAsync(location, options);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async void OnOpenMapButtonClicked(object sender, EventArgs e)
        {
            await OpenMap();
        }
    }
}
