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
            // CreateCategory(connection);
            // CreateManyCategory(connection);
            // UpdateCategory(connection);
            // DeleteCategory(connection);
            // ListCategories(connection);
            // GetCategory(connection);
            // ExecuteProcedure(connection);
            // ExecuteReadProcedure(connection);
            ExecuteScalar(connection);
        }
    }

    static void ListCategories(SqlConnection connection)
    {
        var categories = connection.Query<Category>("SELECT [Id], [Title] FROM [Category]");
        foreach (var item in categories)
        {
            Console.WriteLine($"{item.Id} - {item.Title}");
        }
    }

    static void GetCategory(SqlConnection connection)
    {
        var category = connection.QuerySingleOrDefault<Category>(
            @"SELECT [Id], [Title], [Url], [Summary], [Order], [Description], [Featured]
              FROM [Category]
              WHERE [Id] = @id",
            new
            {
                id = new Guid("af3407aa-11ae-4621-a2ef-2028b85507c4")
            });

        if (category is null)
        {
            Console.WriteLine("Categoria não encontrada");
            return;
        }

        Console.WriteLine($"{category.Id} - {category.Title}");
    }

    static void CreateCategory(SqlConnection connection)
    {
        var category = new Category();
        category.Id = Guid.NewGuid();
        category.Title = "Amazon AWS";
        category.Url = "amazon";
        category.Description = "Categoria destinada a serviços do AWS";
        category.Order = 8;
        category.Summary = "AWS Cloud";
        category.Featured = false;

        //SQL Injection Lembrar
        var insertSql = @"INSERT INTO
                [Category] 
            VALUES(
                @Id,
                @Title,
                @Url,
                @Summary,
                @Order,
                @Description,
                @Featured)";

        var rows = connection.Execute(insertSql, new
        {
            category.Id,
            category.Title,
            category.Url,
            category.Summary,
            category.Order,
            category.Description,
            category.Featured
        });
        Console.WriteLine($"{rows} linhas inseridas");
    }

    static void UpdateCategory(SqlConnection connection)
    {
        var updateQuery = "UPDATE [Category] SET [Title] = @title WHERE [Id] = @id";
        var rows = connection.Execute(updateQuery, new
        {
            id = new Guid("af3407aa-11ae-4621-a2ef-2028b85507c4"),
            title = "Frontend 2021"
        });

        Console.WriteLine($"{rows} registros atualizados");
    }

    static void DeleteCategory(SqlConnection connection)
    {
        var deleteQuery = "DELETE FROM [Category] WHERE [Id] = @id";
        var rows = connection.Execute(deleteQuery, new
        {
            id = new Guid("d5944bed-02a9-4fd3-8e49-cbf7b8680425")
        });

        Console.WriteLine($"{rows} registros deletados");
    }

    static void CreateManyCategory(SqlConnection connection)
    {
        var category = new Category();
        category.Id = Guid.NewGuid();
        category.Title = "Amazon AWS";
        category.Url = "amazon";
        category.Description = "Categoria destinada a serviços do AWS";
        category.Order = 8;
        category.Summary = "AWS Cloud";
        category.Featured = false;

        var category2 = new Category();
        category2.Id = Guid.NewGuid();
        category2.Title = "Categoria Nova";
        category2.Url = "categoria-nova";
        category2.Description = "Categoria nova";
        category2.Order = 9;
        category2.Summary = "Categoria";
        category2.Featured = true;

        //SQL Injection Lembrar
        var insertSql = @"INSERT INTO
                [Category] 
            VALUES(
                @Id,
                @Title,
                @Url,
                @Summary,
                @Order,
                @Description,
                @Featured)";

        var rows = connection.Execute(insertSql, new[]
        {
            new
            {
                category.Id,
                category.Title,
                category.Url,
                category.Summary,
                category.Order,
                category.Description,
                category.Featured
            },
            new
            {
                category2.Id,
                category2.Title,
                category2.Url,
                category2.Summary,
                category2.Order,
                category2.Description,
                category2.Featured
            },
        });
        Console.WriteLine($"{rows} linhas inseridas");
    }

    static void ExecuteProcedure(SqlConnection connection)
    {
        var procedure = "[spDeleteStudent]";
        var pars = new { StudentId = "7053c613-21f5-44e6-8443-232d72b7a0db" };
        var affectedRows = connection.Execute(
            procedure,
            pars,
            commandType: System.Data.CommandType.StoredProcedure);
        Console.WriteLine($"{affectedRows} linhas afetadas");
    }

    static void ExecuteReadProcedure(SqlConnection connection)
    {
        var procedure = "[spGetCoursesByCategory]";
        var pars = new { @CategoryId = "09ce0b7b-cfca-497b-92c0-3290ad9d5142" };
        var courses = connection.Query(
            procedure,
            pars,
            commandType: System.Data.CommandType.StoredProcedure);
        foreach (var item in courses)
        {
            // Id trás os Guid
            Console.WriteLine(item.Title);
        }
    }

    static void ExecuteScalar(SqlConnection connection)
    {
        var category = new Category();
        category.Title = "Amazon AWS";
        category.Url = "amazon";
        category.Description = "Categoria destinada a serviços do AWS";
        category.Order = 8;
        category.Summary = "AWS Cloud";
        category.Featured = false;

        //SQL Injection Lembrar
        var insertSql = @"
            INSERT INTO
                [Category] 
            OUTPUT
                inserted.[Id]    
            VALUES(
                NEWID(),
                @Title,
                @Url,
                @Summary,
                @Order,
                @Description,
                @Featured) 
            SELECT
                SCOPE_IDENTITY()";

        var id = connection.ExecuteScalar<Guid>(insertSql, new
        {
            category.Title,
            category.Url,
            category.Summary,
            category.Order,
            category.Description,
            category.Featured
        });
        Console.WriteLine($"A categoria inserida foi: {id}");
    }
}
