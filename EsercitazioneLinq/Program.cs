using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esercitazione_Linq
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Persona> listOfPeople = CreaListaPersone();
            List<Veicolo> listOfVehicles = CreaListaVeicoli();

            ContaPerColore(listOfVehicles);
            MezziPerProprietario(listOfPeople, listOfVehicles);
            TestVeicoliPosseduti(listOfPeople, listOfVehicles);
        }


        /// <summary>
        /// Metodo che stampa una lista di oggetti Veicolo, con tutte le proprietà
        /// </summary>
        /// <param name="listOfVehicles"></param>
        private static void StampaVeicoli(List<Veicolo> listOfVehicles)
        {
            Console.WriteLine("Tabella Veicoli: ");
            foreach (var item in listOfVehicles)
            {
                Console.WriteLine($"{item.ID} - Targa: {item.Targa}, Colore: {item.Colore}, Prezzo: {item.Prezzo}, ID Proprietario: {item.IDProprietario}");
            }
        }
        /// <summary>
        /// Metodo che stampa una lista di oggetti Persona, con tutte le proprietà
        /// </summary>
        /// <param name="listOfPeople"></param>
        private static void StampaPersone(List<Persona> listOfPeople)
        {
            Console.WriteLine("Tabella Persone: ");
            foreach (var item in listOfPeople)
            {
                Console.WriteLine($"{item.ID} - {item.Nome} {item.Cognome} ({item.Nazione})");
            }
        }

        /// <summary>
        /// Testo il funzionamento dell'Extension Method VeicoliPosseduti, applicandolo a ciascuna persona
        /// della lista listOfPeople
        /// </summary>
        private static void TestVeicoliPosseduti(List<Persona> listOfPeople, List<Veicolo> listOfVehicles)
        {
            StampaPersone(listOfPeople);
            StampaVeicoli(listOfVehicles);
            foreach (Persona testPerson in listOfPeople)
            {
                List<Vehicle> veicoliPosseduti = testPerson.VeicoliPosseduti(listOfVehicles);

                Console.WriteLine($"(ID {testPerson.ID}) {testPerson.Nome} {testPerson.Cognome}: ");
                if (veicoliPosseduti.Count == 0) Console.WriteLine("Non possiede alcun veicolo.");
                else
                {
                    foreach (var item in veicoliPosseduti)
                    {
                        Console.WriteLine($"ID veicolo:{item.ID} \t Prezzo: {item.Prezzo} \t Targa: {item.Targa}");
                    }
                }
            }

        }

        /// <summary>
        /// A partire da una lista di Persone e una di Veicoli, calcola il prezzo medio dei veicoli posseduti
        /// da ciascun proprietario ed il loro peso complessivo. Le tabelle sono intuitivamente legate dall'IDProprietario,
        /// proprietà di Veicolo, uguale all'ID della Persona che è il proprietario del veicolo.
        /// </summary>
        /// <param name="listOfPeople"> Lista di Persone </param>
        /// <param name="listOfVehicles"> Lista di Veicoli</param>
        private static void MezziPerProprietario(List<Persona> listOfPeople, List<Veicolo> listOfVehicles)
        {
            StampaPersone(listOfPeople);
            StampaVeicoli(listOfVehicles);
            //Query Syntax

            var queryMezzi =
                from v in listOfVehicles
                group v by v.IDProprietario
                into vg
                select new
                {
                    IDProprietario = vg.Key,
                    PesoComplessivo = vg.Sum(m => m.Peso),
                    PrezzoMedio = vg.Average(m => m.Prezzo)
                }
                into datiVeicoli
                join p in listOfPeople
                on datiVeicoli.IDProprietario equals p.ID
                select new
                {
                    ID = p.ID,
                    Nome = p.Nome,
                    Cognome = p.Cognome,
                    PesoComplessivo = datiVeicoli.PesoComplessivo,
                    PrezzoMedio = datiVeicoli.PrezzoMedio
                };



            Console.WriteLine("Risultato MezziPerProprietario: ");
            foreach (var item in queryMezzi)
            {
                Console.WriteLine($"{item.Nome} {item.Cognome} (ID {item.ID}): Prezzo medio veicoli posseduti: {item.PrezzoMedio} , Peso Complessivo: {item.PesoComplessivo}");
            }


        }

        /// <summary>
        /// Metodo che conta il numero di veicoli per colore.
        /// </summary>
        /// <param name="listOfVehicles"> L'input è una lista di veicoli, aventi un campo Colore</param>
        private static void ContaPerColore(List<Veicolo> listOfVehicles)
        {
            StampaVeicoli(listOfVehicles);
            //Method Syntax
            var queryByColor = listOfVehicles
                .GroupBy(m => m.Colore)
                .Select(m => new {
                    Colore = m.Key,
                    NumeroAuto = m.Sum(p => 1)
                });
            Console.WriteLine("Risultato di ContaPerColore: ");
            foreach (var m in queryByColor)
            {
                Console.WriteLine($"{m.Colore} - {m.NumeroAuto}");
            }
        }

        private static List<Veicolo> CreaListaVeicoli()
        {
            List<Veicolo> listOfVehicles = new List<Veicolo>();
            listOfVehicles.Add(new Veicolo { ID = 1, Targa = "AX231CD", Peso = 320, Colore = "Blu", Prezzo = 11900, IDProprietario = 1 });
            listOfVehicles.Add(new Veicolo { ID = 2, Targa = "GT789JS", Peso = 473, Colore = "Nero", Prezzo = 30989, IDProprietario = 1 });
            listOfVehicles.Add(new Veicolo { ID = 3, Targa = "BS859FI", Peso = 215, Colore = "Rosso", Prezzo = 3900, IDProprietario = 6 });
            listOfVehicles.Add(new Veicolo { ID = 4, Targa = "DF123PS", Peso = 350, Colore = "Blu", Prezzo = 7500, IDProprietario = 5 });
            listOfVehicles.Add(new Veicolo { ID = 5, Targa = "NS784HS", Peso = 421, Colore = "Verde", Prezzo = 20540, IDProprietario = 2 });
            return listOfVehicles;
        }

        private static List<Persona> CreaListaPersone()
        {
            List<Persona> listOfPeople = new List<Persona>();
            listOfPeople.Add(new Persona { ID = 1, Nome = "Agata", Cognome = "Bruni", Nazione = "Italia" });
            listOfPeople.Add(new Persona { ID = 2, Nome = "John", Cognome = "Snow", Nazione = "Irlanda" });
            listOfPeople.Add(new Persona { ID = 3, Nome = "Francis", Cognome = "Lurac", Nazione = "Francia" });
            listOfPeople.Add(new Persona { ID = 4, Nome = "Giuseppe", Cognome = "Pirelli", Nazione = "Italia" });
            listOfPeople.Add(new Persona { ID = 5, Nome = "Antoine", Cognome = "Rouge", Nazione = "Francia" });
            listOfPeople.Add(new Persona { ID = 6, Nome = "Michael", Cognome = "Jet", Nazione = "Inghilterra" });

            return listOfPeople;
        }
    }
}
