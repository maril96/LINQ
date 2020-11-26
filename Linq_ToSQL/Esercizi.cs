using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq_ToSQL
{

    public class Esercizi
    {
        const string connectionString = @"Persist Security Info = False; Integrated Security = True; Initial Catalog = CinemaDb; Server = WINAPUXGGIRX7PJ\SQLEXPRESS ";

        //Selezionare i film
        public static void SelectMovies()
        {
            //Mi serve creare un nuovo DataContext per andare a mappare
            using (CinemaDataContext db = new CinemaDataContext(connectionString))
            {
                //db farà le veci del DataBase vero perchè conterrà tutta la mappatura

                foreach (var movie in db.Movies)
                {
                    //La relazione tra tabelle viene simulata tramite l'aggiunta alla tabella di una lista
                    Console.WriteLine("{0} - {1} ({2}), {3} min.", movie.ID, movie.Titolo, movie.Genere, movie.Durata);
                }

            }
            //Li stampa: la mappatura ha funzionato!:O

        }

        //Filtriamo i film

        public static void FilterMovieByGenere()
        {
            using (CinemaDataContext db = new CinemaDataContext(connectionString))
            {
                //Questo DataContext potrei crearlo direttamente nel main, ora lo mettiamo in ogni metodo
                //in modo che poi quando li commentiamo ed eseguiamo uno per volta siamo sicuri che li esegua

                //Mostro cosa c'è all'inizio
                foreach (var movie in db.Movies)
                {
                    Console.WriteLine("{0} - {1} ({2}), {3} min.", movie.ID, movie.Titolo, movie.Genere, movie.Durata);
                }
                //Poi faccio il filtraggio...

                //Query: filtro in base al genere
                Console.WriteLine("Genere:");
                string genere = Console.ReadLine();
                //La mia lista ora è una Table di Movy, gestita da un DataContext, ma il comando è uguale
                IQueryable<Movy> moviesFiltered =
                    from m in db.Movies
                    where m.Genere == genere
                    select m;

                foreach (var item in moviesFiltered)
                {
                    Console.WriteLine($"{item.ID} - {item.Titolo} ({item.Genere}) {item.Durata} min");

                }
            }

        }

        //Inserimento record
        public static void InsertMovie()
        {
            using (CinemaDataContext db = new CinemaDataContext(connectionString))
            {
                Console.WriteLine("Tabella iniziale");
                SelectMovies();

                var movieToInsert = new Movy();
                movieToInsert.Titolo = "Lala land";
                movieToInsert.Genere = "Romantico";
                movieToInsert.Durata = 123;

                db.Movies.InsertOnSubmit(movieToInsert);
                //Questo verrà davvero inserito nel momento in cui si chiamerà il SubmitChanges.

                try
                {
                    db.SubmitChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                Console.WriteLine("Tabella finale");
                SelectMovies();
            }



        }

        public static void DeleteMovie()
        {
            using (CinemaDataContext db = new CinemaDataContext(connectionString))
            {

                var movieToDelete = db.Movies.SingleOrDefault(m => m.ID == 2);
                //sto selezionando dalla tabella un singolo elemento, quello che verifica la condizione.
                //Mi restituisce null se non trova niente
                if (movieToDelete != null)
                {
                    db.Movies.DeleteOnSubmit(movieToDelete);
                }

                try
                {
                    db.SubmitChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }


            }

        }

        public static void UpdateMovieByTitle()
        {
            using (CinemaDataContext db= new CinemaDataContext(connectionString))
            {
                Console.WriteLine("Titolo del film da aggiornare:");
                string daAggiornare = Console.ReadLine();

                IQueryable<Movy> filmByTitle =
                    from film in db.Movies
                    where film.Titolo == daAggiornare
                    select film;

                Console.WriteLine("I film trovati sono {0}", filmByTitle.Count());

                if (filmByTitle.Count() == 0 || filmByTitle.Count()>1) return;

                SelectMovies();

                Console.WriteLine("Scrivere i valori aggiornati:");
                Console.WriteLine("Titolo:");
                string titolo = Console.ReadLine();
                Console.WriteLine("Genere:");
                string genere = Console.ReadLine();
                Console.WriteLine("Durata:");
                int durata = Int32.Parse(Console.ReadLine());

                
                foreach (var f in filmByTitle)
                {
                    f.Titolo = titolo;
                    f.Genere = genere;
                    f.Durata = durata;
                }
                //ora andiamo a fare l'update sul db e gestire l'eventuale concorrenza

                try
                {
                    Console.WriteLine("Premi un tasto per inviare le modifiche al db...");
                    Console.ReadKey();
                    db.SubmitChanges(ConflictMode.FailOnFirstConflict);
                //la ConflictMode si usa per gestire tutti i Conflitti di Concurrency 
                }
                catch (ChangeConflictException e)
                {
                    Console.WriteLine("Concurrency error");
                    Console.WriteLine(e);
                    Console.ReadKey();
                    //qui posso risolvere i conflitti scegliendo se modificare o no il db (o farlo parzialmente)
                    db.ChangeConflicts.ResolveAll(RefreshMode.OverwriteCurrentValues);
                    //db.SubmitChanges(); va rimesso qui se scegliamo KeepCurrentValues o KeepChanges,
                    //perchè lui non lo fa prima perchè trova un conflitto. Ora che sa come gestirlo, deve aggiornare il db.
                }


            }
        }
    }
}
