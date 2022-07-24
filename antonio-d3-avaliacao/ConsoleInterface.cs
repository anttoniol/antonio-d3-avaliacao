using System;
using Application;


ConsoleInterface ci = new ConsoleInterface();
ci.start();
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