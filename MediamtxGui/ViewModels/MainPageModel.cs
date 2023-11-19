using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using EjiLib.Core.Threading;
using MediamtxGui.Controls;

namespace MediamtxGui.ViewModels
{
    public class MainPageModel : ObservableObject
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly ContentDialogService _contentDialogService;
        private readonly ITaskContext _taskContext;

        public MainPageModel()
        {
            _contentDialogService = Ioc.Default.GetRequiredService<ContentDialogService>();
            _taskContext = Ioc.Default.GetRequiredService<TaskContext>();
        }
    }
}
