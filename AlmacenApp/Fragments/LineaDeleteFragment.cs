using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AlmacenApp.Clases;
using AlmacenApp.Models;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AlmacenApp.Fragments
{
    public class LineaDeleteFragment : DialogFragment
    {
        public class ResultadoInsert
        {
            public int rpta { get; set; }
        }
        private string idusuario = "";
        private int idproductolinea = 0;
        private string descripcion = "";
        View view = null;
        private string _connectionInfo = "";
        private AppPreferences _ap;
        private readonly Context _mContext = Application.Context;
        Context thiscontext;
        TextView txtDetallelineaFrg;
        Button btnDeleteLineaFrg;
        Button btnCancelarLineaFrg;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override void OnDismiss(IDialogInterface dialog)
        {
            base.OnDismiss(dialog);
            Activity activity = this.Activity;
            ((IDialogInterfaceOnDismissListener)activity).OnDismiss(dialog);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnCreateView(inflater, container, savedInstanceState);
            view = inflater.Inflate(Resource.Layout.lineas_delete_fragment, container, true);
            thiscontext = inflater.Context;
            _ap = new AppPreferences(_mContext);
            _connectionInfo = _ap.getServerurlKey();
            idusuario = _ap.getIdKey();
            idproductolinea = Convert.ToInt32(_ap.getIdProductoLineaKey());
            descripcion = _ap.getProductoLineaKey();
            txtDetallelineaFrg = view.FindViewById<TextView>(Resource.Id.txtDetallelineaFrg);
            btnCancelarLineaFrg = view.FindViewById<Button>(Resource.Id.btnCancelarLineaFrg);
            btnCancelarLineaFrg.Click += BtnCancelarLineaFrg_Click; 
            btnDeleteLineaFrg = view.FindViewById<Button>(Resource.Id.btnDeleteLineaFrg);
            btnDeleteLineaFrg.Click += BtnDeleteLineaFrg_Click; 
            if (descripcion != "")
            {
                this.Activity.RunOnUiThread(() => txtDetallelineaFrg.Text = descripcion);
            }
            return view;
        }

        private async void BtnDeleteLineaFrg_Click(object sender, EventArgs e)
        {
            if (idproductolinea > 0)
            {
                int respuesta = await DeleteLinea(idproductolinea);
                if (respuesta > 0)
                {
                    Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(thiscontext);
                    this.Activity.RunOnUiThread(() => alert.SetTitle("Correcto"));
                    this.Activity.RunOnUiThread(() => alert.SetMessage("Item se eliminó correctamente!"));
                    this.Activity.RunOnUiThread(() => alert.SetNeutralButton("Ok", delegate
                    {
                        _ap.saveIdProductoLineaKey("");
                        var mifragment = (LineaDeleteFragment)FragmentManager.FindFragmentByTag("fragdelete");
                        mifragment?.Dismiss();
                    }));
                    this.Activity.RunOnUiThread(() => alert.Show());
                }
                else
                {
                    this.Activity.RunOnUiThread(() => Toast.MakeText(thiscontext, "Linea no eliminado!", ToastLength.Short).Show());
                }
            }
            else
            {
                Dismiss();
            }
        }

        private async Task<int> DeleteLinea(int idproductolinea)
        {
            ProductoLineaErp newinsert;
            try
            {
                List<ResultadoInsert> lrespuestax = new List<ResultadoInsert>();
                newinsert = new ProductoLineaErp
                {
                    id_producto_linea = idproductolinea
                };
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(_connectionInfo);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string serializedObject = JsonConvert.SerializeObject(newinsert);
                HttpContent contentPost = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("Productoerp/t_producto_lineaDelete", contentPost);
                if (response.IsSuccessStatusCode)
                {
                    var resultao = await response.Content.ReadAsStringAsync();
                    //var resultalinea = JObject.Parse(await response.Content.ReadAsStringAsync());
                    ////JArray resultalinea = JArray.Parse(await response.Content.ReadAsStringAsync());
                    //JsonSerializerSettings settings = new JsonSerializerSettings();
                    //settings.NullValueHandling = NullValueHandling.Ignore;
                    //settings.MissingMemberHandling = MissingMemberHandling.Ignore;
                    if (resultao != null && resultao != "")
                    {
                        return Convert.ToInt32(resultao);
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        private void BtnCancelarLineaFrg_Click(object sender, EventArgs e)
        {
            Dismiss();
        }
    }
}