using Dapper;
using EVCP.DataAccess;

namespace EVCP.Domain.Helpers;

public class SqlScriptRunner
{
    private readonly DapperContext _context;

    public SqlScriptRunner(DapperContext? context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void Run(string filePath)
    {
        string script = File.ReadAllText(filePath);

        using var connection = _context.CreateConnection();

        connection.Open();

        string[] commands = script.Split(';');

        foreach (string command in commands)
        {
            if (!string.IsNullOrWhiteSpace(command))
            {
                Console.WriteLine($"Running '{command}' ...");
                var rowsAffected = connection.Execute(command);
                Console.WriteLine($"DB rows affected: {rowsAffected}");
            }
        }

        connection.Close();

        Console.WriteLine("Script executed successfully.");
    }
}
