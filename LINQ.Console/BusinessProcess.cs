using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace LINQ.ConsoleApp
{

    public delegate void ProcessStarted();
    public delegate void ProcessCompleted(int duration);


    public class BusinessProcess
    {
        public event ProcessStarted Started; //Dichiaro l'evento
        //l'evento si chiama Started e verrà gestito tramite la chiamata di una funzione che ha la firma di ProcessStarted
        public event ProcessCompleted Completed;
        //Volendo potrei anche definirli con lo stesso Delegate se i tipi input/output tornano

        public event EventHandler StartedCore;
        //EventHandler è un Delegate già presente in .NET Core. Chi si sottoscrive riceverà l'oggetto che solleva l'evento e degli argomenti

        public event EventHandler<ProcessEndEventArgs> CompletedCore;
        //qui uso EventHandler ma con degli argomenti, che devo andare a definire
        //o anche:
        public event EventHandler<int> CompletedCore2;
        //in questo modo però posso dargli un solo parametro, se me ne servono di più devo dargli una classe e definire i vari argomenti come proprietà della classe



        public void ProcessData()
        {   
            //simuliamo un processo che richieda un pò di tempo
            
            Console.WriteLine("==Starting Process==");
            Thread.Sleep(2000);
            Console.WriteLine("==Process Started==");
            //voglio sollevare un evento: devo prima definire la forma dell'evento: lo faccio definendo un Delegate nello stesso NameSpace
            if (Started!= null)
                Started(); //sollevo l'evento Started
            //In questo caso il mio Delegate non ha input ed è void
            //MA: se nessuno si sottoscrive all'evento, non ha senso sollevarlo. Se compilo solo questo codice mi dà errore. C'è bisogno che qualcuno si sottoscriva
            //Se qualcuno si è sottoscritto, sollevando l'evento vengono eseguite tutte le funzioni dei sottoscrittori.
            if (StartedCore != null)
                StartedCore(this, new EventArgs());
            //Perchè qui non voglio passargli nessun argomento, quindi ci metto un oggetto EventArgs vuoto


            Thread.Sleep(3000);
            Console.WriteLine("==Process Completed==");
            if(Completed!=null) //Perchè se nessuno si è sottoscritto mi dà errore.
                Completed(5000);
            if (CompletedCore != null)
                CompletedCore(this, new ProcessEndEventArgs { Duration = 4500 });
            //Definisco un ProcessEndEventArgs e inizializzo direttamente i campi.


        }

    }

    public class ProcessEndEventArgs : EventArgs
    {
        public int Duration { get; set; }
    }
}
