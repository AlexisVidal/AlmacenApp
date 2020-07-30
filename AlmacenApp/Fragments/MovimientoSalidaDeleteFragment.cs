using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlmacenApp.Clases;
using AlmacenApp.Resources.DataHelpers;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace AlmacenApp.Fragments
{
    public class MovimientoSalidaDeleteFragment : DialogFragment
    {
        public class ResultadoInsert
        {
            public int rpta { get; set; }
        }
        private string idusuario = "";
        private string nombretrabajador = "";
        private string abreviatura = "";
        private int idmovimiento = 0;
        private string producto = "";
        private decimal cantidad = 0;
        private string cant_medida = "";
        View view = null;
        private string _connectionInfo = "";
        private AppPreferences _ap;
        private readonly Context _mContext = Application.Context;
        Context thiscontext;
        TextView txtTrabajadorFrgPSI;
        TextView txtProductoFrgPSI;
        TextView txtCantidadFrgPSI;
        Button btnDeleteProductoSalidaFrgPSI;
        Button btnCancelarProductoSalidaFrgPSI;
        DataHelpers db;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        public override void OnDismiss(IDialogInterface dialog)
        {
            base.OnDismiss(dialog);
            Activity activity = this.Activity;
            ((IDialogInterfaceOnDismissListener)activity).OnDismiss(dialog);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            db = new DataHelpers();
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnCreateView(inflater, container, savedInstanceState);
            view = inflater.Inflate(Resource.Layout.movimiento_salida_delete_fragment, container, true);
            thiscontext = inflater.Context;
            _ap = new AppPreferences(_mContext);
            _connectionInfo = _ap.getServerurlKey();
            idusuario = _ap.getIdKey();
            idmovimiento = Convert.ToInt32(_ap.getIdMovimientoTempKey());
            nombretrabajador = _ap.getNombreTrabajadorTempKey();
            producto = _ap.getProductoKey();
            abreviatura = _ap.getProductoAbreviaturaKey();
            cantidad = Convert.ToDecimal(_ap.getInventarioCantTempKey());
            cant_medida = cantidad.ToString("N") + " " + abreviatura;

            txtTrabajadorFrgPSI = view.FindViewById<TextView>(Resource.Id.txtTrabajadorFrgPSI);
            txtProductoFrgPSI = view.FindViewById<TextView>(Resource.Id.txtProductoFrgPSI);
            txtCantidadFrgPSI = view.FindViewById<TextView>(Resource.Id.txtCantidadFrgPSI);
            btnCancelarProductoSalidaFrgPSI = view.FindViewById<Button>(Resource.Id.btnCancelarProductoSalidaFrgPSI);
            btnCancelarProductoSalidaFrgPSI.Click += BtnCancelarProductoSalidaFrgPSI_Click;
            btnDeleteProductoSalidaFrgPSI = view.FindViewById<Button>(Resource.Id.btnDeleteProductoSalidaFrgPSI);
            btnDeleteProductoSalidaFrgPSI.Click += BtnDeleteProductoSalidaFrgPSI_Click;
            if (idmovimiento != 0)
            {
                this.Activity.RunOnUiThread(() => txtTrabajadorFrgPSI.Text = nombretrabajador);
                this.Activity.RunOnUiThread(() => txtProductoFrgPSI.Text = producto);
                this.Activity.RunOnUiThread(() => txtCantidadFrgPSI.Text = cant_medida);
            }
            return view;
        }

        private void BtnDeleteProductoSalidaFrgPSI_Click(object sender, EventArgs e)
        {
            if (idmovimiento > 0)
            {
                bool result = db.deleteQueryTableMovimientoById(idmovimiento);
                if (result)
                {
                    Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(thiscontext);
                    this.Activity.RunOnUiThread(() => alert.SetTitle("Correcto"));
                    this.Activity.RunOnUiThread(() => alert.SetMessage("Item se eliminó correctamente!"));
                    this.Activity.RunOnUiThread(() => alert.SetNeutralButton("Ok", delegate
                    {
                        _ap.saveIdProductoTipoKey("");
                        var mifragment = (MovimientoSalidaDeleteFragment)FragmentManager.FindFragmentByTag("fragmovdelete");
                        mifragment?.Dismiss();
                        var mifragment2 = (MovimientoSalidaDeleteFragment)FragmentManager.FindFragmentByTag("fragmovdelete");
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

        private void BtnCancelarProductoSalidaFrgPSI_Click(object sender, EventArgs e)
        {
            Dismiss();
        }
    }
}