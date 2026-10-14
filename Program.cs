using Microsoft.Data.SqlClient;

namespace DataAccess
{
    class Program
    {
        static void Main(string[] args)
        {
            const string connectionString = "Server=localhost,1433;Database=balta;User ID=sa; Password=1q2w3e4r@#$";

             using(var connection= new SqlConnection(connectionString))
            {
                Console.WriteLine("Conectado");
            }

            // var connection = new SqlConnection();
            // connection.Open();
            // //INSERT
            // //UPDATE
            // connection.Close();
            // connection.Open();
            // connection.Dispose();
            // new


        }
    }
}