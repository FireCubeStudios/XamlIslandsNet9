using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraFX.Interop.Windows;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using static TerraFX.Interop.Windows.Windows;

namespace XamlIslandsNet9
{
	/*
	 * new code by me
	 */
	public partial class XamlIslandWindow : IDisposable
	{
		public Frame WindowFrame;

		public void Dispose()
		{
			Program.Windows.Remove(_hwnd);
			_desktopWindowXamlSource.Dispose();
			_desktopWindowXamlSource = null;

			_coreWindow = null;
			WindowFrame = null;

			FreeLibrary(TwinApiAppCoreHandle);
			FreeLibrary(TwinApiAppCoreHandle);
		}
	}
}
