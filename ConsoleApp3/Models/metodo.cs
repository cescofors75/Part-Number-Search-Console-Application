using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using MySql.Data.MySqlClient;

namespace ConsoleApp3.Models
{
    internal class metodo
    {
        public  bool ReferenceStocked(string ref2)
        {
            var context = new testContext();
            var countCrossref = (from o in context.StoreStockeds where o.RefEuro == ref2 select o).Count();
            if (countCrossref > 0) return true; else return false;


        }
        public void PrintRefEuroEan()
        {
            var context = new testContext();
            Console.Write("Enter Euro4x4 part number: ");
            string reference = Console.ReadLine();
            Console.Clear();
            Console.WriteLine();

            var result = context.EurocrossrefEans.Where(o => o.RefEuro == reference)
                          .Select(o => new { o.RefEuro, o.Eancode }).ToList().Distinct();
            foreach (var item in result)
            {
                Console.WriteLine("RefEuro " + item.RefEuro + " EAN " + item.Eancode);
            }

        }

        public void PrintStores(string ref2)
        {
            var context = new testContext();
            var countCrossref = (from o in context.StoreStockeds
                                 where o.RefEuro == ref2
                                 select o).Count();


            if (countCrossref > 0)
            {
                Console.WriteLine();
                /* var referenceEncontrada = context.Europroducts.FirstOrDefault(u => u.Reference == ref2);
                 Console.WriteLine(referenceEncontrada.Reference + "-" + referenceEncontrada.LibelleProduit + "-" + referenceEncontrada.PrixEuroHt);
                 Console.WriteLine();*/

                Console.WriteLine("Store name" + " \t \t\t\t\t \t\t\t\t \t\t\t" + "Price" + " \t " + "Date stocked" + " \t\t " + "Date update");
                foreach (var x in context.StoreStockeds.Where(u => u.RefEuro == ref2).OrderBy(x => x.Storeprice).ToList())
                {

                    // Console.WriteLine(x.StoreName + " \t " + Math.Round(Convert.ToDecimal(x.Storeprice),2, MidpointRounding.AwayFromZero).ToString("F") + " \t "+ x.DateStoked + " \t " + x.DateUptade);
                    Console.WriteLine(x.StoreName + " \t " + Convert.ToDecimal(x.Storeprice).ToString("F") + " \t " + x.DateStoked + " \t " + x.DateUptade);



                }


            }
            else
            {

                Console.WriteLine("Crossref not found VIA SQLSERVER EF ");

            }

        }

        public void SearchEan(string reference2)
        {

            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=td2q2019;";

            string query = "SELECT distinct europroducts.reference as ref, europroducts.libelleproduit AS description, europroducts.prixeuroht as prix , oemnumbers.legacyArticleId as legacy, articleean.eancode as ean ";
            query += "FROM `eurocrossref` INNER JOIN europroducts ON europroducts.reference=eurocrossref.REF_EURO INNER JOIN oemnumbers on oemnumbers.articleNumber=eurocrossref.REF_FRN ";
            query += "INNER JOIN articleean on articleean.legacyArticleId=oemnumbers.legacyArticleId WHERE REF_EURO='" + reference2 + "'";


            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;

            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();


                // Si se encontraron datos
                if (reader.HasRows)
                {
                    Reference reference = new Reference();
                    float f1 = 0;




                    while (reader.Read())
                    {

                        f1 = float.Parse(reader.GetString(2));
                        reference.refe = reader.GetString(0);
                        reference.description = reader.GetString(1);
                        reference.price = float.Parse(reader.GetString(2));
                        reference.legacy = reader.GetString(3);
                        reference.ean = reader.GetString(4);

                        Task<List<Store>> task = searchStores(reference.ean, f1);
                        task.Wait();



                        List<Store> list_sorted = task.Result.OrderByDescending(x => x.date_update).ToList();

                        // Console.WriteLine(task.Result);
                        foreach (var x in list_sorted.ToList())
                        {

                            Console.WriteLine("New store  " + x.name + "-" + x.country + "-" + x.price);
                            saveData(x, reference.refe);
                        }



                    }



                }
                else
                {
                    Console.WriteLine("BARCODE NOT FOUND DATABASE mysql");

                }

                databaseConnection.Close();
            }
            catch (Exception ex)
            {
                if (ex.Message is not null)
                {
                    Console.WriteLine(ex.Message);
                }
            }





        }



        // new function  print all the sto



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

        public async Task<List<Store>> searchStores(string ean, float price)
        {

            List<Store> stocked = new List<Store>();
            string Message2 = "";
            string api_key = "l1sb4dr7rnng4ftxp41smz54soubg2";
            //Define your baseUrl

            string baseUrl = "https://api.barcodelookup.com/v3/products?barcode=" + ean + "&formatted=y&key=" + api_key;
            //Have your using statements within a try/catch block
            try
            {
                //We will now define your HttpClient with your first using statement which will implements a IDisposable interface.
                using (HttpClient client = new HttpClient())
                {


                    //In the next using statement you will initiate the Get Request, use the await keyword so it will execute the using statement in order.
                    using (HttpResponseMessage res = await client.GetAsync(baseUrl))
                    {

                        //Then get the content from the response in the next using statement, then within it you will get the data, and convert it to a c# object.
                        using (HttpContent content = res.Content)
                        {
                            //Now assign your content to your data variable, by converting into a string using the await keyword.


                            var data = await content.ReadAsStringAsync();
                            //If the data isn't null return log convert the data using newtonsoft JObject Parse class method on the data.
                            if (content != null)
                            {

                                //Now log your data object in the con

#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
                                JToken jTokendes = JObject.Parse(data)["products"][0];



                                JToken jToken = JObject.Parse(data)["products"][0]["stores"];
                                int length = jToken.Count();
                                Store store = new Store();

                                int i = 0;
                                foreach (var item in jToken)
                                {
                                    JToken jToken2 = JObject.Parse(data)["products"][0]["stores"][i];

                                    store.country = jToken2["country"].ToString();
                                    store.name = jToken2["name"].ToString();
                                    store.price = jToken2["price"].ToString();
                                    store.date_update = jToken2["last_update"].ToString();
                                    i++;
                                    stocked.Add(new Store() { country = store.country, name = store.name, price = store.price, date_update = store.date_update });





                                }



                            }
                            else
                            {
                                Console.WriteLine("NOT Data----------BARCODE: " + ean);

                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

                // Console.WriteLine("Exception Hit---------BARCODE: " + ean + " NOT FOUND");

            }


            Task<string> task = Task.FromResult(Message2);
            return stocked;

        }




        public void saveData(Store storeSave, String reference)
        {
            String price_solved = storeSave.price.Replace('.', ',');
            var context = new testContext();
            

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



        public void PrintRefStocked()
        {
            var context = new testContext();
            var result = context.StoreStockeds.Select(m => m.RefEuro).Distinct().ToList();

            foreach (var item in result.OrderBy(x => x))
            {
                Console.WriteLine(item);
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
