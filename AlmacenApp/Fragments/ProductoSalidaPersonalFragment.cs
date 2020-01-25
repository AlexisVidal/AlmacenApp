using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlmacenApp.Adapter;
using AlmacenApp.Clases;
using AlmacenApp.Models;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using AlmacenApp.Resources.DataHelpers;

namespace AlmacenApp.Fragments
{
    public class ProductoSalidaPersonalFragment : DialogFragment
    {
        View view = null;
        private string _connectionInfo = "";
        private AppPreferences _ap;
        private readonly Context _mContext = Application.Context;
        Context thiscontext;
        TextView txtTrabajadorNombre;
        string idcodigogeneral = "";
        string nombretrabajador = "";
        private EditText edt_searchproductsListSalida;
        ListView rvproductosSalida;
        List<ProductoErpLite> listproductos;
        TextView txtProductoSelected;
        EditText edtCantxSalida;
        Button btnCantSalida;

        int id_producto = 0;
        string cod_producto = "";
        string nom_producto = "";
        string descripcion_producto_tipo = "";
        DataHelpers db;
        int idalmacen = 0;
        int resultadomovimiento = 0;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }
        public override void OnStart()
        {
            base.OnStart();
            Dialog.Window.SetLayout(ViewGroup.LayoutParams.FillParent, WindowManagerLayoutParams.FillParent);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnCreateView(inflater, container, savedInstanceState);
            view = inflater.Inflate(Resource.Layout.producto_salida_add_product_fragment, container, true);
            thiscontext = inflater.Context;
            _ap = new AppPreferences(_mContext);
            idalmacen = Convert.ToInt32(_ap.getIdAlmacenTempKey());
            db = new DataHelpers();
            db.CreateDatabaseInventarioInicial();
            listproductos = new List<ProductoErpLite>();
            _connectionInfo = _ap.getServerurlKey();
            idcodigogeneral = _ap.getIdCodigoGeneralTempKey();
            txtTrabajadorNombre = view.FindViewById<TextView>(Resource.Id.txtTrabajadorNombre);
            nombretrabajador = _ap.getNombreTrabajadorTempKey();
            if (nombretrabajador != "")
            {
                this.Activity.RunOnUiThread(() => txtTrabajadorNombre.Text = nombretrabajador);
            }
            rvproductosSalida = view.FindViewById<ListView>(Resource.Id.rvproductosSalida);
            edt_searchproductsListSalida = view.FindViewById<EditText>(Resource.Id.edt_searchproductsListSalida);
            edt_searchproductsListSalida.EditorAction += (sender, args) =>
            {
                if (args.ActionId == ImeAction.Search)
                {
                    string x = edt_searchproductsListSalida.Text;
                    _btnSearchP_Click(sender, args);
                    args.Handled = true;
                }
            };
            txtProductoSelected = view.FindViewById<TextView>(Resource.Id.txtProductoSelected);
            edtCantxSalida = view.FindViewById<EditText>(Resource.Id.edtCantxSalida);
            btnCantSalida = view.FindViewById<Button>(Resource.Id.btnCantSalida);
            btnCantSalida.Click += BtnCantSalida_Click;
            return view;
        }

        internal void EliminaMovimiento(object sender, string idmovimiento)
        {
            try
            {
                if (idmovimiento != "0")
                {

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void BtnCantSalida_Click(object sender, EventArgs e)
        {
            try
            {
                decimal cant = 0;
                cant = Convert.ToDecimal(edtCantxSalida.Text);
                if (id_producto == 0)
                {
                    this.Activity.RunOnUiThread(() => Toast.MakeText(thiscontext, "Seleccione primero el producto!", ToastLength.Short).Show());
                }
                else
                {
                    if (cant > 0)
                    {
                        MovimientoErpLite movimiento = new MovimientoErpLite
                        {
                            fk_movimiento_tipo = 8,  //inventario inicial
                            fk_guia_remision_detalle = 0,
                            fk_venta_detalle = 0,
                            fk_comprobante_traslado_detalle = 0,
                            fk_nota_credito_detalle = 0,
                            fk_almacen = idalmacen,
                            fk_producto = id_producto,
                            cod_producto = cod_producto,
                            nombre_producto = nom_producto,
                            producto_tipo = descripcion_producto_tipo,
                            cantidad = cant,
                            IDCODIGOGENERAL = idcodigogeneral
                        };
                        resultadomovimiento = db.insertIntoMovimiento(movimiento);
                    }
                    else
                    {
                        this.Activity.RunOnUiThread(() => Toast.MakeText(thiscontext, "Seleccione primero el producto!", ToastLength.Short).Show());
                    }
                }
            }
            catch (Exception ex)
            {
                this.Activity.RunOnUiThread(() => Toast.MakeText(thiscontext, "Seleccione primero el producto!", ToastLength.Short).Show());
            }
        }

        private void _btnSearchP_Click(object sender, TextView.EditorActionEventArgs args)
        {
            string buscar = edt_searchproductsListSalida.Text;
            if (buscar != "" && buscar.Length > 2)
            {
                InputMethodManager imm = (InputMethodManager)this.Activity.GetSystemService(Context.InputMethodService);
                imm.HideSoftInputFromWindow(edt_searchproductsListSalida.WindowToken, 0);

                listproductos = new List<ProductoErpLite>();
                var b = Task.Run(async () =>
                {
                    var info = await Data.BuscaProductosFkalmacenLite(buscar, idalmacen);
                    if (info != null)
                    {
                        listproductos = info;
                        ProductoErpListFragmentAdapter adapter = null;
                        this.Activity.RunOnUiThread(() => adapter = new ProductoErpListFragmentAdapter(_mContext, listproductos, this));
                        this.Activity.RunOnUiThread(() => rvproductosSalida.Adapter = adapter);
                        this.Activity.RunOnUiThread(() => rvproductosSalida.ItemClick += RvproductosSalida_ItemClick);

                    }
                    else
                    {
                        this.Activity.RunOnUiThread(() => Toast.MakeText(thiscontext, "Imposible Conectarse al servidor!", ToastLength.Short).Show());
                    }

                });
                b.Wait();
            }
        }
        private void RvproductosSalida_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            try
            {
                id_producto = listproductos[e.Position].id_producto;
                cod_producto = listproductos[e.Position].cod_producto.ToString();
                nom_producto = listproductos[e.Position].nom_producto.ToString();
                descripcion_producto_tipo = listproductos[e.Position].producto_tipo.ToString();
                if (id_producto > 0 && nom_producto != "")
                {
                    this.Activity.RunOnUiThread(() => txtProductoSelected.Text = nom_producto);
                }
                else
                {
                    id_producto = 0;
                    cod_producto = "";
                    nom_producto = "";
                    descripcion_producto_tipo = "";
                }
            }
            catch (Exception ex)
            {
                id_producto = 0;
                cod_producto = "";
                nom_producto = "";
                descripcion_producto_tipo = "";
            }
        }
    }
}