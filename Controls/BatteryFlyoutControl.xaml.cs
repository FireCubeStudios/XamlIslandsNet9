using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace XamlIslandsNet9.Controls
{
	public sealed partial class BatteryFlyoutControl : UserControl
	{
		public BatteryFlyoutControl()
		{
			this.InitializeComponent();
			var m = new MicaAltBrush();
			m.Kind = (int)BackdropKind.BaseAlt;
			m.Theme = ElementTheme.Default;
			this.Background = m;
		}
	}
}
