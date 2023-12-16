namespace BookyBook.Presentation;

class Program
{
    static void Main()
        {
            var menu = new MainMenu();
            menu.InitializeData();
            while (!menu.Exit)
            {
                menu.ShowMainMenu();
            }
        }
}
