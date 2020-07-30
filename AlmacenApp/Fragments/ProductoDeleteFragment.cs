using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlmacenApp.Clases;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace AlmacenApp.Fragments
{
    public class ProductoDeleteFragment : DialogFragment
    {
        public class ResultadoInsert
        {
            public int rpta { get; set; }
        }
        private string idusuario = "";
        private int idproducto = 0;
        private string descripcion = "";
        View view = null;
        private string _connectionInfo = "";
        private AppPreferences _ap;
        private readonly Context _mContext = Application.Context;
        Context thiscontext;
        TextView txtDetalleProductoFrg;
        Button btnDeleteProductoFrg;
        Button btnCancelarProductoFrgD;
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
            view = inflater.Inflate(Resource.Layout.producto_delete_fragment, container, true);
            thiscontext = inflater.Context;
            _ap = new AppPreferences(_mContext);
            _connectionInfo = _ap.getServerurlKey();
            idusuario = _ap.getIdKey();
            idproducto = Convert.ToInt32(_ap.getIdProductoKey());
            descripcion = _ap.getProductoKey();
            txtDetalleProductoFrg = view.FindViewById<TextView>(Resource.Id.txtDetalleProductoFrg);
            btnCancelarProductoFrgD = view.FindViewById<Button>(Resource.Id.btnCancelarProductoFrgD);
            btnCancelarProductoFrgD.Click += BtnCancelarProductoFrgD_Click;
            btnDeleteProductoFrg = view.FindViewById<Button>(Resource.Id.btnDeleteProductoFrg);
            btnDeleteProductoFrg.Click += BtnDeleteProductoFrg_Click;
            if (descripcion != "")
            {
                this.Activity.RunOnUiThread(() => txtDetalleProductoFrg.Text = descripcion);
            }
            return view;
        }

        private async void BtnDeleteProductoFrg_Click(object sender, EventArgs e)
        {
            if (idproducto > 0)
            {
                int respuesta = await Data.DeleteProducto(idproducto);
                if (respuesta > 0)
                {
                    Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(thiscontext);
                    this.Activity.RunOnUiThread(() => alert.SetTitle("Correcto"));
                    this.Activity.RunOnUiThread(() => alert.SetMessage("Item se eliminó correctamente!"));
                    this.Activity.RunOnUiThread(() => alert.SetNeutralButton("Ok", delegate
                    {
                        _ap.saveIdProductoTipoKey("");
                        var mifragment = (ProductoDeleteFragment)FragmentManager.FindFragmentByTag("fragdeletep");
                        mifragment?.Dismiss();
                        var mifragment2 = (ProductoDeleteFragment)FragmentManager.FindFragmentByTag("fragdeletep");
                        mifragment2?.Dismiss();
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

        private void BtnCancelarProductoFrgD_Click(object sender, EventArgs e)
        {
            Dismiss();
        }
    }
}