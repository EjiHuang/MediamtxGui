using MediamtxGui.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace MediamtxGui
{
    public partial class App : Application
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static new App Current => (App)Application.Current;
        public MainWindow MainWindow { get; private set; } = null!;
        public XamlRoot XamlRoot { get; private set; } = null!;
        public ServiceProvider ServicesProvider { get; }

        public App()
        {
            InitializeComponent();

            ServicesProvider = DependencyInjection.Initialize();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
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
