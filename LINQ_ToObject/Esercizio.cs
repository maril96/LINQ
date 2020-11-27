using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LINQ_ToObject
{
    public class Esercizio
    {
        //Creazione liste
        public static List<Product> CreateProductList()
        {
            var lista = new List<Product>
            {
                new Product{Id=1, Name="Telefono", UnitPrice=300.99},
                new Product{Id=2, Name="Computer", UnitPrice=800},
                new Product{Id=3, Name="Tablet", UnitPrice=550.99}
            };

            return lista;
        }

        public static List<Order> CreateOrderList()
        {
            var lista = new List<Order>();
            var order = new Order {Id=1, ProductId=1, Quantity=4};
            lista.Add(order);
            var order1 = new Order { Id = 2, ProductId = 2, Quantity =1};
            lista.Add(order1);
            var order2 = new Order {Id=3, ProductId=1, Quantity=1 };
            lista.Add(order2);


            return lista;
        }

        //Esecuzione immediata e ritardata
        public static void DeferredExecution()
        {
            var productList = CreateProductList();
            var orderList = CreateOrderList();
            //Vediamo i risultati
            foreach (var p in productList)
            {
                Console.WriteLine("{0} - {1} - {2}", p.Id, p.Name, p.UnitPrice);
            }

            foreach(var o in orderList)
            {
                Console.WriteLine("{0} - {1} - {2}",o.Id, o.ProductId, o.Quantity);
            }

            //Esecuzione differita:
            //Creazione Query (Method Syntax)
            var list = productList
                .Where(product => product.UnitPrice >= 400) //Siccome sto leggendo productList che è una lista di Product, lui capisce che product è di tipo Product
                .Select(p => new { Nome=p.Name, Prezzo =p.UnitPrice});
            //Filtro la productList prendendo quelli che hanno UnitPrice>=400.
            //Poi me li mette in una lista in cui vedo solo Nome e Prezzo

            //La Where funziona come un foreach: per ogni product in productList verifica se l'espressione a dx è verificata e in questo caso lo "seleziona"
            //La Select viene applicata alla nuova lista di prodotti ottenuta tramite Where
            //Il risultato è un IEnumerable, il cui argomento è l'Anonymous Type a dx nella select


            //Aggiungo prodotto alla lista.
            productList.Add(new Product { Id = 4, Name = "Bici", UnitPrice = 500.99 });

            //Risultati: faccio un foreach che chiama quello che risulta dalla Query
            //In questo caso vedo anche la Bici perchè eseguo la Query solo nel momento in cui la chiamo nel foreach
            Console.WriteLine("Esecuzione Differita:");
            foreach (var p in list)
            {
                Console.WriteLine("{0} - {1}",p.Nome, p.Prezzo);
            }

            //Esecuzione Immediata:
            //Creazione Query (Method Syntax)
            var list1 = productList
                .Where(p => p.UnitPrice >= 400) 
                .Select(p => new { Nome = p.Name, Prezzo = p.UnitPrice })
                .ToList();
            //Io so che list1 è una lista, ma senza fare il ToList ho visibilità solo limitata ad IEnumerable.
            //Il ToList fa upcast e aumenta la visibilità. Per poter eseguire il ToList in questo caso devo eseguire la Query,
            //é per questo che l'esecuzione è immediata.

            productList.Add(new Product { Id = 5, Name = "Divano", UnitPrice = 450.99 });

            //Risultati:
            Console.WriteLine("Esecuzione immediata:");
            foreach (var p in list1) 
            {
                Console.WriteLine("{0} - {1}", p.Nome, p.Prezzo);
            }
            //In questo caso non vedo Divano perchè è stato aggiunto dopo l'esecuzione, che avviene nel momento in cui chiamiamo il ToList


        }
        //Sintassi
        public static void Syntax()
        {
            var productList = CreateProductList();
            var orderList = CreateOrderList();

            //Method Syntax
            var methodList = productList
                .Where(p => p.UnitPrice <= 600)
                .Select(p => new { Nome = p.Name, Prezzo = p.UnitPrice })
                .ToList();

            //Query Syntax
            var queryList =
               ( from p in productList
                 where (p.UnitPrice <= 600)
                 select new { Nome = p.Name, Prezzo = p.UnitPrice }).ToList();

            //queste due forme di query fanno la stessa cosa.

            foreach (var item in queryList)
            {
                Console.WriteLine($"{item.Nome}, {item.Prezzo}");
            }
        }

        //Operatori
        public static void Operators()
        {
            var productList = CreateProductList();
            var orderList = CreateOrderList();

            //Scrittura a schermo delle liste
            Console.WriteLine("Lista Prodotti:");
            foreach (var p in productList)
            {
                Console.WriteLine($"{p.Id} - {p.Name} - {p.UnitPrice}");
            }

            Console.WriteLine("Lista Ordini:");
            foreach (var o in orderList)
            {
                Console.WriteLine($"{o.Id} - {o.ProductId} - {o.Quantity}");
            }


            //Filtro OfType
            Console.WriteLine("OfType:");
            var list = new ArrayList();
            list.Add(productList);
            list.Add("Ciao!");
            list.Add(143);
            //ho una lista di oggetti diversi e voglio filtrare il tipo
            var typeQuery =
                from item in list.OfType<int>()
                select item; //sto selezionando la roba che c'è in lista che ha tipo intero

            foreach (var item in typeQuery)
            {
                Console.WriteLine(item);
            }

            var typeQuery2 =
                from item in list.OfType<List<Product>>()
                select item; //sto selezionando la roba che c'è in lista che ha tipo intero
            //poi potrei stampare i campi di ogni List<Product> che c'è nella mia list


            //Element:
            Console.WriteLine("Elementi:");
            string[] empty = { };
            //var el1 = empty.First(); //dà errore! Perchè empty non contiene nessun elemento
            var el1 = empty.FirstOrDefault(); //mi dà una stringa vuota, che è il valore di default
            Console.WriteLine($"{el1}");

            var p1 = productList.ElementAt(0).Name;
            Console.WriteLine(p1); //stampo il nome dell'elemento in posizione 0 in productList

            //Ordinamento
            Console.WriteLine("Ordinamento:");

            productList.Add(new Product { Id = 4, Name = "Tablet", UnitPrice = 500 });
            productList.Add(new Product { Id = 5, Name = "Telefono", UnitPrice = 700 });
            //voglio ordinare i prodotti per nome e prezzo
            var orderedList =
                from p in productList
                orderby p.Name ascending, p.UnitPrice descending //"ThenBy"
                select new { Nome = p.Name, Prezzo = p.UnitPrice };

            var orderedList2 = productList
                .OrderBy(p => p.Name)
                .ThenByDescending(p => p.UnitPrice)
                .Select(p => new { Nome = p.Name, Prezzo = p.UnitPrice });

            var orderedList3 = productList
                .OrderBy(p => p.Name)
                .ThenByDescending(p => p.UnitPrice)
                .Select(p => new { Nome = p.Name, Prezzo = p.UnitPrice })
                .Reverse();

            Console.WriteLine("Lista 1");
            foreach (var item in orderedList)
            {
                Console.WriteLine($"{item.Nome}-{item.Prezzo}");
            }


            Console.WriteLine("Lista 2");
            foreach (var item in orderedList2)
            {
                Console.WriteLine($"{item.Nome}-{item.Prezzo}");
            }

            Console.WriteLine("Lista 3");
            foreach (var item in orderedList3)
            {
                Console.WriteLine($"{item.Nome}-{item.Prezzo}");
            }

            //Quantificatori
            Console.WriteLine("Quantificatori: ");
            var hasProductWithT = productList.Any(p => p.Name.StartsWith("T"));
            var allProductsWithT = productList.All(p => p.Name.StartsWith("T"));
            Console.WriteLine("Any? {0} \t All? {1}", hasProductWithT, allProductsWithT);

            //Groupby
            Console.WriteLine("GroupBy:");
            //raggruppiamo order per ProductId
            //Query Syntax
            var groupByList =
                from o in orderList
                group o by o.ProductId into groupList
                select groupList;

            //il groupby dà un oggetto che eredita da IGrouping: per ogni chiave restituisce una lista di oggetti ad essa associati.

            foreach (var order in groupByList)
            {
                Console.WriteLine(order.Key); //la chiave in questo caso è il ProductId.
                foreach (var item in order)
                {
                    Console.WriteLine($"\t{item.ProductId} - {item.Quantity}");
                }
            }
            //In groupByList ci sono degli oggetti, ognuno dei quali ha una chiave, e una collection di record associati a quella chiave.
            //Nel nostro caso la chiave order.Key è ProductId, inoltre se facciamo un foreach su order vediamo tutti quegli ordini che hanno come chiave order.Key (cioè quelli in cui il ProductId è proprio order.Key)

            //Method Syntax
            var groupByList2 =
                orderList
                .GroupBy(o => o.ProductId);
            //orderList è una lista di Ordini; le liste hanno un Extension Method, GroupBy, che restituisce un oggetto che implementa un IEnumerable, che contiene degli oggetti che implementano IGrouping

            foreach (var order in groupByList2)
            {
                Console.WriteLine(order.Key);
                foreach (var item in order)
                {
                    Console.WriteLine("\t{0} - {1}", item.ProductId, item.Quantity);
                }
            }

            //GroupBy con funzioni di aggregazione
            //Raggruppiamo gli ordini per prodotto e ricaviamo la somma delle quantità
            Console.WriteLine("GroupBy con aggregato: ");
            //Method Syntax
            var sumQuantityByProduct =
                orderList
                .GroupBy(p => p.ProductId) //raggruppo per ProductId
                .Select(lista => new //risultato finale: una lista con Key e quantità
                {
                    Id = lista.Key,
                    Quantities = lista.Sum(p => p.Quantity)
                });

            foreach (var item in sumQuantityByProduct)
            {
                Console.WriteLine("{0} - {1}", item.Id, item.Quantities);
            }

            //Query Syntax
            var sumQuantityByProduct2 =
                from o in orderList
                group o by o.ProductId into list3
                select new { Id = list3.Key, Quantities = list3.Sum(x => x.Quantity) };

            foreach (var item in sumQuantityByProduct2)
            {
                Console.WriteLine("{0} - {1}", item.Id, item.Quantities);
            }


            //Join: in Linq il Join da solo è sempre inteso come INNER JOIN
            //recuperiamo i prodotti che hanno ordini
            //Nome - Id Ordine - Quantità
            Console.WriteLine("Join: ");

            //Method Syntax
            var joinList = productList
                .Join(
                orderList, //seconda lista
                p => p.Id, //chiave prima lista
                o => o.ProductId, //chiave seconda lista
                (p, o) => new
                {
                    Nome = p.Name,
                    IdOrdine = o.Id,
                    Quantity = o.Quantity
                }
                );

            foreach (var item in joinList)
            {
                Console.WriteLine($"{item.Nome} - {item.IdOrdine} - {item.Quantity}");
            }

            //Query syntax
            var joinedList2 =
                from p in productList
                join o in orderList
                on p.Id equals o.ProductId
                select new
                {
                    Nome = p.Name,
                    IdOrdine = o.Id,
                    Quantity = o.Quantity
                };

            foreach (var item in joinedList2)
            {
                Console.WriteLine($"{item.Nome} - {item.IdOrdine} - {item.Quantity}");
            }

            //GroupJoin: mette insieme la Join e il GroupBy
            //recuperiamo gli ordini per prodotto e sommiamo le quantità
            //Nome Prodotto - Quantità totale

            Console.WriteLine("GroupJoin:");
            //Method Syntax
            var groupJoinList = productList
                .GroupJoin(
                    orderList,
                    p=> p.Id, 
                    o=> o.ProductId, //faccio la join su questi due campi
                    (p,o) => 
                        new {
                        NomeProdotto = p.Name, //raggruppo per gli id ma stampo in base ai nomi
                        TotalQuantity = o.Sum(o=>o.Quantity)});

            foreach (var item in groupJoinList)
            {
                Console.WriteLine($"{item.NomeProdotto} - {item.TotalQuantity}");
            }

            var groupJoinList2 =
                from p in productList
                join o in orderList
                on p.Id equals o.ProductId
                into gr //in gr mi salvo "la relazione" tra le tabelle, infatti posso usarlo solo per fare le operazioni.
                select new //Qui nel select uso il gr solo per fare le operazioni, ma p è quello originario, non quello joinato.
                {
                    Prodotto = p.Name,
                    Quantity = gr.Sum(o => o.Quantity) //faccio riferimento alla tabella di ordini
                };
            Console.WriteLine("Query Syntax:");
            foreach (var item in groupJoinList2)
            {
                Console.WriteLine("{0} - {1}", item.Prodotto, item.Quantity);
            }

            //Come fare l'inner join che vedremmo su SQL, qui vediamo solo i prodotti la cui quantità non è 0
            //Mentre prima vedevamo tutti i prodotti e quelli che non stavano nella join settava il valore di quantità a 0 che è quello di default
            var lista4 =
                from o in orderList
                group o by o.ProductId
                into gr
                select new
                {
                    ProdottoId = gr.Key,
                    Quantity = gr.Sum(o => o.Quantity)
                }
                into gr1 //qui in gr1 non vedo gli elementi che non stanno nella join
                join p in productList
                on gr1.ProdottoId equals p.Id
                select new //Qui p è davvero quello joinato perchè ho fatto il join vero tra p e una effettiva tabella gr
                {
                    p.Name,
                    gr1.Quantity
                };

            Console.WriteLine("InnerJoin vero");
            foreach (var item in lista4)
            {
                Console.WriteLine($"{item.Name} - {item.Quantity}");
            }



        }
    }
}
