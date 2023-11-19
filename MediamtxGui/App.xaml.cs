using MediamtxGui.Core;
using Microsoft.UI.Xaml;
using System;

namespace MediamtxGui
{
    public partial class App : Application
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static new App Current => (App)Application.Current;
        public MainWindow MainWindow { get; private set; } = null!;
        public XamlRoot XamlRoot { get; private set; } = null!;
        public IServiceProvider ServicesProvider { get; }

        public App()
        {
            InitializeComponent();

            ServicesProvider = DependencyInjection.Initialize();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            MainWindow = new MainWindow();

            ((FrameworkElement)MainWindow.Content).Loaded += static (_, _) =>
            {
                Current.XamlRoot = Current.MainWindow.XamlRoot;
            };

            MainWindow.Activate();

            logger.Info("App started.");
        }
    }
}
