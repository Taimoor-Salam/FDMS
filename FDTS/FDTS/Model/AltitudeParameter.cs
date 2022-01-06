using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// FILENAME: AltitudeParameter.cs
// PROGRAMMERS:  Shaq Purcell, Taimoor Salam, Jayson Biswas, Jaydan Zabar
// DATE: November 26, 2021
namespace FDMS.Model
{

    //  Name:   AltitudeParameter
    //  Desc:   The purpose of this class is to emulate and to define the buisness rules of the altitude records
    //          being added to the database.
    class AltitudeParameter
    {
        private int _ID;
        private float _altitude;
        private float _pitch;
        private float _bank;
        private string _aircraftTailID;
        private DateTime _timestamp;


        public float Altitude
        {
            get
            {
                return _altitude;
            }
            set
            {
                _altitude = value;
            }
        }

        public float Pitch
        {
            get
            {
                return _pitch;
            }
            set
            {
                _pitch = value;
            }
        }

        public float Bank
        {
            get
            {
                return _bank;
            }
            set
            {
                _bank = value;
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

        //  NAME:   AltitudeParameter
        //  PARAMS: int ID
        //          float altitude - airplane altitude
        //          float pitch - airplane pitch
        //          float bank - airplane bank
        //          string aircraftTailID - Unique Carrier ID
        //          DateTime timestamp - the time the object was created
        // DESC:    Create an altitude parameter with params as property values
        // RET:     AltitudeParameter - an altitude parameter
        public AltitudeParameter(int ID, float altitude, float pitch, float bank, string aircraftTailID, DateTime timestamp)
        {
            _ID = ID;
            _altitude = altitude;
            _pitch = pitch;
            _bank = bank;
            _timestamp = timestamp;
            _aircraftTailID = aircraftTailID;
        }

        //  NAME:   AltitudeParameter
        //  PARAMS:
        //          float altitude - airplane altitude
        //          float pitch - airplane pitch
        //          float bank - airplane bank
        //          string aircraftTailID - Unique Carrier ID
        //          DateTime timestamp - the time the object was created
        // DESC:    Create an altitude parameter with params as property values
        // RET:     AltitudeParameter - an altitude parameter
        public AltitudeParameter(float altitude, float pitch, float bank, string aircraftTailID, DateTime timestamp)
        {
            _altitude = altitude;
            _pitch = pitch;
            _bank = bank;
            _timestamp = timestamp;
            _aircraftTailID = aircraftTailID;
        }
    }
}
