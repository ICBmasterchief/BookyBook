namespace BookyBook.Presentation;

class Program
{
    static void Main()
        {
            var menu = new MainMenu();

            while (!menu.Exit)
            {
                menu.ShowMainMenu();
            }
        }
}
