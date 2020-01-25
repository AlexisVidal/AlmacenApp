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
    public class AlmacenStockErp
    {
        [JsonProperty("id_almacen_stock")]
        public int id_almacen_stock { get; set; }
        [JsonProperty("fk_almacen")]
        public int fk_almacen { get; set; }
        [JsonProperty("fk_producto")]
        public int fk_producto { get; set; }
        [JsonProperty("existencia")]
        public decimal existencia { get; set; }
        [JsonProperty("pto_limite")]
        public decimal pto_limite { get; set; }
        [JsonProperty("estado")]
        public string estado { get; set; }
        [JsonProperty("id_almacen")]
        public int id_almacen { get; set; }
        [JsonProperty("cod_almacen")]
        public string cod_almacen { get; set; }
        [JsonProperty("nombre")]
        public string nombre { get; set; }
        [JsonProperty("ubicacion")]
        public string ubicacion { get; set; }
        [JsonProperty("estado_almacen")]
        public string estado_almacen { get; set; }


        [JsonProperty("id_producto")]
        public int id_producto { get; set; }
        [JsonProperty("fk_proveedor_producto")]
        public int fk_proveedor_producto { get; set; }
        [JsonProperty("fk_producto_marca")]
        public int fk_producto_marca { get; set; }
        [JsonProperty("fk_producto_subfamilia")]
        public int fk_producto_subfamilia { get; set; }
        [JsonProperty("cod_producto")]
        public string cod_producto { get; set; }
        [JsonProperty("nom_producto")]
        public string nom_producto { get; set; }
        [JsonProperty("codigo_sku")]
        public string codigo_sku { get; set; }
        [JsonProperty("estado_producto")]
        public string estado_producto { get; set; }
        [JsonProperty("id_producto_marca")]
        public int id_producto_marca { get; set; }
        [JsonProperty("descripcion_producto_marca")]
        public string descripcion_producto_marca { get; set; }
        [JsonProperty("estado_producto_marca")]
        public string estado_producto_marca { get; set; }
        [JsonProperty("id_producto_subfamilia")]
        public int id_producto_subfamilia { get; set; }
        [JsonProperty("fk_producto_familia")]
        public int fk_producto_familia { get; set; }
        [JsonProperty("codigo_producto_subfamilia")]
        public string codigo_producto_subfamilia { get; set; }
        [JsonProperty("descripcion_producto_subfamilia")]
        public string descripcion_producto_subfamilia { get; set; }
        [JsonProperty("estado_subfamilia")]
        public string estado_subfamilia { get; set; }
        [JsonProperty("id_producto_familia")]
        public int id_producto_familia { get; set; }
        [JsonProperty("codigo_producto_familia")]
        public string codigo_producto_familia { get; set; }
        [JsonProperty("descripcion_producto_familia")]
        public string descripcion_producto_familia { get; set; }
        [JsonProperty("estado_familia")]
        public string estado_familia { get; set; }



        //OTROS
        public decimal cantidad_descontar { get; set; }

        public int fk_producto_tipo { get; set; }
        public string producto_tipo { get; set; }
        public string codigo_producto_linea { get; set; }
        public int fk_producto_linea { get; set; }
        public string producto_linea { get; set; }
        public string estado_linea { get; set; }
    }
}