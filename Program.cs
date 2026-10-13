using System;
using Microsoft.Data.SqlClient;

class Program
{
    static string connectionString = @"Server=.\SQLEXPRESS;Database=CRSRealEstateDB;Trusted_Connection=True;TrustServerCertificate=True;";

    static void Main(string[] args)
    {
        bool running = true;
        while (running)
        {
            Console.WriteLine("\n--- CRS Real Estate & Client Management ---");
            Console.WriteLine("1. Add Property");
            Console.WriteLine("2. View Properties");
            Console.WriteLine("3. Add Client Inquiry");
            Console.WriteLine("4. View Inquiries");
            Console.WriteLine("5. Search Property by Location");
            Console.WriteLine("6. Delete Property by ID");
            Console.WriteLine("7. Exit");
            Console.Write("Apna option chunein (1-7): ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddProperty();
                    break;
                case "2":
                    ViewProperties();
                    break;
                case "3":
                    AddClientInquiry();
                    break;
                case "4":
                    ViewInquiries();
                    break;
                case "5":
                    SearchProperty();
                    break;
                case "6":
                    DeleteProperty();
                    break;
                case "7":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Galat option chuna hai, kripya fir se try karein.");
                    break;
            }
        }
    }

    static void AddProperty()
    {
        Console.Write("Property Title (e.g., 3 BHK Flat): ");
        string title = Console.ReadLine();

        Console.Write("Location: ");
        string location = Console.ReadLine();

        Console.Write("Price: ");
        decimal price = 0;
        while (!decimal.TryParse(Console.ReadLine(), out price))
        {
            Console.WriteLine("Kripya valid price enter karein:");
        }

        Console.Write("Status (Available/Sold): ");
        string status = Console.ReadLine();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = "INSERT INTO Properties (Title, Location, Price, Status) VALUES (@Title, @Location, @Price, @Status)";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Title", title);
                cmd.Parameters.AddWithValue("@Location", location);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Property successfully add ho gayi!");
            }
        }
    }

    static void ViewProperties()
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = "SELECT * FROM Properties";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("\n--- All Properties ---");
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["PropertyId"]} | Title: {reader["Title"]} | Location: {reader["Location"]} | Price: {reader["Price"]} | Status: {reader["Status"]}");
                    }
                }
            }
        }
    }

    static void AddClientInquiry()
    {
        Console.Write("Client Name: ");
        string name = Console.ReadLine();

        Console.Write("Phone: ");
        string phone = Console.ReadLine();

        Console.Write("Preferred Location: ");
        string location = Console.ReadLine();

        Console.Write("Budget: ");
        decimal budget = 0;
        while (!decimal.TryParse(Console.ReadLine(), out budget))
        {
            Console.WriteLine("Kripya valid budget enter karein:");
        }

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = "INSERT INTO Inquiries (ClientName, Phone, PropertyLocation, Budget) VALUES (@ClientName, @Phone, @PropertyLocation, @Budget)";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ClientName", name);
                cmd.Parameters.AddWithValue("@Phone", phone);
                cmd.Parameters.AddWithValue("@PropertyLocation", location);
                cmd.Parameters.AddWithValue("@Budget", budget);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Inquiry successfully add ho gayi!");
            }
        }
    }

    static void ViewInquiries()
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = "SELECT * FROM Inquiries";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("\n--- Client Inquiries ---");
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["InquiryId"]} | Name: {reader["ClientName"]} | Phone: {reader["Phone"]} | Location: {reader["PropertyLocation"]} | Budget: {reader["Budget"]}");
                    }
                }
            }
        }
    }

    static void SearchProperty()
    {
        Console.Write("Search karne ke liye location enter karein: ");
        string searchLocation = Console.ReadLine();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = "SELECT * FROM Properties WHERE Location LIKE @Location";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Location", "%" + searchLocation + "%");
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("\n--- Search Results ---");
                    bool found = false;
                    while (reader.Read())
                    {
                        found = true;
                        Console.WriteLine($"ID: {reader["PropertyId"]} | Title: {reader["Title"]} | Location: {reader["Location"]} | Price: {reader["Price"]} | Status: {reader["Status"]}");
                    }
                    if (!found)
                    {
                        Console.WriteLine("Is location par koi property nahi mili.");
                    }
                }
            }
        }
    }

    static void DeleteProperty()
    {
        Console.Write("Delete karne ke liye Property ID enter karein: ");
        int propertyId = 0;
        while (!int.TryParse(Console.ReadLine(), out propertyId))
        {
            Console.WriteLine("Kripya valid numeric ID enter karein:");
        }

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = "DELETE FROM Properties WHERE PropertyId = @Id";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Id", propertyId);
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Property successfully delete ho gayi hai.");
                }
                else
                {
                    Console.WriteLine("Is ID ki koi property nahi mili.");
                }
            }
        }
    }
}