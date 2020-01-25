using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlmacenApp.Adapter;
using AlmacenApp.Clases;
using AlmacenApp.Fragments;
using AlmacenApp.Models;
using AlmacenApp.Resources.DataHelpers;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
namespace AlmacenApp.Activities
{
    [Activity(Label = "Salida de Producto", MainLauncher = false, WindowSoftInputMode = SoftInput.AdjustPan, ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape, Theme = "@style/NavigationStyle")]
    public class ProductoSalidaActivity : AppCompatActivity, IDialogInterfaceOnDismissListener
    {
        private AppPreferences _ap;
        private readonly Context _mContext = Application.Context;
        public static string serverurl = "";
        List<ProductoErpLite> listproductos;
        List<PersonalErpLite> listpersonal;
        ListView rvpersonalSalida;
        int eliminable = 0;
        int idproducto = 0;
        DrawerLayout drawerLayout;
        NavigationView navigationView;
        TextView navheader_username;
        V7Toolbar toolbar;
        private EditText edt_searchPersonaList;
        int idalmacen = 0;
        int instanceupdateitem = 0;
        private View _view;
        DataHelpers db;

        ProgressDialog progress;
        ProgressDialog progress2;
        ProgressDialog progress3;
        Spinner spAlmacenSimpleSIp;
        List<AlmacenErp> lalmaceneslite;
        Button btnRegularizaSalida;
        int existensalidas = 0;
        List<SalidaProductoErpLite> lsalidaslite = null;
        Button btnNuevaDataSalida;
        ListView rvpersonalMovimientos;
        List<MovimientoErpLite> lmovimientoslite;
        ProductoSalidaMovimientoActivityAdapter productsalidaactivity;
        public void OnDismiss(IDialogInterface dialog)
        {
            try
            {
                instanceupdateitem = 0;
                LlenaSalidaProductosLocalAUpdate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        internal void EliminaSalidaMovimiento(object sender, string id_movimiento, string nombre_full, string nombre_producto, string abreviatura, string cantidad)
        {
            _ap.saveIdMovimientoTempKey(id_movimiento);
            _ap.saveNombreTrabajadorTempKey(nombre_full);
            _ap.saveProductoKey(nombre_producto);
            _ap.saveProductoAbreviaturaKey(abreviatura);
            _ap.saveInventarioCantTempKey(cantidad);
            eliminable = Convert.ToInt32(id_movimiento);
            if (eliminable > 0)
            {
                this.RunOnUiThread(() =>
                {
                    Toast.MakeText(_mContext, "El item no se puede eliminar!", ToastLength.Long).Show();
                });
            }
            else
            {

                var transaction = this.FragmentManager.BeginTransaction();
                movimien fragdelete = new LineaDeleteFragment();
                fragdelete.Show(transaction, "fragdelete");
            }
        }

        private async Task LlenaSalidaProductosLocalAUpdate()
        {
            int idlastsalida = 0;
            try
            {
                RunOnUiThread(() => rvpersonalMovimientos.SetAdapter(null));
            }
            catch (Exception re)
            {

            }
            try
            {
                db = new DataHelpers();
                lsalidaslite = new List<SalidaProductoErpLite>();
                var vlsalidaslite = db.selectTableSalidaProducto();
                if (vlsalidaslite != null && vlsalidaslite.Count > 0)
                {
                    lsalidaslite = vlsalidaslite.ToList();
                    idlastsalida = vlsalidaslite.Select(z => z.Id).FirstOrDefault();
                    lmovimientoslite = new List<MovimientoErpLite>();
                    var vlmovimientoslite = db.selectTableMovimiento();
                    if (vlmovimientoslite.Any())
                    {
                        var salidamovimientos = vlmovimientoslite.Where(x => x.IdSalida == idlastsalida && x.fk_movimiento_tipo == 8).ToList();
                        if (salidamovimientos.Any())
                        {
                            RunOnUiThread(() => productsalidaactivity = new ProductoSalidaMovimientoActivityAdapter(_mContext, salidamovimientos, this));
                            RunOnUiThread(() => rvpersonalMovimientos.Adapter = productsalidaactivity);
                            RunOnUiThread(() => rvpersonalMovimientos.ItemClick += rvpersonalMovimientos_ItemClick);
                        }
                        else
                        {
                            RunOnUiThread(() => rvpersonalMovimientos.SetAdapter(null));
                        }
                    }
                    else
                    {
                        RunOnUiThread(() => rvpersonalMovimientos.SetAdapter(null));
                    }
                    

                    int nconcantidad = lsalidaslite.Count;
                    if (nconcantidad > 0)
                    {
                        existensalidas = 1;
                        RunOnUiThread(() => Toast.MakeText(this, "EXISTEN MOVIMIENTOS POR REGULARIZAR", ToastLength.Long).Show());
                        RunOnUiThread(() => btnRegularizaSalida.Enabled = true);
                    }
                    else
                    {
                        existensalidas = 0;
                        RunOnUiThread(() => btnRegularizaSalida.Enabled = false);
                    }


                }
                else
                {
                    RunOnUiThread(() => Toast.MakeText(this, "NO EXISTEN MOVIMIENTOS POR REGULARIZAR!", ToastLength.Short).Show());
                }
            }
            catch (Exception ex)
            {
                RunOnUiThread(() => Toast.MakeText(this, "OCURRIO UN ERROR AL MOSTRAR STOCK DE PRODUCTSO! EXEPCION: " + ex.Message, ToastLength.Short).Show());
            }
        }

        private void rvpersonalMovimientos_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.producto_salida_layout);
            _ap = new AppPreferences(_mContext);
            db = new DataHelpers();
            db.CreateDatabaseInventarioInicial();

            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            toolbar = FindViewById<V7Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            var drawerToggle = new ActionBarDrawerToggle(this, drawerLayout, toolbar, Resource.String.drawer_open, Resource.String.drawer_close);
            drawerLayout.SetDrawerListener(drawerToggle);
            drawerToggle.SyncState();
            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            var headerview = navigationView.GetHeaderView(0);
            navheader_username = headerview.FindViewById<TextView>(Resource.Id.navheader_username);
            setupDrawerContent(navigationView); //Calling Function 
            ShowDataUser();
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;

            serverurl = _ap.getServerurlKey();

            rvpersonalSalida = FindViewById<ListView>(Resource.Id.rvpersonalSalida);
            edt_searchPersonaList = FindViewById<EditText>(Resource.Id.edt_searchPersonaList);
            edt_searchPersonaList.EditorAction += (sender, args) =>
            {
                if (args.ActionId == ImeAction.Search)
                {
                    string x = edt_searchPersonaList.Text;
                    _btnSearch_Click(sender, args);
                    args.Handled = true;
                }
            };
            spAlmacenSimpleSIp = FindViewById<Spinner>(Resource.Id.spAlmacenSimpleSIp);
            rvpersonalMovimientos = FindViewById<ListView>(Resource.Id.rvpersonalMovimientos);
            try
            {
                var xlalmaceneslite = await Data.CargaAlmacenes();
                if (xlalmaceneslite != null && xlalmaceneslite.Count > 0)
                {
                    AlmacenErp newalmacen = new AlmacenErp()
                    {
                        id_almacen = -1,
                        nombre = ""
                    };
                    xlalmaceneslite.Add(newalmacen);
                    lalmaceneslite = xlalmaceneslite.OrderBy(x => x.nombre).ToList();
                    LlenaAlmacen(lalmaceneslite);
                }
            }
            catch (Exception ex)
            {

            }

            btnRegularizaSalida = FindViewById<Button>(Resource.Id.btnRegularizaSalida);
            LlenaSalidaProductosLocalAUpdate();
            btnNuevaDataSalida = FindViewById<Button>(Resource.Id.btnNuevaDataSalida);
            btnNuevaDataSalida.Click += BtnNuevaDataSalida_Click;
        }

        private void BtnNuevaDataSalida_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
        }

        private async void LlenaAlmacen(List<AlmacenErp> lalmaceneslite)
        {
            try
            {
                ArrayAdapter adapter = new ArrayAdapter(_mContext, Resource.Layout.spinner_item_medium, lalmaceneslite.OrderBy(x => x.nombre).ToList());
                RunOnUiThread(() => adapter.SetDropDownViewResource(Resource.Layout.spinner_dropdown_item));
                RunOnUiThread(() => spAlmacenSimpleSIp.ItemSelected += SpAlmacenSimpleSI_ItemSelected);
                RunOnUiThread(() => spAlmacenSimpleSIp.Adapter = adapter);
            }
            catch (Exception ex)
            {
                RunOnUiThread(() => Toast.MakeText(this, "NO SE PUDO CARGAR DATA DE ALMACENES! EXCEPCION: " + ex.Message, ToastLength.Short).Show());
            }
        }
        private void SpAlmacenSimpleSI_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {
                idalmacen = lalmaceneslite.ElementAt(spAlmacenSimpleSIp.SelectedItemPosition).id_almacen;
                _ap.saveIdAlmacenTempKey(idalmacen.ToString());
                _ap.saveAlmacenTempKey(lalmaceneslite.ElementAt(spAlmacenSimpleSIp.SelectedItemPosition).nombre.ToString());
            }
            catch (Exception exception)
            {
                idalmacen = 0;
            }
            try
            {
                //RunOnUiThread(() => listProductInventarioSimpleI.SetAdapter(null));
            }
            catch (Exception re)
            {

            }
        }
        private void _btnSearch_Click(object sender, TextView.EditorActionEventArgs args)
        {
            //idproducto = 0;
            string buscar = edt_searchPersonaList.Text;
            if (idalmacen < 0)
            {
                RunOnUiThread(() => Toast.MakeText(this, "Debe seleccionar almacen primero!", ToastLength.Short).Show());
            }
            else
            {
                if (buscar != "" && buscar.Length > 2)
                {
                    InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
                    imm.HideSoftInputFromWindow(edt_searchPersonaList.WindowToken, 0);

                    progress = new ProgressDialog(this);
                    progress.Indeterminate = true;
                    progress.SetProgressStyle(ProgressDialogStyle.Spinner);
                    progress.SetMessage("Buscando... espere...");
                    progress.SetCancelable(false);
                    RunOnUiThread(() =>
                    {
                        progress.Show();
                    });
                    listpersonal = new List<PersonalErpLite>();
                    var b = Task.Run(async () =>
                    {
                        var info = await Data.BuscaPersonal(buscar);
                        if (info != null)
                        {
                            listpersonal = info;
                            PersonalListActivityAdapter adapter = null;
                            RunOnUiThread(() => adapter = new PersonalListActivityAdapter(_mContext, listpersonal, this));
                            RunOnUiThread(() => rvpersonalSalida.Adapter = adapter);
                            RunOnUiThread(() => rvpersonalSalida.ItemClick += ListPersonalManage_ItemClick);

                        }
                        else
                        {
                            RunOnUiThread(() => Toast.MakeText(this, "Imposible Conectarse al servidor!", ToastLength.Short).Show());
                        }

                    }).ContinueWith(info => RunOnUiThread(() => progress.Hide()));
                }
            }
            
        }

        private void ListPersonalManage_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            string Id = listpersonal[e.Position].Id.ToString();
            string id_personal = listpersonal[e.Position].IDCODIGOGENERAL.ToString();
            string nombre_persona = listpersonal[e.Position].nomb_full.ToString();
            if (id_personal.Trim() != "")
            {
                _ap.saveIdCodigoGeneralTempKey(id_personal);
                _ap.saveNombreTrabajadorTempKey(nombre_persona);
                var transaction = this.FragmentManager.BeginTransaction();
                ProductoSalidaPersonalFragment fragnew = new ProductoSalidaPersonalFragment();
                fragnew.Show(transaction, "fragnew");
            }
            
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            navigationView.InflateMenu(Resource.Menu.nav_menu_almacen); //Navigation Drawer Layout Menu Creation 
            return true;
        }
        private void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            var menuItem = e.MenuItem;
            switch (menuItem.ItemId)
            {
                case Resource.Id.menu_almacen_lineas:
                    toolbar.Title = "Lineas";
                    var newact = new Intent(this, typeof(LineaActivity));
                    StartActivity(newact);
                    break;
                case Resource.Id.menu_almacen_productos:
                    toolbar.Title = "Productos";
                    var newactp = new Intent(this, typeof(ProductoActivity));
                    StartActivity(newactp);
                    break;
                case Resource.Id.menu_almacen_inventario:
                    toolbar.Title = "Inventario";
                    var newacti = new Intent(this, typeof(InventarioActivity));
                    StartActivity(newacti);
                    break;
                case Resource.Id.menu_almacen_salidas:
                    toolbar.Title = "Salidas";
                    var newactis = new Intent(this, typeof(ProductoSalidaActivity));
                    StartActivity(newactis);
                    break;
                case Resource.Id.menu_almacen_sublineas:
                    toolbar.Title = "Tipos";
                    var newactt = new Intent(this, typeof(TipoActivity));
                    StartActivity(newactt);
                    break;
            }
        }

        private void ShowDataUser()
        {
            RunOnUiThread(() => navheader_username.Text = _ap.getNombresKey());
        }

        private void setupDrawerContent(NavigationView navigationView)
        {
            navigationView.NavigationItemSelected += (sender, e) =>
            {
                e.MenuItem.SetChecked(true);
                drawerLayout.CloseDrawers();
            };
        }
    }
}