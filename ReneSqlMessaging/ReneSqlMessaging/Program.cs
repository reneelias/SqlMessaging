using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReneSqlMessaging
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection connection = new SqlConnection("Server=GMRMLTV;Database=ReneMessaging;User Id=sa;Password=GreatMinds110;");
            connection.Open();

            bool login = false, exit = false;
            string username = "", password = "";
            Console.WriteLine("Welcome to your reminders app! Leave a message here and check back on it later.");
            while (!login && !exit)
            {
                Console.WriteLine("What would you like to do? [R]egister a new user. [L]ogin. [E]xit.");
                string choice = Console.ReadLine().ToLower();
                switch (choice)
                {

                    case "r":
                        {
                            Register(connection);
                            break;
                        }
                    case "l":
                        {
                            login = Login(connection, ref username, password);
                            if (!login)
                            {
                                Console.WriteLine("Those credentials were incorrect. Please try again.");
                            }
                            else
                            {
                                Console.WriteLine("Welcome!");
                            }
                            break;
                        }
                    case "e":
                        {
                            exit = true;
                            Console.WriteLine("Have a nice day!");
                            break;
                        }
                }
            }

            exit = false;
            while (login && !exit)
            {
                Console.WriteLine("What would you like to do? [W]rite a message. [R]ead a message. [E]xit.");

                string choice = Console.ReadLine().ToLower();
                switch (choice)
                {
                    case "w":
                        {
                            WriteMessage(connection, username);
                            break;
                        }
                    case "r":
                        {
                            ViewAllMessages(connection, username);
                            break;
                        }
                    case "e":
                        {
                            exit = true;
                            break;
                        }
                }
            }
            Console.ReadKey();
            connection.Close();
        }

        static void Register(SqlConnection connection)
        {
            SqlCommand registerCommand = new SqlCommand("usp_Register");
            registerCommand.Connection = connection;
            registerCommand.CommandType = System.Data.CommandType.StoredProcedure;
            Console.Write("Username: ");
            string username = Console.ReadLine();
            Console.Write("Password: ");
            string password = Console.ReadLine();
            registerCommand.Parameters.AddWithValue("@username", username);
            registerCommand.Parameters.AddWithValue("@password", password);
            registerCommand.ExecuteNonQuery();
        }

        static bool Login(SqlConnection connection, ref string username, string password)
        {
            SqlCommand loginCommand = new SqlCommand("usp_Login");
            loginCommand.Connection = connection;
            loginCommand.CommandType = System.Data.CommandType.StoredProcedure;
            Console.Write("Username: ");
            username = Console.ReadLine();
            Console.Write("Password: ");
            password = Console.ReadLine();
            loginCommand.Parameters.AddWithValue("@username", username);
            loginCommand.Parameters.AddWithValue("@password", password);
            loginCommand.ExecuteNonQuery();

            SqlDataAdapter adapter = new SqlDataAdapter(loginCommand);
            DataTable table = new DataTable();
            adapter.Fill(table);

            return table.Rows.Count != 0;
        }

        static void WriteMessage(SqlConnection connection, string username)
        {
            SqlCommand writeMessageCommand = new SqlCommand("usp_StoreMessage");
            writeMessageCommand.Connection = connection;
            writeMessageCommand.CommandType = System.Data.CommandType.StoredProcedure;
            Console.WriteLine("Please write your message below:\n");
            string message = Console.ReadLine();
            string fullTime = DateTime.Now.ToString();
            string date = fullTime.Split(' ')[0];
            string time = fullTime.Split(' ')[1];
            writeMessageCommand.Parameters.AddWithValue("@sender", username);
            writeMessageCommand.Parameters.AddWithValue("@message", message);
            writeMessageCommand.Parameters.AddWithValue("@time", time);
            writeMessageCommand.Parameters.AddWithValue("@date", date);
            writeMessageCommand.ExecuteNonQuery();
        }

        static void ViewAllMessages(SqlConnection connection, string username)
        {
            SqlCommand viewAllMessagesCommand = new SqlCommand("usp_ViewAllMessages");
            viewAllMessagesCommand.Connection = connection;
            viewAllMessagesCommand.CommandType = System.Data.CommandType.StoredProcedure;
            viewAllMessagesCommand.Parameters.AddWithValue("@sender", username);
            viewAllMessagesCommand.ExecuteNonQuery();

            SqlDataAdapter adapter = new SqlDataAdapter(viewAllMessagesCommand);
            DataTable table = new DataTable();
            adapter.Fill(table);

            for(int i = 0; i < table.Rows.Count; i++)
            {
                Console.WriteLine($"Message {i+1}:");
                Console.WriteLine($"Sent on {table.Rows[i][1]} at {table.Rows[i][2]}");
                Console.WriteLine($"\"{table.Rows[i][0]}\"");
                Console.WriteLine("\n------------------------------------------\n");
            }
        }
    }
}
