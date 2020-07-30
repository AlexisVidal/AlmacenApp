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
    public class MovimientoErp
    {

        [JsonProperty("id_movimiento")]
        public int id_movimiento { get; set; }

        [JsonProperty("fk_movimiento_tipo")]
        public int fk_movimiento_tipo { get; set; }

        [JsonProperty("fk_guia_remision_detalle")]
        public int fk_guia_remision_detalle { get; set; }

        [JsonProperty("fk_venta_detalle")]
        public int fk_venta_detalle { get; set; }

        [JsonProperty("fk_comprobante_traslado_detalle")]
        public int fk_comprobante_traslado_detalle { get; set; }

        [JsonProperty("fk_nota_credito_detalle")]
        public int fk_nota_credito_detalle { get; set; }

        [JsonProperty("fk_almacen")]
        public int fk_almacen { get; set; }

        [JsonProperty("fk_producto")]
        public int fk_producto { get; set; }

        [JsonProperty("f_movimiento")]
        public DateTime f_movimiento { get; set; }
       // public string f_movimiento { get; set; }

        [JsonProperty("cantidad")]
        public decimal cantidad { get; set; }

        [JsonProperty("estado")]
        public string estado { get; set; }

        public string NEstado
        {
            get
            {
                string nestado = "";
                if (estado == "0")
                {
                    nestado = "ANULADO";
                }
                else if (estado == "1")
                {
                    nestado = "VIGENTE";
                }
                return nestado;
            }
            set
            {

            }
        }

        //Otras
        [JsonProperty("fk_venta")]
        public int fk_venta { get; set; }

        //KARDEX
        [JsonProperty("ingreso_salida")]
        public string ingreso_salida { get; set; }

        [JsonProperty("descripcion_movimiento_tipo")]
        public string descripcion_movimiento_tipo { get; set; }

        [JsonProperty("codigo_comprobante_ingreso")]
        public string codigo_comprobante_ingreso { get; set; }

        [JsonProperty("tipo_comprobante_ingreso")]
        public string tipo_comprobante_ingreso { get; set; }

        [JsonProperty("nro_comprobante_ingreso")]
        public string nro_comprobante_ingreso { get; set; }

        [JsonProperty("f_emision_ingreso")]
        public DateTime f_emision_ingreso { get; set; }

        [JsonProperty("precio_ingreso")]
        public decimal precio_ingreso { get; set; }

        [JsonProperty("tipo_afectacion_igv_ingreso")]
        public int tipo_afectacion_igv_ingreso { get; set; }

        [JsonProperty("flag_afecto_igv_ingreso")]
        public string flag_afecto_igv_ingreso { get; set; }

        [JsonProperty("porcentaje_igv_ingreso")]
        public decimal porcentaje_igv_ingreso { get; set; }

        [JsonProperty("tipo_isc_ingreso")]
        public int tipo_isc_ingreso { get; set; }

        [JsonProperty("codigo_comprobante_salida")]
        public string codigo_comprobante_salida { get; set; }

        [JsonProperty("tipo_comprobante_salida")]
        public string tipo_comprobante_salida { get; set; }

        [JsonProperty("nro_comprobante_salida")]
        public string nro_comprobante_salida { get; set; }

        [JsonProperty("f_emision_salida")]
        public DateTime f_emision_salida { get; set; }

        [JsonProperty("precio_salida")]
        public decimal precio_salida { get; set; }

        [JsonProperty("tipo_afectacion_igv_salida")]
        public int tipo_afectacion_igv_salida { get; set; }

        [JsonProperty("flag_afecto_igv_salida")]
        public string flag_afecto_igv_salida { get; set; }

        [JsonProperty("porcentaje_igv_salida")]
        public decimal porcentaje_igv_salida { get; set; }


        [JsonProperty("tipo_isc_salida")]
        public int tipo_isc_salida { get; set; }


        public string cod_producto { get; set; }
        public string nombre_producto { get; set; }
        public string marca_producto { get; set; }
        public string IDCODIGOGENERAL { get; set; }
        public int fk_unidad_medida { get; set; }
        public string abreviatura { get; set; }
        public string unidad_medida { get; set; }
        public int fk_salida_almacen { get; set; }
        public int nro_inventario { get; set; }
        public decimal precio_costo { get; set; }
        public string codigo_sku { get; set; }
        public string descripcion_producto_tipo { get; set; }
        public string nombre_almacen { get; set; }
        public string ubicacion { get; set; }
        public string nomb_full { get; set; }
        public string nombres { get; set; }
    }
}