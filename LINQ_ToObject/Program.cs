using System;
using System.Collections.Generic;
using System.Linq;

namespace LINQ_ToObject
{
    class Program
    {
        static void Main(string[] args)
        {

            //Esercizio.DeferredExecution();
            //TryingOverrideDistinct();
            //Esercizio.Operators();
            Esercizio.Syntax();


        }

        private static void TryingOverrideDistinct()
        {
            var products = new List<Product>
            {
                new Product{Id=1, Name="Farina", UnitPrice=1},
                new Product{Id=2, Name="Uova", UnitPrice=2.5},
                new Product{Id=1, Name="Farina", UnitPrice=1},
            };

            int resultCount1 = products.Select(s => s).Distinct().Count();
            int resultCount2 = products.Select(s => new { s.Id, s.Name, s.UnitPrice }).Distinct().Count();

            Console.WriteLine($"{resultCount1} - {resultCount2}");
            //il primo ne vede 3 diversi, il secondo 2 perchè legge solo i campi
            //potrei anche implementare una classe che implementi un comparer e passarne un'istanza a Distinct
            int resultCount3= products.Select(s => s).Distinct(new ProductComparer()).Count();
            Console.WriteLine($"{resultCount3}");
       
            //Distinct() usa il comportamento di defoult. Distinct(ClassInstance : IEqualityComparer<>) usa le funzioni che trova lì dentro.
            
        }
    }
}
