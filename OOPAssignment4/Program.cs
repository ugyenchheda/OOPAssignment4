using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Numerics;
using System.Text.RegularExpressions;

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
                bool more = false;
                int choice;
                do
                {
                    Console.WriteLine();
                    Console.WriteLine("Welcome to Database processing: Select an action(1 - 6)");
                    Console.WriteLine("Enter 1 if you want list all Dentists:");
                    Console.WriteLine("Enter 2 if you want to add a new Dentist:");
                    Console.WriteLine("Enter 3 if you want to look for a Dentist:");
                    Console.WriteLine("Enter 4 if you want to update Dentist information:");
                    Console.WriteLine("Enter 5 if you want to delete Dentist information:");
                    Console.WriteLine("Enter 6 to Do Nothing :");
                    Console.WriteLine();
                    Console.Write("Select an action: ");

                    string received = Console.ReadLine();
                    while (!Int32.TryParse(received, out choice) || choice < 1 || choice > 6)
                    {
                        Console.Write("Not valid, try again: ");
                        received = Console.ReadLine();
                    }

                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine("Your choice: list all Dentists");
                            foreach (Dentist dentist in allDentists)
                            {
                                Console.WriteLine("{0}: {1} ({2})", dentist.Id, dentist.Name, dentist.TelNum);
                            }
                            break;

                        case 2:
                            Console.WriteLine("Your choice: Add a new Dentist.");
                            string dentistName;
                            string dentistTelNum;
                            do
                            {
                                Console.Write("Enter the Doctor's name: ");
                                dentistName = Console.ReadLine();
                                if (String.IsNullOrEmpty(dentistName))
                                    Console.WriteLine("The field was left empty, try again!");

                            } while (String.IsNullOrEmpty(dentistName));


                            Console.Write("Enter Mobile Number in +358XXXXXXXXXX format: ");
                            dentistTelNum = Console.ReadLine();
                            string phonePattern = @"^(\+|00)358\d{9}$";

                            while (!Regex.IsMatch(dentistTelNum, phonePattern))
                            {
                                Console.Write("Not valid, try again: ");
                                dentistTelNum = Console.ReadLine();
                            }

                            Dentist newDentist = new Dentist { Name = dentistName, TelNum = dentistTelNum };
                            dentists.InsertDentist(sqlConnection, newDentist);
                            Console.WriteLine("New dentist " + dentistName+ " inserted into database.");
                            break;

                        case 3:

                            Console.WriteLine("Your choice: Search for Dentist.");
                            string searchName;
                            List<Dentist> foundDentists = new List<Dentist>();
                            do
                            {
                                Console.Write("Enter the Dentist's name: ");
                                searchName = Console.ReadLine();
                                if (String.IsNullOrEmpty(searchName))
                                {
                                    Console.WriteLine("The field was left empty, try again!");
                                }
                                else
                                {
                                    foundDentists = dentists.FindDentist(sqlConnection, searchName);

                                    if (foundDentists.Count == 0)
                                    {
                                        Console.WriteLine("There is no dentist with the name {0}.", searchName);
                                    }
                                    else
                                    {
                                        foreach (Dentist dentist in foundDentists)
                                        {
                                            Console.WriteLine("There is a dentist with name {0} and phone number is ({1}).", dentist.Name, dentist.TelNum);
                                        }
                                    }
                                }
                            } while (String.IsNullOrEmpty(searchName) || foundDentists.Count == 0);
                            break;
                        case 4:

                            Console.WriteLine("Your choice: Modify Dentist's information.");
                            string nameToUpdate;
                            do
                            {
                                Console.Write("Enter the Dentist's name: ");
                                nameToUpdate = Console.ReadLine();
                                if (String.IsNullOrEmpty(nameToUpdate))
                                    Console.WriteLine("The field was left empty, try again!");
                                Dentist dentistToUpdate = allDentists.FirstOrDefault(d => d.Name == nameToUpdate);
                                if (dentistToUpdate == null)
                                {
                                    Console.WriteLine("Dentist with name {0} not found.", nameToUpdate);
                                }
                                else
                                {
                                    Console.Write("Enter the new name of the dentist: ");
                                    string newName = Console.ReadLine();
                                    if (String.IsNullOrEmpty(newName))
                                    {
                                        Console.WriteLine("The name cannot be empty.");
                                        return;
                                    }

                                    Console.Write("Enter the new telephone number of the dentist: ");
                                    string newTelNum = Console.ReadLine();
                                    if (String.IsNullOrEmpty(newTelNum))
                                    {
                                        Console.WriteLine("The telephone number cannot be empty.");
                                        return;
                                    }
                                    dentistToUpdate.Name = newName;
                                    dentistToUpdate.TelNum = newTelNum;
                                    dentists.UpdateDentist(sqlConnection, dentistToUpdate);
                                    Console.WriteLine("Dentist information updated in database.");
                                }
                            } while (String.IsNullOrEmpty(nameToUpdate));
                            break;

                            case 5:
                            Console.WriteLine("Your choice: Remove Dentist Information.");
                            string removeDentist;

                            do
                            {
                                Console.Write("Enter the Dentist's name: ");
                                removeDentist = Console.ReadLine();
                                if (String.IsNullOrEmpty(removeDentist))
                                {
                                    Console.WriteLine("The field was left empty, try again!");
                                }
                                else
                                {
                                    Dentist dentistToDelete = allDentists.FirstOrDefault(d => d.Name.Equals(removeDentist, StringComparison.OrdinalIgnoreCase));
                                    if (dentistToDelete == null)
                                    {
                                        Console.WriteLine("Dentist with name {0} not found.", removeDentist);
                                    }
                                    else
                                    {
                                        dentists.DeleteDentist(sqlConnection, removeDentist);
                                        Console.WriteLine("Dentist information deleted from database.");
                                    }
                                }
                            } while (String.IsNullOrEmpty(removeDentist));

                            break;
                            case 6:
                            Console.WriteLine("Your choice: Do Nothing...");
                            break;
                        default:
                            Console.WriteLine("Your choice: Let it rest...");
                            break;
                    }

                    Console.WriteLine();
                    Console.Write("Do you want to continue with the new operation? (Y/N): ");
                    received = Console.ReadLine().ToUpper();

                    if (received.StartsWith("Y"))
                        more = true;
                    else
                        more = false;
                } while (more);
            }
        }
    }
}
