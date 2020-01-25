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
    public class PersonalErp
    {
        public string IDCODIGOGENERAL { get; set; }
        public string A_MATERNO { get; set; }
        public string A_PATERNO { get; set; }
        public string NOMBRES { get; set; }
        public string CODIGO_CONTROL { get; set; }
        public string nomb_full { get; set; }

        public string NOMBRECOMPLETO
        {
            get
            {
                return A_PATERNO + " " + A_MATERNO + " " + NOMBRES;
            }
            set
            {
            }
        }
    }
}