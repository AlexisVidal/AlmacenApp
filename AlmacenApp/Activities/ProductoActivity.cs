using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlmacenApp.Adapter;
using AlmacenApp.Clases;
using AlmacenApp.Models;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using AlmacenApp.Fragments;
using Android.Views.InputMethods;

namespace AlmacenApp.Activities
{
    [Activity(Label = "Producto", MainLauncher = false, WindowSoftInputMode = SoftInput.AdjustPan, ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape, Theme = "@style/NavigationStyle")]
    public class ProductoActivity : AppCompatActivity, IDialogInterfaceOnDismissListener
    {
        private AppPreferences _ap;
        private readonly Context _mContext = Application.Context;
        public static string serverurl = "";
        List<ProductoErp> listproductos;
        ProgressDialog progress;
        ProductoTipoActivityAdapter productoTipoAdapter;
        ListView listProductsManage;
        int eliminable = 0;

        DrawerLayout drawerLayout;
        NavigationView navigationView;
        TextView navheader_username;
        V7Toolbar toolbar;

        Button btnAllProducts;
        private EditText edt_searchproductsList;
        int idproducto = 0;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.producto_layout);
            _ap = new AppPreferences(_mContext);

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
            listproductos = new List<ProductoErp>();
            listProductsManage = FindViewById<ListView>(Resource.Id.listProductsManage);
            btnAllProducts = FindViewById<Button>(Resource.Id.btnAllProducts);
            btnAllProducts.Click += BtnAllProducts_Click;

            edt_searchproductsList = FindViewById<EditText>(Resource.Id.edt_searchproductsList);
            edt_searchproductsList.EditorAction += (sender, args) =>
            {
                if (args.ActionId == ImeAction.Search)
                {
                    string x = edt_searchproductsList.Text;
                    _btnSearch_Click(sender, args);
                    args.Handled = true;
                }
            };
            var fab = FindViewById<Refractored.Fab.FloatingActionButton>(Resource.Id.fabProductos);
            fab.AttachToListView(listProductsManage);
            fab.Click += (sender, args) =>
            {
                NuevoRegistro();
            };
            fab.Show();
        }

        private void NuevoRegistro()
        {
            CleanVars();
            var transaction = this.FragmentManager.BeginTransaction();
            ProductoNewFragment fragnewpro = new ProductoNewFragment();
            fragnewpro.Cancelable = false;
            fragnewpro.Show(transaction, "fragnewpro");
        }

        private void CleanVars()
        {
            try
            {
                _ap.saveIdProductoKey("0");
                _ap.saveIdProductoUnidadKey("0");
                _ap.saveProductoUnidadKey("");
                _ap.saveIdProductoTipoKey("0");
                _ap.saveIdProductoMarcaKey("0");
                _ap.saveProductoKey("");
                _ap.saveProductoTipoKey("");
                _ap.saveProductoMarcaKey("");
                _ap.saveProductoSkuKey("");
                this.RunOnUiThread(() => { edt_searchproductsList.Text = ""; });
            }
            catch (Exception ex)
            {

            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            navigationView.InflateMenu(Resource.Menu.nav_menu_almacen); //Navigation Drawer Layout Menu Creation 
            return true;
        }
        internal void EliminaProducto(object sender, string sidproducto, string productoitem, string cantproducts)
        {
            _ap.saveIdProductoKey(sidproducto);
            _ap.saveProductoKey(productoitem);
            eliminable = Convert.ToInt32(cantproducts);
            if (eliminable > 0)
            {
                this.RunOnUiThread(() =>
                {
                    Toast.MakeText(_mContext, "El item no se puede eliminar porque posee Productos en almacen. Debe eliminar el stock primero!", ToastLength.Long).Show();
                });
            }
            else
            {
                var transaction = this.FragmentManager.BeginTransaction();
                ProductoDeleteFragment fragdeletep = new ProductoDeleteFragment();
                fragdeletep.Show(transaction, "fragdeletep");
            }
        }

        private void _btnSearch_Click(object sender, TextView.EditorActionEventArgs args)
        {
            idproducto = 0;
            string buscar = edt_searchproductsList.Text;
            if (buscar != "" && buscar.Length > 2)
            {
                InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
                imm.HideSoftInputFromWindow(edt_searchproductsList.WindowToken, 0);

                progress = new ProgressDialog(this);
                progress.Indeterminate = true;
                progress.SetProgressStyle(ProgressDialogStyle.Spinner);
                progress.SetMessage("Buscando... espere...");
                progress.SetCancelable(false);
                RunOnUiThread(() =>
                {
                    progress.Show();
                });
                listproductos = new List<ProductoErp>();
                var b = Task.Run(async () =>
                {
                    var info = await Data.BuscaProductos(buscar);
                    if (info != null)
                    {
                        listproductos = info;
                        ProductoActivityAdapter adapter = null;
                        RunOnUiThread(() => adapter = new ProductoActivityAdapter(_mContext, listproductos, this));
                        RunOnUiThread(() => listProductsManage.Adapter = adapter);
                        RunOnUiThread(() => listProductsManage.ItemClick += ListProductsManage_ItemClick);

                    }
                    else
                    {
                        RunOnUiThread(() => Toast.MakeText(this, "Imposible Conectarse al servidor!", ToastLength.Short).Show());
                    }

                }).ContinueWith(info => RunOnUiThread(() => progress.Hide()));
            }
        }
        private void ListProductsManage_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            string eIdProduct = listproductos[e.Position].id_producto.ToString();
            string eNombProduct = listproductos[e.Position].nom_producto;
            string idtipoitem = listproductos[e.Position].fk_producto_tipo.ToString();
            string idmarcaitem = listproductos[e.Position].fk_producto_marca.ToString();
            string marcaitem = listproductos[e.Position].marca.ToString();
            string tipoitem = listproductos[e.Position].producto_tipo.ToString();
            string codigo_sku = listproductos[e.Position].codigo_sku;
            string cantproducts = listproductos[e.Position].cant_items.ToString();

            string idProductoUnidad = listproductos[e.Position].fk_unidad_medida.ToString();
            string productoUnidad = listproductos[e.Position].unidad_medida.ToString();

            try
            {
                idproducto = Convert.ToInt32(eIdProduct);
                _ap.saveIdProductoKey(eIdProduct);

                _ap.saveIdProductoUnidadKey(idProductoUnidad);
                _ap.saveProductoUnidadKey(productoUnidad);

                _ap.saveIdProductoTipoKey(idtipoitem);
                _ap.saveIdProductoMarcaKey(idmarcaitem);
                _ap.saveProductoKey(eNombProduct);
                _ap.saveProductoTipoKey(tipoitem);
                _ap.saveProductoMarcaKey(marcaitem);
                _ap.saveProductoSkuKey(codigo_sku);
                var transaction = this.FragmentManager.BeginTransaction();
                var prev = FragmentManager.FindFragmentByTag("fragnewpro");
                if (prev != null)
                {
                    transaction.Remove(prev);
                }
                ProductoNewFragment fragnewpro = new ProductoNewFragment();
                fragnewpro.Cancelable = false;
                fragnewpro.Show(transaction, "fragnewpro");
            }
            catch (Exception)
            {
                idproducto = 0;
            }
        }
        private void BtnAllProducts_Click(object sender, EventArgs e)
        {
            idproducto = 0;
            TraeProductos();
        }

        private void TraeProductos()
        {
            progress = new ProgressDialog(this);
            progress.Indeterminate = true;
            progress.SetProgressStyle(ProgressDialogStyle.Spinner);
            progress.SetMessage("Buscando... espere...");
            progress.SetCancelable(false);
            RunOnUiThread(() =>
            {
                progress.Show();
            });
            listproductos = new List<ProductoErp>();
            var b = Task.Run(async () =>
            {
                var info = await Data.CargaProductos();
                if (info != null)
                {
                    listproductos = info;
                    ProductoActivityAdapter adapter = null;
                    RunOnUiThread(() => adapter = new ProductoActivityAdapter(_mContext, listproductos, this));
                    RunOnUiThread(() => listProductsManage.Adapter = adapter);
                    RunOnUiThread(() => listProductsManage.ItemClick += ListProductsManage_ItemClick);

                }
                else
                {
                    RunOnUiThread(() => Toast.MakeText(this, "Imposible Conectarse al servidor!", ToastLength.Short).Show());
                }

            }).ContinueWith(info => RunOnUiThread(() => progress.Hide()));
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
                case Resource.Id.menu_almacen_sublineas:
                    toolbar.Title = "Tipos";
                    var newactt = new Intent(this, typeof(TipoActivity));
                    StartActivity(newactt);
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
                case Resource.Id.menu_almacen_personal:
                    toolbar.Title = "Reporte";
                    var newactirps = new Intent(this, typeof(ProductoPersonalHistoryActivity));
                    StartActivity(newactirps);
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

        public void OnDismiss(IDialogInterface dialog)
        {
            try
            {
                Intent _main = new Intent(this, typeof(ProductoActivity));
                _main.SetFlags(ActivityFlags.NewTask);
                this.StartActivity(_main);

                CleanVars();
                idproducto = 0;
                TraeProductos();
            }
            catch (Exception ex)
            {

            }
        }
    }
}