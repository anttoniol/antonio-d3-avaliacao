using System;
using Application;


ConsoleInterface ci = new ConsoleInterface();
ci.start();
public class ConsoleInterface
{
    private String[] commands = { "acessar", "cancelar" };

    private String returnCommands()
    {
        return String.Join("/", commands);
    }
    private String CheckCommand(String command)
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
        String command = "acessar";
        while (command == "acessar")
        {
            Console.WriteLine($"Informe um comando [{returnCommands()}]: ");
            command = CheckCommand(Console.ReadLine());
            if (String.IsNullOrEmpty(command))
            {
                Console.WriteLine("\nComando inválido\n");
                command = "acessar";
            }
        }
        Console.WriteLine("---------------------------INTERFACE: FIM-------------------------------\n");

        return;
    }
}