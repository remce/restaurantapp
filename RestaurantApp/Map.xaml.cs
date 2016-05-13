using Newtonsoft.Json;
using RestaurantApp.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Windows.Devices.Geolocation;
using Windows.Devices.Enumeration;
using Windows.UI.Xaml.Controls.Maps;
using Windows.Storage.Streams;
using System.Net.Http;


// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace RestaurantApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Map : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private object mapWithMyLocation;

        public Map()
        {
            this.InitializeComponent();
           
            Mymap.ZoomLevel = 10;

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
            //ShowMyLocationOnTheMap();
            PlacePushpins();
            

        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            
            var client = new HttpClient();
            var result = await client.GetStringAsync("https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=50.9161,005.3374&radius=500&types=eat-drinks&key=AIzaSyAqIvsvXvwWYEbFqXcDVUsQE8RUL13Hso4");
            var placesList = JsonConvert.DeserializeObject<RootObject>(result);
            placesList.results = Pushpin();
            
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
          
             
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SettingsPage));
        }
        //private async void OnGetPositionClicked(object sender, RoutedEventArgs e)
        //{
        //    Geolocator geolocator = new Geolocator();
        //    Geoposition position = await geolocator.GetGeopositionAsync();
           
        //}
        //private async void OnAddPushpinClicked(object sender, RoutedEventArgs e)
        //{
         
        //}
        private async void PlacePushpins()
        {
            Geolocator locator = new Geolocator();
            Geoposition geoposition = await locator.GetGeopositionAsync();
            MapIcon icon = new MapIcon();
            icon.Location = geoposition.Coordinate.Point;
            icon.Title = "Your position";
            // icon.Image = RandomAccessStreamReference.CreateFromUri(new Uri("msappx:///Assets/logo.jpg"));
            await Mymap.TrySetViewAsync(geoposition.Coordinate.Point, 10);
            Mymap.MapElements.Add(icon);
        }

        public class Location
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Geometry
        {
            public Location location { get; set; }
        }

        public class Result
        {
            public Geometry geometry { get; set; }
            public string icon { get; set; }
            public string id { get; set; }
            public string name { get; set; }
            public string place_id { get; set; }
            public string reference { get; set; }
            public string scope { get; set; }
            public List<string> types { get; set; }
            public string vicinity { get; set; }
        }

        public class RootObject
        {
            public List<object> html_attributions { get; set; }
            public List<Result> results { get; set; }
            public string status { get; set; }
        }
    }

}
