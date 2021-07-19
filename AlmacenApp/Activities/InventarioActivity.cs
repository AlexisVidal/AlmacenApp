using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlmacenApp.Adapter;
using AlmacenApp.Clases;
using AlmacenApp.Fragments;
using AlmacenApp.Models;
using AlmacenApp.Resources.DataHelpers;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
namespace AlmacenApp.Activities
{
    [Activity(Label = "Inventario", MainLauncher = false, WindowSoftInputMode = SoftInput.AdjustPan, ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape, Theme = "@style/NavigationStyle")]
    public class InventarioActivity : AppCompatActivity, IDialogInterfaceOnDismissListener
    {
        private AppPreferences _ap;
        string idcodigogeneral = "";
        private readonly Context _mContext = Application.Context;
        public static string serverurl = "";
        List<ProductoErpLite> listproductos;
        ListView rvproductosIp;
        int eliminable = 0;
        int idproducto = 0;
        DrawerLayout drawerLayout;
        NavigationView navigationView;
        TextView navheader_username;
        V7Toolbar toolbar;
        private EditText edt_searchproductsList;
        int idalmacen = 0;
        int instanceupdateitem = 0;
        List<AlmacenErp> lalmacenes;
        List<AlmacenErp> lalmaceneslite;
        Spinner spAlmacenSimpleI;
        private View _view;
        DataHelpers db;

        ProgressDialog progress;
        ProgressDialog progress2;
        ProgressDialog progress3;
        ListView listProductInventarioSimpleI;
        ProductoMovimientoActivityAdapter productosinventarioListAdapter;
        List<MovimientoErpLite> lmovimientoslite = new List<MovimientoErpLite>();
        Button btnRegularizaI;
        int existenmovimientos = 0;
        Button btnResetI;
        List<ProductoErpLite> lproductoslite;
        List<AlmacenStockErp> lalmacenestock;
        private EditText _edtSearch;
        private ImageButton _btnSearch;
        private List<ProductoErpLite> lproductos;
        private ListView _rvproductos;
        Button fabSimpleI;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.inventario_layout);
            _ap = new AppPreferences(_mContext);
            db = new DataHelpers();
            db.CreateDatabaseInventarioInicial();

            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            toolbar = FindViewById<V7Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            var drawerToggle = new ActionBarDrawerToggle(this, drawerLayout, toolbar, Resource.String.drawer_open, Resource.String.drawer_close);
            drawerLayout.SetDrawerListener(drawerToggle);
            drawerToggle.SyncState();
            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            var headerview = navigationView.GetHeaderView(0);
            navheader_username = headerview.FindViewById<TextView>(Resource.Id.navheader_username);
            setupDrawerContent(navigationView); //Calling Function 
            ShowDataUser();
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;

            serverurl = _ap.getServerurlKey();
            idcodigogeneral = _ap.getIdCodigoGeneralLoginTempKey();
            listproductos = new List<ProductoErpLite>();
            spAlmacenSimpleI = FindViewById<Spinner>(Resource.Id.spAlmacenSimpleIp);
            rvproductosIp = FindViewById<ListView>(Resource.Id.rvproductosIp);

            edt_searchproductsList = FindViewById<EditText>(Resource.Id.edt_searchproductsList);
            edt_searchproductsList.EditorAction += (sender, args) =>
            {
                if (args.ActionId == ImeAction.Search)
                {
                    string x = edt_searchproductsList.Text;
                    _btnSearch_Click(sender, args);
                    args.Handled = true;
                }
            };
            try
            {
                var xlalmaceneslite = await Data.CargaAlmacenes();
                if (xlalmaceneslite != null && xlalmaceneslite.Count > 0)
                {
                    AlmacenErp newalmacen = new AlmacenErp()
                    {
                        id_almacen = -1,
                        nombre = ""
                    };
                    xlalmaceneslite.Add(newalmacen);
                    lalmaceneslite = xlalmaceneslite.OrderBy(x => x.nombre).ToList();
                    LlenaAlmacen(lalmaceneslite);
                }
            }
            catch (Exception ex)
            {

            }
            fabSimpleI = FindViewById<Button>(Resource.Id.fabSimpleIp);
            listProductInventarioSimpleI = FindViewById<ListView>(Resource.Id.listProductInventarioSimpleIp);
            fabSimpleI.Click += FabSimpleI_Click;
            btnResetI = FindViewById<Button>(Resource.Id.btnResetIp);
            btnResetI.Click += BtnResetI_Click;
            btnRegularizaI = FindViewById<Button>(Resource.Id.btnRegularizaIp);
            try
            {
                var existemoves = db.selectTableMovimiento();
                if (existemoves != null && existemoves.Count > 0 && idalmacen > 0 && existemoves.Where(x => x.fk_almacen == idalmacen && x.fk_movimiento_tipo == 7).ToList().Count > 0)
                {
                    var concantidad = existemoves.Where(y => y.cantidad > 0).ToList();
                    int nconcantidad = concantidad.Count;
                    if (nconcantidad > 0)
                    {
                        existenmovimientos = 1;
                        RunOnUiThread(() => Toast.MakeText(this, "EXISTEN MOVIMIENTOS POR REGULARIZAR", ToastLength.Long).Show());
                        RunOnUiThread(() => btnRegularizaI.Enabled = true);
                    }
                    else
                    {
                        existenmovimientos = 0;
                        RunOnUiThread(() => btnRegularizaI.Enabled = false);
                    }

                }
                else
                {
                    existenmovimientos = 0;
                    RunOnUiThread(() => btnRegularizaI.Enabled = false);
                }
            }
            catch (Exception)
            {
                existenmovimientos = 0;
                RunOnUiThread(() => btnRegularizaI.Enabled = false);
            }
            btnRegularizaI.Click += BtnRegularizaI_Click;
        }

        private async void BtnRegularizaI_Click(object sender, EventArgs e)
        {
            int insercionesbd = 0;
            int newidmovimiento = 0;
            int newidalmacenmovimiento = 0;
            int newidmovimientorec = 0;

            try
            {
                progress3 = new ProgressDialog(this);
                progress3.Indeterminate = true;
                progress3.SetProgressStyle(ProgressDialogStyle.Spinner);
                progress3.SetMessage("Registrando modo online...");
                progress3.SetCancelable(false);
                RunOnUiThread(() =>
                {
                    progress3.Show();
                });
                await Task.Run(async () =>
                {
                    List<MovimientoErpLite> lMovimientoLite = new List<MovimientoErpLite>();
                    var xlMovimientoLite = db.selectTableMovimiento();
                    lMovimientoLite = xlMovimientoLite.Where(y => y.fk_almacen == idalmacen && y.cantidad > 0 && y.fk_movimiento_tipo == 7).ToList();

                    if (lMovimientoLite != null && lMovimientoLite.Count > 0)
                    {
                        #region t_almacen_movimiento
                        string codmovimiento = "";
                        AlmacenMovimientoErp almamov = new AlmacenMovimientoErp()
                        {
                            fk_almacen = lMovimientoLite[0].fk_almacen,
                            fk_movimiento_tipo = lMovimientoLite[0].fk_movimiento_tipo,
                            codigo_movimiento_tipo = "I",
                            IDCODIGOGENERAL = idcodigogeneral,
                            cliente = "",
                            direccion = "",
                            oc_os = "",
                            maquina_unidad = "",
                            observaciones = "",
                            IDRESPONSABLE = ""
                        };
                        newidalmacenmovimiento = await Data.InsertaAlmacenMovimientoDB(almamov);
                        #endregion
                        foreach (var movitem in lMovimientoLite)
                        {
                            MovimientoErp movimiento = new MovimientoErp
                            {
                                fk_movimiento_tipo = movitem.fk_movimiento_tipo,
                                fk_guia_remision_detalle = 0,
                                fk_venta_detalle = 0,
                                fk_comprobante_traslado_detalle = 0,
                                fk_nota_credito_detalle = 0,
                                fk_almacen = movitem.fk_almacen,
                                fk_producto = movitem.fk_producto,
                                cantidad = movitem.cantidad,
                                IDCODIGOGENERAL = movitem.IDCODIGOGENERAL,
                                fk_salida_almacen = movitem.fk_salida_almacen,
                                fk_almacen_movimiento = newidalmacenmovimiento
                            };
                            newidmovimiento = await Data.InsertaMovimientoDB(movimiento);
                            insercionesbd++;
                            if (newidmovimiento > 0)
                            {

                            }
                        }

                    }
                }).ContinueWith(data => RunOnUiThread(() => progress3.Hide())).ConfigureAwait(false);
            }
            catch (Exception es)
            {
                RunOnUiThread(() => Toast.MakeText(this, "HUBO UN ERROR EN EL REGISTRO! EXCEPTION: " + es.Message, ToastLength.Short).Show());
            }
            if (insercionesbd > 0)
            {
                RunOnUiThread(() => Toast.MakeText(this, "SE REGISTRÓ CORRECTAMENTE! \nSE REGISTRARON: " + insercionesbd.ToString().PadLeft(2, '0') + " PRODUCTOS", ToastLength.Long).Show());
                try
                {
                    var q = Task.Run(async () =>
                    {
                        db.deleteQueryTableMovimientoByAlmacen(idalmacen);
                    });
                    q.Wait();
                    Intent _main = new Intent(this, typeof(InventarioActivity));
                    _main.SetFlags(ActivityFlags.NewTask);
                    this.StartActivity(_main);

                }
                catch (Exception exi)
                {
                    RunOnUiThread(() => Toast.MakeText(this, "HUBO UN ERROR EN EL REGISTRO! EXCEPTION: " + exi.Message, ToastLength.Long).Show());
                    Intent _main = new Intent(this, typeof(InventarioActivity));
                    _main.SetFlags(ActivityFlags.NewTask);
                    this.StartActivity(_main);
                }
            }
            else
            {
                RunOnUiThread(() => Toast.MakeText(this, "HUBO UN ERROR EN EL REGISTRO!", ToastLength.Short).Show());
            }
        }

        private void BtnResetI_Click(object sender, EventArgs e)
        {
            try
            {
                db.CreateDatabaseMovimiento();
                RunOnUiThread(() => Toast.MakeText(this, "LA DATA SE LIMPIO CORRECTAMENTE!", ToastLength.Long).Show());
                Intent _main = new Intent(this, typeof(InventarioActivity));
                _main.SetFlags(ActivityFlags.NewTask);
                this.StartActivity(_main);
            }
            catch (Exception ex)
            {

            }
        }

        private async void FabSimpleI_Click(object sender, EventArgs e)
        {
            if (idalmacen > 0 && existenmovimientos == 0)
            {
                await LlenaProductosLocal();
            }
            else if (idalmacen > 0 && existenmovimientos > 0)
            {
                await LlenaProductosLocalAUpdate();
            }
        }

        private async Task LlenaProductosLocalAUpdate()
        {
            try
            {
                RunOnUiThread(() => listProductInventarioSimpleI.SetAdapter(null));
            }
            catch (Exception re)
            {

            }

            try
            {
                db = new DataHelpers();
                lmovimientoslite = new List<MovimientoErpLite>();
                var vlmovimientoslite = db.selectTableMovimiento();
                if (vlmovimientoslite != null && vlmovimientoslite.Count > 0)
                {
                    lmovimientoslite = vlmovimientoslite.Where(q => q.fk_almacen == idalmacen).ToList();
                    RunOnUiThread(() => productosinventarioListAdapter = new ProductoMovimientoActivityAdapter(_mContext, lmovimientoslite, this));
                    RunOnUiThread(() => listProductInventarioSimpleI.Adapter = productosinventarioListAdapter);
                    RunOnUiThread(() => listProductInventarioSimpleI.ItemClick += ListProductInventarioSimpleI_ItemClick);


                    var concantidad = lmovimientoslite.Where(y => y.cantidad > 0).ToList();
                    int nconcantidad = concantidad.Count;
                    if (nconcantidad > 0)
                    {
                        existenmovimientos = 1;
                        RunOnUiThread(() => Toast.MakeText(this, "EXISTEN MOVIMIENTOS POR REGULARIZAR", ToastLength.Long).Show());
                        RunOnUiThread(() => btnRegularizaI.Enabled = true);
                    }
                    else
                    {
                        existenmovimientos = 0;
                        RunOnUiThread(() => btnRegularizaI.Enabled = false);
                    }


                }
                else
                {
                    RunOnUiThread(() => listProductInventarioSimpleI.SetAdapter(null));
                    RunOnUiThread(() => Toast.MakeText(this, "NO EXISTEN PRODUCTOS EN ESTE ALMACEN!", ToastLength.Short).Show());
                }
            }
            catch (Exception ex)
            {
                RunOnUiThread(() => Toast.MakeText(this, "OCURRIO UN ERROR AL MOSTRAR STOCK DE PRODUCTSO! EXEPCION: " + ex.Message, ToastLength.Short).Show());
            }
        }
        private void ListProductInventarioSimpleI_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            string Id = lmovimientoslite[e.Position].Id.ToString();
            string id_movimiento = lmovimientoslite[e.Position].id_movimiento.ToString();
            string nombre_producto = lmovimientoslite[e.Position].nombre_producto.ToString();
            int cantidad = Convert.ToInt32(lmovimientoslite[e.Position].cantidad);

            string cantidadexiste = cantidad.ToString();
            if (instanceupdateitem == 0)
            {
                instanceupdateitem = 1;
                _ap.saveIdInventarioTempKey(Id);
                _ap.saveIdMovimientoTempKey(id_movimiento);
                _ap.saveInventarioNombreProdTempKey(nombre_producto);
                _ap.saveInventarioCantTempKey(cantidadexiste);


                MuestraSeleccion(Convert.ToInt32(Id), nombre_producto, cantidad);
            }
        }

        private void MuestraSeleccion(int v, string nombre_producto, int cantidad)
        {
            try
            {
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                InventarioProductoSelectFragment selecatenderfg = new InventarioProductoSelectFragment();
                selecatenderfg.Show(transaction, "selecinventfg");
            }
            catch (Exception ex)
            {
                RunOnUiThread(() => Toast.MakeText(this, "Error al modificar item! Exepcion: " + ex.Message, ToastLength.Short).Show());
            }
        }

        private async Task LlenaProductosLocal()
        {
            try
            {
                RunOnUiThread(() => listProductInventarioSimpleI.SetAdapter(null));
            }
            catch (Exception re)
            {

            }
            try
            {
                if (existenmovimientos == 0)
                {
                    db.CreateDatabaseMovimiento();
                }
            }
            catch (Exception ex)
            {

            }
            try
            {
                db = new DataHelpers();
                List<ProductoErpLite> lProductoAllLitetemp = new List<ProductoErpLite>();
                lProductoAllLitetemp = db.selectProductoAllLite();
                if (lProductoAllLitetemp != null && lProductoAllLitetemp.Count > 0)
                {
                    try
                    {
                        db.deleteQueryTableMovimientoByAlmacen(idalmacen);
                    }
                    catch (Exception)
                    {

                    }

                    foreach (var item in lProductoAllLitetemp)
                    {
                        MovimientoErpLite movimiento = new MovimientoErpLite
                        {
                            fk_movimiento_tipo = 7,  //inventario inicial
                            fk_guia_remision_detalle = 0,
                            fk_venta_detalle = 0,
                            fk_comprobante_traslado_detalle = 0,
                            fk_nota_credito_detalle = 0,
                            fk_almacen = idalmacen,
                            fk_producto = item.id_producto,
                            cod_producto = item.cod_producto,
                            nombre_producto = item.nom_producto,
                            producto_tipo = item.producto_tipo,
                            cantidad = 0,
                            IDCODIGOGENERAL = "",
                            fk_salida_almacen = 0
                        };
                        int resultadomovimiento = db.insertIntoMovimiento(movimiento);
                    }

                    lmovimientoslite = new List<MovimientoErpLite>();
                    var vlmovimientoslite = db.selectTableMovimiento();
                    lmovimientoslite = vlmovimientoslite.Where(q => q.fk_almacen == idalmacen).ToList();
                    RunOnUiThread(() => productosinventarioListAdapter = new ProductoMovimientoActivityAdapter(_mContext, lmovimientoslite, this));
                    RunOnUiThread(() => listProductInventarioSimpleI.Adapter = productosinventarioListAdapter);
                    RunOnUiThread(() => listProductInventarioSimpleI.ItemClick += ListProductInventarioSimpleI_ItemClick);
                }
                else
                {
                    RunOnUiThread(() => listProductInventarioSimpleI.SetAdapter(null));
                    RunOnUiThread(() => Toast.MakeText(this, "NO EXISTEN PRODUCTOS EN ESTE ALMACEN!", ToastLength.Short).Show());
                }
            }
            catch (Exception ex)
            {
                RunOnUiThread(() => Toast.MakeText(this, "OCURRIO UN ERROR AL MOSTRAR STOCK DE PRODUCTSO! EXEPCION: " + ex.Message, ToastLength.Short).Show());
            }
        }

        private async void LlenaAlmacen(List<AlmacenErp> lalmaceneslite)
        {
            try
            {
                ArrayAdapter adapter = new ArrayAdapter(_mContext, Resource.Layout.spinner_item_medium, lalmaceneslite.OrderBy(x => x.nombre).ToList());
                RunOnUiThread(() => adapter.SetDropDownViewResource(Resource.Layout.spinner_dropdown_item));
                RunOnUiThread(() => spAlmacenSimpleI.ItemSelected += SpAlmacenSimpleI_ItemSelected);
                RunOnUiThread(() => spAlmacenSimpleI.Adapter = adapter);
            }
            catch (Exception ex)
            {
                RunOnUiThread(() => Toast.MakeText(this, "NO SE PUDO CARGAR DATA DE ALMACENES! EXCEPCION: " + ex.Message, ToastLength.Short).Show());
            }
        }

        private void SpAlmacenSimpleI_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {
                idalmacen = lalmaceneslite.ElementAt(spAlmacenSimpleI.SelectedItemPosition).id_almacen;

            }
            catch (Exception exception)
            {
                idalmacen = 0;
            }
            try
            {
                RunOnUiThread(() => listProductInventarioSimpleI.SetAdapter(null));
            }
            catch (Exception re)
            {

            }

            try
            {
                var existemoves = db.selectTableMovimiento();
                if (existemoves != null && existemoves.Count > 0 && idalmacen > 0 && existemoves.Where(x => x.fk_almacen == idalmacen).ToList().Count > 0)
                {
                    var concantidad = existemoves.Where(y => y.cantidad > 0).ToList();
                    int nconcantidad = concantidad.Count;
                    if (nconcantidad > 0)
                    {
                        existenmovimientos = 1;
                        RunOnUiThread(() => Toast.MakeText(this, "EXISTEN MOVIMIENTOS POR REGULARIZAR", ToastLength.Long).Show());
                        RunOnUiThread(() => btnRegularizaI.Enabled = true);
                    }
                    else
                    {
                        existenmovimientos = 0;
                        RunOnUiThread(() => btnRegularizaI.Enabled = false);
                    }

                }
                else
                {
                    existenmovimientos = 0;
                    RunOnUiThread(() => btnRegularizaI.Enabled = false);
                }
            }
            catch (Exception)
            {
                existenmovimientos = 0;
                RunOnUiThread(() => btnRegularizaI.Enabled = false);
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            navigationView.InflateMenu(Resource.Menu.nav_menu_almacen); //Navigation Drawer Layout Menu Creation 
            return true;
        }
        private void _btnSearch_Click(object sender, TextView.EditorActionEventArgs args)
        {
            //idproducto = 0;
            string buscar = edt_searchproductsList.Text;
            if (buscar != "" && buscar.Length > 2)
            {
                InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
                imm.HideSoftInputFromWindow(edt_searchproductsList.WindowToken, 0);

                progress = new ProgressDialog(this);
                progress.Indeterminate = true;
                progress.SetProgressStyle(ProgressDialogStyle.Spinner);
                progress.SetMessage("Buscando... espere...");
                progress.SetCancelable(false);
                RunOnUiThread(() =>
                {
                    progress.Show();
                });
                listproductos = new List<ProductoErpLite>();
                var b = Task.Run(async () =>
                {
                    var info = await Data.BuscaProductosLite(buscar);
                    if (info != null)
                    {
                        listproductos = info;
                        ProductoErpListActivityAdapter adapter = null;
                        RunOnUiThread(() => adapter = new ProductoErpListActivityAdapter(_mContext, listproductos, this));
                        RunOnUiThread(() => rvproductosIp.Adapter = adapter);
                        RunOnUiThread(() => rvproductosIp.ItemClick += ListProductsManage_ItemClick);

                    }
                    else
                    {
                        RunOnUiThread(() => Toast.MakeText(this, "Imposible Conectarse al servidor!", ToastLength.Short).Show());
                    }

                }).ContinueWith(info => RunOnUiThread(() => progress.Hide()));
            }
        }
        private void ListProductsManage_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //string eIdProduct = listproductos[e.Position].id_producto.ToString();
            //string eNombProduct = listproductos[e.Position].nom_producto;
            //string idtipoitem = listproductos[e.Position].fk_producto_tipo.ToString();
            //string idmarcaitem = listproductos[e.Position].fk_producto_marca.ToString();
            //string marcaitem = listproductos[e.Position].marca.ToString();
            //string tipoitem = listproductos[e.Position].producto_tipo.ToString();
            //string codigo_sku = listproductos[e.Position].codigo_sku;
            //string cantproducts = listproductos[e.Position].cant_items.ToString();
            //try
            //{
            //    idproducto = Convert.ToInt32(eIdProduct);
            //    _ap.saveIdProductoKey(eIdProduct);
            //    _ap.saveIdProductoTipoKey(idtipoitem);
            //    _ap.saveIdProductoMarcaKey(idmarcaitem);
            //    _ap.saveProductoKey(eNombProduct);
            //    _ap.saveProductoTipoKey(tipoitem);
            //    _ap.saveProductoMarcaKey(marcaitem);
            //    _ap.saveProductoSkuKey(codigo_sku);

            //}
            //catch (Exception)
            //{
            //    idproducto = 0;
            //}



            if (idalmacen == -1)
            {
                RunOnUiThread(() => Toast.MakeText(this, "Seleccione almacen primero!", ToastLength.Short).Show());
            }
            else
            {
                if (listproductos.Any())
                {
                    int resultadomovimiento = 0;
                    int id_producto = listproductos[e.Position].id_producto;
                    string cod_producto = listproductos[e.Position].cod_producto.ToString();
                    string nom_producto = listproductos[e.Position].nom_producto.ToString();
                    string descripcion_producto_tipo = listproductos[e.Position].producto_tipo.ToString();

                    var movingresadoex = db.selectTableMovimiento();
                    if (movingresadoex != null && movingresadoex.Any())
                    {
                        var existe = movingresadoex.Where(x => x.fk_producto == id_producto && x.fk_almacen == idalmacen && x.id_movimiento == 0).FirstOrDefault();
                        if (existe != null)
                        {
                            resultadomovimiento = existe.Id;
                        }
                        else
                        {
                            MovimientoErpLite movimiento = new MovimientoErpLite
                            {
                                fk_movimiento_tipo = 7,  //inventario inicial
                                fk_guia_remision_detalle = 0,
                                fk_venta_detalle = 0,
                                fk_comprobante_traslado_detalle = 0,
                                fk_nota_credito_detalle = 0,
                                fk_almacen = idalmacen,
                                fk_producto = id_producto,
                                cod_producto = cod_producto,
                                nombre_producto = nom_producto,
                                producto_tipo = descripcion_producto_tipo,
                                cantidad = 0,
                                IDCODIGOGENERAL = ""
                            };
                            resultadomovimiento = db.insertIntoMovimiento(movimiento);
                        }
                    }
                    else
                    {
                        MovimientoErpLite movimiento = new MovimientoErpLite
                        {
                            fk_movimiento_tipo = 7,  //inventario inicial
                            fk_guia_remision_detalle = 0,
                            fk_venta_detalle = 0,
                            fk_comprobante_traslado_detalle = 0,
                            fk_nota_credito_detalle = 0,
                            fk_almacen = idalmacen,
                            fk_producto = id_producto,
                            cod_producto = cod_producto,
                            nombre_producto = nom_producto,
                            producto_tipo = descripcion_producto_tipo,
                            cantidad = 0,
                            IDCODIGOGENERAL = ""
                        };
                        resultadomovimiento = db.insertIntoMovimiento(movimiento);
                    }


                    if (resultadomovimiento > 0)
                    {
                        var movingresado = db.selectTableMovimiento();
                        if (movingresado != null && movingresado.Any())
                        {
                            var estees = movingresado.Where(x => x.Id == resultadomovimiento).FirstOrDefault();
                            string Id = estees.Id.ToString();
                            string id_movimiento = estees.id_movimiento.ToString();
                            string nombre_producto = estees.nombre_producto.ToString();
                            int cantidad = Convert.ToInt32(estees.cantidad);

                            string cantidadexiste = cantidad.ToString();
                            if (instanceupdateitem == 0)
                            {
                                instanceupdateitem = 1;
                                _ap.saveIdInventarioTempKey(Id);
                                _ap.saveIdMovimientoTempKey(id_movimiento);
                                _ap.saveInventarioNombreProdTempKey(nombre_producto);
                                _ap.saveInventarioCantTempKey(cantidadexiste);


                                MuestraSeleccion(Convert.ToInt32(Id), nombre_producto, cantidad);
                            }
                        }
                    }
                }
            }
        }
        private void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            var menuItem = e.MenuItem;
            switch (menuItem.ItemId)
            {
                case Resource.Id.menu_almacen_lineas:
                    toolbar.Title = "Lineas";
                    var newact = new Intent(this, typeof(LineaActivity));
                    StartActivity(newact);
                    break;
                case Resource.Id.menu_almacen_sublineas:
                    toolbar.Title = "Tipos";
                    var newactt = new Intent(this, typeof(TipoActivity));
                    StartActivity(newactt);
                    break;
                case Resource.Id.menu_almacen_productos:
                    toolbar.Title = "Productos";
                    var newactp = new Intent(this, typeof(ProductoActivity));
                    StartActivity(newactp);
                    break;
                case Resource.Id.menu_almacen_salidas:
                    toolbar.Title = "Salidas";
                    var newactis = new Intent(this, typeof(ProductoSalidaActivity));
                    StartActivity(newactis);
                    break;
                case Resource.Id.menu_almacen_personal:
                    toolbar.Title = "Reporte";
                    var newactirps = new Intent(this, typeof(ProductoPersonalHistoryActivity));
                    StartActivity(newactirps);
                    break;

            }
        }

        private void ShowDataUser()
        {
            RunOnUiThread(() => navheader_username.Text = _ap.getNombresKey());
        }

        private void setupDrawerContent(NavigationView navigationView)
        {
            navigationView.NavigationItemSelected += (sender, e) =>
            {
                e.MenuItem.SetChecked(true);
                drawerLayout.CloseDrawers();
            };
        }


        public async void OnDismiss(IDialogInterface dialog)
        {
            try
            {
                instanceupdateitem = 0;
                LlenaProductosLocalAUpdate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}