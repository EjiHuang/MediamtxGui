using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using EjiLib.Core.Threading;
using MediamtxGui.Controls;
using MediamtxGui.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace MediamtxGui.Core
{
    internal static class DependencyInjection
    {
        public static ServiceProvider Initialize()
        {
            ServiceProvider serviceProvider = new ServiceCollection()

            // Microsoft extension
            //.AddLogging(builder => builder.AddUnconditionalDebug())
            .AddMemoryCache()

            // Extensions


            // Add singleton
            .AddSingleton<App>()
            .AddSingleton<ContentDialogService>()
            .AddSingleton<TaskContext>()
            //.AddSingleton<ConfigFileService>()
            .AddSingleton<MainPageModel>()

            // Discrete services
            .AddSingleton<IMessenger, WeakReferenceMessenger>()

            .BuildServiceProvider(true);

            Ioc.Default.ConfigureServices(serviceProvider);

            return serviceProvider;
        }
    }
}
