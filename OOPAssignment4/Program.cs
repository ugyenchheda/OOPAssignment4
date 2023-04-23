using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPAssignment4
{
    class Dentist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TelNum { get; set; }
    }

    class Dentists
    {
        public List<Dentist> GetAllDentists(SqlConnection connection)
        {
            List<Dentist> dentists = new List<Dentist>();

            using (SqlCommand command = new SqlCommand("SELECT * FROM Dentist", connection))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    dentists.Add(new Dentist
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        TelNum = reader.GetString(2)
                    });
                }
            }

            return dentists;
        }

        public void InsertDentist(SqlConnection connection, Dentist dentist)
        {
            using (SqlCommand command = new SqlCommand("INSERT INTO Dentist (Name, TelNum) VALUES (@name, @telNum)", connection))
            {
                command.Parameters.AddWithValue("@name", dentist.Name);
                command.Parameters.AddWithValue("@telNum", dentist.TelNum);
                command.ExecuteNonQuery();
            }
        }

        public List<Dentist> FindDentist(SqlConnection connection, string name)
        {
            List<Dentist> dentists = new List<Dentist>();

            using (SqlCommand command = new SqlCommand("SELECT * FROM Dentist WHERE Name LIKE '%' + @name + '%'", connection))
            {
                command.Parameters.AddWithValue("@name", name);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dentists.Add(new Dentist
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            TelNum = reader.GetString(2)
                        });
                    }
                }
            }

            return dentists;
        }

        public void UpdateDentist(SqlConnection connection, Dentist dentist)
        {
            using (SqlCommand command = new SqlCommand("UPDATE Dentist SET Name = @name, TelNum = @telNum WHERE Id = @id", connection))
            {
                command.Parameters.AddWithValue("@name", dentist.Name);
                command.Parameters.AddWithValue("@telNum", dentist.TelNum);
                command.Parameters.AddWithValue("@id", dentist.Id);
                command.ExecuteNonQuery();
            }
        }
    }
    public void DeleteDentist(SqlConnection myConnection, Dentist dentist)
    {
        string deleteDentist = "DELETE FROM Dentist WHERE Id = @Id";

        using (SqlCommand command = new SqlCommand(deleteDentist, myConnection))
        {
            command.Parameters.AddWithValue("@Id", dentist.Id);

            command.ExecuteNonQuery();
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=Dentist;Integrated Security=true;";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                // Retrieve all dentists from the database
                Dentists dentists = new Dentists();
                List<Dentist> allDentists = dentists.GetAllDentists(sqlConnection);

                while (true)
                {
                    Console.WriteLine("Enter a number to select an action:");
                    Console.WriteLine("1. View all dentists");
                    Console.WriteLine("2. Insert a new dentist");
                    Console.WriteLine("3. Find dentists by name");
                    Console.WriteLine("4. Update a dentist's information");
                    Console.WriteLine("5. Delete Dentist and information:");
                    Console.WriteLine("6. Exit");

                    string input = Console.ReadLine();

                    switch (input)
                    {
                        case "1":
                            // View all dentists
                            foreach (Dentist dentist in allDentists)
                            {
                                Console.WriteLine("{0}: {1} ({2})", dentist.Id, dentist.Name, dentist.TelNum);
                            }
                            break;

                        case "2":
                            // Insert a new dentist
                            Console.Write("Enter the name of the new dentist: ");
                            string name = Console.ReadLine();
                            Console.Write("Enter the telephone number of the new dentist: ");
                            string telNum = Console.ReadLine();
                            Dentist newDentist = new Dentist { Name = name, TelNum = telNum };
                            dentists.InsertDentist(sqlConnection, newDentist);
                            Console.WriteLine("New dentist inserted into database.");
                            foreach (Dentist dentist in allDentists)
                            {
                                Console.WriteLine("{0}: {1} ({2})", dentist.Id, dentist.Name, dentist.TelNum);
                            }
                            break;

                        case "3":
                            // Find dentists by name
                            Console.Write("Enter the name of the dentist to find: ");
                            string searchName = Console.ReadLine();
                            List<Dentist> foundDentists = dentists.FindDentist(sqlConnection, searchName);
                            foreach (Dentist dentist in foundDentists)
                            {
                                Console.WriteLine("{0}: {1} ({2})", dentist.Id, dentist.Name, dentist.TelNum);
                            }
                            break;

                        case "4":
                            // Update a dentist's information
                            Console.Write("Enter the ID of the dentist to update: ");
                            int idToUpdate = int.Parse(Console.ReadLine());
                            Dentist dentistToUpdate = allDentists.FirstOrDefault(d => d.Id == idToUpdate);
                            if (dentistToUpdate == null)
                            {
                                Console.WriteLine("Dentist with ID {0} not found.", 4);
                            }
                            else
                            {
                                Console.Write("Enter the new name of the dentist: ");
                                string newName = Console.ReadLine();
                                Console.Write("Enter the new telephone number of the dentist: ");
                                string newTelNum = Console.ReadLine();
                                dentistToUpdate.Name = newName;
                                dentistToUpdate.TelNum = newTelNum;
                                dentists.UpdateDentist(sqlConnection, dentistToUpdate);
                                Console.WriteLine("Dentist information updated in database.");
                            }
                            break;

                        case "5":
                            // Delete a dentist by ID
                            Console.Write("Enter the ID of the dentist to delete: ");
                            int idToDelete = int.Parse(Console.ReadLine());
                            Dentist dentistToDelete = allDentists.FirstOrDefault(d => d.Id == idToDelete);
                            if (dentistToDelete == null)
                            {
                                Console.WriteLine("Dentist with ID {0} not found.", idToDelete);
                            }
                            else
                            {
                                dentists.DeleteDentist(sqlConnection, dentistToDelete);
                                Console.WriteLine("Dentist with ID {0} deleted from the database.", idToDelete);
                            }

                            break;
                    }
                }
            }
        }
    }
}
