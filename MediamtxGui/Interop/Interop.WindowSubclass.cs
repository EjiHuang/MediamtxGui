using System;
using System.Runtime.InteropServices;

namespace MediamtxGui.Interop
{
    public static partial class Interop
    {
        public static partial class Comctl32
        {
            public delegate IntPtr SUBCLASSPROC(
              IntPtr hWnd,
              int msg,
              IntPtr wParam,
              IntPtr lParam,
              UIntPtr uIdSubclass,
              UIntPtr dwRefData
            );

            [DllImport(Libraries.Comctl32, ExactSpelling = true)]
            public static extern bool SetWindowSubclass(
                IntPtr hWnd,
                SUBCLASSPROC pfnSubclass,
                UIntPtr uIdSubclass,
                UIntPtr dwRefData
            );

            [DllImport(Libraries.Comctl32, ExactSpelling = true)]
            public static extern IntPtr DefSubclassProc(
                IntPtr hWnd,
                int msg,
                IntPtr wParam,
                IntPtr lParam
            );
        }
    }
}
