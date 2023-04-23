using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPAssignment4
{

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
