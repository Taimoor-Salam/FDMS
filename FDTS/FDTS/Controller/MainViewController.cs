using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDMS.Model;
using FDMS.DAL;
using System.Collections;
using System.Collections.ObjectModel;

namespace FDMS.Controller
{
    class MainViewController
    {
        static private FlightDataDB dB = new FlightDataDB();
        public ObservableCollection<GForceParameter> gForces = new ObservableCollection<GForceParameter>();
        public ObservableCollection<AltitudeParameter> altitudes = new ObservableCollection<AltitudeParameter>(); 

        public MainViewController()
        {

        }

        public void LoadGForces(string aircraftTail)
        {
           gForces = dB.GetGForceParameters(aircraftTail);
        }

        public void LoadAltitudes(string aircraftTail)
        {
            altitudes = dB.GetAltitudeParameters(aircraftTail);
        }


    }
}
