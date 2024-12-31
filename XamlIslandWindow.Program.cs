using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TerraFX.Interop.Windows;
using TerraFX.Interop.WinRT;

using static TerraFX.Interop.Windows.WM;
using static TerraFX.Interop.Windows.WS;
using static TerraFX.Interop.Windows.HWND;
using static TerraFX.Interop.Windows.GWL;
using static TerraFX.Interop.Windows.SWP;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Interop.WinRT.WinRT;
using Windows.UI.Xaml;

namespace XamlIslandsNet9
{
	public partial class XamlIslandWindow
	{
		//private static App _xamlApp = null;
		public bool xamlInit = false;

		public unsafe XamlIslandWindow()
		{
			fixed (char* lpszClassName = "XamlIslandsClass")
			fixed (char* lpWindowName = "Xaml Islands Window")
			{
				WNDCLASSW wc;
				wc.lpfnWndProc = &Program.WndProc;
				wc.hInstance = GetModuleHandleW(null);
				wc.lpszClassName = lpszClassName;
				RegisterClassW(&wc);

				HWND hwnd = CreateWindowExW(WS_EX_NOREDIRECTIONBITMAP, wc.lpszClassName, lpWindowName, WS_OVERLAPPEDWINDOW | WS_VISIBLE, CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT, HWND.NULL, HMENU.NULL, wc.hInstance, null);

				if (hwnd == HWND.NULL) throw new Exception("Failed to create window.");

				OnWindowCreate(hwnd);

				Program.Windows.Add(hwnd, this);
			}
		}

		public void OnWindowCreate(HWND hwnd)
		{
			RoInitialize(RO_INIT_TYPE.RO_INIT_SINGLETHREADED);
			_hwnd = hwnd;
			InitializeXaml();
			xamlInit = true;

			#region new code
			unsafe
			{
				/*
				nint style = GetWindowLongPtr(_hwnd, GWL_STYLE);
				style &= ~(WS_CAPTION | WS_THICKFRAME | WS_MAXIMIZEBOX | WS_MINIMIZEBOX); // Disable resizing and maximize/minimize
				SetWindowLongPtr(_hwnd, GWL_STYLE, style);
				
				*/

				// so window does not appear in taskbar
				// disabled to test window
				/*nint exStyle = GetWindowLongPtr(hWnd, GWL_EXSTYLE);
				exStyle |= WS_EX_TOOLWINDOW; // Ensure it does not appear in the taskbar
				SetWindowLongPtr(hWnd, GWL_EXSTYLE, exStyle);*/

				// Force the window to update its appearance
				// Resize the window to 300x300 and keep it always on top
				//SetWindowPos(hWnd, (HWND)HWND_TOPMOST, 0, 0, 300, 300, SWP_NOMOVE | SWP_FRAMECHANGED);
			}
			#endregion
		}
	}
}
