using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp
{
    class Places : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Places()
        {

        }

        //public async Task GetPlaces(string PlacesString)
        //{

        //    await PlacesString;
        //}
    }
}
