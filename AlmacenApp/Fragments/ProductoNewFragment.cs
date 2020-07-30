using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class ProductoNewFragment : DialogFragment
    {
        public class ResultadoInsert
        {
            public int rpta { get; set; }
        }

        private string idusuario = "";
        private int idproducto = 0;
        private int idproductotipo = 0;
        private int idproductounidad = 0;
        private int idproductomarca = 0;

        private string descripcion = "";
        private string codigo_sku = "";
        View view = null;
        private string _connectionInfo = "";
        private AppPreferences _ap;
        private readonly Context _mContext = Application.Context;
        Context thiscontext;
        Button btnGuardarProductoFrg;
        Button btnCancelarNewProductoFrg;
        TextInputLayout txt_Producto;
        EditText edt_Producto;
        TextInputLayout txt_SkuProducto;
        EditText edt_SkuProducto;
        Spinner spinner_tipo;
        Spinner spinner_marca;
        Spinner spinner_unidad;
        ProgressDialog progress;
        List<ProductoTipoErp> listtipos;
        List<ProductoMarcaErp> listmarcas;
        List<UnidadMedidaErp> listunidades;

        public override void OnDismiss(IDialogInterface dialog)
        {
            base.OnDismiss(dialog);
            Activity activity = this.Activity;
            ((IDialogInterfaceOnDismissListener)activity).OnDismiss(dialog);
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnCreateView(inflater, container, savedInstanceState);
            view = inflater.Inflate(Resource.Layout.producto_new_fragment, container, true);
            thiscontext = inflater.Context;
            _ap = new AppPreferences(_mContext);
            listtipos = new List<ProductoTipoErp>();
            listmarcas = new List<ProductoMarcaErp>();
            listunidades = new List<UnidadMedidaErp>();
            _connectionInfo = _ap.getServerurlKey();
            idusuario = _ap.getIdKey();
            idproducto = Convert.ToInt32(_ap.getIdProductoKey());
            idproductotipo = Convert.ToInt32(_ap.getIdProductoTipoKey());
            idproductomarca = Convert.ToInt32(_ap.getIdProductoMarcaKey());
            idproductounidad = Convert.ToInt32(_ap.getIdProductoUnidadKey());
            descripcion = _ap.getProductoKey();
            codigo_sku = _ap.getProductoSkuKey();
            spinner_tipo = view.FindViewById<Spinner>(Resource.Id.spinner_tipo);
            spinner_marca = view.FindViewById<Spinner>(Resource.Id.spinner_marca);
            spinner_unidad = view.FindViewById<Spinner>(Resource.Id.spinner_unidad);
            //spinner_linea.ItemSelected += Spinner_linea_ItemSelected;
            edt_Producto = view.FindViewById<EditText>(Resource.Id.edt_Producto);
            txt_Producto = view.FindViewById<TextInputLayout>(Resource.Id.txt_Producto);
            edt_SkuProducto = view.FindViewById<EditText>(Resource.Id.edt_SkuProducto);
            txt_SkuProducto = view.FindViewById<TextInputLayout>(Resource.Id.txt_SkuProducto);
            btnCancelarNewProductoFrg = view.FindViewById<Button>(Resource.Id.btnCancelarNewProductoFrg);

            btnGuardarProductoFrg = view.FindViewById<Button>(Resource.Id.btnGuardarProductoFrg);
            btnGuardarProductoFrg.Click += BtnGuardarProductoFrg_Click;
            btnCancelarNewProductoFrg.Click += BtnCancelarNewProductoFrg_Click;

            if (descripcion != "")
            {
                this.Activity.RunOnUiThread(() => edt_Producto.Text = descripcion);
            }
            if (codigo_sku != "")
            {
                this.Activity.RunOnUiThread(() => edt_SkuProducto.Text = codigo_sku);
            }
            LlenaTipos();
            LlenaMarcas();
            LlenaMedidas();

            return view;
        }

        private void LlenaMedidas()
        {
            var x = Task.Run(async () =>
            {
                var lentidad = await Data.CargaUnidadMedidaErp();
                this.Activity.RunOnUiThread(() => spinner_unidad.Adapter = null);
                if (lentidad.Any())
                {
                    listunidades = lentidad.ToList();
                    try
                    {
                        ArrayAdapter adapter = new ArrayAdapter(_mContext, Resource.Layout.spinner_item_medium, listunidades.ToList());
                        this.Activity.RunOnUiThread(() =>
                        {
                            adapter.SetDropDownViewResource(Resource.Layout.spinner_dropdown_item);
                            spinner_unidad.ItemSelected += Spinner_unidad_ItemSelected;
                            spinner_unidad.Adapter = adapter;
                            if (idproductounidad > 0)
                            {
                                var itemsel = listunidades.FirstOrDefault(x => x.id_unidad_medida == idproductounidad);
                                int indexo = listunidades.IndexOf(itemsel);
                                spinner_unidad.SetSelection(indexo);
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
            x.Wait();
        }

        private void Spinner_unidad_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {
                idproductounidad = listunidades.ElementAt(spinner_unidad.SelectedItemPosition).id_unidad_medida;

            }
            catch (Exception exception)
            {
                idproductounidad = 0;
            }
        }

        private void LlenaMarcas()
        {
            
            var w =Task.Run(async () =>
            {
                var lentidad = await Data.CargaProductoMarcas();
                this.Activity.RunOnUiThread(() => spinner_marca.Adapter = null);
                if (lentidad.Any())
                {
                    listmarcas = lentidad.Where(x => x.estado.Equals("1")).ToList();
                    try
                    {
                        ArrayAdapter adapter = new ArrayAdapter(_mContext, Resource.Layout.spinner_item_medium, listmarcas.ToList());
                        this.Activity.RunOnUiThread(() =>
                        {
                            adapter.SetDropDownViewResource(Resource.Layout.spinner_dropdown_item);
                            spinner_marca.ItemSelected += Spinner_marca_ItemSelected;
                            spinner_marca.Adapter = adapter;
                            if (idproductotipo > 0)
                            {
                                var itemsel = listmarcas.FirstOrDefault(x => x.id_producto_marca == idproductomarca);
                                int indexo = listmarcas.IndexOf(itemsel);
                                spinner_marca.SetSelection(indexo);
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

        private void Spinner_marca_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {
                idproductomarca = listmarcas.ElementAt(spinner_marca.SelectedItemPosition).id_producto_marca;

            }
            catch (Exception exception)
            {
                idproductomarca = 0;
            }
        }

        private void LlenaTipos()
        {

            var q = Task.Run(async () =>
            {
                var lentidad = await Data.CargaProductoTipos();
                this.Activity.RunOnUiThread(() => spinner_tipo.Adapter = null);
                if (lentidad.Any())
                {
                    listtipos = lentidad.Where(x => x.estado_tipo.Equals("1")).ToList();
                    try
                    {
                        ArrayAdapter adapter = new ArrayAdapter(_mContext, Resource.Layout.spinner_item_medium, listtipos.ToList());
                        this.Activity.RunOnUiThread(() =>
                        {
                            adapter.SetDropDownViewResource(Resource.Layout.spinner_dropdown_item);
                            spinner_tipo.ItemSelected += Spinner_tipo_ItemSelected;
                            spinner_tipo.Adapter = adapter;
                            if (idproductotipo > 0)
                            {
                                var itemsel = listtipos.FirstOrDefault(x => x.id_producto_tipo == idproductotipo);
                                int indexo = listtipos.IndexOf(itemsel);
                                spinner_tipo.SetSelection(indexo);
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
            q.Wait();
        }

        private void Spinner_tipo_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {
                idproductotipo = listtipos.ElementAt(spinner_tipo.SelectedItemPosition).id_producto_tipo;

            }
            catch (Exception exception)
            {
                idproductomarca = 0;
            }
        }

        private async void BtnGuardarProductoFrg_Click(object sender, EventArgs e)
        {
            
            string descripcion = edt_Producto.Text;
            string codigosku = edt_SkuProducto.Text;
            if (descripcion == "")
            {
                this.Activity.RunOnUiThread(() => Toast.MakeText(thiscontext, "La descripcion no puede estar vacia!", ToastLength.Short).Show());
                return;
            }
            else
            {
                int respuesta = 0;
                string respu = "Item se registró correctamente!";
                if (idproducto == 0 && idproductomarca > 0 && idproductotipo > 0 && idproductounidad > 0)
                {
                    respuesta = await Data.InserProducto(idproductounidad, idproductomarca,  idproductotipo, descripcion.ToUpper(), codigosku.ToUpper());
                }
                else
                {
                    respuesta = await Data.UpdateProducto(idproducto, idproductounidad, idproductomarca, idproductotipo, descripcion.ToUpper(), codigosku.ToUpper());
                    respu = "Item se actualizó correctamente!";
                }
                if (respuesta > 0)
                {
                    this.Activity.RunOnUiThread(() => Toast.MakeText(thiscontext, "Registro Exitoso!", ToastLength.Long).Show());
                    var mifragment = (ProductoNewFragment)FragmentManager.FindFragmentByTag("fragnewpro");
                    mifragment?.Dismiss();

                    var mifragment2 = (ProductoNewFragment)FragmentManager.FindFragmentByTag("fragnewpro");
                    mifragment2?.Dismiss();

                    //Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(thiscontext);
                    //this.Activity.RunOnUiThread(() => alert.SetTitle("Correcto"));
                    //this.Activity.RunOnUiThread(() => alert.SetMessage(respu));
                    //this.Activity.RunOnUiThread(() => alert.SetNeutralButton("Ok", delegate
                    //{
                    //    _ap.saveIdProductoTipoKey("");
                    //    _ap.saveIdProductoLineaKey("");
                    //    _ap.saveProductoTipoKey("");
                    //    _ap.saveProductoLineaKey("");
                    //    var mifragment = (ProductoNewFragment)FragmentManager.FindFragmentByTag("fragnewpro");
                    //    mifragment?.Dismiss();
                    //}));
                    //this.Activity.RunOnUiThread(() => alert.Show());
                }
                else
                {
                    this.Activity.RunOnUiThread(() => Toast.MakeText(thiscontext, "Tipo no registrado!", ToastLength.Short).Show());
                }
            }
        }

        private void BtnCancelarNewProductoFrg_Click(object sender, EventArgs e)
        {
            var mifragment = (ProductoNewFragment)FragmentManager.FindFragmentByTag("fragnewpro");
            mifragment?.Dismiss();

            var mifragment2 = (ProductoNewFragment)FragmentManager.FindFragmentByTag("fragnewpro");
            mifragment2?.Dismiss();
            Dismiss();
        }
    }
}