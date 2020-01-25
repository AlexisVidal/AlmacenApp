using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace AlmacenApp.Models
{
    public class AlmacenErp
    {
        [JsonProperty("id_almacen")]
        public int id_almacen { get; set; }
        [JsonProperty("cod_almacen")]
        public string cod_almacen { get; set; }
        [JsonProperty("nombre")]
        public string nombre { get; set; }
        [JsonProperty("ubicacion")]
        public string ubicacion { get; set; }
        public string estado { get; set; }
        public string NEstado
        {
            get
            {
                string nestado = "";
                if (estado == "1")
                {
                    nestado = "ACTIVO";
                }
                else
                {
                    nestado = "INACTIVO";
                }
                return nestado;
            }
            set
            {
            }
        }
        public override string ToString()
        {
            return nombre;
        }
    }
}