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
    public class UnidadMedidaErp
    {
        public int id_unidad_medida { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public string abreviatura { get; set; }
        public override string ToString()
        {
            return abreviatura + " - " + descripcion;
        }
    }
}