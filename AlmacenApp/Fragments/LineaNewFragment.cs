using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlmacenApp.Clases;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace AlmacenApp.Fragments
{
    public class LineaNewFragment : DialogFragment
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
        Button btnGuardarLineaFrg;
        Button btnCancelarNewLineaFrg;
        TextInputLayout txt_linea;
        EditText edt_linea;
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
            view = inflater.Inflate(Resource.Layout.lineas_new_fragment, container, true);
            thiscontext = inflater.Context;
            _ap = new AppPreferences(_mContext);
            _connectionInfo = _ap.getServerurlKey();
            idusuario = _ap.getIdKey();
            idproductolinea = Convert.ToInt32(_ap.getIdProductoLineaKey());
            descripcion = _ap.getProductoLineaKey();
            edt_linea = view.FindViewById<EditText>(Resource.Id.edt_linea);
            txt_linea = view.FindViewById<TextInputLayout>(Resource.Id.txt_linea);
            btnCancelarNewLineaFrg = view.FindViewById<Button>(Resource.Id.btnCancelarNewLineaFrg);

            btnGuardarLineaFrg = view.FindViewById<Button>(Resource.Id.btnGuardarLineaFrg);
            btnGuardarLineaFrg.Click += BtnGuardarLineaFrg_Click;
            btnCancelarNewLineaFrg.Click += BtnCancelarNewLineaFrg_Click;
            if (descripcion != "")
            {
                this.Activity.RunOnUiThread(() => edt_linea.Text = descripcion);
            }
            return view;
        }

        private async void BtnGuardarLineaFrg_Click(object sender, EventArgs e)
        {
            string descripcion = edt_linea.Text;
            if (descripcion == "")
            {
                this.Activity.RunOnUiThread(() => Toast.MakeText(thiscontext, "La descripcion no puede estar vacia!", ToastLength.Short).Show());
                return;
            }
            else
            {
                int respuesta = 0;
                string respu = "Item se registró correctamente!";
                if (idproductolinea == 0)
                {
                    respuesta = await Data.InserLinea(descripcion.ToUpper());
                }
                else
                {
                    respuesta = await Data.UpdateLinea(descripcion.ToUpper(), idproductolinea);
                    respu = "Item se actualizó correctamente!";
                }
                if (respuesta > 0)
                {
                    Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(thiscontext);
                    this.Activity.RunOnUiThread(() => alert.SetTitle("Correcto"));
                    this.Activity.RunOnUiThread(() => alert.SetMessage(respu));
                    this.Activity.RunOnUiThread(() => alert.SetNeutralButton("Ok", delegate
                    {
                        _ap.saveIdProductoLineaKey("");
                        var mifragment = (LineaNewFragment)FragmentManager.FindFragmentByTag("fragnew");
                        mifragment?.Dismiss();
                    }));
                    this.Activity.RunOnUiThread(() => alert.Show());
                }
                else
                {
                    this.Activity.RunOnUiThread(() => Toast.MakeText(thiscontext, "Linea no registrada!", ToastLength.Short).Show());
                }
            }

        }

        private void BtnCancelarNewLineaFrg_Click(object sender, EventArgs e)
        {
            Dismiss();
        }
    }
}