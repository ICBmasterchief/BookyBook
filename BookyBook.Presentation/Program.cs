namespace BookyBook.Presentation;

class Program
{
    static void Main()
        {
            Console.WriteLine("Hello, World!");
            var menu = new MainMenu();

            while (!menu.Exit)
            {
                menu.ShowMainMenu();
            }
        }
}
