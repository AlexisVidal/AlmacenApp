using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AlmacenApp.Clases
{
    public class AppPreferences
    {
        private ISharedPreferences idUsuarioSP;
        private ISharedPreferencesEditor idUsuarioSPE;
        private Context mContetxt;

        private static String PREFERENCE_ACCESS_KEY = "PREFERENCE_ACCESS_KEY";
        public AppPreferences(Context context)
        {
            this.mContetxt = context;
            idUsuarioSP = PreferenceManager.GetDefaultSharedPreferences(mContetxt);
            idUsuarioSPE = idUsuarioSP.Edit();
        }
        public void saveAccessKey(string key)
        {
            idUsuarioSPE.PutString(PREFERENCE_ACCESS_KEY, key);
            idUsuarioSPE.Commit();
        }
        public string getAccessKey()
        {
            return idUsuarioSP.GetString(PREFERENCE_ACCESS_KEY, "");
        }

        public static String IDUSUARIO = "IDUSUARIO";
        public void saveIdKey(string key)
        {
            idUsuarioSPE.PutString(IDUSUARIO, key);
            idUsuarioSPE.Commit();
        }
        public string getIdKey()
        {
            return idUsuarioSP.GetString(IDUSUARIO, "");
        }

        public static String IDCODIGOGENERALLOGIN = "IDCODIGOGENERALLOGIN";
        public void saveIdCodigoGeneralLoginTempKey(string key)
        {
            idUsuarioSPE.PutString(IDCODIGOGENERALLOGIN, key);
            idUsuarioSPE.Commit();
        }
        public string getIdCodigoGeneralLoginTempKey()
        {
            return idUsuarioSP.GetString(IDCODIGOGENERALLOGIN, "");
        }
        public static String NOMBRES = "NOMBRES";
        public void saveNombresKey(string key)
        {
            idUsuarioSPE.PutString(NOMBRES, key);
            idUsuarioSPE.Commit();
        }
        public string getNombresKey()
        {
            return idUsuarioSP.GetString(NOMBRES, "");
        }

        public static String RUC = "RUC";
        public void saveRucKey(string key)
        {
            idUsuarioSPE.PutString(RUC, key);
            idUsuarioSPE.Commit();
        }
        public string getRucKey()
        {
            return idUsuarioSP.GetString(RUC, "");
        }

        public static String EMPRESA = "EMPRESA";
        public void saveEmpresaKey(string key)
        {
            idUsuarioSPE.PutString(EMPRESA, key);
            idUsuarioSPE.Commit();
        }
        public string getEmpresaKey()
        {
            return idUsuarioSP.GetString(EMPRESA, "");
        }

        public static String DIRECCION = "DIRECCION";
        public void saveDireccionKey(string key)
        {
            idUsuarioSPE.PutString(DIRECCION, key);
            idUsuarioSPE.Commit();
        }
        public string getDireccionKey()
        {
            return idUsuarioSP.GetString(DIRECCION, "");
        }

        public static String SERVERURL = "SERVERURL";
        public void saveServerurlKey(string key)
        {
            idUsuarioSPE.PutString(SERVERURL, key);
            idUsuarioSPE.Commit();
        }



        public string getServerurlKey()
        {
            return idUsuarioSP.GetString(SERVERURL, "");
        }

        public static String IDPRODUCTOLINEA = "IDPRODUCTOLINEA";
        public void saveIdProductoLineaKey(string key)
        {
            idUsuarioSPE.PutString(IDPRODUCTOLINEA, key);
            idUsuarioSPE.Commit();
        }
        public string getIdProductoLineaKey()
        {
            return idUsuarioSP.GetString(IDPRODUCTOLINEA, "");
        }

        public static String PRODUCTOLINEA = "PRODUCTOLINEA";
        public void saveProductoLineaKey(string key)
        {
            idUsuarioSPE.PutString(PRODUCTOLINEA, key);
            idUsuarioSPE.Commit();
        }
        public string getProductoLineaKey()
        {
            return idUsuarioSP.GetString(PRODUCTOLINEA, "");
        }

        public static String IDPRODUCTOUNIDAD = "IDPRODUCTOUNIDAD";
        public void saveIdProductoUnidadKey(string key)
        {
            idUsuarioSPE.PutString(IDPRODUCTOUNIDAD, key);
            idUsuarioSPE.Commit();
        }
        public string getIdProductoUnidadKey()
        {
            return idUsuarioSP.GetString(IDPRODUCTOUNIDAD, "");
        }

        public static String PRODUCTOUNIDAD = "PRODUCTOUNIDAD";
        public void saveProductoUnidadKey(string key)
        {
            idUsuarioSPE.PutString(PRODUCTOUNIDAD, key);
            idUsuarioSPE.Commit();
        }
        public string getProductoUnidadKey()
        {
            return idUsuarioSP.GetString(PRODUCTOUNIDAD, "");
        }

        public static String IDPRODUCTOTIPO = "IDPRODUCTOTIPO";
        public void saveIdProductoTipoKey(string key)
        {
            idUsuarioSPE.PutString(IDPRODUCTOTIPO, key);
            idUsuarioSPE.Commit();
        }
        public string getIdProductoTipoKey()
        {
            return idUsuarioSP.GetString(IDPRODUCTOTIPO, "");
        }

        public static String PRODUCTOTIPO = "PRODUCTOTIPO";
        public void saveProductoTipoKey(string key)
        {
            idUsuarioSPE.PutString(PRODUCTOTIPO, key);
            idUsuarioSPE.Commit();
        }
        public string getProductoTipoKey()
        {
            return idUsuarioSP.GetString(PRODUCTOTIPO, "");
        }

        public static String IDPRODUCTO = "IDPRODUCTO";
        public void saveIdProductoKey(string key)
        {
            idUsuarioSPE.PutString(IDPRODUCTO, key);
            idUsuarioSPE.Commit();
        }
        public string getIdProductoKey()
        {
            return idUsuarioSP.GetString(IDPRODUCTO, "");
        }

        public static String PRODUCTO = "PRODUCTO";
        public void saveProductoKey(string key)
        {
            idUsuarioSPE.PutString(PRODUCTO, key);
            idUsuarioSPE.Commit();
        }
        public string getProductoKey()
        {
            return idUsuarioSP.GetString(PRODUCTO, "");
        }

        public static String IDPRODUCTOMARCA = "IDPRODUCTOMARCA";
        public void saveIdProductoMarcaKey(string key)
        {
            idUsuarioSPE.PutString(IDPRODUCTOMARCA, key);
            idUsuarioSPE.Commit();
        }
        public string getIdProductoMarcaKey()
        {
            return idUsuarioSP.GetString(IDPRODUCTOMARCA, "");
        }

        public static String PRODUCTOMARCA = "PRODUCTOMARCA";
        public void saveProductoMarcaKey(string key)
        {
            idUsuarioSPE.PutString(PRODUCTOMARCA, key);
            idUsuarioSPE.Commit();
        }
        public string getProductoMarcaKey()
        {
            return idUsuarioSP.GetString(PRODUCTOMARCA, "");
        }

        public static String PRODUCTOSKU = "PRODUCTOSKU";
        public void saveProductoSkuKey(string key)
        {
            idUsuarioSPE.PutString(PRODUCTOSKU, key);
            idUsuarioSPE.Commit();
        }
        public string getProductoSkuKey()
        {
            return idUsuarioSP.GetString(PRODUCTOSKU, "");
        }

        public static String IDINVENTARIO = "IDINVENTARIO";

        public void saveIdInventarioTempKey(string key)
        {
            idUsuarioSPE.PutString(IDINVENTARIO, key);
            idUsuarioSPE.Commit();
        }
        public string getIdInventarioTempKey()
        {
            return idUsuarioSP.GetString(IDINVENTARIO, "");
        }
        public static String IDMOVINVENTARIO = "IDMOVINVENTARIO";
        public void saveIdMovimientoTempKey(string key)
        {
            idUsuarioSPE.PutString(IDMOVINVENTARIO, key);
            idUsuarioSPE.Commit();
        }
        public string getIdMovimientoTempKey()
        {
            return idUsuarioSP.GetString(IDMOVINVENTARIO, "");
        }
        public static String INVENTARIONOMPROD = "INVENTARIONOMPROD";
        public void saveInventarioNombreProdTempKey(string key)
        {
            idUsuarioSPE.PutString(INVENTARIONOMPROD, key);
            idUsuarioSPE.Commit();
        }
        public string getInventarioNombreProdTempKey()
        {
            return idUsuarioSP.GetString(INVENTARIONOMPROD, "");
        }
        public static String CANTINVENTARIO = "CANTINVENTARIO";
        public void saveInventarioCantTempKey(string key)
        {
            idUsuarioSPE.PutString(CANTINVENTARIO, key);
            idUsuarioSPE.Commit();
        }
        public string getInventarioCantTempKey()
        {
            return idUsuarioSP.GetString(CANTINVENTARIO, "");
        }

        public static String IDCODIGOGENERAL = "IDCODIGOGENERAL";
        public void saveIdCodigoGeneralTempKey(string key)
        {
            idUsuarioSPE.PutString(IDCODIGOGENERAL, key);
            idUsuarioSPE.Commit();
        }
        public string getIdCodigoGeneralTempKey()
        {
            return idUsuarioSP.GetString(IDCODIGOGENERAL, "");
        }

        public static String NOMBRETRABAJADOR = "NOMBRETRABAJADOR";
        public void saveNombreTrabajadorTempKey(string key)
        {
            idUsuarioSPE.PutString(NOMBRETRABAJADOR, key);
            idUsuarioSPE.Commit();
        }
        public string getNombreTrabajadorTempKey()
        {
            return idUsuarioSP.GetString(NOMBRETRABAJADOR, "");
        }

        public static String IDALMACEN = "IDALMACEN";
        public void saveIdAlmacenTempKey(string key)
        {
            idUsuarioSPE.PutString(IDALMACEN, key);
            idUsuarioSPE.Commit();
        }
        public string getIdAlmacenTempKey()
        {
            return idUsuarioSP.GetString(IDALMACEN, "");
        }

        public static String ALMACEN = "ALMACEN";
        public void saveAlmacenTempKey(string key)
        {
            idUsuarioSPE.PutString(ALMACEN, key);
            idUsuarioSPE.Commit();
        }
        public string getAlmacenTempKey()
        {
            return idUsuarioSP.GetString(ALMACEN, "");
        }


        public static String PRODUCTOABREVIATURA = "PRODUCTOABREVIATURA";
        public void saveProductoAbreviaturaKey(string key)
        {
            idUsuarioSPE.PutString(PRODUCTOABREVIATURA, key);
            idUsuarioSPE.Commit();
        }
        public string getProductoAbreviaturaKey()
        {
            return idUsuarioSP.GetString(PRODUCTOABREVIATURA, "");
        }


        public static String IDSALIDAMOVIMIENTO = "IDSALIDAMOVIMIENTO";
        public void saveIdSalidaMovimientoTempKey(string key)
        {
            idUsuarioSPE.PutString(IDSALIDAMOVIMIENTO, key);
            idUsuarioSPE.Commit();
        }
        public string getIdSalidaMovimientoTempKey()
        {
            return idUsuarioSP.GetString(IDSALIDAMOVIMIENTO, "");
        }
    }
}