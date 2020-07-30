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
using SQLite;

namespace AlmacenApp.Models
{
    public class SalidaProductoErpLite
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int id_salida_almacen { get; set; }
        public string IDCODIGOGENERAL { get; set; }
        public DateTime fecha_registro { get; set; }
        public string estado { get; set; }
        public string nomb_full { get; set; }
        public string nombre_almacen { get; set; }
        public string ubicacion { get; set; }
        public int fk_almacen { get; set; }
    }
}