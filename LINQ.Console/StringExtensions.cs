using System;
using System.Collections.Generic;
using System.Text;

namespace LINQ.ConsoleApp
{
    //voglio aggiungere a string un metodo, che chiamo ToDouble:
    //questo metodo può ora essere utilizzato in tutto il Project, oppure è utilizzabile se il Progetto corrente ha un riferimento al Progetto che contiene l'Extension Method
    
    //Solitamente si fa una classe statica per ogni tipo che voglio estendere. Ma potrei anche fare una classe di estensione in cui metto tutte quelle che mi servono
    
    public static class StringExtensions
    {
        public static IEnumerable<T> Where<T>(this IEnumerable<T> data, string property, string propertyValue)
        {
            var results = new List<T>();

            
            foreach (T value in data)
            {
                results.Add(value);
            }
            return results as IEnumerable<T>;
        }

        public static double ToDouble(this string value)
        {
            //"this string" usato all'interno di un metodo statico di una classe statica mi dice che sto estendendo la classe string
            double.TryParse(value, out double convertedValue);
            //se il TryParse non va a buon fine ConvertedValue prende il valore di default per il suo tipo, che in questo caso è 0.
            return convertedValue;
        }

        public static string WithPrefix(this string value, string prefix)
        {
            //Extension Method con un parametro vero
            return $"{prefix}-{value}";
            //$ fa la String Interpolation: dentro le graffe posso far riferimento ad altre parti del codice
            //return prefix +"-"+value;
            //farebbe la stessa cosa, o anche:
            //return string.Format("{0}-{1}", prefix, value);
        }
    }

}
