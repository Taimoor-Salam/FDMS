using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDMS
{
    public class searchFilters
    {
        // Accel_X
        private float _upperBound_AccelX;
        public float upperBound_AccelX
        {
            get { return this._upperBound_AccelX; }
            set { this._upperBound_AccelX = value;}
        }
        private float _lowerBound_AccelX;
        public float lowerBound_AccelX
        {
            get { return this._lowerBound_AccelX; }
            set { this._lowerBound_AccelX = value;}
        }

        // Accel_Y
        private float _upperBound_AccelY;
        public float upperBound_AccelY
        {
            get { return this._upperBound_AccelY; }
            set { this._upperBound_AccelY = value;}
        }
        private float _lowerBound_AccelY;
        public float lowerBound_AccelY
        {
            get { return this._lowerBound_AccelY; }
            set { this._lowerBound_AccelY = value;}
        }

        // Accel_Z
        private float _upperBound_AccelZ;
        public float upperBound_AccelZ
        {
            get { return this._upperBound_AccelZ; }
            set { this._upperBound_AccelZ = value;}
        }
        private float _lowerBound_AccelZ;
        public float lowerBound_AccelZ
        {
            get { return this._lowerBound_AccelZ; }
            set { this._lowerBound_AccelZ = value;}
        }

        // Weight
        private float _upperBound_Weight;
        public float upperBound_Weight
        {
            get { return this._upperBound_Weight;}
            set { this._upperBound_Weight = value;}
        }
        private float _lowerBound_Weight;
        public float lowerBound_Weight
        {
            get { return this._lowerBound_Weight;}
            set { this._lowerBound_Weight = value;}
        }

        // Altitude
        private float _upperBound_Altitude;
        public float upperBound_Altitude
        {
            get { return this._upperBound_Altitude;}
            set { this._upperBound_Altitude = value;}
        }
        private float _lowerBound_Altitude;
        public float lowerBound_Altitude
        {
            get { return this._lowerBound_Altitude;}
            set { this._lowerBound_Altitude = value;}
        }

        // Pitch
        private float _upperBound_Pitch;
        public float upperBound_Pitch
        {
            get { return this._upperBound_Pitch;}
            set { this._upperBound_Pitch = value;}
        }
        private float _lowerBound_Pitch;
        public float lowerBound_Pitch
        {
            get { return this._lowerBound_Pitch;}
            set { this._lowerBound_Pitch = value;}
        }

        // Bank
        private float _upperBound_Bank;
        public float upperBound_Bank
        {
            get { return this._upperBound_Bank;}
            set { this._upperBound_Bank = value;}
        }
        private float _lowerBound_Bank;
        public float lowerBound_Bank
        {
            get { return this._lowerBound_Bank;}
            set { this._lowerBound_Bank = value;}
        }
    }
}
