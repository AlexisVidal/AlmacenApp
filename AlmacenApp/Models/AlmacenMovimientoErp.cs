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

namespace AlmacenApp.Models
{
    public class AlmacenMovimientoErp
    {
        public int id_almacen_movimiento { get; set; }
        public DateTime fecha { get; set; }
        public int fk_almacen { get; set; }
        public int fk_movimiento_tipo { get; set; }
        public string codigo_movimiento_tipo { get; set; }
        public string IDCODIGOGENERAL { get; set; }
        public string cliente { get; set; }
        public string direccion { get; set; }
        public string oc_os { get; set; }
        public string maquina_unidad { get; set; }
        public string observaciones { get; set; }
        public string IDRESPONSABLE { get; set; }
        public int id_almacen { get; set; }
        public string cod_almacen { get; set; }
        public string nombre { get; set; }
        public string ubicacion { get; set; }
        public int id_movimiento_tipo { get; set; }
        public string descripcion { get; set; }
        public string codigo { get; set; }
        public string A_PATERNO { get; set; }
        public string A_MATERNO { get; set; }
        public string NOMBRES { get; set; }
        public string RESPONSABLE { get; set; }
    }
}