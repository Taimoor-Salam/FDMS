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
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel;
using System.IO;
using System.Reflection;


namespace FDMS
{
    /// <summary>
    /// Interaction logic for Database.xaml
    /// </summary>
    public partial class Database : Page
    {
        searchFilters currentFilters;
        ObservableCollection<databaseOutput> databaseResult;

        const int accelXFilter = 1;
        const int accelYFilter = 2;
        const int accelZFilter = 3;
        const int weightFilter = 4;
        const int altitudeFilter = 5;
        const int pitchFilter = 6;
        const int bankFilter = 7;

        // Default values for filters
        const float upperBound_Accel = 100;
        const float lowerBound_Accel = -100;
        const float upperBound_Weight = 3000;
        const float lowerBound_Weight = 1;
        const float upperBound_Altitude = 4500;
        const float lowerBound_Altitude = 100;
        const float upperBound_Pitch = 3;
        const float lowerBound_Pitch = -3;
        const float upperBound_Bank = 5;
        const float lowerBound_Bank = -5;

        const string connectionString = "Server=tcp:settest.database.windows.net,1433;Initial Catalog=FDMS;Persist Security Info=False;User ID=FlightDataManagment;Password=SENG3020laura!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"; // Temporary Placement of connectionString

        int selectedFilter;
        public Database()
        {
            InitializeComponent();
            currentFilters = new searchFilters();
        }

        private void searchDB_BTN_Click(object sender, RoutedEventArgs e)
        {
            if (leadFilterBox.SelectedIndex == -1)
            {
                // User must select a sorting option
                MessageBox.Show("A sorting filter must be selected.","ERROR: Missing Sorting Filter");
                return;
            }

            // Before making a request to the db, we must check if the ranges for the values are not 0.
            if (currentFilters.upperBound_AccelX == 0 || currentFilters.lowerBound_AccelX == 0)
            {
                // Default values
                currentFilters.upperBound_AccelX = upperBound_Accel;
                currentFilters.lowerBound_AccelX = lowerBound_Accel;
            }

            if (currentFilters.upperBound_AccelY == 0 || currentFilters.lowerBound_AccelY == 0)
            {
                // Default values
                currentFilters.upperBound_AccelY = upperBound_Accel;
                currentFilters.lowerBound_AccelY = lowerBound_Accel;
            }

            if (currentFilters.upperBound_AccelZ == 0 || currentFilters.lowerBound_AccelZ == 0)
            {
                // Default values
                currentFilters.upperBound_AccelZ = upperBound_Accel;
                currentFilters.lowerBound_AccelZ = lowerBound_Accel;
            }

            if (currentFilters.upperBound_Weight == 0 || currentFilters.lowerBound_Weight == 0)
            {
                // Default values
                currentFilters.upperBound_Weight = upperBound_Weight;
                currentFilters.lowerBound_Weight = lowerBound_Weight;
            }
            
            if (currentFilters.upperBound_Altitude == 0 || currentFilters.lowerBound_Altitude == 0)
            {
                // Default values
                currentFilters.upperBound_Altitude = upperBound_Altitude;
                currentFilters.lowerBound_Altitude = lowerBound_Altitude;
            }

            if (currentFilters.upperBound_Pitch == 0 || currentFilters.lowerBound_Pitch == 0)
            {
                // Default values
                currentFilters.upperBound_Pitch = upperBound_Pitch;
                currentFilters.lowerBound_Pitch = lowerBound_Pitch;
            }

            if (currentFilters.upperBound_Bank == 0 || currentFilters.lowerBound_Bank == 0)
            {
                // Default values
                currentFilters.upperBound_Bank = upperBound_Bank;
                currentFilters.lowerBound_Bank = lowerBound_Bank;
            }

            // Now let's make a query to the DB
            databaseResult = new ObservableCollection<databaseOutput>();

            using (var connection = new SqlConnection(connectionString))
            {
                string sqlStatement = @"SELECT [G-Force Parameters].ID, altitude, pitch, bank, accelX, accelY, accelZ, [weight], aircraftTail, [G-Force Parameters].dateCreated 
                                        FROM [G-Force Parameters] INNER JOIN [Altitude Parameters] ON [G-Force Parameters].ID = [Altitude Parameters].ID AND 
                                        [G-Force Parameters].aircraftTailID = [Altitude Parameters].aircraftTailID INNER JOIN [Aircraft Tails] ON ([G-Force Parameters].aircraftTailID = [Aircraft Tails].ID)
                                        WHERE
                                        altitude <= " + currentFilters.upperBound_Altitude + " AND altitude >= " + currentFilters.lowerBound_Altitude + " AND " +
                                        "pitch <= " + currentFilters.upperBound_Pitch + " AND pitch >= " + currentFilters.lowerBound_Pitch + " AND " +
                                        "bank <= " + currentFilters.upperBound_Bank + " AND bank >= " + currentFilters.lowerBound_Bank + " AND " +
                                        "accelX <= " + currentFilters.upperBound_AccelX + " AND accelX >= " + currentFilters.lowerBound_AccelX + " AND " +
                                        "accelY <= " + currentFilters.upperBound_AccelY + " AND accelY >= " + currentFilters.lowerBound_AccelY + " AND " +
                                        "accelZ <= " + currentFilters.upperBound_AccelZ + " AND accelZ >= " + currentFilters.lowerBound_AccelZ + " AND " +
                                        "[weight] <= " + currentFilters.upperBound_Weight + " AND [weight] >= " + currentFilters.lowerBound_Weight;

                SqlCommand command = new SqlCommand(sqlStatement, connection);

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataTable table = new DataTable();

                adapter.Fill(table);

                foreach (DataRow row in table.Rows)
                {
                    databaseResult.Add(new databaseOutput
                    (
                            Convert.ToInt32(row["ID"]),
                            (float)Convert.ToDecimal(row["accelX"]),
                            (float)Convert.ToDecimal(row["accelY"]),
                            (float)Convert.ToDecimal(row["accelZ"]),
                            (float)Convert.ToDecimal(row["weight"]),
                            (float)Convert.ToDecimal(row["altitude"]),
                            (float)Convert.ToDecimal(row["pitch"]),
                            (float)Convert.ToDecimal(row["bank"]),
                            row["aircraftTail"].ToString(),
                            Convert.ToDateTime(row["dateCreated"])
                    ));
                }

                //databaseDisplay.ItemsSource = databaseResult;

                // Organize database by main filter
                
                ICollectionView sortedResult = CollectionViewSource.GetDefaultView(databaseResult);

                using (sortedResult.DeferRefresh())
                {
                    sortedResult.SortDescriptions.Clear();
                    if(leadFilterBox.SelectedIndex == 0)
                    {
                        // Sort by ID
                        sortedResult.SortDescriptions.Add(new SortDescription("ID", 0));

                    }
                    else if(leadFilterBox.SelectedIndex == 1)
                    {
                        // Sort by timeStamps
                        sortedResult.SortDescriptions.Add(new SortDescription("TimeStamp", 0));
                    }
                }

                databaseDisplay.ItemsSource = sortedResult;
                
            }
        }
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = e.OriginalSource as RadioButton;

            if((string)radioButton.Content == "Accel-X")
            {
                selectedFilter = accelXFilter;
            } 
            else if ((string)radioButton.Content == "Accel-Y")
            {
                selectedFilter = accelYFilter;
            } 
            else if ((string)radioButton.Content == "Accel-X")
            {
                selectedFilter = accelZFilter;
            } 
            else if ((string)radioButton.Content == "Weight")
            {
                selectedFilter = weightFilter;
            }
            else if ((string)radioButton.Content == "Altitude")
            {
                selectedFilter = altitudeFilter;
            }
            else if ((string)radioButton.Content == "Pitch")
            {
                selectedFilter = pitchFilter;
            }
            else
            {
                selectedFilter = bankFilter;
            }
        }

        private void confirmRange_BTN_Click(object sender, RoutedEventArgs e)
        {
            if(upperBoundFilter.Text == "" || lowerBoundFilter.Text == "")
            {
                // Ranges cannot be blank
                MessageBox.Show("Range fields cannot be blank.", "ERROR: Missing Range Values");
            } 
            else if(float.Parse(upperBoundFilter.Text) < float.Parse(lowerBoundFilter.Text))
            {
                // upperbound value must be higher than lower bound value
                MessageBox.Show("Upper bound value (Right) must be greater than lower bound value (Left).", "ERROR: Range Invalid");
            }
            else
            {
                if (selectedFilter == accelXFilter)
                {
                    currentFilters.upperBound_AccelX = float.Parse(upperBoundFilter.Text);
                    currentFilters.lowerBound_AccelX = float.Parse(lowerBoundFilter.Text);
                    MessageBox.Show("Accel-x values set.", "Range Set");
                }
                else if (selectedFilter == accelYFilter)
                {
                    currentFilters.upperBound_AccelY = float.Parse(upperBoundFilter.Text);
                    currentFilters.lowerBound_AccelY = float.Parse(lowerBoundFilter.Text);
                    MessageBox.Show("Accel-y values set.", "Range Set");
                }
                else if (selectedFilter == accelZFilter)
                {
                    currentFilters.upperBound_AccelZ = float.Parse(upperBoundFilter.Text);
                    currentFilters.lowerBound_AccelZ = float.Parse(lowerBoundFilter.Text);
                    MessageBox.Show("Accel-z values set.", "Range Set");
                }
                else if (selectedFilter == weightFilter)
                {
                    currentFilters.upperBound_Weight = float.Parse(upperBoundFilter.Text);
                    currentFilters.lowerBound_Weight = float.Parse(lowerBoundFilter.Text);
                    MessageBox.Show("Weight values set.", "Range Set");
                }
                else if (selectedFilter == altitudeFilter)
                {
                    currentFilters.upperBound_Altitude = float.Parse(upperBoundFilter.Text);
                    currentFilters.lowerBound_Altitude = float.Parse(lowerBoundFilter.Text);
                    MessageBox.Show("Altitude values set.", "Range Set");
                }
                else if (selectedFilter == pitchFilter)
                {
                    currentFilters.upperBound_Pitch = float.Parse(upperBoundFilter.Text);
                    currentFilters.lowerBound_Pitch = float.Parse(lowerBoundFilter.Text);
                    MessageBox.Show("Pitch values set.", "Range Set");
                }
                else if (selectedFilter == bankFilter)
                {
                    currentFilters.upperBound_Bank = float.Parse(upperBoundFilter.Text);
                    currentFilters.lowerBound_Bank = float.Parse(lowerBoundFilter.Text);
                    MessageBox.Show("Bank values set.", "Range Set");
                }
                else
                {
                    return;
                }
            }
        }

        private void log_BTN_Click(object sender, RoutedEventArgs e)
        {
            String curDir = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            try
            {
                string logData = "";
                foreach(databaseOutput item in databaseResult)
                {
                    logData += item.ID.ToString() + " " + item.Accel_X.ToString() + " " + item.Accel_Y.ToString() + " " + item.Accel_Z.ToString() + " " + item.Weight.ToString() + " " + item.Altitude.ToString() + " " + item.Pitch.ToString() + " " + item.Bank.ToString() + " " + item.AircraftTailID.ToString() + " " + item.TimeStamp.ToString() + "\n";
                }
                string fileName = "/DATABASE_LOG_" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + ".txt";
                string path = curDir + fileName;

                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(logData);
                }
                //File.WriteAllLines(path, logData, Encoding.UTF8);
                //StreamWriter logFile = new StreamWriter(curDir + fileName);
                
            }
            catch(System.IO.IOException)
            {

            }
        }
    }
}
