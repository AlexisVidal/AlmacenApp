using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Views;
using AlmacenApp.Clases;
using Android.Content;
using System;
using AlmacenApp.Fragments;
using AlmacenApp.Activities;

namespace AlmacenApp
{
    [Activity(Label = "@string/app_name", MainLauncher = false, WindowSoftInputMode = SoftInput.AdjustPan, ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape, Theme = "@style/NavigationStyle")]
    public class MainActivity : AppCompatActivity
    {
        private AppPreferences _ap;
        private readonly Context _mContext = Application.Context;
        //TextView txtUsuario;
        //TextView txtEmpresa;
        Button btnAlmacen;
        Button btnTaller;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            _ap = new AppPreferences(_mContext);
            //txtUsuario = FindViewById<TextView>(Resource.Id.txtUsuario);
            //txtEmpresa = FindViewById<TextView>(Resource.Id.txtEmpresa);
            btnAlmacen = FindViewById<Button>(Resource.Id.btnAlmacen);
            btnTaller = FindViewById<Button>(Resource.Id.btnTaller);
            //ShowDataUser();
            btnAlmacen.Click += BtnAlmacen_Click;
            btnTaller.Click += BtnTaller_Click;
        }

        private void BtnTaller_Click(object sender, EventArgs e)
        {
            
        }

        private void BtnAlmacen_Click(object sender, EventArgs e)
        {
            //var newact = new Intent(this, typeof(AlmacenActivity));
            var newact = new Intent(this, typeof(InventarioActivity));
            try
            {
                StartActivity(newact);
                Finish();
            }
            catch (Exception az)
            {
                RunOnUiThread(() => Toast.MakeText(this, "Exception: " + az.Message, ToastLength.Short).Show());
            }
        }

        //private void ShowDataUser()
        //{
        //    RunOnUiThread(() => txtUsuario.Text = _ap.getNombresKey());
        //    RunOnUiThread(() => txtEmpresa.Text = _ap.getEmpresaKey());
        //}

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        }
    }
}