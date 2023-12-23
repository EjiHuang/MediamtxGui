using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

namespace MediamtxGui.Controls
{
    [TemplatePart(Name = PartSwapChainPanelName, Type = typeof(SwapChainPanel))]
    public abstract class VideoViewBase : Control
    {
        private const string PartSwapChainPanelName = "SwapChainPanel";

        SwapChainPanel? _panel;
        SharpDX.Direct3D11.Device? _d3D11Device;
        SharpDX.DXGI.Device3? _device3;
        SwapChain2? _swapChain2;
        SwapChain1? _swapChain;
        DeviceContext? _deviceContext;

        bool _loaded;

        public VideoViewBase()
        {
            DefaultStyleKey = typeof(VideoViewBase);

            Unloaded += (s, e) => DestroySwapChain();
        }

        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call ApplyTemplate. 
        /// In simplest terms, this means the method is called just before a UI element displays in your app.
        /// Override this method to influence the default post-template logic of a class.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _panel = (SwapChainPanel)GetTemplateChild(PartSwapChainPanelName);

            DestroySwapChain();

            _panel.SizeChanged += (s, eventArgs) =>
            {
                if (_loaded)
                {
                    //UpdateSize();
                }
                else
                {
                    //CreateSwapChain();
                }
            };

            _panel.CompositionScaleChanged += (s, eventArgs) =>
            {
                if (_loaded)
                {
                    //UpdateScale();
                }
            };
        }

        /// <summary>
        /// Clears the current view restoring the initial visual state.
        /// This is a LibVLCSharp UWP-specific workaround for the following issue: https://code.videolan.org/videolan/vlc/-/issues/23667 
        /// </summary>
        public void Clear()
        {
            if (_loaded && _swapChain is not null && _deviceContext is not null)
            {
                using var backBuffer = _swapChain.GetBackBuffer<Texture2D>(0);
                using var target = new RenderTargetView(_d3D11Device, backBuffer);

                _deviceContext.ClearRenderTargetView(target, new RawColor4(0, 0, 0, 0));
                _swapChain.Present(0, PresentFlags.None);
            }
        }

        /// <summary>
        /// Gets the swapchain parameters to pass to the <see cref="LibVLC"/> constructor.
        /// If you don't pass them to the <see cref="LibVLC"/> constructor, the video won't
        /// be displayed in your application.
        /// Calling this property will throw an <see cref="InvalidOperationException"/> if the VideoView is not yet full Loaded.
        /// </summary>
        /// <returns>The list of arguments to be given to the <see cref="LibVLC"/> constructor.</returns>
        public string[] SwapChainOptions
        {
            get
            {
                if (!_loaded)
                {
                    throw new InvalidOperationException("You must wait for the VideoView to be loaded before calling GetSwapChainOptions()");
                }

                _deviceContext = _d3D11Device!.ImmediateContext;

                return new string[]
                {
                    $"--winrt-d3dcontext=0x{_deviceContext.NativePointer.ToString("x")}",
                    $"--winrt-swapchain=0x{_swapChain!.NativePointer.ToString("x")}"
                };
            }
        }

        /// <summary>
        /// Called when the video view is fully loaded
        /// </summary>
        protected abstract void OnInitialized();

        /// <summary>
        /// Initializes the SwapChain
        /// </summary>
        void CreateSwapChain()
        {
            // Do not create the swapchain when the VideoView is collapsed.
            if (_panel == null || _panel.ActualHeight == 0)
                return;

            SharpDX.DXGI.Factory2? dxgiFactory = null;
            try
            {

            }
            catch (Exception ex)
            {
                DestroySwapChain();
                if (ex is SharpDXException)
                {
                    throw new Exception("SharpDX operation failed, see InnerException for details", ex);
                }

                throw;
            }
        }

        private void DestroySwapChain()
        {
            _swapChain2?.Dispose();
            _swapChain2 = null;

            _device3?.Dispose();
            _device3 = null;

            if (_panel != null)
            {
                using (var panelNative = ComObject.As<ISwapChainPanelNative>(_panel))
                {
                    panelNative.SwapChain = null;
                }
            }

            _swapChain?.Dispose();
            _swapChain = null;

            _deviceContext?.Dispose();
            _deviceContext = null;

            _d3D11Device?.Dispose();
            _d3D11Device = null;

            _loaded = false;
        }
    }
}
