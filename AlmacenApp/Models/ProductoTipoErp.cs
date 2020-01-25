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
    public class ProductoTipoErp
    {
        public int id_producto_tipo { get; set; }

        public int fk_producto_linea { get; set; }

        public string codigo_tipo { get; set; }

        public string producto_tipo { get; set; }

        public string abreviatura_tipo { get; set; }

        public string estado_tipo { get; set; }

        public string NEstado
        {
            get
            {
                string nestado = "";
                if (estado_tipo == "1")
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

        public string codigo_linea { get; set; }

        public string linea { get; set; }
        public int cant_items { get; set; }
        public override string ToString()
        {
            return producto_tipo;
        }
    }
}