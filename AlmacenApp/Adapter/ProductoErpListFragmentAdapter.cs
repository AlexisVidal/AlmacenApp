using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlmacenApp.Fragments;
using AlmacenApp.Models;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AlmacenApp.Adapter
{
    class ProductoErpListFragmentAdapter : BaseAdapter<ProductoErpLite>
    {
        private List<ProductoErpLite> listItems;
        Context myContext;
        ProductoSalidaPersonalFragment produinvent;

        public ProductoErpListFragmentAdapter(Context context, List<ProductoErpLite> items, ProductoSalidaPersonalFragment proinvent)
        {
            listItems = items;
            myContext = context;
            produinvent = proinvent;
        }
        public override ProductoErpLite this[int position]
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
                row = LayoutInflater.From(myContext).Inflate(Resource.Layout.producto_movimiento_view, null, false);
            }
            //var ibtndeleteproductatender = row.FindViewById<ImageButton>(Resource.Id.ibtndeleteproductatender);
            //ibtndeleteproductatender.Focusable = false;
            //ibtndeleteproductatender.FocusableInTouchMode = false;
            //ibtndeleteproductatender.Clickable = false;

            TextView txtCodProductoI = row.FindViewById<TextView>(Resource.Id.txtCodProductoI);
            txtCodProductoI.Text = listItems[position].cod_producto.ToString();

            TextView txtProductoI = row.FindViewById<TextView>(Resource.Id.txtProductoI);
            txtProductoI.Text = listItems[position].nom_producto.ToString();

            TextView txtUnidadI = row.FindViewById<TextView>(Resource.Id.txtUnidadI);
            txtUnidadI.Text = listItems[position].abreviatura.ToString();

            TextView txtTipoI = row.FindViewById<TextView>(Resource.Id.txtTipoI);
            txtTipoI.Text = listItems[position].producto_tipo.ToString();

            TextView txtMarcaProductoI = row.FindViewById<TextView>(Resource.Id.txtMarcaProductoI);
            txtMarcaProductoI.Text = listItems[position].marca.ToString();
            

            //TextView txtTotaldetallesoli = row.FindViewById<TextView>(Resource.Id.txtTotaldetallesoli);
            //txtTotaldetallesoli.Text = "STOCK: " + listItems[position].existencia.ToString();

            TextView txtCantI = row.FindViewById<TextView>(Resource.Id.txtCantI);
            txtCantI.Text = listItems[position].cant_items.ToString();

            return row;
        }
    }
}