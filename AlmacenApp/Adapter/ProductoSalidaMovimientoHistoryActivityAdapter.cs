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
    class ProductoSalidaMovimientoHistoryActivityAdapter : BaseAdapter<MovimientoErp>
    {
        private List<MovimientoErp> listItems;
        Context myContext;
        ProductoPersonalHistoryActivity produinvent;

        public ProductoSalidaMovimientoHistoryActivityAdapter(Context context, List<MovimientoErp> items, ProductoPersonalHistoryActivity proinvent)
        {
            listItems = items;
            myContext = context;
            produinvent = proinvent;
        }
        public override MovimientoErp this[int position]
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
                row = LayoutInflater.From(myContext).Inflate(Resource.Layout.producto_salida_history_view, null, false);
            }

            TextView txtFechaPSIR = row.FindViewById<TextView>(Resource.Id.txtFechaPSIR);
            txtFechaPSIR.Text = listItems[position].f_movimiento.ToString("dd/MM/yyyy");
            TextView txtPersonalPSI = row.FindViewById<TextView>(Resource.Id.txtPersonalPSIR);
            txtPersonalPSI.Text = listItems[position].nomb_full.ToString();

            TextView txtProductoPSI = row.FindViewById<TextView>(Resource.Id.txtProductoPSIR);
            txtProductoPSI.Text = listItems[position].nombre_producto.ToString();

            TextView txtUnidadPSI = row.FindViewById<TextView>(Resource.Id.txtUnidadPSIR);
            txtUnidadPSI.Text = listItems[position].abreviatura.ToString();

            TextView txtCantPSI = row.FindViewById<TextView>(Resource.Id.txtCantPSIR);
            txtCantPSI.Text = listItems[position].cantidad.ToString();

            TextView txtAlmacenPSI = row.FindViewById<TextView>(Resource.Id.txtAlmacenPSIR);
            txtAlmacenPSI.Text = listItems[position].nombre_almacen.ToString();

            return row;
        }

    }
}