using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlmacenApp.Models
{
    public class VehiculoErp
    {
        public int id_vehiculo { get; set; }
        public int fk_equipo { get; set; }
        public string equipo { get; set; }
        public int fk_tipo_equipo { get; set; }
        public string tipo_equipo { get; set; }
        public int fk_vehiculo_marca { get; set; }
        public string marca { get; set; }
        public int fk_carroceria { get; set; }
        public string carroceria { get; set; }
        public string codigo { get; set; }
        public string modelo { get; set; }
        public string serie { get; set; }
        public string placa { get; set; }
        public string nro_motor { get; set; }
        public int anio_fabricacion { get; set; }
        public string combustible { get; set; }
        public string potencia { get; set; }
        public int cilindros { get; set; }
        public decimal cilindrada { get; set; }
        public decimal carga_util { get; set; }
        public int ejes { get; set; }
        public int pasajeros { get; set; }
        public int asientos { get; set; }
        public int ruedas { get; set; }
        public decimal peso_bruto { get; set; }
        public decimal peso_neto { get; set; }
        public decimal longitud { get; set; }
        public decimal altura { get; set; }
        public decimal ancho { get; set; }
        public string image_url { get; set; }
        public string IDUSUARIO { get; set; }
        public string estado { get; set; }
        public string destado { get; set; }
        public override string ToString()
        {
            return marca + " " + modelo + " " + placa;
        }
    }
}