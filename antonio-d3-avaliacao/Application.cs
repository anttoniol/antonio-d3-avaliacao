using System;
using System.Data;
using MySql.Data.MySqlClient;
using BCrypt.Net;

namespace Application
{
    internal class Conexao
    {
        private string connStr;

        internal Conexao()
        {
            connStr = "server=localhost;user id=root;password=root;database=antonio_d3_avaliacao";
        }

        internal string[] Select(string email, string password)
        {
            try
            {
                string query = "SELECT id, nome, senha FROM login WHERE email = @email;";

                MySqlConnection connection = new MySqlConnection(connStr);
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@password", password);
                connection.Open();

                MySqlDataReader dataReader;
                dataReader = command.ExecuteReader();

                DataTable dataTable = new DataTable();
                dataTable.Load(dataReader);

                connection.Close();

                if (dataTable.Rows.Count == 0)
                    return null;

                bool match = BCrypt.Net.BCrypt.Verify(password, dataTable.Rows[0]["senha"].ToString());
                if (match)
                    return new string[] {dataTable.Rows[0]["nome"].ToString(), dataTable.Rows[0]["id"].ToString()};
                return null;
            }
            catch
            {
                Console.WriteLine("\nHouve um erro na conexão com o banco de dados\n");
                return null;
            }
        }

        internal bool Insert(string name, string email, string password)
        {
            try
            {
                string query = "INSERT INTO login(nome, email, senha) VALUES(@name, @email, @password);";

                MySqlConnection connection = new MySqlConnection(connStr);
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@password", password);
                connection.Open();
                command.ExecuteReader();
                connection.Close();

                return true;
            }
            catch
            {
                Console.WriteLine("\nHouve um erro na conexão com o banco de dados\n");
                return false;
            }   
        }
    }

    internal class Platform
    {
        private string[] commands = {"deslogar", "encerrar sistema"};

        private string CheckCommand(string command)
        {
            if (!commands.Contains(command))
                return "";

            return command;
        }

        private string returnCommands()
        {
            return string.Join("/", commands);
        }

        internal string start()
        {
            Console.WriteLine("-------------------------PLATAFORMA: INÍCIO-----------------------------\n");
            String command;
            do
            {
                Console.WriteLine($"Informe um comando [{returnCommands()}]: ");
                command = CheckCommand(Console.ReadLine());
                if (string.IsNullOrEmpty(command))
                    Console.WriteLine("\nComando inválido\n");
            } while (string.IsNullOrEmpty(command));
            
            Console.WriteLine("--------------------------PLATAFORMA: FIM-------------------------------\n");
            return command;
        }
    }

    public class AccessControl
    {
        private string[] malicious =
        {
            "drop", "delete", "select", ";", "--", "insert", "delete", "xp_", "'", "update", "/*", "*/"
        };

        private void registerOnFile(string message)
        {
            string fileName = "log.txt";
            string temp = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.Combine(temp, fileName);

            bool fileExist = File.Exists(path);
            if (!fileExist)
                File.Create(path);

            File.AppendAllText(path, message + Environment.NewLine);
        }

        private string validate(string data)
        {
            string secureData = data;
            foreach(string bad in malicious) 
                secureData = secureData.Replace(bad, "", true, null);
            return secureData;
        }

        private string checkPlatformCommand(string command)
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

        public bool createLogin()
        {
            string name, email, password;

            Console.WriteLine("Informe o nome: ");
            name = validate(Console.ReadLine());
            Console.WriteLine("\nInforme o email: ");
            email = validate(Console.ReadLine());
            Console.WriteLine("\nInforme a senha: ");
            password = BCrypt.Net.BCrypt.HashPassword(validate(Console.ReadLine()));

            return new Conexao().Insert(name, email, password);
        }

        public string start()
        {
            Console.WriteLine("\n-------------------------AUTENTICAÇÃO: INÍCIO---------------------------\n");
            string email, password;
            string[] data;

            do
            {
                Console.WriteLine("Informe o email: ");
                email = validate(Console.ReadLine());
                Console.WriteLine("\nInforme a senha: ");
                password = validate(Console.ReadLine());

                data = new Conexao().Select(email, password);
                if (data == null)
                    Console.WriteLine("\nDados inválidos, tente novamente\n");
            } while (data == null);

            Console.WriteLine("\nLogin realizado com sucesso!\n");
            String msgLogin = $"O usuário {data[0]}({data[1]}) " +
                $"acessou o sistema às {DateTime.Now.ToString("HH:mm:ss tt")} do dia {DateTime.Now.ToString("dd-MM-yyyy")}.";
            registerOnFile(msgLogin);

            Platform platform = new Platform();
            string platformCommand = checkPlatformCommand(platform.start());
            if(platformCommand == "acessar")
            {
                String msgLogout = $"O usuário {data[0]}({data[1]}) " +
                $"deslogou-se do sistema às {DateTime.Now.ToString("HH:mm:ss tt")} do dia {DateTime.Now.ToString("dd-MM-yyyy")}.";
                registerOnFile(msgLogout);
            }
            Console.WriteLine("--------------------------AUTENTICAÇÃO: FIM-----------------------------\n");
            return platformCommand;
        }
    }
}

