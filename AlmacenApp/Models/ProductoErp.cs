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
    public class ProductoErp
    {
        public int id_producto { get; set; }
        public int fk_unidad_medida { get; set; }
        public int fk_producto_marca { get; set; }
        public int fk_producto_familia { get; set; }
        public string cod_producto { get; set; }
        public string nom_producto { get; set; }
        public string codigo_sku { get; set; }
        public string estado { get; set; }
        public string nombre_full { get; set; }
        public string marca { get; set; }
        public string familia { get; set; }
        public int fk_producto_subfamilia { get; set; }
        public string subfamilia { get; set; }
        public int fk_producto_tipo { get; set; }
        public string producto_tipo { get; set; }
        public int fk_producto_linea { get; set; }
        public string producto_linea { get; set; }
        public int cant_items { get; set; }

        public string abreviatura { get; set; }
        public string unidad_medida { get; set; }
        public override string ToString()
        {
            return nom_producto;
        }
    }
}