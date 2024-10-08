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
            var location = new Location(47.645160, -122.1306032);
            var options = new MapLaunchOptions { Name = "Microsoft Building 25", NavigationMode = NavigationMode.Driving };

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
            await OpenMap(); // Call the method to open maps
        }
    }
    }
    


