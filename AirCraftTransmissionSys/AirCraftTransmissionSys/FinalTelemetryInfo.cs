/*Name: FinalTelemetryInfo
 * Description: This class uses IObserver to observe and get notified on changes 
 * 
 * 
 * 
 */

using System;
namespace AirCraftTransmissionSys
{
    interface IObserver
    {
        void Update(ISubjectTelemetry subject);
    }

    class FinalTelemetryInfo : IObserver
    {
        public static int Timestamp = 0;
        public static int AccelX = 1;
        public static int AccelY = 2;
        public static int AccelZ = 3;
        public static int Weight = 4;
        public static int Altitude = 5;
        public static int Pitch = 6;
        public static int Bank = 7;

        public int checkSum = 0;
        private String[] parameters = Array.Empty<string>();
        public string finalTInfo = "";

        //update on notification
        public void Update(ISubjectTelemetry subject)
        {
            if (subject is TelemetryInfo tele)
            {
                SeparateEachValue(tele.telemetryInfo);
                BuildFinalInfo();
                CalculateCheckSum();
            }
        }

        //Each value is seperated from the String info in this method
        private void SeparateEachValue(String info)
        {
            info = info.Remove(info.Length - 1);
            parameters = info.Split(',');
        }

        //Builds the final info for transmission
        private void BuildFinalInfo()
        {
            foreach (String parameter in parameters)
            {
                if(parameter != "")
                {
                    finalTInfo = finalTInfo + parameter.Trim() + ","; //using comma between each parameters
                }
                
            }
        }

        //calculates the checksum using the sum Altitude, Pitch, Bank divided by 3
        private void CalculateCheckSum()
        {
            try
            {
                float alt = float.Parse(parameters[Altitude]);
                float pitch = float.Parse(parameters[Pitch]);
                float bank = float.Parse(parameters[Bank]);
                int checkSum = ((int)alt + (int)pitch + (int)bank) / 3;
                SetCheckSum(checkSum);
            }
            catch (IndexOutOfRangeException)
            {
                return;
            }

        }

        //Get method for check sum
        public int GetCheckSum()
        {
            return checkSum;
        }

        //Set method for check sum
        public void SetCheckSum(int cSum)
        {
            checkSum = cSum;
        }

        //get method for finalTInfo
        public String GetfinalTInfo()
        {
            return finalTInfo;
        }

        //method for clearing info
        public void ClearInfo()
        {
            this.checkSum = 0;
            parameters = Array.Empty<string>();
            finalTInfo = "";
        }
    }

}
