using Dapper;
using DataAccess.Models;
using Microsoft.Data.SqlClient;

// Dapper

namespace DataAccess;

class Program
{
    static void Main(string[] args)
    {
        const string connectionString = "Server=localhost,1433;Database=balta;User ID=sa;Password=1q2w3e4r@#$;TrustServerCertificate=True;";

        using (var connection = new SqlConnection(connectionString))
        {
            var category = new Category();
            var insertSql = "INSERT INTO [Category] Values(id, title, url, summary, order, description, featured)";

            var categories = connection.Query<Category>("SELECT [Id], [Title] FROM [Category]");
            foreach (var item in categories)
            {
                Console.WriteLine($"{item.Id} - {item.Title}");
            }
        }
    }
}
