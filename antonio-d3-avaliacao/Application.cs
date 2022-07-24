using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace Application
{
    internal class Conexao
    {
        private String connStr;

        internal Conexao()
        {
            connStr = "server=localhost;user id=root;password=root;database=antonio_d3_avaliacao";
        }

        internal int SelectLogin(String email, String password)
        {
            try
            {
                String query = "SELECT id FROM login WHERE email = @email AND senha = @password;";
                
                MySqlConnection connection = new MySqlConnection(connStr);
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@password", password);
                connection.Open();

                MySqlDataReader dataReader;
                dataReader = command.ExecuteReader();

                DataTable dataTable = new DataTable();
                dataTable.Load(dataReader);

                if (dataTable.Rows.Count == 0)
                    return -1;

                bool converted = int.TryParse(dataTable.Rows[0]["id"].ToString(), out int id);
                connection.Close();

                if (converted)
                    return id;
            }
            catch
            {
                Console.WriteLine("\nHouve um erro na conexão com o banco de dados\n");
            }

            return -1;
        }
    }

    internal class Platform
    {
        private String[] commands = {"deslogar", "encerrar sistema"};

        private String CheckCommand(String command)
        {
            if (!commands.Contains(command))
                return "";

            return command;
        }

        private String returnCommands()
        {
            return String.Join("/", commands);
        }

        internal String start()
        {
            Console.WriteLine("-------------------------PLATAFORMA: INÍCIO-----------------------------\n");
            String action = "";
            while (String.IsNullOrEmpty(action))
            {
                Console.WriteLine($"Informe um comando [{returnCommands()}]: ");
                action = CheckCommand(Console.ReadLine());
                if (String.IsNullOrEmpty(action))
                    Console.WriteLine("\nComando inválido\n");
            }
            Console.WriteLine("--------------------------PLATAFORMA: FIM-------------------------------\n");
            return action;
        }
    }

    public class AccessControl
    {
        private String[] malicious =
        {
            "drop", "delete", "select", ";", "--", "insert", "delete", "xp_", "'", "update", "/*", "*/"
        };

        private String validate(String data)
        {
            String secureData = data;
            foreach(String bad in malicious) 
                secureData = secureData.Replace(bad, "", true, null);
            return secureData;
        }

        private String checkPlatformCommand(String command)
        {
            switch(command)
            {
                case "deslogar":
                    return "acessar";

                case "encerrar sistema":
                    return "cancelar";

                default:
                    return "";
            }
        }

        public String start()
        {
            Console.WriteLine("\n-------------------------AUTENTICAÇÃO: INÍCIO---------------------------\n");
            int id = -1;
            String email;
            String password;
            while (id < 0)
            {
                Console.WriteLine("Informe o email: ");
                email = validate(Console.ReadLine());
                Console.WriteLine("\nInforme a senha: ");
                password = validate(Console.ReadLine());

                id = new Conexao().SelectLogin(email, password);
                if (id < 0)
                    Console.WriteLine("\nDados inválidos, tente novamente\n");
            }
            Console.WriteLine("\nLogin realizado com sucesso!\n");

            Platform platform = new Platform();
            String platformCommand = checkPlatformCommand(platform.start());
            Console.WriteLine("--------------------------AUTENTICAÇÃO: FIM-----------------------------\n");
            return platformCommand;
        }
    }
}

