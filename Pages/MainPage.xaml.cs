using PowerStatus;
using System;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Graphics;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;

namespace XamlIslandsNet9.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
			/* var inkPresenter = MyInkCanvas.InkPresenter;
			 inkPresenter.InputDeviceTypes = Windows.UI.Core.CoreInputDeviceTypes.Mouse |
											 Windows.UI.Core.CoreInputDeviceTypes.Touch |
											 Windows.UI.Core.CoreInputDeviceTypes.Pen;*/

		}
        
        private void PageLoaded(object sender, RoutedEventArgs e)
		{
			var m = new MicaAltBrush();
			m.Kind = (int)BackdropKind.BaseAlt;
			m.Theme = ElementTheme.Default;
			VolumeControl.Background = m;
			MediaNPSMControl.Background = m;
			BatteryControl.Background = m;

			var statusProvider = new PowerStatusProvider();
			var status = statusProvider.GetStatus();
			Debug.WriteLine($"{status}");
			Debug.WriteLine($"{status}");


			/* // We aren't even using Bing tile source, so let's just hide the warning
			 TryHideBingWarning(mapControl);

			 HttpMapTileDataSource dataSource = new("http://c.tile.openstreetmap.org/{zoomlevel}/{x}/{y}.png");

			 MapTileSource tileSource = new(dataSource)
			 {
				 Visible = true,
				 Layer = MapTileLayer.BackgroundReplacement,
				 IsFadingEnabled = false
			 };

			 mapControl.TileSources.Add(tileSource);*/
		}

		private async void Button_Click(object sender, RoutedEventArgs e)
		{
			CoreApplicationView newView = CoreApplication.CreateNewView();
			int newViewId = 0;
			await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
			{
				Frame frame = new Frame();
				Window.Current.Content = frame;
				// You have to activate the window in order to show it later.
				Window.Current.Activate();

				newViewId = ApplicationView.GetForCurrentView().Id;
			});
			bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
		}
		/*
private static void TryHideBingWarning(MapControl mapControl)
{
	var mapGrid = VisualTreeHelper.GetChild(mapControl, 0) as Grid;
	if (mapGrid is not null)
		mapGrid = mapGrid.FindName("MapGrid") as Grid;

	if (mapGrid is not null)
	{
		var mapBrorder = mapGrid.Children.FirstOrDefault(x => x is Border);

		if (mapBrorder is not null)
			mapBrorder.Visibility = Visibility.Collapsed;
	}
}*/
	}
}
