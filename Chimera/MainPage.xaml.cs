namespace MauiApp6
{
    public partial class MainPage : ContentPage
    {
        

        public MainPage()
        {
            InitializeComponent();
        }

        public async Task OpenMap()
        {
            var location = new Location(30.020831889819476, 31.494954569908185);
            var options = new MapLaunchOptions { Name = "Point 90 Mall", NavigationMode = NavigationMode.Driving };

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
    


