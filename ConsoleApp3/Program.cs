// See https://aka.ms/new-console-template for more information

using ConsoleApp3.Models;
using Newtonsoft.Json.Linq;
using MySql.Data.MySqlClient;


#pragma warning disable CS8604
#pragma warning disable CS8600
using (var context = new testContext())
{
   
    Console.WriteLine("Comparative Price Version 2022");
    Console.WriteLine("Enter reerence Euro4x4: ");


    string reference = Console.ReadLine();


     Searchreference(reference);
    
    if (ReferenceStocked(reference))
    {
        Console.WriteLine("Reference stores stocked");
        PrintStores(reference);
    }
    else
    {
        Console.WriteLine("New reference to stocked stores");
        SearchEan(reference);
    }
    

    void Searchreference(string reference)
    {

        var countReference = (from o in context.Europroducts
                              where o.Reference == reference
                              select o).Count();

        if (countReference > 0)
        {

            var referenceEncontrada = context.Europroducts.FirstOrDefault(u => u.Reference == reference);
            Console.WriteLine(referenceEncontrada.Reference);
            Console.WriteLine(referenceEncontrada.LibelleProduit);
            Console.WriteLine(referenceEncontrada.PrixEuroHt);


        }
        else
        {

            Console.WriteLine("Ref not found VIA SQLSERVER EF");
        }
    }
    bool ReferenceStocked(string ref2)
    {
        var countCrossref = (from o in context.StoreStockeds
                             where o.RefEuro == ref2
                             select o).Count();


        if (countCrossref > 0)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    void PrintStores(string ref2)
    {
        var countCrossref = (from o in context.StoreStockeds
                             where o.RefEuro == ref2
                             select o).Count();


        if (countCrossref > 0)
        {
             foreach (var x in context.StoreStockeds.Where(u => u.RefEuro == ref2))
             {
                 // Console.WriteLine(x.Reference);
                 Console.WriteLine(x.StoreName);

                 //searchEan(x.Reference);

             }

            
        }
        else
        {

            Console.WriteLine("Crossref not found VIA SQLSERVER EF ");
           
        }

    }
    void SearchEan(string reference2)
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

                   

                    List<Store> list_sorted = task.Result.OrderBy(x=>x.price).ToList();

                    // Console.WriteLine(task.Result);
                    foreach (var x in list_sorted.ToList())
                    {

                        Console.WriteLine(x.name + "-" + x.country + "-" + x.price);
                        saveData(x,reference.refe);   
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



    



    async Task<List<Store>>searchStores(string ean, float price)
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
                                store.date_update= jToken2["last_update"].ToString();
                                i++;
                                stocked.Add(new Store() { country = store.country, name = store.name, price= store.price, date_update=store.date_update});
                                
                               

                                

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



    void saveData(Store storeSave, String reference)
{
        String price_solved = storeSave.price.Replace('.', ',');


    var t = new StoreStocked
    {
        
        StoreName = storeSave.name,
        StoreCountry = storeSave.country,
        Storeprice= Convert.ToDouble(price_solved),
        DateStoked = DateTime.Now,
        DateUptade = Convert.ToDateTime(storeSave.date_update),
        RefEuro=reference
    };
    context.StoreStockeds.Add(t);
    context.SaveChanges();
        

        
}

}




/*

    foreach (var personas in context.Personas.Take(1)) //Tolist
    {
     Console.WriteLine(personas.Name+" "+personas.Telf);
    //  Console.WriteLine(personas.Telf);
    }
  var count = context.Personas.Count();
    Console.WriteLine(count);
    var person = (from e in context.Personas
                  join p in context.Pedidos
                  on e.Id equals p.IdPersona
                 
                  where e.Id == 1
                  select new
                  {
                      ID = e.Id,
                      FirstName = e.Name,
                      Telf = e.Telf,
                      Pedido_id = p.Id,
                      Total = p.Total
                  }).ToList();
    foreach (var p in person)
    {
        Console.WriteLine("{0} {1} {2} {3} {4}", p.ID, p.FirstName, p.Telf, p.Pedido_id, p.Total);
    }
  
    //db.Products.OrderByDescending(p => p.ID).FirstOrDefault().ID;
    var person2 = context.Personas.OrderBy(p => p.Id).Last();
    Console.WriteLine("Enter name: ");
    var nombre = Console.ReadLine();
    Console.WriteLine("Enter nº telf: ");
    var telf = Console.ReadLine();
    var t = new Persona
    {
        Id = person2.Id + 1,
        Name = nombre, 
        Telf = telf
    };
    context.Personas.Add(t);
    context.SaveChanges();
    Console.WriteLine("Datos guardados");
    var  per = context.Personas.Find(2);
    Console.WriteLine(per.Name);
    var per2 = context.Personas.FirstOrDefault(u => u.Name == "Monika");
    Console.WriteLine(per2.Telf);
    foreach (var personas in context.Personas.ToList())
    {
        Console.WriteLine(personas.Name + " - " + personas.Telf);
        //  Console.WriteLine(personas.Telf);
    }
   
    var store = new Store();
    store.name = "AUTODOC FR";
    store.price = "110€";
    Console.WriteLine(store.name+ " -" + store.price);

 
 void searchEan2(string oem)
    {
        /*   var person = (from e in context.Personas
                         join p in context.Pedidos
                         on e.Id equals p.IdPersona
                         where e.Id == 1
                         select new
                         {
                             ID = e.Id,
                             FirstName = e.Name,
                             Telf = e.Telf,
                             Pedido_id = p.Id,
                             Total = p.Total
                         }).ToList();
           foreach (var p in person)
           {
               Console.WriteLine("{0} {1} {2} {3} {4}", p.ID, p.FirstName, p.Telf, p.Pedido_id, p.Total);

               // await searchStores(p.Ean)
           }*/





