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

        internal bool SelectLogin(String email, String password)
        {
            try
            {
                String query = "SELECT COUNT(*) as quantity FROM login WHERE email = @email AND senha = @password;";
                
                MySqlConnection connection = new MySqlConnection(connStr);
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@password", password);
                connection.Open();

                //Console.WriteLine($"Query: {query}");

                MySqlDataReader dataReader;
                dataReader = command.ExecuteReader();

                DataTable dataTable = new DataTable();
                dataTable.Load(dataReader);

                bool converted = int.TryParse(dataTable.Rows[0]["quantity"].ToString(), out int numberOfResults);
                connection.Close();

                if (converted && numberOfResults > 0)
                    return true;
            }
            catch
            {
                Console.WriteLine("Houve um erro na conexão com o banco de dados");
            }

            return false;
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
            String action = "";
            while (String.IsNullOrEmpty(action))
            {
                Console.WriteLine($"Informe um comando [{returnCommands()}]: ");
                action = CheckCommand(Console.ReadLine());
                if (String.IsNullOrEmpty(action))
                    Console.WriteLine("Comando inválido");
            }
            return action;
        }
    }

    public class AccessControl
    {
        private String[] commands = {"acessar", "cancelar"};
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

        private String checkPlatformCommand(String action)
        {
            if (action.Equals("deslogar"))
                return "acessar";
            else if (action.Equals("encerrar sistema"))
                return "cancelar";
            return "";
        }

        private String access()
        {
            bool success = false;
            String email;
            String password;
            while (!success)
            {
                Console.WriteLine("Informe o email: ");
                email = validate(Console.ReadLine());
                Console.WriteLine("Informe a senha: ");
                password = validate(Console.ReadLine());

                success = new Conexao().SelectLogin(email, password);
                if (!success)
                    Console.WriteLine("Dados inválidos, tente novamente");
            }

            Platform platform = new Platform();
            return checkPlatformCommand(platform.start());
        }

        private String CheckCommand(String command)
        {
            if (!commands.Contains(command))
                return "";

            switch (command)
            {
                case "acessar":
                    return access();

                case "cancelar":
                    return "cancelar";

                default:
                    return "";
            }
        }

        private String returnCommands()
        {
            return String.Join("/", commands);
        }

        public void start()
        {
            String action = "acessar";
            while (action.Equals("acessar"))
            {
                Console.WriteLine($"Informe um comando [{returnCommands()}]: ");
                action = CheckCommand(Console.ReadLine());
                if (String.IsNullOrEmpty(action))
                {
                    Console.WriteLine("Comando inválido");
                    action = "acessar";
                }
            }
            return;
        }
    }
}

