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
    public class AlmacenErpLite
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int id_almacen { get; set; }
        public string cod_almacen { get; set; }
        public string nombre { get; set; }
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
    }
}