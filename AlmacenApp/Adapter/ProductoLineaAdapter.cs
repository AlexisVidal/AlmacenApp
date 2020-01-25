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
    class ProductoLineaAdapter : BaseAdapter<ProductoLineaErp>
    {
        private List<ProductoLineaErp> listItems;
        Context myContext;
        LineaFragment lineaFragment;

        public ProductoLineaAdapter(Context context, List<ProductoLineaErp> items, LineaFragment linea)
        {
            listItems = items;
            myContext = context;
            lineaFragment = linea;
        }
        public override ProductoLineaErp this[int position] 
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
                row = LayoutInflater.From(myContext).Inflate(Resource.Layout.lineas_grid_view, null, false);
            }
            var imageDeletelinea = row.FindViewById<ImageButton>(Resource.Id.ibtndeletelinea);
            imageDeletelinea.Focusable = false;
            imageDeletelinea.FocusableInTouchMode = false;
            imageDeletelinea.Clickable = true;

            try
            {
                imageDeletelinea.Click += (sender, args) => lineaFragment.EliminaLinea(sender, listItems[position].id_producto_linea.ToString(), listItems[position].descripcion.ToString(), listItems[position].cant_tipos.ToString()); 
            }
            catch (Exception ex)
            {
                Console.WriteLine("error: " + ex.Message);
            }
            TextView txtcodlinea = row.FindViewById<TextView>(Resource.Id.txtcodlinea);
            txtcodlinea.Text = listItems[position].codigo;

            TextView txtlinea = row.FindViewById<TextView>(Resource.Id.txtlinea);
            txtlinea.Text = listItems[position].descripcion.ToString();

            return row;
        } 
    }

}