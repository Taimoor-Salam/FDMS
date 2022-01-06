/*Name: TelemetryInfo
 * Description: This class uses ISubjectTelemetry interface. It registers to a channel 
 * and notifies if there is a change to a variable
 * 
 */

using System;
using System.Collections.Generic;


namespace AirCraftTransmissionSys
{
    //interface between TelemetryInfo and IObserver
    interface ISubjectTelemetry
    {
        void Register(IObserver observer);
        void Notify();
    }

    class TelemetryInfo : ISubjectTelemetry
    {
        private List<IObserver> _telemetryObservers;

        private String _tInfo;

        public String telemetryInfo
        {
            get { return _tInfo; }
            set
            {
                _tInfo = value;
                Notify();
            }
        }

        public TelemetryInfo()
        {
            _telemetryObservers = new List<IObserver>();
        }

        //registering via interface
        public void Register(IObserver observer)
        {
            _telemetryObservers.Add(observer);
        }

        //update on notification
        public void Notify()
        {
            _telemetryObservers.ForEach(o =>
            {
                o.Update(this);
            });
        }
    }


}
