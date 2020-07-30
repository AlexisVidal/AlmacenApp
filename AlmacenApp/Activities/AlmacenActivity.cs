using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Android.Support.V4.Widget;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using AlmacenApp.Clases;
using AlmacenApp.Activities;

namespace AlmacenApp.Fragments
{
    [Activity(Label = "Almacen", MainLauncher = false, WindowSoftInputMode = SoftInput.AdjustPan, ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape, Theme = "@style/NavigationStyle", NoHistory = true)]
    public class AlmacenActivity : AppCompatActivity
    {
        private AppPreferences _ap;
        private readonly Context _mContext = Application.Context;
        DrawerLayout drawerLayout;
        NavigationView navigationView;
        TextView navheader_username;
        V7Toolbar toolbar;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.almacen_layout);
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
        }

        private void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            //FragmentTransaction ft = this.FragmentManager.BeginTransaction();
            var menuItem = e.MenuItem;
            switch (menuItem.ItemId)
            {
                case Resource.Id.menu_almacen_lineas:
                    toolbar.Title = "Lineas";
                    //LineaFragment lineafragment = new LineaFragment();
                    //var fragmentx = SupportFragmentManager.BeginTransaction();
                    //fragmentx.Add(Resource.Id.frame, lineafragment);
                    //fragmentx.Commit();
                    var newact = new Intent(this, typeof(LineaActivity));
                    StartActivity(newact);
                    //return true;
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
        void setupDrawerContent(NavigationView navigationView)
        {
            navigationView.NavigationItemSelected += (sender, e) =>
            {
                e.MenuItem.SetChecked(true);
                drawerLayout.CloseDrawers();
            };
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            navigationView.InflateMenu(Resource.Menu.nav_menu_almacen); //Navigation Drawer Layout Menu Creation 
            return true;
        }


    }
}