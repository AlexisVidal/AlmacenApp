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
        private EditText edt_observacionSalida;
        ListView rvproductosSalida;
        List<ProductoErpLite> listproductos;

        ListView rvproductosSelectedSalida;

        TextView txtProductoSelected;
        EditText edtCantxSalida;
        Button btnCantSalida;

        int id_producto = 0;
        string cod_producto = "";
        string nom_producto = "";
        string descripcion_producto_tipo = "";
        string abreviatura = "";
        string almacen = "";
        DataHelpers db;
        int idalmacen = 0;
        int resultadomovimiento = 0;
        Button btnClosePSIFrg;
        int idsalidaactual = 0;
        int idvehiculo = 0;

        List<SalidaProductoErpLite> lsalidaslite;
        List<MovimientoErpLite> lmovimientoslite;
        ProductoSalidaPersonalFragmentAdapter productsalidaactivity;

        Spinner spVehiculos;
        List<VehiculoErp> lvehiculos;

        public override void OnDismiss(IDialogInterface dialog)
        {
            base.OnDismiss(dialog);
            Activity activity = this.Activity;
            ((IDialogInterfaceOnDismissListener)activity).OnDismiss(dialog);
        }
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
            idsalidaactual = Convert.ToInt32(_ap.getIdSalidaMovimientoTempKey());
            idalmacen = Convert.ToInt32(_ap.getIdAlmacenTempKey());
            db = new DataHelpers();
            db.CreateDatabaseInventarioInicial();
            listproductos = new List<ProductoErpLite>();
            _connectionInfo = _ap.getServerurlKey();
            idcodigogeneral = _ap.getIdCodigoGeneralTempKey();
            nom_producto = _ap.getProductoKey();
            abreviatura = _ap.getProductoAbreviaturaKey();
            almacen = _ap.getAlmacenTempKey();
            txtTrabajadorNombre = view.FindViewById<TextView>(Resource.Id.txtTrabajadorNombre);
            btnClosePSIFrg = view.FindViewById<Button>(Resource.Id.btnClosePSIFrg);
            nombretrabajador = _ap.getNombreTrabajadorTempKey();
            if (nombretrabajador != "")
            {
                this.Activity.RunOnUiThread(() => txtTrabajadorNombre.Text = nombretrabajador);
            }
            rvproductosSalida = view.FindViewById<ListView>(Resource.Id.rvproductosSalida);
            rvproductosSelectedSalida = view.FindViewById<ListView>(Resource.Id.rvproductosSelectedSalida);

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

            edt_observacionSalida = view.FindViewById<EditText>(Resource.Id.edt_observacionSalida);

            txtProductoSelected = view.FindViewById<TextView>(Resource.Id.txtProductoSelected);
            edtCantxSalida = view.FindViewById<EditText>(Resource.Id.edtCantxSalida);
            btnCantSalida = view.FindViewById<Button>(Resource.Id.btnCantSalida);

            spVehiculos = view.FindViewById<Spinner>(Resource.Id.spVehiculos);
            
            btnCantSalida.Click += BtnCantSalida_Click;
            btnClosePSIFrg.Click += BtnClosePSIFrg_Click;
            LlenaData();
            return view;
        }

        private void BtnClosePSIFrg_Click(object sender, EventArgs e)
        {
            
            var mifragment = (ProductoSalidaPersonalFragment)FragmentManager.FindFragmentByTag("fragnew");
            mifragment?.Dismiss();
            var mifragment2 = (ProductoSalidaPersonalFragment)FragmentManager.FindFragmentByTag("fragnew");
            mifragment2?.Dismiss();
            Dismiss();
        }


        internal void EliminaMovimiento(object sender, string idmovimiento)
        {
            try
            {
                if (idmovimiento != "0")
                {
                    bool result = db.deleteQueryTableMovimientoById(Convert.ToInt32(idmovimiento));
                    if (result)
                    {
                        Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(thiscontext);
                        this.Activity.RunOnUiThread(() => alert.SetTitle("Correcto"));
                        this.Activity.RunOnUiThread(() => alert.SetMessage("Item se eliminó correctamente!"));
                        this.Activity.RunOnUiThread(() => alert.SetNeutralButton("Ok", delegate
                        {
                            LlenaData();
                        }));
                        this.Activity.RunOnUiThread(() => alert.Show());
                    }
                    else
                    {
                        this.Activity.RunOnUiThread(() => Toast.MakeText(thiscontext, "Tipo no eliminado!", ToastLength.Short).Show());
                    }
                }
            }
            catch (Exception ex)
            {
                LlenaData();
            }
        }

        private void BtnCantSalida_Click(object sender, EventArgs e)
        {
            try
            {
                decimal cant = 0;
                cant = Convert.ToDecimal(edtCantxSalida.Text);

                string observacion = edt_observacionSalida.Text.Trim().ToUpper();
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
                            nombre_full = nombretrabajador,
                            nombre_producto = nom_producto,
                            producto_tipo = descripcion_producto_tipo,
                            cantidad = cant,
                            IDCODIGOGENERAL = idcodigogeneral,
                            IdSalida = idsalidaactual,
                            abreviatura = abreviatura,
                            almacen = almacen,
                            fk_vehiculo = idvehiculo,
                            observaciones = observacion
                        };
                        resultadomovimiento = db.insertIntoMovimiento(movimiento);
                        if (resultadomovimiento > 0)
                        {
                            this.Activity.RunOnUiThread(() => Toast.MakeText(thiscontext, "Se insertó correctamente!", ToastLength.Short).Show());
                            id_producto = 0;
                            cod_producto = "";
                            nom_producto = "";
                            descripcion_producto_tipo = "";
                            almacen = "";
                            this.Activity.RunOnUiThread(() => edtCantxSalida.Text = "");
                            this.Activity.RunOnUiThread(() => txtProductoSelected.Text = "");
                            LlenaData();
                        }
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

        private void LlenaData()
        {
            int movimientos = 0;
            try
            {
                this.Activity.RunOnUiThread(() => rvproductosSelectedSalida.SetAdapter(null));
            }
            catch (Exception re)
            {

            }
            try
            {
                lmovimientoslite = new List<MovimientoErpLite>();
                var vlmovimientoslite = db.selectTableMovimiento();
                if (vlmovimientoslite.Any())
                {
                    var salidamovimientos = vlmovimientoslite.Where(x => x.IdSalida == idsalidaactual && x.fk_movimiento_tipo == 8 && x.IDCODIGOGENERAL.Equals(idcodigogeneral)).ToList();
                    if (salidamovimientos.Any())
                    {
                        movimientos = salidamovimientos.Count();
                        this.Activity.RunOnUiThread(() => productsalidaactivity = new ProductoSalidaPersonalFragmentAdapter(_mContext, salidamovimientos, this));
                        this.Activity.RunOnUiThread(() => rvproductosSelectedSalida.Adapter = productsalidaactivity);
                        this.Activity.RunOnUiThread(() => rvproductosSelectedSalida.ItemClick += rvproductosSelectedSalida_ItemClick);
                    }
                    else
                    {
                        this.Activity.RunOnUiThread(() => rvproductosSelectedSalida.SetAdapter(null));
                    }
                }
                else
                {
                    this.Activity.RunOnUiThread(() => rvproductosSelectedSalida.SetAdapter(null));
                }
            }
            catch (Exception ex)
            {
                this.Activity.RunOnUiThread(() => Toast.MakeText(thiscontext, "OCURRIO UN ERROR AL MOSTRAR LISTADO! EXEPCION: " + ex.Message, ToastLength.Short).Show());
            }
        }
        private void rvproductosSelectedSalida_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {

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
                abreviatura = listproductos[e.Position].abreviatura.ToString();
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
                    abreviatura = "";
                }
            }
            catch (Exception ex)
            {
                id_producto = 0;
                cod_producto = "";
                nom_producto = "";
                descripcion_producto_tipo = "";
                abreviatura = "";
            }
        }

        async public override void OnResume()
        {
            base.OnResume();
            try
            {
                var xlvehiculos = await Data.CargaVehiculos();
                if (xlvehiculos != null && xlvehiculos.Count > 0)
                {
                    VehiculoErp newalmacen = new VehiculoErp()
                    {
                        id_vehiculo = -1,
                        placa = "",
                        marca = "",
                        modelo = ""
                    };
                    xlvehiculos.Add(newalmacen);
                    lvehiculos = xlvehiculos.OrderBy(x => x.marca).ToList();
                    LlenaVehiculos(lvehiculos);
                }
            }
            catch (Exception ex)
            {

            }

        }
        private async void LlenaVehiculos(List<VehiculoErp> lvehiculos)
        {
            try
            {
                ArrayAdapter adapter = new ArrayAdapter(_mContext, Resource.Layout.spinner_item_medium, lvehiculos.OrderBy(x => x.marca).ToList());
                this.Activity.RunOnUiThread(() => adapter.SetDropDownViewResource(Resource.Layout.spinner_dropdown_item));
                this.Activity.RunOnUiThread(() => spVehiculos.ItemSelected += SpVehiculos_ItemSelected);
                this.Activity.RunOnUiThread(() => spVehiculos.Adapter = adapter);
            }
            catch (Exception ex)
            {
                this.Activity.RunOnUiThread(() => Toast.MakeText(thiscontext, "NO SE PUDO CARGAR DATA DE VEHICULOS! EXCEPCION: " + ex.Message, ToastLength.Short).Show());
            }
        }

        private void SpVehiculos_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {
                idvehiculo = lvehiculos.ElementAt(spVehiculos.SelectedItemPosition).id_vehiculo;
                _ap.saveIdVehiculoTempKey(idvehiculo.ToString());
                _ap.savePlacaTempKey(lvehiculos.ElementAt(spVehiculos.SelectedItemPosition).placa.ToString());
            }
            catch (Exception exception)
            {
                idvehiculo = 0;
            }
        }
    }
}