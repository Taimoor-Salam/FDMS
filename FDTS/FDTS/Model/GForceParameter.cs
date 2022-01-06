using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// FILENAME: GForceParameter.cs
// PROGRAMMERS: Jayson Biswas, Taimoor Salam, Jaydan Zabar, Shaq Purcell
// DATE: November 26, 2021
namespace FDMS.Model
{
    //  Name:   GForceParameter
    //  Desc:   The purpose of this class is to emulate and to define the buisness rules of the GForce records
    //          being added to the database.
    class GForceParameter
    {
        private int _ID;
        private float _accelX;
        private float _accelY;
        private float _accelZ;
        private float _weight;
        private string _aircraftTailID;
        private DateTime _timestamp;


        public float AccelX
        {
            get
            {
                return _accelX;
            }
            set
            {
                _accelX = value;
            }
        }

        public float AccelY
        {
            get
            {
                return _accelY;
            }
            set
            {
                _accelY = value;
            }
        }

        public float AccelZ
        {
            get
            {
                return _accelZ;
            }
            set
            {
                _accelZ = value;
            }
        }

        public float Weight
        {
            get
            {
                return _weight;
            }
            set
            {
                _weight = value;
            }
        }

        public string AircraftTailID
        {
            get
            {
                return _aircraftTailID;
            }
        }

        public DateTime Timestamp
        {
            get
            {
                return _timestamp;
            }
        }

        public int ID
        {
            get
            {
                return _ID;
            }
        }


        //  NAME:   GForceParameter
        //  PARAMS: int ID
        //          float accelX - airplane acceleration X
        //          float accelY - airplane acceleration Y
        //          float accelZ - airplane acceleration Z
        //          float weight - airplace weight
        //          string aircraftTailID - Unique Carrier ID
        //          DateTime timestamp - the time the object was created
        // DESC:    Create an GForce parameter with params as property values
        // RET:     GForceParameter - an altitude parameter
        public GForceParameter(int ID, float accelX, float accelY, float accelZ, float weight, string aircraftTailID, DateTime timestamp)
        {
            _ID = ID;
            _accelX = accelX;
            _accelY = accelY;
            _accelZ = accelZ;
            _weight = weight;
            _aircraftTailID = aircraftTailID;
            _timestamp = timestamp;
        }

        //  NAME:   GForceParameter
        //  PARAMS:
        //          float accelX - airplane acceleration X
        //          float accelY - airplane acceleration Y
        //          float accelZ - airplane acceleration Z
        //          float weight - airplace weight
        //          string aircraftTailID - Unique Carrier ID
        //          DateTime timestamp - the time the object was created
        // DESC:    Create an GForce parameter with params as property values
        // RET:     GForceParameter - an altitude parameter
        public GForceParameter(float accelX, float accelY, float accelZ, float weight, string aircraftTailID, DateTime timestamp)
        {
            _accelX = accelX;
            _accelY = accelY;
            _accelZ = accelZ;
            _weight = weight;
            _aircraftTailID = aircraftTailID;
            _timestamp = timestamp;
        }
    }
}
