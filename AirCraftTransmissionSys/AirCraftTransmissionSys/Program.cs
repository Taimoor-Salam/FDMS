/* Name: Program.cs   
 * Description: This class represents the aircraft transmission system. This class read 
 * data from an ASCII Text file and sends the data to the telemetry info and final telemetry info for 
 * distribution. It uses observer design pattern
 * 
 */



using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.IO;

namespace AirCraftTransmissionSys
{
    class Program
    {
        static void Main(string[] args)
        {

            String[] info = Array.Empty<string>();
            TelemetryInfo teleInfo = new TelemetryInfo();

            String tail = "C-GEFC.txt";
            uint sequenceNo = 0;
            String packet = "";
            int checkSum = 1;
            uint count = 1;

            FinalTelemetryInfo fInfo = new FinalTelemetryInfo();

            teleInfo.Register(fInfo);
            UDPClient client = new UDPClient();

            String curDir = Directory.GetCurrentDirectory();
            String filePath = curDir + "/../../../flights/C-GEFC.txt";  //get file path
            //Console.WriteLine(curDir);
            if (File.Exists(filePath))                      //read file if exists
            {
                info = File.ReadAllLines(filePath);
            }
            
            foreach (String information in info)
            {
                
                information.Trim();                         //put string in array
                teleInfo.telemetryInfo = information;
                packet = fInfo.GetfinalTInfo();
                checkSum = fInfo.GetCheckSum();             // get the checkSum
                sequenceNo = count;
                fInfo.ClearInfo();                          
                if(checkSum != 0)
                {
                    client.SendPacket(tail, sequenceNo, packet, checkSum);      //send packet via udpclient
                }       
                
                count++;
                Thread.Sleep(1000);             ///sleep for a second and the send data
            }
            
        }
    }
}
