using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AlmacenApp.Models;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AlmacenApp.Clases
{
    public class Data
    {
        public class Busqueda
        {
            public string descripcion { get; set; }
        }
        public class Busqueda2
        {
            public string descripcion { get; set; }
            public int fk_almacen { get; set; }
        }
        public class ResultadoInsert
        {
            public int rpta { get; set; }
        }
        private static readonly Context _mContext = Application.Context;
        private static AppPreferences _ap = new AppPreferences(_mContext);
        private static string _connectionInfo = _ap.getServerurlKey();
        internal static async Task<int> InserLinea(string descripcion)
        {
            ProductoLineaErp newinsert;
            _connectionInfo = _ap.getServerurlKey();
            try
            {
                List<ResultadoInsert> lrespuestax = new List<ResultadoInsert>();
                newinsert = new ProductoLineaErp
                {
                    descripcion = descripcion
                };
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(_connectionInfo);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string serializedObject = JsonConvert.SerializeObject(newinsert);
                HttpContent contentPost = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("Productoerp/t_producto_lineaInsert", contentPost);
                if (response.IsSuccessStatusCode)
                {
                    var resultao = await response.Content.ReadAsStringAsync();
                    //var resultalinea = JObject.Parse(await response.Content.ReadAsStringAsync());
                    ////JArray resultalinea = JArray.Parse(await response.Content.ReadAsStringAsync());
                    //JsonSerializerSettings settings = new JsonSerializerSettings();
                    //settings.NullValueHandling = NullValueHandling.Ignore;
                    //settings.MissingMemberHandling = MissingMemberHandling.Ignore;
                    if (resultao != null && resultao != "")
                    {
                        return Convert.ToInt32(resultao);
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        internal static async Task<int> DeleteProducto(int idproducto)
        {
            _connectionInfo = _ap.getServerurlKey();
            ProductoErp newinsert;
            try
            {
                List<ResultadoInsert> lrespuestax = new List<ResultadoInsert>();
                newinsert = new ProductoErp
                {
                    id_producto = idproducto
                };
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(_connectionInfo);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string serializedObject = JsonConvert.SerializeObject(newinsert);
                HttpContent contentPost = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("Productoerp/t_productoDelete", contentPost);
                if (response.IsSuccessStatusCode)
                {
                    var resultao = await response.Content.ReadAsStringAsync();
                    if (resultao != null && resultao != "")
                    {
                        return Convert.ToInt32(resultao);
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        internal static async Task<List<AlmacenErp>> CargaAlmacenes()
        {
            _connectionInfo = _ap.getServerurlKey();
            try
            {
                HttpClient client = new HttpClient();
                //var _connectionInfo = serverurl;
                client.BaseAddress = new Uri(_connectionInfo);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("Almacen/all");

                if (response.IsSuccessStatusCode && response.RequestMessage != null)
                {
                    JArray jentidad = JArray.Parse(await response.Content.ReadAsStringAsync());
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.NullValueHandling = NullValueHandling.Ignore;
                    settings.MissingMemberHandling = MissingMemberHandling.Ignore;
                    if (jentidad != null && jentidad.Count > 0)
                    {
                        var listlineaxs = JsonConvert.DeserializeObject<List<AlmacenErp>>(jentidad.ToString(), settings);
                        return listlineaxs;
                    }
                    else
                    {
                        return new List<AlmacenErp>();
                    }
                }
                else
                {
                    return new List<AlmacenErp>();
                }
            }
            catch (Exception ex)
            {
                return new List<AlmacenErp>();
            }
        }

        internal static async Task<int> UpdateLinea(string descripcion, int idproductolinea)
        {
            _connectionInfo = _ap.getServerurlKey();
            ProductoLineaErp newinsert;
            try
            {
                List<ResultadoInsert> lrespuestax = new List<ResultadoInsert>();
                newinsert = new ProductoLineaErp
                {
                    id_producto_linea = idproductolinea,
                    descripcion = descripcion
                };
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(_connectionInfo);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string serializedObject = JsonConvert.SerializeObject(newinsert);
                HttpContent contentPost = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("Productoerp/t_producto_lineaUpdate", contentPost);
                if (response.IsSuccessStatusCode)
                {
                    var resultao = await response.Content.ReadAsStringAsync();
                    //var resultalinea = JObject.Parse(await response.Content.ReadAsStringAsync());
                    ////JArray resultalinea = JArray.Parse(await response.Content.ReadAsStringAsync());
                    //JsonSerializerSettings settings = new JsonSerializerSettings();
                    //settings.NullValueHandling = NullValueHandling.Ignore;
                    //settings.MissingMemberHandling = MissingMemberHandling.Ignore;
                    if (resultao != null && resultao != "")
                    {
                        return Convert.ToInt32(resultao);
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        public static async Task<List<ProductoLineaErp>> CargaProductoLineas()
        {
            _connectionInfo = _ap.getServerurlKey();
            try
            {
                HttpClient client = new HttpClient();
                //var _connectionInfo = serverurl;
                client.BaseAddress = new Uri(_connectionInfo);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("Productoerp/t_producto_lineaSelectAll");

                if (response.IsSuccessStatusCode && response.RequestMessage != null)
                {
                    JArray jentidad = JArray.Parse(await response.Content.ReadAsStringAsync());
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.NullValueHandling = NullValueHandling.Ignore;
                    settings.MissingMemberHandling = MissingMemberHandling.Ignore;
                    if (jentidad != null && jentidad.Count > 0)
                    {
                        var listlineaxs = JsonConvert.DeserializeObject<List<ProductoLineaErp>>(jentidad.ToString(), settings);
                        return listlineaxs;
                    }
                    else
                    {
                        return new List<ProductoLineaErp>();
                    }
                }
                else
                {
                    return new List<ProductoLineaErp>();
                }
            }
            catch (Exception ex)
            {
                return new List<ProductoLineaErp>();
            }
        }

        internal static async Task<int> InsertaMovimientoDB(MovimientoErp movimiento)
        {
            _connectionInfo = _ap.getServerurlKey();
            int newidmovimiento = 0;
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(_connectionInfo);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string serializedObject = JsonConvert.SerializeObject(movimiento);
                HttpContent contentPost = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("Movimiento/agregar", contentPost);
                if (response.IsSuccessStatusCode && response.RequestMessage != null)
                {
                    JArray solicitus = JArray.Parse(await response.Content.ReadAsStringAsync());
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.NullValueHandling = NullValueHandling.Ignore;
                    settings.MissingMemberHandling = MissingMemberHandling.Ignore;
                    if (solicitus != null && solicitus.Count > 0)
                    {
                        var lnuevomovimient = JsonConvert.DeserializeObject<List<MovimientoErp>>(solicitus.ToString(), settings);

                        if (lnuevomovimient != null && lnuevomovimient.Count > 0)
                        {
                            MovimientoErp nuevomovimiento = new MovimientoErp();
                            nuevomovimiento = lnuevomovimient.FirstOrDefault();

                            newidmovimiento = nuevomovimiento.id_movimiento;
                        }
                        else
                        {
                            newidmovimiento = 0;
                        }
                    }
                    else
                    {
                        newidmovimiento = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                newidmovimiento = 0;
            }
            return newidmovimiento;
        }
        internal static async Task<int> InsertaAlmacenMovimientoDB(AlmacenMovimientoErp movimiento)
        {
            _connectionInfo = _ap.getServerurlKey();
            int newidmovimiento = 0;
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(_connectionInfo);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string serializedObject = JsonConvert.SerializeObject(movimiento);
                HttpContent contentPost = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("Movimiento/t_almacen_movimientoInsert", contentPost);
                if (response.IsSuccessStatusCode && response.RequestMessage != null)
                {
                    var respuesta = await response.Content.ReadAsStringAsync();
                    //JArray solicitus = JArray.Parse();
                    //JsonSerializerSettings settings = new JsonSerializerSettings();
                    //settings.NullValueHandling = NullValueHandling.Ignore;
                    //settings.MissingMemberHandling = MissingMemberHandling.Ignore;
                    if (respuesta != null && respuesta != "")
                    {
                        newidmovimiento = Convert.ToInt32(respuesta);
                    }
                    else
                    {
                        newidmovimiento = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                newidmovimiento = 0;
            }
            return newidmovimiento;
        }
        internal static async Task<int> InsertaSalidaProductoDB(SalidaProductoErp salida)
        {
            _connectionInfo = _ap.getServerurlKey();
            int newidsalida = 0;
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(_connectionInfo);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string serializedObject = JsonConvert.SerializeObject(salida);
                HttpContent contentPost = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("Movimiento/t_salida_almacenInsert", contentPost);
                if (response.IsSuccessStatusCode)
                {
                    var resultao = await response.Content.ReadAsStringAsync();
                    if (resultao != null && resultao != "")
                    {
                        newidsalida = Convert.ToInt32(resultao);
                    }
                    else
                    {
                        newidsalida = 0;
                    }
                }
                else
                {
                    newidsalida = 0;
                }
            }
            catch (Exception ex)
            {
                newidsalida = 0;
            }
            return newidsalida;
        }

        public static async Task<List<ProductoTipoErp>> CargaProductoTipos()
        {
            _connectionInfo = _ap.getServerurlKey();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(_connectionInfo);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("Productoerp/t_producto_tipoSelectAll");

                if (response.IsSuccessStatusCode && response.RequestMessage != null)
                {
                    JArray jentidad = JArray.Parse(await response.Content.ReadAsStringAsync());
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.NullValueHandling = NullValueHandling.Ignore;
                    settings.MissingMemberHandling = MissingMemberHandling.Ignore;
                    if (jentidad != null && jentidad.Count > 0)
                    {
                        var listentidad = JsonConvert.DeserializeObject<List<ProductoTipoErp>>(jentidad.ToString(), settings);
                        return listentidad;
                    }
                    else
                    {
                        return new List<ProductoTipoErp>();
                    }
                }
                else
                {
                    return new List<ProductoTipoErp>();
                }
            }
            catch (Exception ex)
            {
                return new List<ProductoTipoErp>();
            }
        }
        internal static async Task<int> InserTipo(int idproductolinea, string descripcion, string abreviatura_tipo)
        {
            _connectionInfo = _ap.getServerurlKey();
            ProductoTipoErp newinsert;
            try
            {
                List<ResultadoInsert> lrespuestax = new List<ResultadoInsert>();
                newinsert = new ProductoTipoErp
                {
                    fk_producto_linea = idproductolinea,
                    codigo_tipo = "",
                    abreviatura_tipo = abreviatura_tipo,
                    producto_tipo = descripcion
                };
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(_connectionInfo);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string serializedObject = JsonConvert.SerializeObject(newinsert);
                HttpContent contentPost = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("Productoerp/t_producto_tipoInsert", contentPost);
                if (response.IsSuccessStatusCode)
                {
                    var resultao = await response.Content.ReadAsStringAsync();
                    if (resultao != null && resultao != "")
                    {
                        return Convert.ToInt32(resultao);
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        internal static async Task<int> UpdateProducto(int idproducto, int idproductounidad, int idproductomarca, int idproductotipo, string descripcion, string codigosku)
        {
            _connectionInfo = _ap.getServerurlKey();
            ProductoErp newinsert;
            try
            {
                List<ResultadoInsert> lrespuestax = new List<ResultadoInsert>();
                newinsert = new ProductoErp
                {
                    id_producto = idproducto,
                    fk_unidad_medida = idproductounidad,
                    fk_producto_marca = idproductomarca,
                    fk_producto_tipo = idproductotipo,
                    nom_producto = descripcion,
                    cod_producto = "",
                    codigo_sku = codigosku,
                    estado = "1"
                };
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(_connectionInfo);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string serializedObject = JsonConvert.SerializeObject(newinsert);
                HttpContent contentPost = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("Productoerp/modificar", contentPost);
                if (response.IsSuccessStatusCode)
                {
                    var resultao = await response.Content.ReadAsStringAsync();
                    if (resultao != null && resultao != "")
                    {
                        return Convert.ToInt32(resultao);
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        internal static async Task<int> InserProducto(int idproductounidad, int idproductomarca, int idproductotipo, string descripcion, string codigosku)
        {
            _connectionInfo = _ap.getServerurlKey();
            ProductoErp newinsert;
            try
            {
                List<ResultadoInsert> lrespuestax = new List<ResultadoInsert>();
                newinsert = new ProductoErp
                {
                    fk_unidad_medida = idproductounidad,
                    fk_producto_marca = idproductomarca,
                    fk_producto_tipo = idproductotipo,
                    nom_producto = descripcion,
                    cod_producto = "",
                    codigo_sku = codigosku
                };
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(_connectionInfo);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string serializedObject = JsonConvert.SerializeObject(newinsert);
                HttpContent contentPost = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("Productoerp/agregar", contentPost);
                if (response.IsSuccessStatusCode)
                {
                    var resultao = await response.Content.ReadAsStringAsync();
                    if (resultao != null && resultao != "")
                    {
                        return Convert.ToInt32(resultao);
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        internal static async Task<int> UpdateTipo(string descripcion, int idproductotipo, int idproductolinea)
        {
            _connectionInfo = _ap.getServerurlKey();
            ProductoTipoErp newinsert;
            try
            {
                List<ResultadoInsert> lrespuestax = new List<ResultadoInsert>();
                newinsert = new ProductoTipoErp
                {
                    id_producto_tipo = idproductotipo,
                    fk_producto_linea = idproductolinea,
                    producto_tipo = descripcion
                };
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(_connectionInfo);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string serializedObject = JsonConvert.SerializeObject(newinsert);
                HttpContent contentPost = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("Productoerp/t_producto_tipoUpdate", contentPost);
                if (response.IsSuccessStatusCode)
                {
                    var resultao = await response.Content.ReadAsStringAsync();
                    if (resultao != null && resultao != "")
                    {
                        return Convert.ToInt32(resultao);
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }
        public static async Task<int> DeleteTipo(int idproductotipo)
        {
            _connectionInfo = _ap.getServerurlKey();
            ProductoTipoErp newinsert;
            try
            {
                List<ResultadoInsert> lrespuestax = new List<ResultadoInsert>();
                newinsert = new ProductoTipoErp
                {
                    id_producto_tipo = idproductotipo
                };
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(_connectionInfo);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string serializedObject = JsonConvert.SerializeObject(newinsert);
                HttpContent contentPost = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("Productoerp/t_producto_tipoDelete", contentPost);
                if (response.IsSuccessStatusCode)
                {
                    var resultao = await response.Content.ReadAsStringAsync();
                    if (resultao != null && resultao != "")
                    {
                        return Convert.ToInt32(resultao);
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }
        public static async Task<List<ProductoErp>> BuscaProductos(string buscar)
        {
            _connectionInfo = _ap.getServerurlKey();
            try
            {
                buscar = buscar.Trim();
                string buscars = buscar.Replace(" ", "| OR /");
                buscars = "/" + buscars + "|";
                var entidad = new Busqueda { descripcion = buscars };
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(_connectionInfo);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string serializedObject = JsonConvert.SerializeObject(entidad);
                HttpContent contentPost = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("Productoerp/t_productoSelectText_4", contentPost);


                if (response.IsSuccessStatusCode && response.RequestMessage != null)
                {
                    JArray jentidad = JArray.Parse(await response.Content.ReadAsStringAsync());
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.NullValueHandling = NullValueHandling.Ignore;
                    settings.MissingMemberHandling = MissingMemberHandling.Ignore;
                    if (jentidad != null && jentidad.Count > 0)
                    {
                        var listlineaxs = JsonConvert.DeserializeObject<List<ProductoErp>>(jentidad.ToString(), settings);
                        return listlineaxs;
                    }
                    else
                    {
                        return new List<ProductoErp>();
                    }
                }
                else
                {
                    return new List<ProductoErp>();
                }
            }
            catch (Exception ex)
            {
                return new List<ProductoErp>();
            }
        }
        public static async Task<List<ProductoErpLite>> BuscaProductosLite(string buscar)
        {
            _connectionInfo = _ap.getServerurlKey();
            try
            {
                buscar = buscar.Trim();
                string buscars = buscar.Replace(" ", "| OR /");
                buscars = "/" + buscars + "|";
                var entidad = new Busqueda { descripcion = buscars };
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(_connectionInfo);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string serializedObject = JsonConvert.SerializeObject(entidad);
                HttpContent contentPost = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("Productoerp/t_productoSelectText_4", contentPost);


                if (response.IsSuccessStatusCode && response.RequestMessage != null)
                {
                    JArray jentidad = JArray.Parse(await response.Content.ReadAsStringAsync());
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.NullValueHandling = NullValueHandling.Ignore;
                    settings.MissingMemberHandling = MissingMemberHandling.Ignore;
                    if (jentidad != null && jentidad.Count > 0)
                    {
                        var listlineaxs = JsonConvert.DeserializeObject<List<ProductoErpLite>>(jentidad.ToString(), settings);
                        return listlineaxs;
                    }
                    else
                    {
                        return new List<ProductoErpLite>();
                    }
                }
                else
                {
                    return new List<ProductoErpLite>();
                }
            }
            catch (Exception ex)
            {
                return new List<ProductoErpLite>();
            }
        }
        public static async Task<List<ProductoErpLite>> BuscaProductosFkalmacenLite(string buscar, int fkalmacen)
        {
            _connectionInfo = _ap.getServerurlKey();
            try
            {
                buscar = buscar.Trim();
                string buscars = buscar.Replace(" ", "| OR /");
                buscars = "/" + buscars + "|";
                var entidad = new Busqueda2 { descripcion = buscars, fk_almacen = fkalmacen };
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(_connectionInfo);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string serializedObject = JsonConvert.SerializeObject(entidad);
                HttpContent contentPost = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("Productoerp/t_productoSelectText_FkAlmacen", contentPost);


                if (response.IsSuccessStatusCode && response.RequestMessage != null)
                {
                    JArray jentidad = JArray.Parse(await response.Content.ReadAsStringAsync());
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.NullValueHandling = NullValueHandling.Ignore;
                    settings.MissingMemberHandling = MissingMemberHandling.Ignore;
                    if (jentidad != null && jentidad.Count > 0)
                    {
                        var listlineaxs = JsonConvert.DeserializeObject<List<ProductoErpLite>>(jentidad.ToString(), settings);
                        return listlineaxs;
                    }
                    else
                    {
                        return new List<ProductoErpLite>();
                    }
                }
                else
                {
                    return new List<ProductoErpLite>();
                }
            }
            catch (Exception ex)
            {
                return new List<ProductoErpLite>();
            }
        }
        public static async Task<List<ProductoErp>> CargaProductos()
        {
            _connectionInfo = _ap.getServerurlKey();
            try
            {
                HttpClient client = new HttpClient();
                //var _connectionInfo = serverurl;
                client.BaseAddress = new Uri(_connectionInfo);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("Productoerp/t_productoSelectAll");

                if (response.IsSuccessStatusCode && response.RequestMessage != null)
                {
                    JArray jentidad = JArray.Parse(await response.Content.ReadAsStringAsync());
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.NullValueHandling = NullValueHandling.Ignore;
                    settings.MissingMemberHandling = MissingMemberHandling.Ignore;
                    if (jentidad != null && jentidad.Count > 0)
                    {
                        var listlineaxs = JsonConvert.DeserializeObject<List<ProductoErp>>(jentidad.ToString(), settings);
                        return listlineaxs;
                    }
                    else
                    {
                        return new List<ProductoErp>();
                    }
                }
                else
                {
                    return new List<ProductoErp>();
                }
            }
            catch (Exception ex)
            {
                return new List<ProductoErp>();
            }
        }
        public static async Task<List<ProductoMarcaErp>> CargaProductoMarcas()
        {
            _connectionInfo = _ap.getServerurlKey();
            try
            {
                HttpClient client = new HttpClient();
                //var _connectionInfo = serverurl;
                client.BaseAddress = new Uri(_connectionInfo);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("ProductoMarca/all");

                if (response.IsSuccessStatusCode && response.RequestMessage != null)
                {
                    JArray jentidad = JArray.Parse(await response.Content.ReadAsStringAsync());
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.NullValueHandling = NullValueHandling.Ignore;
                    settings.MissingMemberHandling = MissingMemberHandling.Ignore;
                    if (jentidad != null && jentidad.Count > 0)
                    {
                        var listlineaxs = JsonConvert.DeserializeObject<List<ProductoMarcaErp>>(jentidad.ToString(), settings);
                        return listlineaxs;
                    }
                    else
                    {
                        return new List<ProductoMarcaErp>();
                    }
                }
                else
                {
                    return new List<ProductoMarcaErp>();
                }
            }
            catch (Exception ex)
            {
                return new List<ProductoMarcaErp>();
            }
        }
        public static async Task<List<PersonalErpLite>> BuscaPersonal(string buscar)
        {
            _connectionInfo = _ap.getServerurlKey();
            try
            {
                buscar = buscar.Trim();
                string buscars = buscar.Replace(" ", "| OR /");
                buscars = "/" + buscars + "|";
                var entidad = new Busqueda { descripcion = buscars };
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(_connectionInfo);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string serializedObject = JsonConvert.SerializeObject(entidad);
                HttpContent contentPost = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("Personalerp/t_personal_generalSelectText", contentPost);


                if (response.IsSuccessStatusCode && response.RequestMessage != null)
                {
                    JArray jentidad = JArray.Parse(await response.Content.ReadAsStringAsync());
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.NullValueHandling = NullValueHandling.Ignore;
                    settings.MissingMemberHandling = MissingMemberHandling.Ignore;
                    if (jentidad != null && jentidad.Count > 0)
                    {
                        var listlineaxs = JsonConvert.DeserializeObject<List<PersonalErpLite>>(jentidad.ToString(), settings);
                        return listlineaxs;
                    }
                    else
                    {
                        return new List<PersonalErpLite>();
                    }
                }
                else
                {
                    return new List<PersonalErpLite>();
                }
            }
            catch (Exception ex)
            {
                return new List<PersonalErpLite>();
            }
        }
        public static async Task<List<UnidadMedidaErp>> CargaUnidadMedidaErp()
        {
            _connectionInfo = _ap.getServerurlKey();
            try
            {
                HttpClient client = new HttpClient();
                //var _connectionInfo = serverurl;
                client.BaseAddress = new Uri(_connectionInfo);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("Productoerp/t_unidad_medidaSelectAll");

                if (response.IsSuccessStatusCode && response.RequestMessage != null)
                {
                    JArray jentidad = JArray.Parse(await response.Content.ReadAsStringAsync());
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.NullValueHandling = NullValueHandling.Ignore;
                    settings.MissingMemberHandling = MissingMemberHandling.Ignore;
                    if (jentidad != null && jentidad.Count > 0)
                    {
                        var listlineaxs = JsonConvert.DeserializeObject<List<UnidadMedidaErp>>(jentidad.ToString(), settings);
                        return listlineaxs;
                    }
                    else
                    {
                        return new List<UnidadMedidaErp>();
                    }
                }
                else
                {
                    return new List<UnidadMedidaErp>();
                }
            }
            catch (Exception ex)
            {
                return new List<UnidadMedidaErp>();
            }
        }
        public static async Task<List<SalidaProductoErp>> GetSalidasAlmacen()
        {
            _connectionInfo = _ap.getServerurlKey();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(_connectionInfo);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("Movimiento/t_salida_almacenAll");

                if (response.IsSuccessStatusCode && response.RequestMessage != null)
                {
                    JArray jentidad = JArray.Parse(await response.Content.ReadAsStringAsync());
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.NullValueHandling = NullValueHandling.Ignore;
                    settings.MissingMemberHandling = MissingMemberHandling.Ignore;
                    if (jentidad != null && jentidad.Count > 0)
                    {
                        var listentidad = JsonConvert.DeserializeObject<List<SalidaProductoErp>>(jentidad.ToString(), settings);
                        return listentidad;
                    }
                    else
                    {
                        return new List<SalidaProductoErp>();
                    }
                }
                else
                {
                    return new List<SalidaProductoErp>();
                }
            }
            catch (Exception ex)
            {
                return new List<SalidaProductoErp>();
            }
        }

        public static async Task<List<MovimientoErp>> CargaMovimientosSalidas()
        {
            _connectionInfo = _ap.getServerurlKey();
            try
            {
                HttpClient client = new HttpClient();
                //var _connectionInfo = serverurl;
                client.BaseAddress = new Uri(_connectionInfo);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("Movimiento/t_movimientoInventarioSalidaAll");

                if (response.IsSuccessStatusCode && response.RequestMessage != null)
                {
                    JArray jentidad = JArray.Parse(await response.Content.ReadAsStringAsync());
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.NullValueHandling = NullValueHandling.Ignore;
                    settings.MissingMemberHandling = MissingMemberHandling.Ignore;
                    if (jentidad != null && jentidad.Count > 0)
                    {
                        var listlineaxs = JsonConvert.DeserializeObject<List<MovimientoErp>>(jentidad.ToString(), settings);
                        return listlineaxs;
                    }
                    else
                    {
                        return new List<MovimientoErp>();
                    }
                }
                else
                {
                    return new List<MovimientoErp>();
                }
            }
            catch (Exception ex)
            {
                return new List<MovimientoErp>();
            }
        }

        internal static async Task<List<VehiculoErp>> CargaVehiculos()
        {
            _connectionInfo = _ap.getServerurlKey();
            try
            {
                HttpClient client = new HttpClient();
                //var _connectionInfo = serverurl;
                client.BaseAddress = new Uri(_connectionInfo);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("Tallererp/t_vehiculoSelectAll");

                if (response.IsSuccessStatusCode && response.RequestMessage != null)
                {
                    JArray jentidad = JArray.Parse(await response.Content.ReadAsStringAsync());
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.NullValueHandling = NullValueHandling.Ignore;
                    settings.MissingMemberHandling = MissingMemberHandling.Ignore;
                    if (jentidad != null && jentidad.Count > 0)
                    {
                        var listlineaxs = JsonConvert.DeserializeObject<List<VehiculoErp>>(jentidad.ToString(), settings);
                        return listlineaxs;
                    }
                    else
                    {
                        return new List<VehiculoErp>();
                    }
                }
                else
                {
                    return new List<VehiculoErp>();
                }
            }
            catch (Exception ex)
            {
                return new List<VehiculoErp>();
            }
        }
    }
}