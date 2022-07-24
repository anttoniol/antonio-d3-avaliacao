using System;
using Application;


//CreateLogin();
RunConsoleInterface();

static void RunConsoleInterface()
{
    ConsoleInterface ci = new ConsoleInterface();
    ci.start();
}

//Criar usuário no banco com senha criptografada
static void CreateLogin()
{
    Application.AccessControl ac = new Application.AccessControl();
    bool success = ac.createLogin();
    if (success)
        Console.WriteLine("\nConta criada com sucesso!\n");
    else
        Console.WriteLine("\nOcorreu um erro na criação da conta\n"); 
}

public class ConsoleInterface
{
    private string[] commands = { "acessar", "cancelar" };

    private string returnCommands()
    {
        return string.Join("/", commands);
    }
    private string CheckCommand(string command)
    {
        if (!commands.Contains(command))
            return "";

        Application.AccessControl ac = new Application.AccessControl();

        switch(command)
        {
            case "acessar":
                return ac.start();

            case "cancelar":
                return "cancelar";

            default:
                return "";
        }
    }
    public void start()
    {
        Console.WriteLine("--------------------------INTERFACE: INÍCIO-----------------------------\n");
        string command;
        do
        {
            Console.WriteLine($"Informe um comando [{returnCommands()}]: ");
            command = CheckCommand(Console.ReadLine());
            if (string.IsNullOrEmpty(command))
                Console.WriteLine("\nComando inválido\n");
        } while (command == "acessar" || string.IsNullOrEmpty(command));
       
        Console.WriteLine("---------------------------INTERFACE: FIM-------------------------------\n");

        return;
    }
}