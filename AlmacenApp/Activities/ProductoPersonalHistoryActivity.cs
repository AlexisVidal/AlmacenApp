using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlmacenApp.Adapter;
using AlmacenApp.Clases;
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
using Android.Widget;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Android.Content.PM;
using iTextSharp.text.html.simpleparser;

namespace AlmacenApp.Activities
{
    [Activity(Label = "Historial de Salidas", MainLauncher = false, WindowSoftInputMode = SoftInput.AdjustPan, ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape, Theme = "@style/NavigationStyle", NoHistory = true)]
    public class ProductoPersonalHistoryActivity : AppCompatActivity, IDialogInterfaceOnDismissListener
    {
        private AppPreferences _ap;
        private readonly Context _mContext = Application.Context;
        public static string serverurl = "";
        DrawerLayout drawerLayout;
        NavigationView navigationView;
        TextView navheader_username;
        V7Toolbar toolbar;
        private View _view;
        DataHelpers db;
        string idusuario = "";
        List<AlmacenErp> lalmaceneslite;
        List<MovimientoErp> lmovimientosdistinc;
        List<MovimientoErp> lmovimientos;
        List<SalidaProductoErp> lsalidas;
        Spinner spAlmacenSimpleReporte;
        int idalmacen = 0;
        int idsalida = 0;
        string idpersonal = "";
        ListView rvsalidaRp;
        ListView rvpersonalSalidaRp;
        ListView rvpersonalMovimientosRp;
        ProductoSalidaHistorialActivityAdapter salidaadapter;
        SalidaPersonalListActivityAdapter salidapersonadapter;
        ProductoSalidaMovimientoHistoryActivityAdapter productosalidamovadapter;
        List<MovimientoErp> lmovimientotemp = new List<MovimientoErp>();
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.productos_personal_history_layout);
            _ap = new AppPreferences(_mContext);
            db = new DataHelpers();
            db.CreateDatabaseInventarioInicial();
            idusuario = _ap.getIdCodigoGeneralLoginTempKey();
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
            rvsalidaRp = FindViewById<ListView>(Resource.Id.rvsalidaRp);
            rvpersonalSalidaRp = FindViewById<ListView>(Resource.Id.rvpersonalSalidaRp);
            rvpersonalMovimientosRp = FindViewById<ListView>(Resource.Id.rvpersonalMovimientosRp);
            spAlmacenSimpleReporte = FindViewById<Spinner>(Resource.Id.spAlmacenSimpleReporte);
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
            var fab = FindViewById<Refractored.Fab.FloatingActionButton>(Resource.Id.fabReport);
            fab.AttachToListView(rvpersonalMovimientosRp);
            fab.Click += (sender, args) =>
            {
                PrintList();
            };
            fab.Show();
        }

       


        private async void PrintList()
        {
            try
            {
                if (lmovimientotemp.Any())
                {

                    //var directory = new Java.IO.File(Android.OS.Environment.DirectoryDownloads,"pdf").ToString();
                    //string directory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                    string directory = (Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads)).Path;
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    var path = Path.Combine(directory, "reporte_movimiento.pdf");

                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    var fs = new FileStream(path, FileMode.Create);

                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    Font times = new Font(bfTimes, 10, Font.NORMAL, Color.BLACK);
                    Font timesbig = new Font(bfTimes, 13, Font.BOLD, Color.BLACK);
                    Font timesbigt = new Font(bfTimes, 11, Font.BOLD, Color.BLACK);
                    Document document = new Document(PageSize.A4.Rotate(), 5, 5, 5, 5);
                    
                    PdfWriter writer = PdfWriter.GetInstance(document, fs);
                    HTMLWorker worker = new HTMLWorker(document);
                    document.Open();
                    Paragraph ptitle = new Paragraph("REPORTE DE SALIDA DE MATERIAL", timesbig);
                    ptitle.Alignment = Element.ALIGN_CENTER;
                    document.Add(ptitle);
                    document.Add(new Paragraph(""));
                    //document.Add(new Table(6, lmovimientotemp.Count()));

                    PdfPTable table = new PdfPTable(6);
                    table.WidthPercentage = 100;
                    table.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //PdfPCell header = new PdfPCell(new Phrase("REPORTE DE SALIDA DE MATERIAL"));
                    //header.Colspan = 6;
                    //table.AddCell(header);
                    table.AddCell(new Paragraph("FECHA", timesbigt));
                    table.AddCell(new Paragraph("PERSONAL", timesbigt));
                    table.AddCell(new Paragraph("PRODUCTO", timesbigt));
                    table.AddCell(new Paragraph("U.M.", timesbigt));
                    table.AddCell(new Paragraph("CANT.", timesbigt));
                    table.AddCell(new Paragraph("ALMACEN", timesbigt));
                    foreach (var item in lmovimientotemp)
                    {
                        table.AddCell(new Paragraph(item.f_movimiento.ToString("dd/MM/yyyy"), times));
                        table.AddCell(new Paragraph(item.nomb_full, times));
                        table.AddCell(new Paragraph(item.nombre_producto, times));
                        table.AddCell(new Paragraph(item.abreviatura, times));
                        table.AddCell(new Paragraph(item.cantidad.ToString("N"), times));
                        table.AddCell(new Paragraph(item.nombre_almacen, times));
                    }

                    //StringBuilder html = new StringBuilder();
                    //html.Append("<? xml version='1.0' encoding='utf-8' ?><html><head><title></title></head>");
                    //html.Append("<center>Simple Sample html</H1>");
                    //html.Append("<H4>By User1</H4>");
                    //html.Append("<H2>Demonstrating a few HTML features</H2>");
                    //html.Append("</center>");
                    //html.Append("<p>HTML doesn't normally use line breaks for ordinary text. A white space of any size is treated as a single space. This is because the author of the page has no way of knowing the size of the reader's screen, or what size type they will have their browser set for.");
                    //html.Append("</p></body</html>");
                    //TextReader reader = new StringReader(html.ToString());
                    //worker.StartDocument();
                    //worker.Parse(reader);
                    //worker.EndDocument();
                    //worker.Close();

                    document.Add(table);
                    document.Close();
                    writer.Close();
                    fs.Close();

                    Java.IO.File file = new Java.IO.File(path);
                    Intent intent = new Intent(Intent.ActionView);
                    intent.SetDataAndType(Android.Net.Uri.FromFile(file), "application/pdf");
                    StartActivity(intent);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void LlenaAlmacen(List<AlmacenErp> lalmaceneslite)
        {
            try
            {
                ArrayAdapter adapter = new ArrayAdapter(_mContext, Resource.Layout.spinner_item_medium, lalmaceneslite.OrderBy(x => x.nombre).ToList());
                RunOnUiThread(() => adapter.SetDropDownViewResource(Resource.Layout.spinner_dropdown_item));
                RunOnUiThread(() => spAlmacenSimpleReporte.ItemSelected += spAlmacenSimpleReporte_ItemSelected);
                RunOnUiThread(() => spAlmacenSimpleReporte.Adapter = adapter);
            }
            catch (Exception ex)
            {
                RunOnUiThread(() => Toast.MakeText(this, "NO SE PUDO CARGAR DATA DE ALMACENES! EXCEPCION: " + ex.Message, ToastLength.Short).Show());
            }
        }
        private void spAlmacenSimpleReporte_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {
                idalmacen = lalmaceneslite.ElementAt(spAlmacenSimpleReporte.SelectedItemPosition).id_almacen;
                _ap.saveIdAlmacenTempKey(idalmacen.ToString());
                _ap.saveAlmacenTempKey(lalmaceneslite.ElementAt(spAlmacenSimpleReporte.SelectedItemPosition).nombre.ToString());
                if (idalmacen > 0)
                {
                    idsalida = 0;
                    idpersonal = "";
                    LlenaSalidas(idalmacen);
                }
            }
            catch (Exception exception)
            {
                idalmacen = 0;
            }
        }

        private async void LlenaSalidas(int idalmacen)
        {
            lsalidas = new List<SalidaProductoErp>();
            try
            {
                var listasalidas = await Data.GetSalidasAlmacen();
                if (listasalidas.Any())
                {
                    var listados = listasalidas.Where(x => x.fk_almacen == idalmacen).ToList();
                    if (listados.Any())
                    {
                        try
                        {
                            RunOnUiThread(() => rvsalidaRp.SetAdapter(null));
                        }
                        catch (Exception re)
                        {

                        }
                        try
                        {
                            lsalidas = listados;
                            RunOnUiThread(() => salidaadapter = new ProductoSalidaHistorialActivityAdapter(_mContext, lsalidas, this));
                            RunOnUiThread(() => rvsalidaRp.Adapter = salidaadapter);
                            RunOnUiThread(() => rvsalidaRp.ItemClick += rvsalidaRp_ItemClick);

                            LlenaPersonal(0);
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }
                    else
                    {
                        RunOnUiThread(() => rvsalidaRp.SetAdapter(null));
                    }
                }
                else
                {
                    RunOnUiThread(() => rvsalidaRp.SetAdapter(null));
                }
            }
            catch (Exception ex)
            {
                RunOnUiThread(() => rvsalidaRp.SetAdapter(null));
            }
        }

        private async void LlenaPersonal(int idsalidas)
        {
            lmovimientosdistinc = new List<MovimientoErp>();
            lmovimientos = new List<MovimientoErp>();
            try
            {
                var movimientos = await Data.CargaMovimientosSalidas();
                lmovimientos = movimientos.Where(x=>x.fk_almacen == idalmacen).ToList();
                if (lmovimientos.Any() && idsalidas == 0)
                {
                    var listapersonas = (from y in lmovimientos
                                         select new
                                         {
                                             y.IDCODIGOGENERAL,
                                             y.nombres
                                         }).Distinct().ToList();

                    rvpersonalSalidaRp.SetAdapter(null);
                    if (listapersonas.Any())
                    {
                        foreach (var item in listapersonas)
                        {
                            lmovimientosdistinc.Add(new MovimientoErp()
                            {
                                IDCODIGOGENERAL = item.IDCODIGOGENERAL,
                                nombres = item.nombres
                            });
                        }
                        RunOnUiThread(() => salidapersonadapter = new SalidaPersonalListActivityAdapter(_mContext, lmovimientosdistinc, this));
                        RunOnUiThread(() => rvpersonalSalidaRp.Adapter = salidapersonadapter);
                        RunOnUiThread(() => rvpersonalSalidaRp.ItemClick += rvpersonalSalidaRp_ItemClick);


                    }
                }
                else if (lmovimientos.Any() && idsalidas > 0)
                {
                    var listapersonas = (from y in lmovimientos
                                         where y.fk_salida_almacen == idsalidas
                                         select new
                                         {
                                             y.IDCODIGOGENERAL,
                                             y.nombres
                                         }).Distinct().ToList();

                    rvpersonalSalidaRp.SetAdapter(null);
                    if (listapersonas.Any())
                    {
                        foreach (var item in listapersonas)
                        {
                            lmovimientosdistinc.Add(new MovimientoErp()
                            {
                                IDCODIGOGENERAL = item.IDCODIGOGENERAL,
                                nombres = item.nombres
                            });
                        }
                        RunOnUiThread(() => salidapersonadapter = new SalidaPersonalListActivityAdapter(_mContext, lmovimientosdistinc, this));
                        RunOnUiThread(() => rvpersonalSalidaRp.Adapter = salidapersonadapter);
                        RunOnUiThread(() => rvpersonalSalidaRp.ItemClick += rvpersonalSalidaRp_ItemClick);


                    }
                }
                LlenaMovimiento(idsalida, idpersonal);
            }
            catch (Exception ex)
            {

            }
        }

        private void rvpersonalSalidaRp_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            try
            {
                idpersonal = lmovimientosdistinc[e.Position].IDCODIGOGENERAL.ToString();
                LlenaMovimiento(idsalida, idpersonal);
            }
            catch (Exception exception)
            {
                idpersonal = "";
            }
        }

        private void LlenaMovimiento(int idsalida, string idpersonal)
        {
            
            try
            {
                rvpersonalMovimientosRp.SetAdapter(null);
                if (lmovimientos.Any())
                {
                    if (idsalida == 0 && idpersonal.Equals(""))
                    {
                        lmovimientotemp = lmovimientos;
                    }
                    else if (idsalida > 0 && idpersonal.Equals(""))
                    {
                        lmovimientotemp = lmovimientos.Where(x => x.fk_salida_almacen == idsalida).ToList();
                    }
                    else if (idsalida == 0 && !idpersonal.Equals(""))
                    {
                        lmovimientotemp = lmovimientos.Where(x => x.IDCODIGOGENERAL.Equals(idpersonal)).ToList();
                    }
                    else if (idsalida > 0 && !idpersonal.Equals(""))
                    {
                        lmovimientotemp = lmovimientos.Where(x => x.fk_salida_almacen == idsalida && x.IDCODIGOGENERAL.Equals(idpersonal)).ToList();
                    }
                    RunOnUiThread(() => productosalidamovadapter = new ProductoSalidaMovimientoHistoryActivityAdapter(_mContext, lmovimientotemp, this));
                    RunOnUiThread(() => rvpersonalMovimientosRp.Adapter = productosalidamovadapter);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void rvsalidaRp_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            try
            {
                idsalida = lsalidas[e.Position].id_salida_almacen;
                LlenaPersonal(idsalida);
            }
            catch (Exception exception)
            {
                idsalida = 0;
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
                case Resource.Id.menu_almacen_productos:
                    toolbar.Title = "Productos";
                    var newactp = new Intent(this, typeof(ProductoActivity));
                    StartActivity(newactp);
                    break;
                case Resource.Id.menu_almacen_inventario:
                    toolbar.Title = "Inventario";
                    var newacti = new Intent(this, typeof(InventarioActivity));
                    StartActivity(newacti);
                    break;
                case Resource.Id.menu_almacen_salidas:
                    toolbar.Title = "Salidas";
                    var newactis = new Intent(this, typeof(ProductoSalidaActivity));
                    StartActivity(newactis);
                    break;
                case Resource.Id.menu_almacen_sublineas:
                    toolbar.Title = "Tipos";
                    var newactt = new Intent(this, typeof(TipoActivity));
                    StartActivity(newactt);
                    break;
                case Resource.Id.menu_almacen_personal:
                    toolbar.Title = "Reporte";
                    var newactirps = new Intent(this, typeof(ProductoPersonalHistoryActivity));
                    StartActivity(newactirps);
                    break;
            }
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            navigationView.InflateMenu(Resource.Menu.nav_menu_almacen); //Navigation Drawer Layout Menu Creation 
            return true;
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

        public void OnDismiss(IDialogInterface dialog)
        {
            throw new NotImplementedException();
        }
    }
}