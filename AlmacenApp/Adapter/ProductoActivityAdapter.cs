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
    class ProductoActivityAdapter : BaseAdapter<ProductoErp>
    {

        private List<ProductoErp> listItems;
        Context myContext;
        ProductoActivity tipoActivity;

        public ProductoActivityAdapter(Context context, List<ProductoErp> items, ProductoActivity tipo)
        {
            listItems = items;
            myContext = context;
            tipoActivity = tipo;
        }

        public override ProductoErp this[int position]
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
                row = LayoutInflater.From(myContext).Inflate(Resource.Layout.productos_grid_view, null, false);
            }
            var imageDeleteproducto = row.FindViewById<ImageButton>(Resource.Id.ibtndeleteproducto);
            imageDeleteproducto.Focusable = false;
            imageDeleteproducto.FocusableInTouchMode = false;
            imageDeleteproducto.Clickable = true;

            try
            {
                imageDeleteproducto.Click += (sender, args) => tipoActivity.EliminaProducto(sender, listItems[position].id_producto.ToString(), listItems[position].producto_tipo.ToString(), listItems[position].cant_items.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("error: " + ex.Message);
            }
            TextView txtcodproducto = row.FindViewById<TextView>(Resource.Id.txtcodproducto);
            txtcodproducto.Text = listItems[position].cod_producto;

            TextView txtmarcaproducto = row.FindViewById<TextView>(Resource.Id.txtmarcaproducto);
            txtmarcaproducto.Text = listItems[position].marca.ToString();

            TextView txtunidadproducto = row.FindViewById<TextView>(Resource.Id.txtunidadproducto);
            txtunidadproducto.Text = listItems[position].abreviatura.ToString();

            TextView txtlineaproducto = row.FindViewById<TextView>(Resource.Id.txtlineaproducto);
            txtlineaproducto.Text = listItems[position].producto_linea.ToString();

            TextView txttipoproducto = row.FindViewById<TextView>(Resource.Id.txttipoproducto);
            txttipoproducto.Text = listItems[position].producto_tipo.ToString();

            TextView txtproducto = row.FindViewById<TextView>(Resource.Id.txtproducto);
            txtproducto.Text = listItems[position].nom_producto.ToString();

            return row;
        }

        //Fill in cound here, currently 0



    }

}