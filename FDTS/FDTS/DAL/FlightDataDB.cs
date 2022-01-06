 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Data.SqlClient;
using FDMS.Model;
using System.Collections.ObjectModel;

// FILENAME: FlightDataDB.cs
// PROGRAMMERS: Jayson Biswas, Taimoor Salam, Jaydan Zabar, Shaq Purcell
// DATE: November 26, 2021
namespace FDMS.DAL
{
    //  Name:   FlightDataDB
    //  Desc:   The purpose of this class is to facilitate the communication of the database to server application.
    //          The data returned from the database is in the form of models based off the database schema
    class FlightDataDB
    {
        //const string connectionString = "Data Source=DESKTOP-489IL7R;Initial Catalog=FDMS;Integrated Security=True"; // Temporary Placement of connectionString
        const string connectionString = "Server=tcp:settest.database.windows.net,1433;Initial Catalog=FDMS;Persist Security Info=False;User ID=FlightDataManagment;Password=SENG3020laura!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"; // Temporary Placement of connectionString


        // NAME:    GetGForceParameters
        // PARAM:   string aircraftTailID - The id of the aircraft, a unique string
        // DESC:    Connect to the database and retrieve a list of G-Force Parameters with the same id passed in the parameter
        // RET:     gForces - A list of gForces that are listed in the dB
        public ObservableCollection<GForceParameter> GetGForceParameters(string aircraftTailID)
        {
            ObservableCollection<GForceParameter> gForces = new ObservableCollection<GForceParameter>();
            using (var connection = new SqlConnection(connectionString))
            {
                string sqlStatement = @"SELECT [G-Force Parameters].ID, accelX, accelY, accelZ, [weight], aircraftTail, [G-Force Parameters].dateCreated FROM [G-Force Parameters] INNER JOIN [Aircraft Tails] ON ([G-Force Parameters].aircraftTailID = [Aircraft Tails].ID)
                                        WHERE aircraftTail = @aircraftTailID
                                        ORDER BY [G-Force Parameters].ID ASC";

                SqlCommand command = new SqlCommand(sqlStatement, connection);

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                command.Parameters.AddWithValue("@aircraftTailID", aircraftTailID);

                DataTable table = new DataTable();

                adapter.Fill(table);

                foreach(DataRow row in table.Rows)
                {
                    gForces.Add(new GForceParameter
                        (
                            Convert.ToInt32(row["ID"]),
                            (float)Convert.ToDecimal(row["accelX"]),
                            (float)Convert.ToDecimal(row["accelY"]),
                            (float)Convert.ToDecimal(row["accelZ"]),
                            (float)Convert.ToDecimal(row["weight"]),
                            row["aircraftTail"].ToString(),
                            Convert.ToDateTime(row["dateCreated"])
                        ));
                }
            }

            return gForces;
        }

        // NAME:    GetAltitudeParameters
        // PARAM:   string aircraftTailID - The id of the aircraft, a unique string
        // DESC:    Connect to the database and retrieve a list of Altitude Parameters with the same id passed in the parameter
        // RET:     altitudes - A list of altitude parameters
        public ObservableCollection<AltitudeParameter> GetAltitudeParameters(string aircraftTailID)
        {
            ObservableCollection<AltitudeParameter> altitudes = new ObservableCollection<AltitudeParameter>();
            using (var connection = new SqlConnection(connectionString))
            {
                string sqlStatement = @"SELECT [Altitude Parameters].ID, altitude, pitch, bank, aircraftTail, [Altitude Parameters].dateCreated FROM [Altitude Parameters] INNER JOIN [Aircraft Tails] ON ([Altitude Parameters].aircraftTailID = [Aircraft Tails].ID)
                                        WHERE aircraftTail = @aircraftTailID
                                        ORDER BY [Altitude Parameters].ID ASC;";

                SqlCommand command = new SqlCommand(sqlStatement, connection);

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                command.Parameters.AddWithValue("@aircraftTailID", aircraftTailID);

                DataTable table = new DataTable();

                adapter.Fill(table);

                foreach (DataRow row in table.Rows)
                {
                    altitudes.Add(new AltitudeParameter
                        (
                            Convert.ToInt32(row["ID"]),
                            (float)Convert.ToDecimal(row["altitude"]),
                            (float)Convert.ToDecimal(row["pitch"]),
                            (float)Convert.ToDecimal(row["bank"]),
                            row["aircraftTail"].ToString(),
                            Convert.ToDateTime(row["dateCreated"])
                        ));
                }
            }

            return altitudes;
        }

        // NAME:    InsertGForceParameter
        // PARAM:   GForceParameter gForce - The G-Force Parameter that is to be added to the database
        // DESC:    Connect to database and insert a new G-Force parameter
        // RET:     int error - The amount of rows effect by the insert, -1 is an error
        public int InsertGForceParameter(GForceParameter gForce)
        {
            int result = -1;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sqlStatement = @"INSERT INTO [G-Force Parameters] (accelX, accelY, accelZ, [weight], aircraftTailID, dateCreated) 
                                        VALUES (@accelX, @accelY, @accelZ, @weight, (SELECT ID FROM [Aircraft Tails] 
                                        WHERE aircraftTail = @aircraftTailID), @dateCreated)";

                SqlCommand command = new SqlCommand(sqlStatement, connection);

                command.Parameters.AddWithValue("@accelX", gForce.AccelX);
                command.Parameters.AddWithValue("@accelY", gForce.AccelY);
                command.Parameters.AddWithValue("@accelZ", gForce.AccelZ);
                command.Parameters.AddWithValue("@weight", gForce.Weight);
                command.Parameters.AddWithValue("@aircraftTailID", gForce.AircraftTailID);
                command.Parameters.AddWithValue("@dateCreated", gForce.Timestamp);

                result = command.ExecuteNonQuery();
                connection.Close();
            }

            return result;
        }

        // NAME:    InsertAltitudeParameter
        // PARAM:   AltitudeParameter altitude - The altitude parameter to be added to the database
        // DESC:    Connect to database and insert a new Altitude parameter
        // RET:     int error - The amount of rows effect by the insert, -1 is an error
        public int InsertAltitudeParameter(AltitudeParameter altitude)
        {
            int result = -1;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                //string sqlStatement = @"INSERT INTO [Altitude Parameters] (altitude, pitch, bank, aircraftTailID, dateCreated) 
                //VALUES (@altitude, @pitch, @bank, (SELECT ID FROM [Aircraft Tails] 
                //WHERE aircraftTail = @aircraftTailID), @dateCreated)";

                string sqlStatement = @"IF NOT EXISTS(SELECT TOP 1 * FROM [Aircraft Tails] WHERE aircraftTail = @aircraftTailID)
                                        BEGIN INSERT INTO [Aircraft Tails] (aircraftTail,dateCreated) VALUES (@aircraftTailID, @dateCreated)
                                        END INSERT INTO [Altitude Parameters] (altitude, pitch, bank, aircraftTailID, dateCreated)
                                        VALUES (@altitude, @pitch, @bank, (SELECT ID FROM [Aircraft Tails]
                                        WHERE aircraftTail = @aircraftTailID), @dateCreated)";

                SqlCommand command = new SqlCommand(sqlStatement, connection);
                
                command.Parameters.AddWithValue("@altitude", altitude.Altitude);
                command.Parameters.AddWithValue("@pitch", altitude.Pitch);
                command.Parameters.AddWithValue("@bank", altitude.Bank);
                command.Parameters.AddWithValue("@aircraftTailID", altitude.AircraftTailID);
                command.Parameters.AddWithValue("@dateCreated", altitude.Timestamp);
                
                result = command.ExecuteNonQuery();
                connection.Close();
            }

            return result;
        }
    }
}
