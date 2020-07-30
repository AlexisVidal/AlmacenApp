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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Android.Support.Design.Widget;
using Refractored.Fab;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using AlmacenApp.Fragments;

namespace AlmacenApp.Activities
{
    [Activity(Label = "Producto Tipo", MainLauncher = false, WindowSoftInputMode = SoftInput.AdjustPan, ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape, Theme = "@style/NavigationStyle", NoHistory = true)]
    public class TipoActivity : AppCompatActivity, IDialogInterfaceOnDismissListener
    {
        private AppPreferences _ap;
        private readonly Context _mContext = Application.Context;
        public static string serverurl = "";
        List<ProductoTipoErp> listtipos;
        ProgressDialog progress;
        ProductoTipoActivityAdapter productoTipoAdapter;
        ListView listTiposview;
        int eliminable = 0;

        DrawerLayout drawerLayout;
        NavigationView navigationView;
        TextView navheader_username;
        V7Toolbar toolbar;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.tipo_layout);
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
            listtipos = new List<ProductoTipoErp>();
            listTiposview = FindViewById<ListView>(Resource.Id.listTiposview);
            TraeTipos();
            eliminable = 0;

            var fab = FindViewById<Refractored.Fab.FloatingActionButton>(Resource.Id.fabTipos);
            fab.AttachToListView(listTiposview);
            fab.Click += (sender, args) =>
            {
                NuevoRegistro();
            };
            fab.Show();
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            navigationView.InflateMenu(Resource.Menu.nav_menu_almacen); //Navigation Drawer Layout Menu Creation 
            return true;
        }
        private void NuevoRegistro()
        {
            _ap.saveIdProductoTipoKey("0");
            _ap.saveIdProductoLineaKey("0");
            _ap.saveProductoLineaKey("");
            _ap.saveProductoTipoKey("");
            var transaction = this.FragmentManager.BeginTransaction();
            TipoNewFragment fragnew = new TipoNewFragment();
            fragnew.Show(transaction, "fragnew");
        }

        private void TraeTipos()
        {
            progress = new ProgressDialog(this);
            progress.Indeterminate = true;
            progress.SetProgressStyle(ProgressDialogStyle.Spinner);
            progress.SetMessage("Cargando data...");
            progress.SetCancelable(false);
            this.RunOnUiThread(() =>
            {
                progress.Show();
            });
            Task.Run(async () =>
            {
                var lentidad = await Data.CargaProductoTipos();
                this.RunOnUiThread(() => listTiposview.SetAdapter(null));
                if (lentidad.Any())
                {
                    listtipos = lentidad.Where(x => x.estado_tipo.Equals("1")).ToList();
                    try
                    {
                        this.RunOnUiThread(() =>
                        {
                            productoTipoAdapter = new ProductoTipoActivityAdapter(_mContext, listtipos, this);
                            listTiposview.Adapter = productoTipoAdapter;
                            listTiposview.ItemClick += ListTiposview_ItemClick;
                            eliminable = 0;
                        });
                    }
                    catch (Exception ex)
                    {
                        this.RunOnUiThread(() =>
                        {
                            Toast.MakeText(_mContext, "Error: " + ex.Message, ToastLength.Long).Show();
                        });
                    }
                }
            }).ContinueWith(data => this.RunOnUiThread(() => progress.Hide()));
        }

        

        private void ListTiposview_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            string idproductotipo = listtipos[e.Position].id_producto_tipo.ToString();
            string codigo = listtipos[e.Position].codigo_tipo.ToString();
            string idlineaitem = listtipos[e.Position].fk_producto_linea.ToString();
            string lineaitem = listtipos[e.Position].linea.ToString();
            string tipoitem = listtipos[e.Position].producto_tipo.ToString();
            string cantproducts = listtipos[e.Position].cant_items.ToString();

            _ap.saveIdProductoTipoKey(idproductotipo);
            _ap.saveIdProductoLineaKey(idlineaitem);
            _ap.saveProductoLineaKey(lineaitem);
            _ap.saveProductoTipoKey(tipoitem);
            eliminable = Convert.ToInt32(cantproducts);
            var transaction = this.FragmentManager.BeginTransaction();
            TipoNewFragment fragnew = new TipoNewFragment();
            fragnew.Show(transaction, "fragnew");
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

        void setupDrawerContent(NavigationView navigationView)
        {
            navigationView.NavigationItemSelected += (sender, e) =>
            {
                e.MenuItem.SetChecked(true);
                drawerLayout.CloseDrawers();
            };
        }
        void IDialogInterfaceOnDismissListener.OnDismiss(IDialogInterface dialog)
        {
            try
            {
                string varidproductotipo = _ap.getIdProductoTipoKey();
                if (varidproductotipo == "")
                {
                    TraeTipos();
                }
                else
                {

                }
            }
            catch (Exception)
            {

            }
        }

        internal void EliminaTipo(object sender, string idproductotipo, string tipoitem, string canttipos)
        {
            _ap.saveIdProductoLineaKey(idproductotipo);
            _ap.saveProductoLineaKey(tipoitem);
            eliminable = Convert.ToInt32(canttipos);
            if (eliminable > 0)
            {
                this.RunOnUiThread(() =>
                {
                    Toast.MakeText(_mContext, "El item no se puede eliminar porque posee Productos activos. Debe eliminar los tipos primero!", ToastLength.Long).Show();
                });
            }
            else
            {
                var transaction = this.FragmentManager.BeginTransaction();
                TipoDeleteFragment fragdelete = new TipoDeleteFragment();
                fragdelete.Show(transaction, "fragdelete");
            }
        }
    }
}