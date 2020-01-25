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

namespace AlmacenApp.Fragments
{
    public class TipoDeleteFragment : DialogFragment
    {
        public class ResultadoInsert
        {
            public int rpta { get; set; }
        }
        private string idusuario = "";
        private int idproductotipo = 0;
        private string descripcion = "";
        View view = null;
        private string _connectionInfo = "";
        private AppPreferences _ap;
        private readonly Context _mContext = Application.Context;
        Context thiscontext;
        TextView txtDetalletipoFrg;
        Button btnDeleteTipoFrg;
        Button btnCancelarTipoFrg;

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
            view = inflater.Inflate(Resource.Layout.tipo_delete_fragment, container, true);
            thiscontext = inflater.Context;
            _ap = new AppPreferences(_mContext);
            _connectionInfo = _ap.getServerurlKey();
            idusuario = _ap.getIdKey();
            idproductotipo = Convert.ToInt32(_ap.getIdProductoTipoKey());
            descripcion = _ap.getProductoTipoKey();
            txtDetalletipoFrg = view.FindViewById<TextView>(Resource.Id.txtDetalletipoFrg);
            btnCancelarTipoFrg = view.FindViewById<Button>(Resource.Id.btnCancelarTipoFrg);
            btnCancelarTipoFrg.Click += BtnCancelarTipoFrg_Click;
            btnDeleteTipoFrg = view.FindViewById<Button>(Resource.Id.btnDeleteTipoFrg);
            btnDeleteTipoFrg.Click += BtnDeleteTipoFrg_Click;
            if (descripcion != "")
            {
                this.Activity.RunOnUiThread(() => txtDetalletipoFrg.Text = descripcion);
            }
            return view;
        }

        private void BtnCancelarTipoFrg_Click(object sender, EventArgs e)
        {
            Dismiss();
        }

        private async void BtnDeleteTipoFrg_Click(object sender, EventArgs e)
        {
            if (idproductotipo > 0)
            {
                int respuesta = await Data.DeleteTipo(idproductotipo);
                if (respuesta > 0)
                {
                    Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(thiscontext);
                    this.Activity.RunOnUiThread(() => alert.SetTitle("Correcto"));
                    this.Activity.RunOnUiThread(() => alert.SetMessage("Item se eliminó correctamente!"));
                    this.Activity.RunOnUiThread(() => alert.SetNeutralButton("Ok", delegate
                    {
                        _ap.saveIdProductoTipoKey("");
                        var mifragment = (TipoDeleteFragment)FragmentManager.FindFragmentByTag("fragdelete");
                        mifragment?.Dismiss();
                    }));
                    this.Activity.RunOnUiThread(() => alert.Show());
                }
                else
                {
                    this.Activity.RunOnUiThread(() => Toast.MakeText(thiscontext, "Tipo no eliminado!", ToastLength.Short).Show());
                }
            }
            else
            {
                Dismiss();
            }
        }

        

    }
}