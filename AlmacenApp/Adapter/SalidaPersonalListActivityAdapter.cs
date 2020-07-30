﻿using System;
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
    class SalidaPersonalListActivityAdapter : BaseAdapter<MovimientoErp>
    {
        private List<MovimientoErp> listItems;
        Context myContext;
        ProductoPersonalHistoryActivity produinvent;

        public SalidaPersonalListActivityAdapter(Context context, List<MovimientoErp> items, ProductoPersonalHistoryActivity proinvent)
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
                row = LayoutInflater.From(myContext).Inflate(Resource.Layout.personal_grid_view, null, false);
            }

            TextView txtCodSalidaPersonal = row.FindViewById<TextView>(Resource.Id.txtCodSalidaPersonal);
            txtCodSalidaPersonal.Text = listItems[position].IDCODIGOGENERAL.ToString();

            TextView txtNombresSalidaPersonal = row.FindViewById<TextView>(Resource.Id.txtNombresSalidaPersonal);
            txtNombresSalidaPersonal.Text = listItems[position].nombres.ToString();


            return row;
        }

    }
}