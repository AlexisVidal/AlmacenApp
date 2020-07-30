using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class InventarioProductoSelectFragment : DialogFragment
    {
        DataHelpers db;
        View view = null;
        //int idusuario = 0;
        private string idusuario = "";
        private int idinventario = 0;
        private int idproductoitem = 0;
        private string nomproductoinvent = "";
        private int cantexiste = 0;
        private int cantxatenderactual = 0;
        string _connectionInfo = "";

        private AppPreferences _ap;
        private readonly Context _mContext = Application.Context;
        Context thiscontext;

        TextView txtProductMI;
        //TextView txtCantI;
        EditText edtCantxI;
        Button btnSaveCantMI;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            db = new DataHelpers();

            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnCreateView(inflater, container, savedInstanceState);
            view = inflater.Inflate(Resource.Layout.inventario_producto_fragment, container, true);
            thiscontext = inflater.Context;
            _ap = new AppPreferences(_mContext);
            //para modificar item
            try
            {
                idinventario = Convert.ToInt32(_ap.getIdInventarioTempKey());
                //idproductoitem = Convert.ToInt32(_ap.getIdDetalleSolicitudProductoTempKey());
                nomproductoinvent = _ap.getInventarioNombreProdTempKey();
                cantexiste = Convert.ToInt32(_ap.getInventarioCantTempKey());

            }
            catch (Exception ex)
            {
                Console.WriteLine("No se puede convertir iddetallepedido. motivo: " + ex.Message);
                idinventario = 0;
                //idproductoitem = 0;
                nomproductoinvent = "";
                cantexiste = 0;
            }
            //para modificar item
            idusuario = _ap.getIdKey();
            txtProductMI = view.FindViewById<TextView>(Resource.Id.txtProductMI);
            //txtCantI = view.FindViewById<TextView>(Resource.Id.txtCantI);
            edtCantxI = view.FindViewById<EditText>(Resource.Id.edtCantxI);
            btnSaveCantMI = view.FindViewById<Button>(Resource.Id.btnSaveCantMI);
            btnSaveCantMI.Click += BtnSaveCantMI_Click;

            this.Activity.RunOnUiThread(() => txtProductMI.Text = nomproductoinvent);
            //this.Activity.RunOnUiThread(() => txtCantI.Text = "CANTIDAD REGISTRAR: " + cantexiste.ToString());
            //this.Activity.RunOnUiThread(() => edtCantxI.Text = cantxatender.ToString());
            return view;
        }

        private async void BtnSaveCantMI_Click(object sender, EventArgs e)
        {
            string cantpidiendo = edtCantxI.Text;
            if (cantpidiendo == "")
            {
                this.Activity.RunOnUiThread(() => Toast.MakeText(_mContext, "El campo Cantidad esta vacio!", ToastLength.Short).Show());
                return;
            }
            else
            {
                try
                {
                    cantxatenderactual = Convert.ToInt32(cantpidiendo);
                }
                catch (Exception)
                {
                    this.Activity.RunOnUiThread(() => Toast.MakeText(_mContext, "El valor ingresado no es correcto!", ToastLength.Short).Show());
                    this.Activity.RunOnUiThread(() => edtCantxI.Text = "0");
                    return;
                }
            }

            if (cantxatenderactual <= 0)
            {
                this.Activity.RunOnUiThread(() => Toast.MakeText(_mContext, "El valor ingresado no puede ser menor o igual a 0!", ToastLength.Short).Show());
                this.Activity.RunOnUiThread(() => edtCantxI.Text = "0");
                return;
            }
            else
            {

                int movimientoupdated = await UpdateMovimientoAsync(idinventario, Convert.ToInt32(cantexiste) + Convert.ToInt32(cantxatenderactual));
                if (movimientoupdated > 0)
                {
                    this.Activity.RunOnUiThread(() => Toast.MakeText(thiscontext, "Item se actualizó correctamente!", ToastLength.Long).Show());
                    var mifragment = (InventarioProductoSelectFragment)FragmentManager.FindFragmentByTag("selecinventfg");
                    mifragment?.Dismiss();
                    var mifragment2 = (InventarioProductoSelectFragment)FragmentManager.FindFragmentByTag("selecinventfg");
                    mifragment2?.Dismiss();
                }
                else
                {
                    this.Activity.RunOnUiThread(() => Toast.MakeText(thiscontext, "Item no actualizado!", ToastLength.Long).Show());
                }
            }
        }

        private async Task<int> UpdateMovimientoAsync(int iddetalleitem, int cantatendidaactual)
        {
            try
            {
                bool result = db.updateMovimientoLite(iddetalleitem, cantatendidaactual);
                if (result)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception er)
            {
                return 0;
                Console.WriteLine(er.Message);
            }
        }
        public override void OnDismiss(IDialogInterface dialog)
        {
            base.OnDismiss(dialog);
            Activity activity = this.Activity;
            ((IDialogInterfaceOnDismissListener)activity).OnDismiss(dialog);
        }
    }
}