using Xamarin.Forms;
using YoutubeInWebView.Services;

namespace YoutubeInWebView
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DependencyService.RegisterSingleton(new VideoRepository());
            DependencyService.RegisterSingleton<IApiService>(new MockApiService());

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
