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
    class ProductoSalidaPersonalFragmentAdapter : BaseAdapter<MovimientoErpLite>
    {

        private List<MovimientoErpLite> listItems;
        Context myContext;
        ProductoSalidaPersonalFragment producsal;

        public ProductoSalidaPersonalFragmentAdapter(Context context, List<MovimientoErpLite> items, ProductoSalidaPersonalFragment producs)
        {
            listItems = items;
            myContext = context;
            producsal = producs;
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
                row = LayoutInflater.From(myContext).Inflate(Resource.Layout.producto_personal_movimiento_view, null, false);
            }
            var imageDeletelinea = row.FindViewById<ImageButton>(Resource.Id.ibtndeletelineaSI);
            imageDeletelinea.Focusable = false;
            imageDeletelinea.FocusableInTouchMode = false;
            imageDeletelinea.Clickable = true;

            try
            {
                imageDeletelinea.Click += (sender, args) => producsal.EliminaMovimiento(sender, listItems[position].Id.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("error: " + ex.Message);
            }
            TextView txtIdMovimientoSI = row.FindViewById<TextView>(Resource.Id.txtIdMovimientoSI);
            txtIdMovimientoSI.Text = listItems[position].Id.ToString();

            TextView txtCodPersonalSI = row.FindViewById<TextView>(Resource.Id.txtCodPersonalSI);
            txtCodPersonalSI.Text = listItems[position].IDCODIGOGENERAL.ToString();

            TextView txtProductoSI = row.FindViewById<TextView>(Resource.Id.txtProductoSI);
            txtProductoSI.Text = listItems[position].nombre_producto.ToString();

            TextView txtUnidadSI = row.FindViewById<TextView>(Resource.Id.txtUnidadSI);
            txtUnidadSI.Text = listItems[position].abreviatura.ToString();

            TextView txtCantSI = row.FindViewById<TextView>(Resource.Id.txtCantSI);
            txtCantSI.Text = listItems[position].abreviatura.ToString();

            return row;
        }

        //Fill in cound here, currently 0



    }

}