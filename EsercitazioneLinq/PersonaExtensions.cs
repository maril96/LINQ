using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esercitazione_Linq
{
    public static class PersonaExtensions
    {
        /// <summary>
        /// Extension Method della classe Persona.
        /// Data una lista di veicoli, restituisce un elenco di quelli posseduti dall'istanza
        /// </summary>
        /// <param name="persona"></param>
        /// <param name="listOfVehicles"></param>
        public static List<Vehicle> VeicoliPosseduti(this Persona persona, List<Veicolo> listOfVehicles)
        {

            var queryMyVehicles = listOfVehicles
                .Where(v => v.IDProprietario == persona.ID)
                .Select(v => new Vehicle { ID = v.ID, Targa = v.Targa, Prezzo = v.Prezzo }).ToList();



            return queryMyVehicles;
        }

    }

}
