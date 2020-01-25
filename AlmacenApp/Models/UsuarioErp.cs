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
    public class UsuarioErp
    {
        public string IDUSUARIO { get; set; }
        public string USR_NOMBRES { get; set; }
        public string IDUSUARIOTIPO { get; set; }
        public string IDCODIGOGENERAL { get; set; }
        public string NOMBRES { get; set; }
        public string A_PATERNO { get; set; }
        public string A_MATERNO { get; set; }
        public string PASSWORD { get; set; }
        public string ESTADO { get; set; }
        public string USR_INICIALES { get; set; }
        public string BLOQUEO { get; set; }
        public string DURACION { get; set; }
        public string idclieprov { get; set; }
        public string EMAIL { get; set; }
        public string estado_personal { get; set; }

        public string NOMBRESFULL { get; set; }
    }
}