using System;
using System.Linq;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Runtime.InteropServices;

namespace FDMS.Server
{

        public class UDPServer
        {
            //public static ObservableCollection<FlightDataItem> _Code = new ObservableCollection<FlightDataItem>();
            //public static ObservableCollection<FlightDataItem> Code { get { return _Code; } }
            public static int Timestamp = 0;
            public static int AccelX = 1;
            public static int AccelY = 2;
            public static int AccelZ = 3;
            public static int Weight = 4;
            public static int Altitude = 5;
            public static int Pitch = 6;
            public static int Bank = 7;
            public bool notClosed = true;
            public struct PacketData
            {
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6)]
                public string Name;
                public uint sequence;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 500)]
                public string data;
                public int checkSum;
            }

            public struct telemetryData
            {
                public string Name;
                public uint sequence;
                public string timeStamp;
                public float accelX;
                public float accelY;
                public float accelZ;
                public float weight;
                public float Alt;
                public float pitch;
                public float bank;
                public int checkSum;
            }
            telemetryData seperatedData = new telemetryData();
            //public List<telemetryData> allData = new List<telemetryData>();
            public static List<PacketData> _receivedData = new List<PacketData>();
            //public volatile bool isEmpty = true;
            public void Receive()
            {
                int recv;
                byte[] data = new byte[5000];


                IPEndPoint ep = new IPEndPoint(IPAddress.Any, 5000);
                Socket sck = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                sck.Bind(ep);

                Console.WriteLine("Listening...");

                IPEndPoint sender = new IPEndPoint(IPAddress.Any, 5000);
                EndPoint tmpRemote = (EndPoint)sender;
                recv = sck.ReceiveFrom(data, ref tmpRemote);
                PacketData dataStruct = fromBytes(data);
                //Console.WriteLine("Message Received from {0}", tmpRemote.ToString());
                //Console.WriteLine(Encoding.ASCII.GetString(data, 0, recv));
                //Console.WriteLine("{0},{1},{2},{3}", dataStruct.Name, dataStruct.sequence, dataStruct.data, dataStruct.checkSum);
                try
                {
                    while (notClosed)
                    {
                        data = new byte[5000];
                        recv = sck.ReceiveFrom(data, ref tmpRemote);
                        dataStruct = fromBytes(data);
                        //Console.WriteLine(Encoding.ASCII.GetString(data, 0, recv));
                        //Console.WriteLine("{0},{1},{2}{3}", dataStruct.Name, dataStruct.sequence, dataStruct.data, dataStruct.checkSum);
                        _receivedData.Add(dataStruct);
                        //printList();

                        //Console.WriteLine("{0},{1},{2},{3}", dataStruct.Name, dataStruct.sequence, dataStruct.data, dataStruct.checkSum);

                        string[] telemetry = dataStruct.data.Split(',');
                        string[] dateTime = telemetry[0].Split(' ');

                        string date = dateTime[0];
                        string time = dateTime[1];
                        float accelX = (float)Convert.ToDecimal(telemetry[1]);
                        float accelY = (float)Convert.ToDecimal(telemetry[2]);
                        float accelZ = (float)Convert.ToDecimal(telemetry[3]);
                        float weight = (float)Convert.ToDecimal(telemetry[4]);
                        float altitude = (float)Convert.ToDecimal(telemetry[5]);
                        float pitch = (float)Convert.ToDecimal(telemetry[6]);
                        float bank = (float)Convert.ToDecimal(telemetry[7]);

                        //MainWindow.Page1.LiveDataBlock.Text = MainWindow.Page1.LiveDataBlock.Text + "TESTING" +  accelX.ToString() + accelY.ToString() + accelZ.ToString() + weight.ToString() + altitude.ToString() + pitch.ToString() + bank.ToString() + time + "\n";
                        int validateChecksum = ((int)bank + (int)pitch + (int)altitude / 3);

                        if (validateChecksum == dataStruct.checkSum)
                        {
                            // Create Altitude and GForce Objects
                            FDMS.Model.AltitudeParameter AltitudeEntry = new Model.AltitudeParameter(altitude, pitch, bank, dataStruct.Name, DateTime.ParseExact(telemetry[0], "d_M_yyyy H:mm:s", System.Globalization.CultureInfo.InvariantCulture));
                            FDMS.Model.GForceParameter GForceEntry = new Model.GForceParameter(accelX, accelY, accelZ, weight, dataStruct.Name, DateTime.ParseExact(telemetry[0], "d_M_yyyy H:mm:s", System.Globalization.CultureInfo.InvariantCulture));

                            // Send objects to database
                            FDMS.DAL.FlightDataDB databaseEntry = new DAL.FlightDataDB();
                            databaseEntry.InsertAltitudeParameter(AltitudeEntry);
                            databaseEntry.InsertGForceParameter(GForceEntry);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Closing Socket...");
                    sck.Close();
                }

                sck.Close();

            }

            public void closeSocket()
            {
                notClosed = false;
            }

            public telemetryData printList()
            {
                CleanStruct();
                if (_receivedData.Count != 0)
                {
                    //Console.WriteLine("{0},{1},{2}{3}", _receivedData[0].Name, _receivedData[0].sequence, _receivedData[0].data, _receivedData[0].checkSum);
                    if (_receivedData[0].data != null)
                    {
                        SeparateData(_receivedData[0]);
                        if (!CheckSum())
                        {
                            //Console.WriteLine("CheckSum did not match");
                        }
                        //Console.WriteLine("{0}", allData[0].accelX);
                        //allData.RemoveAt(0);
                    }

                    _receivedData.RemoveAt(0);

                }

                return seperatedData;
            }

            private void CleanStruct()
            {
                seperatedData.Name = "";
                seperatedData.sequence = 0;
                seperatedData.timeStamp = "";
                seperatedData.accelX = 0;
                seperatedData.accelY = 0;
                seperatedData.accelZ = 0;
                seperatedData.weight = 0;
                seperatedData.Alt = 0;
                seperatedData.pitch = 0;
                seperatedData.bank = 0;
                seperatedData.checkSum = 0;
            }

            private void SeparateData(PacketData recvdData)
            {
                seperatedData.Name = recvdData.Name;
                seperatedData.sequence = recvdData.sequence;

                recvdData.data = recvdData.data.Remove(recvdData.data.Length - 1);
                String[] dataSplit = recvdData.data.Split(',');
                seperatedData.timeStamp = dataSplit[Timestamp];
                seperatedData.accelX = float.Parse(dataSplit[AccelX]);
                seperatedData.accelY = float.Parse(dataSplit[AccelY]);
                seperatedData.accelZ = float.Parse(dataSplit[AccelZ]);
                seperatedData.weight = float.Parse(dataSplit[Weight]);
                seperatedData.Alt = float.Parse(dataSplit[Altitude]);
                seperatedData.pitch = float.Parse(dataSplit[Pitch]);
                seperatedData.bank = float.Parse(dataSplit[Bank]);
                seperatedData.checkSum = recvdData.checkSum;

                //allData.Add(seperatedData);
            }

            private bool CheckSum()
            {
                float cSum = (seperatedData.Alt + seperatedData.pitch + seperatedData.bank) / 3;
                if (seperatedData.checkSum == (int)cSum)
                {
                    return true;
                }
                return false;
            }
            static PacketData fromBytes(byte[] arr)
            {
                PacketData str = new PacketData();

                int size = Marshal.SizeOf(str);
                IntPtr ptr = Marshal.AllocHGlobal(size);

                Marshal.Copy(arr, 0, ptr, size);

                str = (PacketData)Marshal.PtrToStructure(ptr, str.GetType());
                Marshal.FreeHGlobal(ptr);
                return str;
            }


        }
    }
