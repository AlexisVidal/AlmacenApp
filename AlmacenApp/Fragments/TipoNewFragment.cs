using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlmacenApp.Activities;
using AlmacenApp.Clases;
using AlmacenApp.Models;
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
    public class TipoNewFragment : DialogFragment
    {
        public class ResultadoInsert
        {
            public int rpta { get; set; }
        }

        private string idusuario = "";
        private int idproductotipo = 0;
        private int idproductolinea = 0;
        private string descripcion = "";
        View view = null;
        private string _connectionInfo = "";
        private AppPreferences _ap;
        private readonly Context _mContext = Application.Context;
        Context thiscontext;
        Button btnGuardarTipoFrg;
        Button btnCancelarNewTipoFrg;
        TextInputLayout txt_tipo;
        EditText edt_tipo;
        Spinner spinner_linea;
        ProgressDialog progress;
        List<ProductoLineaErp> listlineas;
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
            view = inflater.Inflate(Resource.Layout.tipo_new_fragment, container, true);
            thiscontext = inflater.Context;
            _ap = new AppPreferences(_mContext);
            listlineas = new List<ProductoLineaErp>();
            _connectionInfo = _ap.getServerurlKey();
            idusuario = _ap.getIdKey();
            idproductotipo = Convert.ToInt32(_ap.getIdProductoTipoKey());
            idproductolinea = Convert.ToInt32(_ap.getIdProductoLineaKey());
            descripcion = _ap.getProductoTipoKey();
            spinner_linea = view.FindViewById<Spinner>(Resource.Id.spinner_linea);
            //spinner_linea.ItemSelected += Spinner_linea_ItemSelected;
            edt_tipo = view.FindViewById<EditText>(Resource.Id.edt_tipo);
            txt_tipo = view.FindViewById<TextInputLayout>(Resource.Id.txt_tipo);
            btnCancelarNewTipoFrg = view.FindViewById<Button>(Resource.Id.btnCancelarNewTipoFrg);

            btnGuardarTipoFrg = view.FindViewById<Button>(Resource.Id.btnGuardarTipoFrg);
            btnGuardarTipoFrg.Click += BtnGuardarTipoFrg_Click;
            btnCancelarNewTipoFrg.Click += BtnCancelarNewTipoFrg_Click;
            
            if (descripcion != "")
            {
                this.Activity.RunOnUiThread(() => edt_tipo.Text = descripcion);
            }
            LlenaLineas();
            return view;
        }

        private void LlenaLineas()
        {
            
            var w = Task.Run(async () =>
            {
                var lentidad = await Data.CargaProductoLineas();
                this.Activity.RunOnUiThread(() => spinner_linea.Adapter = null);
                if (lentidad.Any())
                {
                    listlineas = lentidad.Where(x => x.estado.Equals("1")).ToList();
                    try
                    {
                        ArrayAdapter adapter = new ArrayAdapter(_mContext, Resource.Layout.spinner_item_medium, listlineas.OrderBy(x => x.descripcion).ToList());
                        this.Activity.RunOnUiThread(() =>
                        {
                            adapter.SetDropDownViewResource(Resource.Layout.spinner_dropdown_item);
                            spinner_linea.ItemSelected += Spinner_linea_ItemSelected;
                            spinner_linea.Adapter = adapter;
                            if (idproductolinea > 0)
                            {
                                var itemsel = listlineas.FirstOrDefault(x => x.id_producto_linea == idproductolinea);
                                int indexo = listlineas.IndexOf(itemsel);
                                spinner_linea.SetSelection(indexo);
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        this.Activity.RunOnUiThread(() =>
                        {
                            Toast.MakeText(_mContext, "Error: " + ex.Message, ToastLength.Long).Show();
                        });
                    }
                }
            });
            w.Wait();
        }

        private void Spinner_linea_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {
                idproductolinea = listlineas.ElementAt(spinner_linea.SelectedItemPosition).id_producto_linea;

            }
            catch (Exception exception)
            {
                idproductolinea = 0;
            }
            try
            {
                //this.Activity.RunOnUiThread(() => spinner_linea.SetAdapter(null));
            }
            catch (Exception re)
            {

            }
        }

        private void BtnCancelarNewTipoFrg_Click(object sender, EventArgs e)
        {
            Dismiss();
        }

        private async void BtnGuardarTipoFrg_Click(object sender, EventArgs e)
        {
            
            string descripcion = edt_tipo.Text;
            if (descripcion == "")
            {
                this.Activity.RunOnUiThread(() => Toast.MakeText(thiscontext, "La descripcion no puede estar vacia!", ToastLength.Short).Show());
                return;
            }
            else
            {
                int respuesta = 0;
                string respu = "Item se registró correctamente!";
                if (idproductotipo == 0 && idproductolinea > 0)
                {
                    respuesta = await Data.InserTipo(idproductolinea,descripcion.ToUpper(),"");
                }
                else
                {
                    respuesta = await Data.UpdateTipo(descripcion.ToUpper(), idproductotipo, idproductolinea);
                    respu = "Item se actualizó correctamente!";
                }
                if (respuesta > 0)
                {
                    Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(thiscontext);
                    this.Activity.RunOnUiThread(() => alert.SetTitle("Correcto"));
                    this.Activity.RunOnUiThread(() => alert.SetMessage(respu));
                    this.Activity.RunOnUiThread(() => alert.SetNeutralButton("Ok", delegate
                    {
                        _ap.saveIdProductoTipoKey("");
                        _ap.saveIdProductoLineaKey("");
                        _ap.saveProductoTipoKey("");
                        _ap.saveProductoLineaKey("");
                        var mifragment = (TipoNewFragment)FragmentManager.FindFragmentByTag("fragnew");
                        
                        mifragment?.Dismiss();
                    }));
                    this.Activity.RunOnUiThread(() => alert.Show());
                }
                else
                {
                    this.Activity.RunOnUiThread(() => Toast.MakeText(thiscontext, "Tipo no registrado!", ToastLength.Short).Show());
                }
            }

        }
    }
}