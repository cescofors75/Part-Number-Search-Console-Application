using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3.Models
{
    internal class metodo
    {
       public void print_all_stores()

        {
            var list = getAllStores();
            foreach (var item in list)
            {
                Console.WriteLine(item.name + "-" + item.country + "-" + item.price);
            }








        }
        public List<Store> getAllStores()
        {

            List<Store> Stores = new List<Store>();
            var context = new testContext();
            foreach (var store in context.StoreStockeds) {


            Stores.Add(new Store() { country = store.StoreCountry, name = store.StoreName, price = store.Storeprice.ToString(), date_update = store.DateUptade.ToString() });
            }
            return Stores;
        }



        public void screen()
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
        public void saveData(Store storeSave, String reference)
        {
            String price_solved = storeSave.price.Replace('.', ',');
            using (var context = new testContext())
            {

                var t = new StoreStocked
            {

                StoreName = storeSave.name,
                StoreCountry = storeSave.country,
                Storeprice = Convert.ToDouble(price_solved),
                DateStoked = DateTime.Now,
                DateUptade = Convert.ToDateTime(storeSave.date_update),
                RefEuro = reference
            };
            context.StoreStockeds.Add(t);
            context.SaveChanges();
            Console.WriteLine("Data store stocked sqlserver.");

            }
        }

        public void Searchreference(string reference)
        {
            using (var context = new testContext())
            {

                Console.OutputEncoding = System.Text.Encoding.UTF8;
            var countReference = (from o in context.Europroducts
                                  where o.Reference == reference
                                  select o).Count();

            if (countReference > 0)
            {

                var referenceEncontrada = context.Europroducts.FirstOrDefault(u => u.Reference == reference);
                Console.WriteLine(referenceEncontrada.Reference + "-" + referenceEncontrada.LibelleProduit + "-" + Convert.ToDecimal(referenceEncontrada.PrixEuroHt).ToString("F") + "€");
                Console.WriteLine();


            }
            else
            {

                Console.WriteLine("Ref not found VIA SQLSERVER EF");
                Console.WriteLine();
            }
            }
        }
    }
}
