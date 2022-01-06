using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace FDMS
{
    /// <summary>
    /// Interaction logic for LiveData.xaml
    /// </summary>
    public partial class LiveData : Page
    {
        public LiveData()
        {
            InitializeComponent();
            //addToList();
            //liveDisplay.ItemsSource = UDPServer._Code;

        }

        public FDMS.Server.UDPServer server = new FDMS.Server.UDPServer();
        string textBlock = "";
        delegate void Update_list_callback();
        string textBlockName = "";
        float textBlockaccelX;
        float textBlockaccelY;
        float textBlockaccelZ;
        float textBlockweight;
        float textBlockAlt;
        float textBlockpitch;
        float textBlockbank;
        string textBlocktimeStamp = "";
        uint textBlockSequence;
        uint prevTextBlockSequence = 0;
        bool addToListThread = true;


        public void runServer()
        {
            ThreadStart tStart = new ThreadStart(server.Receive);
            Thread t1 = new Thread(tStart);

            ThreadStart tStart2 = new ThreadStart(addToList);
            Thread t2 = new Thread(tStart2);

            if (server.notClosed == true)
            {
                //server.Receive();
                t1.Start();
                t2.Start();

                updateListThread();
            }
            else
            {
                t1.Join();
                t2.Join();
            }


            //String[] row = { tData.Name, tData.sequence.ToString(), tData.timeStamp, tData.pitch.ToString() };

            //t1.Join();
            //t2.Join();
            //t2.Start();
            //t1.Join();
        }

        public void addToList()
        {
            FDMS.Server.UDPServer.telemetryData teleData = new FDMS.Server.UDPServer.telemetryData();

            while (server.notClosed == true)
            {
                teleData = server.printList();

                if (teleData.Name != "")
                {
                    textBlockName = teleData.Name;
                    textBlockaccelX = teleData.accelX;
                    textBlockaccelY = teleData.accelY;
                    textBlockaccelZ = teleData.accelZ;
                    textBlockweight = teleData.weight;
                    textBlockAlt = teleData.Alt;
                    textBlockpitch = teleData.pitch;
                    textBlockbank = teleData.bank;
                    textBlocktimeStamp = teleData.timeStamp;
                    textBlockSequence = teleData.sequence;
                    //textBlock = teleData.Name + teleData.accelX.ToString() + teleData.accelY.ToString() + teleData.accelZ.ToString() + teleData.weight.ToString() + teleData.Alt.ToString() + teleData.pitch.ToString() + teleData.bank.ToString() + teleData.timeStamp + "\n";

                }
            }

        }


        void updateListThread()
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Long_Running();
            }).Start();
        }

        void updateList()
        {
            if (textBlockName != "" && textBlockSequence != prevTextBlockSequence)
            {
                liveDisplay.Items.Insert(0, new FlightDataItem { TailID = textBlockName, accelX = textBlockaccelX, accelY = textBlockaccelY, accelZ = textBlockaccelZ, weight = textBlockweight, altitude = textBlockAlt, pitch = textBlockpitch, bank = textBlockbank, time = textBlocktimeStamp });
                prevTextBlockSequence = textBlockSequence;
            }

            //LiveDataBlock.Text = textBlock + LiveDataBlock.Text;
        }

        void Long_Running()
        {
            int delay_ms = 1000;

            Thread.Sleep(delay_ms);

            while (true)
            {
                Dispatcher.BeginInvoke(new Update_list_callback(updateList));
                Thread.Sleep(delay_ms);
            }



        }
    }
}
