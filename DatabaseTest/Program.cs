using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections;

namespace DatabaseTest
{
    class Program
    {
        static string conn = "Data Source=10.1.10.114,1434;Network Library=DBMSSOCN;Initial Catalog = TestDB;User ID = ugoadmin;Password = ugo-2019;";
        static void Main(string[] args)
        {
            
            string queryA = "SELECT * FROM Customer";
            string queryB = "SELECT DISTINCT c.CustStatus, COUNT(*) AS Number FROM Customer c GROUP BY CustStatus";
            string queryC = "SELECT * FROM Customer c WHERE DATEDIFF(WEEK, c.LastSeen, GETDATE()) > 12 AND (c.CustStatus LIKE 'Active')";
            string queryD = "SELECT DISTINCT c.CustStatus, SUM(TotalNoOfOrders) FROM Customer c WHERE CustStatus LIKE 'Active' GROUP BY CustStatus";
            string queryE = "SELECT DISTINCT c.CustStatus, MAX(TotalNoOfOrders) FROM Customer c WHERE CustStatus LIKE 'Active' GROUP BY CustStatus";
            char userInput;
            do
            { 
                Console.Clear();
                Console.WriteLine("Alfred Pre Alpha Showcase");
                Console.WriteLine("Press 1 to see all customers");
                Console.WriteLine("Press 2 to see a breakdown of active vs dormant customers");
                Console.WriteLine("Press 3 to see customers we're losing");
                Console.WriteLine("Press 4 to see how many orders active customers have placed");
                Console.WriteLine("Press 5 to see the max amount an active user has spend on an order");
                Console.WriteLine("Press 6 to see all email addresses associated with a user");
                Console.WriteLine("Press q to quit\n\n");
                userInput = Console.ReadKey().KeyChar;
                if (userInput != 'q')
                {
                    if (userInput == '1' || userInput == '3')
                    {
                        string query = (userInput == '1' ? queryA : queryC);
                        ArrayList customers = GetCustomers(query, conn);
                        PrintCustomerResults(customers);
                    }
                    else if (userInput == '2' || userInput == '4' || userInput == '5')
                    {
                        string query = (userInput == '2' ? queryB : (userInput == '4' ? queryD : queryE));
                        ArrayList results = GetStringAndNumber(query, conn);
                        PrintSmallResults(results);
                    }
                    else if (userInput == '6')
                    {
                        Console.WriteLine("Enter the customer's ID");
                        string temp = Console.ReadLine();
                        string query = "SELECT * FROM Customer_Email e WHERE e.CustID = " + temp;
                        ArrayList results = GetStrings(query);
                        foreach (string s in results)
                        {
                            Console.WriteLine(s);
                        }
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("Invalid input");
                    }   
                }
            } while (userInput != 'q');

        }
        public static ArrayList GetStrings(string query)
        {
            ArrayList results = new ArrayList();
            try
            {
                //Console.WriteLine("Working");
                SqlConnection myConnect = new SqlConnection(conn);
                myConnect.Open();
                //Console.WriteLine("Connected");
                using (SqlCommand queryOfTheDay = new SqlCommand(query, myConnect))
                using (SqlDataReader reader = queryOfTheDay.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(reader.GetString(1));
                    }
                }
                myConnect.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error");
            }
            return results;
        }
        static public void PrintCustomerResults(ArrayList customers)
        {
            Console.Clear();
            Console.WriteLine("ID\tName\tStatus\tDate Created\tLast Active\tTotal items ordered\tAvg basket size\tTotal spent\tAvg basket price");
            foreach (Customer c in customers)
            {
                Console.WriteLine(c.ToString());
            }
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
        static public void PrintSmallResults(ArrayList results)
        {
            Console.Clear();
            Console.WriteLine("Customer Status\tNumber");
            foreach (StringAndNumber c in results)
            {
                Console.WriteLine(c.title + "\t" + c.count);

            }
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
        public static ArrayList GetCustomers(string query, string connection)
        {
            ArrayList results = new ArrayList();
            try
            {
                //Console.WriteLine("Working");
                SqlConnection myConnect = new SqlConnection(connection);
                myConnect.Open();
                Console.WriteLine("Connected");
                using (SqlCommand queryOfTheDay = new SqlCommand(query, myConnect))
                using (SqlDataReader reader = queryOfTheDay.ExecuteReader())
                {
                    int count = 0;
                    while (reader.Read())
                    {
                        int custID = int.Parse(reader.GetValue(0).ToString());
                        DateTime created = DateTime.Parse(reader.GetValue(1).ToString());
                        DateTime lastSeen = DateTime.Parse(reader.GetValue(2).ToString());
                        int orders = int.Parse(reader.GetValue(3).ToString());
                        int basketSum = int.Parse(reader.GetValue(4).ToString());
                        int basketAvg = int.Parse(reader.GetValue(5).ToString());
                        int priceSum = int.Parse(reader.GetValue(6).ToString());
                        int priceAvg = int.Parse(reader.GetValue(7).ToString());
                        string status = reader.GetString(8);
                        string name = reader.GetString(9);
                        Customer temp = new Customer(custID, created, lastSeen, orders, basketSum, basketAvg, priceSum, priceAvg, status, name);
                        results.Add(temp);
                        Console.Write("-");
                        count++;
                    }
                    Console.WriteLine("\nRead " + count + " users from file");
                }
                myConnect.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error");
            }
            return results;
        }
        public static ArrayList GetStringAndNumber(string query, string connection)
        {
            ArrayList results = new ArrayList();
            try
            {
                //Console.WriteLine("Working");
                SqlConnection myConnect = new SqlConnection(connection);
                myConnect.Open();
                Console.WriteLine("Connected");
                using (SqlCommand queryOfTheDay = new SqlCommand(query, myConnect))
                using (SqlDataReader reader = queryOfTheDay.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string title = reader.GetString(0);
                        int count = int.Parse(reader.GetValue(1).ToString());
                        StringAndNumber temp = new StringAndNumber(title, count);
                        results.Add(temp);
                    }
                }
                myConnect.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error");
            }
            return results;
        }
    }
    
}
