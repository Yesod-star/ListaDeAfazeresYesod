using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

class Program
{
    private static string connectionString;

    static void Main(string[] args)
    {
        // Load configuration from appsettings.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        connectionString = configuration.GetConnectionString("dados");

        bool exit = false;

        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Inserir nova tarefa");
            Console.WriteLine("2. Listar todas as tarefas");
            Console.WriteLine("3. Atualizar uma tarefa");
            Console.WriteLine("4. Deletar uma tarefa");
            Console.WriteLine("5. Sair");
            Console.Write("Escolha uma opção: ");
            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    InserirTarefa();
                    break;
                case "2":
                    ListarTarefas();
                    break;
                case "3":
                    AtualizarTarefa();
                    break;
                case "4":
                    DeletarTarefa();
                    break;
                case "5":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Opção inválida. Pressione qualquer tecla para tentar novamente.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private static void InserirTarefa()
    {
        Console.Write("Digite a tarefa: ");
        var tarefa = Console.ReadLine();
        Console.WriteLine("Digite a data da tarefa (yyyy-mm-dd): ");
        var data = Console.ReadLine();

        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var query = "INSERT INTO cad_tarefas(tarefa, data_tarefa) VALUES (@tarefa, @data)";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@tarefa", tarefa);
                command.Parameters.AddWithValue("@data", DateTime.Parse(data));
                command.ExecuteNonQuery();
            }
        }

        Console.WriteLine("Tarefa inserida com sucesso.");
        Console.ReadKey();
    }

    private static void ListarTarefas()
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var query = "SELECT id, tarefa, data_tarefa FROM cad_tarefas";
            using (var command = new MySqlCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["id"]}, Tarefa: {reader["tarefa"]}, Data: {reader["data_tarefa"]}");
                }
            }
        }
        Console.WriteLine("Pressione qualquer tecla para voltar ao menu.");
        Console.ReadKey();
    }

    private static void AtualizarTarefa()
    {
        Console.Write("Digite o ID da tarefa que deseja atualizar: ");
        var id = Console.ReadLine();
        Console.Write("Digite a nova tarefa: ");
        var novaTarefa = Console.ReadLine();
        Console.WriteLine("Digite a nova data da tarefa (yyyy-mm-dd): ");
        var novaData = Console.ReadLine();

        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var query = "UPDATE cad_tarefas SET tarefa = @novaTarefa, data_tarefa = @novaData WHERE id = @id";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", int.Parse(id));
                command.Parameters.AddWithValue("@novaTarefa", novaTarefa);
                command.Parameters.AddWithValue("@novaData", DateTime.Parse(novaData));
                command.ExecuteNonQuery();
            }
        }

        Console.WriteLine("Tarefa atualizada com sucesso.");
        Console.ReadKey();
    }

    private static void DeletarTarefa()
    {
        Console.Write("Digite o ID da tarefa que deseja deletar: ");
        var id = Console.ReadLine();

        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var query = "DELETE FROM cad_tarefas WHERE id = @id";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", int.Parse(id));
                command.ExecuteNonQuery();
            }
        }

        Console.WriteLine("Tarefa deletada com sucesso.");
        Console.ReadKey();
    }
}
