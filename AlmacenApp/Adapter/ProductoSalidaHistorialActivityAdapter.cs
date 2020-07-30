using System;
using System.Collections.Generic;
using System.Globalization;
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
    class ProductoSalidaHistorialActivityAdapter : BaseAdapter<SalidaProductoErp>
    {
        private List<SalidaProductoErp> listItems;
        Context myContext;
        ProductoPersonalHistoryActivity produinvent;

        public ProductoSalidaHistorialActivityAdapter(Context context, List<SalidaProductoErp> items, ProductoPersonalHistoryActivity proinvent)
        {
            listItems = items;
            myContext = context;
            produinvent = proinvent;
        }
        public override SalidaProductoErp this[int position]
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
                row = LayoutInflater.From(myContext).Inflate(Resource.Layout.salidas_grid_view, null, false);
            }

            TextView txtUsuarioSalida = row.FindViewById<TextView>(Resource.Id.txtUsuarioSalida);
            txtUsuarioSalida.Text = listItems[position].nomb_full.Trim();

            TextView txtFechaSalida = row.FindViewById<TextView>(Resource.Id.txtFechaSalida);
            txtFechaSalida.Text = listItems[position].fecha_registro.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            return row;
        }

    }
}