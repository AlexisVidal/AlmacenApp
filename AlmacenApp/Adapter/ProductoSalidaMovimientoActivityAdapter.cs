using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlmacenApp.Activities;
using AlmacenApp.Models;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AlmacenApp.Adapter
{
    class ProductoSalidaMovimientoActivityAdapter : BaseAdapter<MovimientoErpLite>
    {
        private List<MovimientoErpLite> listItems;
        Context myContext;
        ProductoSalidaActivity produinvent;

        public ProductoSalidaMovimientoActivityAdapter(Context context, List<MovimientoErpLite> items, ProductoSalidaActivity proinvent)
        {
            listItems = items;
            myContext = context;
            produinvent = proinvent;
        }
        public override MovimientoErpLite this[int position]
        {
            get
            {
                return listItems[position];
            }
        }

        public override int Count
        {
            get
            {
                return listItems.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
            {
                row = LayoutInflater.From(myContext).Inflate(Resource.Layout.producto_salida_movimiento_view, null, false);
            }
            var imageDeletesalidaprod = row.FindViewById<ImageButton>(Resource.Id.ibtndeletePSI);
            imageDeletesalidaprod.Focusable = false;
            imageDeletesalidaprod.FocusableInTouchMode = false;
            imageDeletesalidaprod.Clickable = true;

            try
            {
                imageDeletesalidaprod.Click += (sender, args) => produinvent.EliminaSalidaMovimiento(sender, 
                    listItems[position].id_movimiento.ToString(), 
                    listItems[position].nombre_full.ToString(), 
                    listItems[position].nombre_producto.ToString(), 
                    listItems[position].abreviatura.ToString(), 
                    listItems[position].cantidad.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("error: " + ex.Message);
            }

            TextView txtPersonalPSI = row.FindViewById<TextView>(Resource.Id.txtPersonalPSI);
            txtPersonalPSI.Text = listItems[position].nombre_full.ToString();

            TextView txtProductoPSI = row.FindViewById<TextView>(Resource.Id.txtProductoPSI);
            txtProductoPSI.Text = listItems[position].nombre_producto.ToString();

            TextView txtUnidadPSI = row.FindViewById<TextView>(Resource.Id.txtUnidadPSI);
            txtUnidadPSI.Text = listItems[position].abreviatura.ToString();

            TextView txtCantPSI = row.FindViewById<TextView>(Resource.Id.txtCantPSI);
            txtCantPSI.Text = listItems[position].cantidad.ToString();

            TextView txtAlmacenPSI = row.FindViewById<TextView>(Resource.Id.txtAlmacenPSI);
            txtAlmacenPSI.Text = listItems[position].almacen.ToString();

            return row;
        }

    }
}