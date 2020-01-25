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
    class ProductoTipoActivityAdapter : BaseAdapter<ProductoTipoErp>
    {

        private List<ProductoTipoErp> listItems;
        Context myContext;
        TipoActivity tipoActivity;

        public ProductoTipoActivityAdapter(Context context, List<ProductoTipoErp> items, TipoActivity tipo)
        {
            listItems = items;
            myContext = context;
            tipoActivity = tipo;
        }

        public override ProductoTipoErp this[int position]
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
                row = LayoutInflater.From(myContext).Inflate(Resource.Layout.tipos_grid_view, null, false);
            }
            var imageDeletetipo = row.FindViewById<ImageButton>(Resource.Id.ibtndeletetipo);
            imageDeletetipo.Focusable = false;
            imageDeletetipo.FocusableInTouchMode = false;
            imageDeletetipo.Clickable = true;

            try
            {
                imageDeletetipo.Click += (sender, args) => tipoActivity.EliminaTipo(sender, listItems[position].id_producto_tipo.ToString(), listItems[position].producto_tipo.ToString(), listItems[position].cant_items.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("error: " + ex.Message);
            }
            TextView txtcodtipo = row.FindViewById<TextView>(Resource.Id.txtcodtipo);
            txtcodtipo.Text = listItems[position].codigo_tipo;

            TextView txtlineatipo = row.FindViewById<TextView>(Resource.Id.txtlineatipo);
            txtlineatipo.Text = listItems[position].linea.ToString();

            TextView txttipo = row.FindViewById<TextView>(Resource.Id.txttipo);
            txttipo.Text = listItems[position].producto_tipo.ToString();

            return row;
        }

        //Fill in cound here, currently 0



    }

}