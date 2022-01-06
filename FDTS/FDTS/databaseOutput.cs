using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDMS
{
    public class databaseOutput
    {
        private DateTime _timeStamp;
        private int _ID;
        private float _Accel_X;
        private float _Accel_Y;
        private float _Accel_Z;
        private float _Weight;
        private float _Altitude;
        private float _Pitch;
        private float _Bank;
        private string _aircraftTailID;

        // Getters and Setters
        public DateTime TimeStamp
        {
            get { return _timeStamp; }
            set { _timeStamp = value; }
        }

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public float Accel_X
        {
            get { return _Accel_X; }
            set { _Accel_X = value; }
        }

        public float Accel_Y
        {
            get { return _Accel_Y; }   
            set { _Accel_Y = value; }
        }

        public float Accel_Z
        {
            get { return _Accel_Z; }
            set { _Accel_Z = value; }
        }

        public float Weight
        {
            get { return _Weight; }
            set { _Weight = value; }
        }

        public float Altitude
        {
            get { return _Altitude; }
            set { _Altitude = value; }
        }
        
        public float Pitch
        {
            get { return _Pitch; }
            set { _Pitch = value; }
        }

        public float Bank
        {
            get { return _Bank; }
            set { _Bank = value; }
        }

        public string AircraftTailID
        {
            get { return _aircraftTailID; }
            set { _aircraftTailID = value; }
        }

        public databaseOutput(int ID, float accel_x, float accel_y, float accel_z, float weight, float altitude, float pitch, float bank, string aircraftTailID, DateTime timeStamp)
        {
            _ID = ID;
            _Accel_X = accel_x;
            _Accel_Y = accel_y;
            _Accel_Z = accel_z;
            _Weight = weight;
            _Altitude = altitude;
            _Pitch = pitch;
            _Bank = bank;
            _aircraftTailID = aircraftTailID;
            _timeStamp = timeStamp;
        }
    }
}
