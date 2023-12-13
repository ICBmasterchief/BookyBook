using Spectre.Console;

namespace BookyBook.Presentation;
public class MainMenu
{
    public bool Exit = false;
    public void ShowMainMenu()
    {
        AnsiConsole.Clear();

        var option = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Main Menu")
                .PageSize(10) // Número de opciones visibles en cada página
                .AddChoices("Log in or Sign Up")
                .AddChoices("Search for books")
                .AddChoices("Show book catalog")
                .AddChoices("Exit")
        );

        ProcessOption(option);
    }

    private void ProcessOption(string option)
    {
        switch (option)
        {
            case "Log in or Sign Up":
                // Lógica para iniciar sesión o registrarse
                //var text = AnsiConsole.Prompt(new TextPrompt<string>("YOU ARE LOGGING IN"));
                Console.WriteLine("");
                AnsiConsole.MarkupLine("YOU ARE LOGGING IN");
                Thread.Sleep(5000);
                AnsiConsole.Clear();
                break;
            case "Search for books":
                // Lógica para buscar libros
                AnsiConsole.MarkupLine("YOU ARE SEARCHING FOR BOOKS");
                Thread.Sleep(5000);
                AnsiConsole.Clear();
                break;
            case "Show book catalog":
                // Lógica para ver catálogo de libros
                AnsiConsole.MarkupLine("HERE YOU HAVE ALL THE BOOKS");
                Thread.Sleep(5000);
                AnsiConsole.Clear();
                break;
            case "Exit":
                AnsiConsole.MarkupLine("EXITING THE APP IN 5 SECONDS");
                Thread.Sleep(5000);
                AnsiConsole.Clear();
                Exit = true;
                //Environment.Exit(0);
                break;
            default:
                AnsiConsole.MarkupLine("[red]Invalid option. Try again.[/]");
                break;
        }
    }
}

    