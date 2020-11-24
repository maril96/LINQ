using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LINQ.ConsoleApp
{
    class Program
    {
        //costruisco un delegate. Il nome che assegnamo sarà il nome del Tipo:
        //A una variabile di tipo Sum possiamo assegnare tutte le funzioni che hanno la firma definita nella dichiarazione
        public delegate int Sum(int val1, int val2);
        public static int PrimaSomma(int valore1, int valore2)
        {
            return valore1 + valore2;
        }

        public static int SecondaSomma(int valore1, double valore2)
        {
            return valore1 + (int)valore2;
        }

        public static void Chiamami (Sum funzioneDaChiamare)
        {
            funzioneDaChiamare(1, 2);
            //passo la funzione come parametro usando il Delegate come Tipo di Dato.
            //poi gli passerò la funzione vera con la stessa firma del Delegate, o direttamente un qualcosa di tipo Sum
        }
        static void Main(string[] args)
        {
            //TestVar(); //Variabili di tipo implicito
            //TestGenericsAndFields(); //E anche anonymous type
            //MetodiAggiuntivi();

            //TestDelegate();
            TestLambda();

        }

        private static void TestLambda()
        {
            List<Employee> employees = new List<Employee>() {
            new Employee{Name="Roberto", ID=1},
            new Employee{Name="Alice", ID=2}
            };

            //poi lo finiremo

            var result = employees.Where("ID", "1");
            var result2 = employees.Where("Name", "Roberto");
            //in questo modo posso andare a definire la mia espressione "a run time", in base a quello che mi serve


            //Dichiaro un'espressione che mi farà da parametro
            ParameterExpression y = Expression.Parameter(typeof(int),"x");
            //La x è un nome che interviene nel Debug e nella stampa: va a riempire y.Name           
            
            Expression<Func<int, int>> squareExpression =
                Expression.Lambda<Func<int, int>>( //Mi dice che forma ha la Lambda
                        Expression.Multiply(y,y), //questo è il body della Lambda
                        
                        new ParameterExpression[] {y} //questi sono i parametri: in realtà è un array di parametri, che inizializzo con il solo y
                    );

            Func<int, int> funzione = squareExpression.Compile();
            Console.WriteLine(funzione(3));

        }

        private static void TestDelegate()
        {
            //DELEGATE
            Sum lamiasomma = PrimaSomma;
            //è uguale a scrivere: 
            //Sum lamiasomma = new Sum(PrimaSomma);

            //lamiasomma = SecondaSomma; dà errore: SecondaSomma ha una firma diversa rispetto a quella definita in Sum

            Chiamami(lamiasomma);
            //o anche:
            Chiamami(PrimaSomma);


            var process = new BusinessProcess();
            process.Started += Process_Started; //aggancio un EventHandler: una fz che sta in ascolto nel momento in cui viene sollevato un evento. 
            //La funzione deve avere la stessa firma del Delegate che è il tipo di Started.
            process.Completed += Process_Completed;
            process.StartedCore += Process_StartedCore;
            process.CompletedCore += Process_CompletedCore;
            
            process.ProcessData();

            //Possibili Delegate sono le Func e le Action:
            Func<int> primaFunc;
            //Func si usa per funzioni con un parametro di output e n di input
            //è come definire sopra: public delegate int primaFunc();
            //Qui sto dicendo che restituisce un int e non prende argomenti

            //Come tipi: Sum=Func<int, int, int>, in cui i primi tipi indicano gli input e l'ultimo indica il tipo dell'output

            Action<int> primaAction;
            //Le Action si usano per funzioni che non hanno valori in uscita

            /* Func e Action sono dei Delegate, e gli posso assegnare delle funzioni con i parametri input/output descritti
             * La comodità è non definire un Delegate, ma si usa un Delegate già esistente, che viene rappresentato in questo modo più semplice*/

            //Anche le LAMBDA-EXPRESSION sono dei modi per scrivere metodi e passarli ad altri metodi
            Func<int,int> lambdaZero = x => 2 * x;
            //x=>2*x è una Lambda-Expression, che infatti è una funzione, e possiamo assegnare a Func<int,int>
            //Questa Lambda-Expression e la funzione Mult definita sotto sono la stessa cosa. Farlo così però è più semplice
            Func<int, int> lambdaMult = x => { return 2 * x; };
            //tra graffe si aspetta un return (perchè in teoria possono esserci più righe quindi se lo aspetta
            Func<int, int> lambdaZeroZero = Mult;

            List<int> myList = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var result = Where(myList, x => x > 2);
            //la funzione che prendo in input, sottoforma di Lambda-Expression è quella che sarà la condizione da controllare dentro la funzione Where
            foreach (var item in result)
            {
                Console.WriteLine(item); 
            }


            Func<int, int, int> lambdaOne = (x, y) => x * y;
            //due input, un output
            Func<int, double, bool> lambdaTwo = (x, y) => x > (int)y;





        }

        private static IEnumerable<int> Where(IEnumerable<int> data, Func<int,bool> condizione)
        {
            //potremmo anche scriverlo come Extension Method andandolo a inserire nella classe statica delle estensioni e usando this davanti al primo "input"

            var result = new List<int>();
            foreach (var value in data)
            {
                if (condizione(value))
                    result.Add(value);
            }
            return result;

        }
        private static int Mult(int value)
        {
            return 2 * value;
        }
        private static void Process_CompletedCore(object sender, ProcessEndEventArgs e)
        {
            Console.WriteLine($"Ricevuto CompletedCore (duration: {e.Duration})");
        }

        private static void Process_StartedCore(object sender, EventArgs e)
        {
            Console.WriteLine("Ricevuto StartedCore");
        }

        private static void Process_Completed(int duration)
        {
            Console.WriteLine($"Process Completed (duration: {duration})");
        }

        private static void MetodiAggiuntivi()
        {

            //Metodi aggiuntivi:
            string example = "Example";
            example.ToDouble();
            example.WithPrefix("My");
        }

        private static void TestGenericsAndFields()
        {
            List<int> data = new List<int> { 1, 2, 3, 4 };
            foreach (var value in data)
            {
                Console.WriteLine("#" + value);
            }

            List<Employee> data2 = new List<Employee> { };

            foreach (var item in data2)
            {
                Console.WriteLine(item.Name);
                //item prende il tipo Employee, per cui posso usare tutte le sue proprietà
            }


            //TIPO ANONIMO:
            var person = new { firstName = "Roberto", lastName = "Ajolfi" };

            var person2 = new { firstName = "Marilena", cognome = "Pandolfi" };
            //il compilatore assegna un certo nome che io non posso vedere.
            //E' come se stessi creando direttamente un istanza senza avere una classe base, e definendone i vari campi. 
            //Lui li interpreta come se vedesse davvero la classe, che io però non posso istanziare di nuovo perchè non ce l'ho, non l'ho definita.
            //Una volta definito un Tipo Anonimo posso utilizzarlo ma non posso creare un istanza dello "stesso tipo", devo comunque creare un nuovo Tipo Anonimo
            //I tipi anonimi vengono indicati con 'a. Chiaramente se a me servono più istanze con le stesse proprietà mi conviene definire una classe,
            //in questo modo definisco invece al volo un tipo di dato, che presumibilmente non mi servirà più.

            Impiegato<int> imp1; //imp1 è un Impiegato che ha un ID di tipo int
            Impiegato<string> imp2; //imp2 è un Impiegato che ha un ID di tipo stringa
        }

        private static void TestVar()
        {
            Console.WriteLine("==LINQ==");
            string firstName = "Roberto";
            var lastName = "Ajolfi"; //se vado su lastName mi dice che è di tipo stirnga
                                     //Nonn posso poi assegnare tipo lastName = 0.4; perchè mi dice che non può fare la conversione implicita da double a string.
                                     //L'errore è già a compile time.

            //Anche un comando del genere è consentito: using (var file = new StreamWriter()) ;
        }

        private static void Process_Started()
        {
            //questa è la funzione che aggancerò all'evento Started
            Console.WriteLine("Ricevuto - Processo avviato!");
        
        }
    }



    internal class Employee
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    internal class Impiegato<T>
    {
        private int _p; //questo è un campo (privato)
        public int P { //questa è una proprietà che salva il suo valore in un campo (privato)
           get
            {
                return _p;
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("ID must be greater than zero.");
                _p = value;
            }
        }
        public T ID { get; set; }
        public string Nome { get; set; } //questa è una proprietà

    }
}

