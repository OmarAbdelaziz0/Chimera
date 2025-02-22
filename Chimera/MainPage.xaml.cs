namespace MauiApp6
{
    public partial class MainPage : ContentPage
    {
        private readonly MongoDBService _mongoDBService;
        public MainPage()
        {
            InitializeComponent();
            _mongoDBService = new MongoDBService(); // Instantiate the service
        }
        private async void OnInsertButtonClicked(object sender, EventArgs e)
        {
            var email = "@tkh.edu.eg"; 
            var gender = "Male"; 

            // Check if the email is valid
            if (IsValidEmail(email))
            {
                // Check if the gender is either Male or Female
                if (IsValidGender(gender))
                {
                    var person = new PersonModel
                    {
                        FirstName = "Omar",
                        LastName = "Abdelaziz",
                        Age = 50,
                        Email = email,
                        Gender = gender
                    };

                    await _mongoDBService.InsertPersonAsync(person); 
                    await DisplayAlert("Success", "Person inserted into MongoDB.", "OK");
                }
                else
                {
                    await DisplayAlert("Error", "Gender must be either 'Male' or 'Female'.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "Invalid email address. Ensure it ends with @tkh.edu.eg and is correctly formatted.", "OK");
            }
        }

        private bool IsValidEmail(string email)
        {
            // Check if the email ends with @tkh.edu.eg
            if (!email.EndsWith("@tkh.edu.eg"))
            {
                return false;
            }

            // Regex to check if the email format is valid
            var emailRegex = new System.Text.RegularExpressions.Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }

        private bool IsValidGender(string gender)
        {
            // Check if gender is either Male or Female
            return gender.Equals("Male", StringComparison.OrdinalIgnoreCase) || gender.Equals("Female", StringComparison.OrdinalIgnoreCase);
        }

        public async Task OpenMap()
        {
            var location = new Location(29.97202768067485, 31.709378607949088);
            var options = new MapLaunchOptions { Name = "The Knowledge Hub Universities, Coventry University & Nova University Lisbon Branches in Egypt", NavigationMode = NavigationMode.Driving };

            try
            {
                await Map.Default.OpenAsync(location, options);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);// No map application available to open 
            }
        }
        private async void OnOpenMapButtonClicked(object sender, EventArgs e)
        {
            await OpenMap();
        }
    }
}



