using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProjectWpfAppExpVersion
{
    class DatabaseManager
    {
        SqlConnection SqlConn = new SqlConnection("Server = tcp:thetest.database.windows.net, 1433; Initial Catalog = myTestDatabase; Persist Security Info=False;User ID = thetest; Password=Letmein123--; MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout = 30;");
        SqlCommand SqlStr = new SqlCommand();
        SqlDataReader SqlReader;
        String SqlStmt;

        public void PopulateListings(string t, string p, string d, string e, string o, string b, string r, string reg)
        {
            try
            {
                SqlStr.Connection = SqlConn;
                SqlStmt = "Insert into Vehicles(Title, Price, Dealer, Engine, Odometer, BidCount, ReserveMet, Region) values ('" + t + "','" + p + "','" + d + "','" + e + "','" + o + "','" + b + "','" + r + "','" + reg + "')";
                SqlStr.CommandText = SqlStmt;
                SqlConn.Open();
                SqlStr.ExecuteNonQuery();
                SqlConn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Database Exception", ex.Message);
                SqlConn.Close();
            }
        }

    }
}
