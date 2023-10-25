// See https://aka.ms/new-console-template for more information

using ConsoleApp.Models;
using ConsoleApp3.Models;

#pragma warning disable CS8604
#pragma warning disable CS8600


    var context = new testContext();
    var metodo = new metodo();
    var screen = new Screen();

    screen.init();
    bool exit = false;
    do
    {
        Console.Write("Enter  part number: ");
        string reference = Console.ReadLine();
        Console.Clear();
        Console.WriteLine();

        if (reference == "list") {
            Console.WriteLine("Listpart number stocked:");
            metodo.PrintRefStocked();
            
        }

        if (reference == "ean")
        {
            Console.WriteLine("List EAN from  part number: ");
            
            metodo.PrintRefEuroEan();
            
        }
        if (reference == "exit") exit = true; 



        metodo.Searchreference(reference);

        if (metodo.ReferenceStocked(reference))
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine(" part number stocked");
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            metodo.PrintStores(reference);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine("New part number to stocked yours stores");
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            metodo.SearchEan(reference);
        }
        Console.ForegroundColor = ConsoleColor.White;
        Console.BackgroundColor = ConsoleColor.DarkBlue;

     

        
    } while (!exit);







