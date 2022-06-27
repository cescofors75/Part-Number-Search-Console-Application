using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Models
{
    internal class Screen
    {
        public void init()
        {

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Clear();
            Console.WriteLine();





            Console.WriteLine("                            __ __       __ __                      __   ");
            Console.WriteLine("  ___  __  ___________     / // / _  __/ // /    ____  ____ ______/ /______");
            Console.WriteLine(" / _ \\/ / / / ___/ __ \\   / // /_| |/_/ // /_   / __ \\/ __ `/ ___/ __/ ___/");
            Console.WriteLine("/  __/ /_/ / /  / /_/ /  /__  __/>  </__  __/  / /_/ / /_/ / /  / /_(__  )");
            Console.WriteLine("\\___/\\__,_/_/   \\____/     /_/ /_/|_|  /_/    / .___/\\__,_/_/   \\__/____/");
            Console.WriteLine("                                             /_/ ");


            Console.WriteLine("Comparative Price Version 2022");
            Console.WriteLine();
            Console.WriteLine("Options :" + "\n" + "Enter 'list' to print Euro4x4 parts stocked" + "\n" + "Enter 'ean' to print list EAN from Euro4x4parts parts\nPress 'exit' to Quit and ");
            Console.WriteLine();
        }

    }
}
