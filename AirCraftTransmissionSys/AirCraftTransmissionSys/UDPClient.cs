/* Name: UDPClient.cs   
 * Description: This class represents the UDPClient for FDMS. This Class sends the packet according to the orrect syntax
 */

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Text;
using System.Runtime.InteropServices;

namespace AirCraftTransmissionSys
{
    class UDPClient
    {
        private string ip = "";
        private int port = 5000;
        public string aircraftData="";
        public int PacketSequenceNo = -1;
        PacketData packet = new PacketData();

        //this struct holds, the name, sequence, data and checksum for the packet
        public struct PacketData
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6)]
            public string Name;
            public uint sequence;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 500)]
            public string data;
            public int checkSum;
        }

        //returns current pc IP address
        private IPAddress GetIP()
        {
            IPAddress localIp;
            String myIP = "";
            if (this.ip == "")
            {
                string hostName = Dns.GetHostName(); // Retrieve the Name of HOST  
                // Get the IP  
                IPHostEntry host = Dns.GetHostEntry(hostName);
                foreach(IPAddress ipAdd in host.AddressList)
                {
                    if(ipAdd.AddressFamily.ToString() == "InterNetwork")        //only take the internetwork ipv4 address
                    {
                        myIP = ipAdd.ToString();                        
                        break;
                    }
                }
                localIp = IPAddress.Parse(myIP);
            }
            else
            {
                localIp = IPAddress.Parse(this.ip);
            }
            
            return localIp;
        }

        //this function send the packet
        public bool SendPacket(string flightName, uint sequenceNo, string dataPacket, int checksum)
        {
            packet.Name = flightName;
            packet.sequence = sequenceNo;
            packet.data = dataPacket;
            packet.checkSum = checksum;

            //Console.WriteLine("{0},{1},{2},{3}", packet.Name, packet.sequence, packet.data, packet.checkSum);
            byte[] packetData = getBytes(packet);               //get bytes from packet
            IPEndPoint ep = new IPEndPoint(GetIP(), port);
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            try
            {
                sock.SendTo(packetData, ep);    //sending struct that follows the syntax of the packet
                return true;
            }
            catch(SystemException e)
            {
                return false;
            }
        }

        //this method converts struct to byte array
        public byte[] getBytes(PacketData str)
        {
            int size = Marshal.SizeOf(str);
            byte[] arr = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(str, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }
    }
}