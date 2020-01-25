using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using AlmacenApp.Clases;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using AlmacenApp.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using AlmacenApp.Adapter;

namespace AlmacenApp.Fragments
{
    [Activity(Label = "Lineas", MainLauncher = false, WindowSoftInputMode = SoftInput.AdjustPan, ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape, Theme = "@style/NavigationStyle")]
    public class LineaFragment : Android.Support.V4.App.Fragment
    {
        private AppPreferences _ap;
        private readonly Context _mContext = Application.Context;
        public static string serverurl = "";
        DrawerLayout drawerLayout;
        NavigationView navigationView;
        TextView navheader_username;
        //SfDataGrid sfgrid;
        //SfDataPager sfpager;
        List<ProductoLineaErp> listlineas;
        ProgressDialog progress;
        View view;
        ProductoLineaAdapter productoLineaAdapter;
        ListView listLineasview;
        int eliminable = 0;
        public override void OnCreate(Bundle savedInstanceState)
        {
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTg3MjA2QDMxMzcyZTM0MmUzMEt3V0NtY0VhYlpnUXJkOXJ1dTVySFg0T1FoMlgvTXZwQ1E5RTNzbGdic0U9");
            base.OnCreate(savedInstanceState);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTg3MjA2QDMxMzcyZTM0MmUzMEt3V0NtY0VhYlpnUXJkOXJ1dTVySFg0T1FoMlgvTXZwQ1E5RTNzbGdic0U9");
            view = LayoutInflater.From(Activity).Inflate(Resource.Layout.linea_layout, null);
            _ap = new AppPreferences(_mContext);

            serverurl = _ap.getServerurlKey();
            listlineas = new List<ProductoLineaErp>();
            listLineasview = view.FindViewById<ListView>(Resource.Id.listLineasview);

            //sfgrid = view.FindViewById<SfDataGrid>(Resource.Id.sfDataGrid1);
            //sfpager = view.FindViewById<SfDataPager>(Resource.Id.sfDataPager1);
            TraeLineas();
            eliminable = 0;
            return view;
        }

        internal void EliminaLinea(object sender, string idproductolinea, string lineaitem, string canttipos)
        {
            _ap.saveIdProductoLineaKey(idproductolinea);
            _ap.saveProductoLineaKey(lineaitem);
            eliminable = Convert.ToInt32(canttipos);
            if (eliminable > 0)
            {
                Activity.RunOnUiThread(() =>
                {
                    Toast.MakeText(this.Activity, "El item no se puede eliminar porque posee Tipos. Debe eliminar los tipos primero!", ToastLength.Long).Show();
                });
            }
            else
            {
                
                var transaction = Activity.FragmentManager.BeginTransaction();
                LineaDeleteFragment fragdelete = new LineaDeleteFragment();
                fragdelete.Show(transaction, "fragdelete");
            }
        }

        public void TraeLineas()
        {
            progress = new ProgressDialog(Activity);
            progress.Indeterminate = true;
            progress.SetProgressStyle(ProgressDialogStyle.Spinner);
            progress.SetMessage("Cargando data...");
            progress.SetCancelable(false);
            Activity.RunOnUiThread(() =>
            {
                progress.Show();
            });
            Task.Run(async () =>
            {
                var lentidad = await CargaProductoLineas();
                Activity.RunOnUiThread(() => listLineasview.SetAdapter(null));
                if (lentidad.Any())
                {
                    listlineas = lentidad;
                    try
                    {
                        Activity.RunOnUiThread(() =>
                        {
                            productoLineaAdapter = new ProductoLineaAdapter(this.Activity, listlineas, this);
                            listLineasview.Adapter = productoLineaAdapter;
                            listLineasview.ItemClick += ListLineasview_ItemClick;
                            eliminable = 0;
                        });
                    }
                    catch (Exception ex)
                    {
                        Activity.RunOnUiThread(() =>
                        {
                            Toast.MakeText(this.Activity, "Error: " + ex.Message, ToastLength.Long).Show();
                        });
                    }
                }
            }).ContinueWith(data => Activity.RunOnUiThread(() => progress.Hide()));
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
        }

        private async Task<List<ProductoLineaErp>> CargaProductoLineas()
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


    }
}