using CommunityToolkit.Mvvm.DependencyInjection;
using MediamtxGui.Controls;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using Windows.Graphics;
using WinRT.Interop;
using static MediamtxGui.Interop.Interop.Comctl32;

namespace MediamtxGui
{
    public sealed partial class MainWindow : Window
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public XamlRoot XamlRoot => Content.XamlRoot;
        private readonly ContentDialogService _contentDialogService;

        public MainWindow()
        {
            InitializeComponent();
            InitializeWindow();

            _contentDialogService = Ioc.Default.GetRequiredService<ContentDialogService>();
        }

        #region Window initialization

        private const string WindowTitle = "Mediamtx gui version 1.0.0";
        private const int StartupWidth = 640;
        private const int StartupHeight = 500;

        private const int WM_CLOSE = 0x0010;
        private SUBCLASSPROC? _subClassDelegate;

        public IntPtr Hwnd { get; private set; }
        private AppWindow? _appWindow;

        private void InitializeWindow()
        {
            Title = WindowTitle;

            Hwnd = WindowNative.GetWindowHandle(this);
            _subClassDelegate = new SUBCLASSPROC(WindowSubClass);
            bool bReturn = SetWindowSubclass(Hwnd, _subClassDelegate, UIntPtr.Zero, UIntPtr.Zero);

            var windowId = Win32Interop.GetWindowIdFromWindow(Hwnd);
            _appWindow = AppWindow.GetFromWindowId(windowId);
            _appWindow.ResizeClient(new SizeInt32(StartupWidth, StartupHeight));
        }

        private IntPtr WindowSubClass(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, UIntPtr uIdSubclass, UIntPtr dwRefData)
        {
            switch (msg)
            {
                case WM_CLOSE:
                    {
                        CloseWindow();
                        return IntPtr.Zero;
                    }
            }

            return DefSubclassProc(hWnd, msg, wParam, lParam);
        }

        private async void CloseWindow()
        {
            var result = await _contentDialogService.ShowYesNoMessageDialog(WindowTitle, "Are you sure you want to close?");
            if (result == Microsoft.UI.Xaml.Controls.ContentDialogResult.Primary)
            {
                logger.Info("App closed normally.");
                Close();
            }
        }

        #endregion
    }
}
