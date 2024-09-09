using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System;

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


        while (true)
        {
            MostrarMenu();

            var opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    InserirDados();
                    break;
                case "2":
                    LerDados();
                    break;
                case "3":
                    AtualizarDados();
                    break;
                case "4":
                    DeletarDados();
                    break;
                case "5":
                    Console.WriteLine("Saindo... Até logo!");
                    return; // Encerra o programa
                default:
                    Console.WriteLine("Opção inválida. Por favor, escolha um número entre 1 e 5.");
                    break;
            }
        }
    }

    private static void MostrarMenu()
    {
        Console.WriteLine("1. Inserir dados");
        Console.WriteLine("2. Ler dados");
        Console.WriteLine("3. Atualizar dados");
        Console.WriteLine("4. Deletar dados");
        Console.WriteLine("5. Sair");
    }

    private static void InserirDados()
    {
        Console.Write("Digite a tarefa: ");
        var tarefa = Console.ReadLine();
        Console.WriteLine("Digite a data da tarefa (yyyy-mm-dd): ");
        var data = Console.ReadLine();
        Console.WriteLine("Digite o nome: ");
        var nome = Console.ReadLine();

        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var query = "INSERT INTO cad_tarefas(tarefa, data_tarefa, nome) VALUES (@tarefa, @data, @nome)";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@tarefa", tarefa);
                command.Parameters.AddWithValue("@data", data);
                command.Parameters.AddWithValue("@nome", nome);
                command.ExecuteNonQuery();
            }
        }

        Console.WriteLine("Tarefa inserida com sucesso.");
    }

    private static void LerDados()
    {
        Console.WriteLine("Digite o ID da tarefa que deseja ler: ");
        var id = Console.ReadLine();

        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var query = "SELECT * FROM cad_tarefas WHERE id = @id";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["id"]}");
                        Console.WriteLine($"Tarefa: {reader["tarefa"]}");
                        Console.WriteLine($"Data: {reader["data_tarefa"]}");
                        Console.WriteLine($"Nome: {reader["nome"]}");
                    }
                    else
                    {
                        Console.WriteLine("Tarefa não encontrada.");
                    }
                }
            }
        }
    }

    private static void AtualizarDados()
    {
        Console.WriteLine("Digite o ID da tarefa que deseja atualizar: ");
        var id = Console.ReadLine();
        Console.Write("Digite a nova tarefa: ");
        var tarefa = Console.ReadLine();
        Console.WriteLine("Digite a nova data da tarefa (yyyy-mm-dd): ");
        var data = Console.ReadLine();
        Console.WriteLine("Digite o novo nome: ");
        var nome = Console.ReadLine();

        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var query = "UPDATE cad_tarefas SET tarefa = @tarefa, data_tarefa = @data, nome = @nome WHERE id = @id";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@tarefa", tarefa);
                command.Parameters.AddWithValue("@data", data);
                command.Parameters.AddWithValue("@nome", nome);
                command.Parameters.AddWithValue("@id", id);
                var rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Tarefa atualizada com sucesso.");
                }
                else
                {
                    Console.WriteLine("Tarefa não encontrada.");
                }
            }
        }
    }

    private static void DeletarDados()
    {
        Console.WriteLine("Digite o ID da tarefa que deseja deletar: ");
        var id = Console.ReadLine();

        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var query = "DELETE FROM cad_tarefas WHERE id = @id";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                var rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Tarefa deletada com sucesso.");
                }
                else
                {
                    Console.WriteLine("Tarefa não encontrada.");
                }
            }
        }
    }
}
