using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AlmacenApp.Adapter;
using AlmacenApp.Clases;
using AlmacenApp.Fragments;
using AlmacenApp.Models;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Android.Support.Design.Widget;
using Refractored.Fab;

namespace AlmacenApp.Activities
{
    [Activity(Label = "Producto Linea", MainLauncher = false, WindowSoftInputMode = SoftInput.AdjustPan, ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape, Theme = "@style/NavigationStyle")]
    public class LineaActivity : AppCompatActivity, IDialogInterfaceOnDismissListener
    {
        private AppPreferences _ap;
        private readonly Context _mContext = Application.Context;
        public static string serverurl = "";
        List<ProductoLineaErp> listlineas;
        ProgressDialog progress;
        ProductoLineaActivityAdapter productoLineaAdapter;
        ListView listLineasview;
        int eliminable = 0;

        DrawerLayout drawerLayout;
        NavigationView navigationView;
        TextView navheader_username;
        V7Toolbar toolbar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.linea_layout);
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
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected; ;

            serverurl = _ap.getServerurlKey();
            listlineas = new List<ProductoLineaErp>();
            listLineasview = FindViewById<ListView>(Resource.Id.listLineasview);
            TraeLineas();
            eliminable = 0;

            var fab = FindViewById<Refractored.Fab.FloatingActionButton>(Resource.Id.fab);
            fab.AttachToListView(listLineasview);
            fab.Click += (sender, args) =>
            {
                NuevoRegistro();
            };
            fab.Show();
        }

        private void NuevoRegistro()
        {
            _ap.saveIdProductoLineaKey("0");
            _ap.saveProductoLineaKey("");
            var transaction = this.FragmentManager.BeginTransaction();
            LineaNewFragment fragnew = new LineaNewFragment();
            fragnew.Show(transaction, "fragnew");
        }

        private void ShowDataUser()
        {
            RunOnUiThread(() => navheader_username.Text = _ap.getNombresKey());
        }

        private void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            //FragmentTransaction ft = this.FragmentManager.BeginTransaction();
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
            }
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            navigationView.InflateMenu(Resource.Menu.nav_menu_almacen); //Navigation Drawer Layout Menu Creation 
            return true;
        }
        void setupDrawerContent(NavigationView navigationView)
        {
            navigationView.NavigationItemSelected += (sender, e) =>
            {
                e.MenuItem.SetChecked(true);
                drawerLayout.CloseDrawers();
            };
        }

        internal void EliminaLinea(object sender, string idproductolinea, string lineaitem, string canttipos)
        {
            _ap.saveIdProductoLineaKey(idproductolinea);
            _ap.saveProductoLineaKey(lineaitem);
            eliminable = Convert.ToInt32(canttipos);
            if (eliminable > 0)
            {
                this.RunOnUiThread(() =>
                {
                    Toast.MakeText(_mContext, "El item no se puede eliminar porque posee Tipos. Debe eliminar los tipos primero!", ToastLength.Long).Show();
                });
            }
            else
            {

                var transaction = this.FragmentManager.BeginTransaction();
                LineaDeleteFragment fragdelete = new LineaDeleteFragment();
                fragdelete.Show(transaction, "fragdelete");
            }
        }
        private void TraeLineas()
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
                var lentidad = await CargaProductoLineas();
                this.RunOnUiThread(() => listLineasview.SetAdapter(null));
                if (lentidad.Any())
                {
                    listlineas = lentidad.Where(x=>x.estado.Equals("1")).ToList();
                    try
                    {
                        this.RunOnUiThread(() =>
                        {
                            productoLineaAdapter = new ProductoLineaActivityAdapter(_mContext, listlineas, this);
                            listLineasview.Adapter = productoLineaAdapter;
                            listLineasview.ItemClick += ListLineasview_ItemClick; ;
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

        private void ListLineasview_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            string idproductolinea = listlineas[e.Position].id_producto_linea.ToString();
            string codigo = listlineas[e.Position].codigo.ToString();
            string lineaitem = listlineas[e.Position].descripcion.ToString();
            string canttipos = listlineas[e.Position].cant_tipos.ToString();

            _ap.saveIdProductoLineaKey(idproductolinea);
            _ap.saveProductoLineaKey(lineaitem);
            eliminable = Convert.ToInt32(canttipos);
            var transaction = this.FragmentManager.BeginTransaction();
            LineaNewFragment fragnew = new LineaNewFragment();
            fragnew.Show(transaction, "fragnew");
        }

        public static async Task<List<ProductoLineaErp>> CargaProductoLineas()
        {
            try
            {
                HttpClient client = new HttpClient();
                var _connectionInfo = serverurl;
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

        void IDialogInterfaceOnDismissListener.OnDismiss(IDialogInterface dialog)
        {
            try
            {
                string varidproductolinea = _ap.getIdProductoLineaKey();
                if (varidproductolinea == "")
                {
                    TraeLineas();
                }
                else
                {
                    
                }
            }
            catch (Exception)
            {

            }
        }
    }
}