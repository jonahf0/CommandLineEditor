using System;
using MainPage;

 class App
{
    static void Main(string[] args)
    {

        if (args.Length > 0)
        {
            MainPageWithStarter NewApp = new MainPageWithStarter(args[0]);
            NewApp.Run();
        }

        else
        {
            MainPageWithoutStarter NewApp = new MainPageWithoutStarter();

            NewApp.Run();
        }

    }
}
