using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlmacenApp.Models;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using SQLite;

namespace AlmacenApp.Resources.DataHelpers
{
    public class DataHelpers
    {
        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        public bool CreateDatabaseInventarioInicial()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Ventas.db")))
                {
                    try
                    {
                        connection.DropTable<ProductoErpLite>();
                    }
                    catch (SQLiteException ex)
                    {
                        Log.Info("SQLiteEx", ex.Message);
                    }
                    connection.CreateTable<ProductoErpLite>();
                    connection.CreateTable<MovimientoErpLite>();
                    connection.CreateTable<SalidaProductoErpLite>();
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public bool CleanDatabaseInventarioInicial()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Ventas.db")))
                {
                    try
                    {
                        connection.DropTable<ProductoErpLite>();
                        connection.DropTable<SalidaProductoErpLite>();
                        connection.DropTable<MovimientoErpLite>();
                    }
                    catch (SQLiteException ex)
                    {
                        Log.Info("SQLiteEx", ex.Message);
                    }
                    connection.CreateTable<ProductoErpLite>();
                    connection.CreateTable<MovimientoErpLite>();
                    connection.CreateTable<SalidaProductoErpLite>();
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public bool CreateDatabaseAlmacen()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Ventas.db")))
                {
                    connection.CreateTable<AlmacenErp>();
                    connection.CreateTable<AlmacenErpLite>();

                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public bool insertAlmacen(AlmacenErp entidad)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Ventas.db")))
                {
                    connection.Insert(entidad);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public List<AlmacenErp> selectTableAlmacen()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Ventas.db")))
                {
                    return connection.Table<AlmacenErp>().ToList();

                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }

        public bool deleteTableAlmacen(AlmacenErp entidad)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Ventas.db")))
                {
                    connection.Delete(entidad);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool selectQueryTableAlmacen(int Id)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Ventas.db")))
                {
                    connection.Query<AlmacenErp>("SELECT * FROM AlmacenErp Where id_almacen = ?", Id);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public bool deleteQueryTableAllAlmacen()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Ventas.db")))
                {
                    connection.Query<AlmacenErp>("DELETE FROM AlmacenErp Where id_almacen != 0 ");
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }


        public bool insertAlmacenStock(AlmacenStockErpLite entidad)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Ventas.db")))
                {
                    connection.Insert(entidad);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public List<AlmacenStockErpLite> selectTableAlmacenStock()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Ventas.db")))
                {
                    return connection.Table<AlmacenStockErpLite>().ToList();

                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }

        public bool updateAlmacenStock(int Id, decimal cantidad)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Ventas.db")))
                {
                    connection.Query<AlmacenStockErpLite>("UPDATE AlmacenStockErpLite Set SaldoMostrar = ? Where id_almacen_stock = ?", cantidad, Id);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool deleteTableAlmacenStock(AlmacenStockErpLite entidad)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Ventas.db")))
                {
                    connection.Delete(entidad);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool selectQueryTableAlmacenStock(int Id)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Ventas.db")))
                {
                    connection.Query<AlmacenStockErpLite>("SELECT * FROM AlmacenStockErpLite Where id_almacen_stock = ?", Id);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public bool deleteQueryTableAllAlmacenStock()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Ventas.db")))
                {
                    connection.Query<AlmacenStockErpLite>("DELETE FROM AlmacenStockErpLite Where id_almacen_stock > 0");
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool CreateDatabaseMovimiento()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Ventas.db")))
                {
                    try
                    {
                        //connection.DropTable<ProductoAllLite>();
                        connection.DropTable<MovimientoErpLite>();
                    }
                    catch (SQLiteException ex)
                    {
                        Log.Info("SQLiteEx", ex.Message);
                    }
                    //connection.CreateTable<ProductoAllLite>();
                    connection.CreateTable<MovimientoErpLite>();
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }


        public int insertIntoMovimiento(MovimientoErpLite entidad)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Ventas.db")))
                {
                    connection.Insert(entidad);
                    return entidad.Id;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return 0;
            }
        }

        public List<MovimientoErpLite> selectTableMovimiento()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Ventas.db")))
                {
                    return connection.Table<MovimientoErpLite>().ToList();

                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }

        public bool updateMovimientoLite(int Id, int cantidad)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Ventas.db")))
                {
                    connection.Query<MovimientoErpLite>("UPDATE MovimientoErpLite Set cantidad = ? Where Id = ?", cantidad, Id);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool deleteTableMovimiento(MovimientoErpLite entidad)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Ventas.db")))
                {
                    connection.Delete(entidad);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool selectQueryTableMovimientoLite(int Id)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Ventas.db")))
                {
                    connection.Query<MovimientoErpLite>("SELECT * FROM MovimientoErpLite Where id_movimiento = ?", Id);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public bool deleteQueryTableMovimiento()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Ventas.db")))
                {
                    connection.Query<MovimientoErpLite>("DELETE FROM MovimientoErpLite Where Id > 0");
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public bool deleteQueryTableMovimientoTipo(int tipo)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Ventas.db")))
                {
                    connection.Query<MovimientoErpLite>("DELETE FROM MovimientoErpLite Where fk_movimiento_tipo = ?", tipo);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public bool deleteQueryTableMovimientoByAlmacen(int idalmacen)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Ventas.db")))
                {
                    connection.Query<MovimientoErpLite>("DELETE FROM MovimientoErpLite Where fk_almacen = ?", idalmacen);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public bool deleteQueryTableMovimientoById(int Id)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Ventas.db")))
                {
                    connection.Query<MovimientoErpLite>("DELETE FROM MovimientoErpLite Where Id = ?", Id);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public bool insertProductoAllLite(ProductoErp entidad)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Ventas.db")))
                {
                    connection.Insert(entidad);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public bool insertProductoAllLite2(ProductoErpLite entidad)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Ventas.db")))
                {
                    connection.Insert(entidad);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public List<ProductoErpLite> selectProductoAllLite()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Ventas.db")))
                {
                    return connection.Table<ProductoErpLite>().ToList();

                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }

        public bool updateQueryProductoAllLite(int Id, int cantidad_inicial)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Ventas.db")))
                {
                    connection.Query<ProductoErpLite>("UPDATE ProductoErpLite Set cantidad_inicial = ? Where Id = ?", cantidad_inicial, Id);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool deleteProductoAllLite(ProductoErpLite entidad)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Ventas.db")))
                {
                    connection.Delete(entidad);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool selectQueryProductoAllLite(int Id)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Ventas.db")))
                {
                    connection.Query<ProductoErpLite>("SELECT * FROM ProductoErpLite Where Id = ?", Id);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public bool deleteQueryTableAllProductoAllLite()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Ventas.db")))
                {
                    connection.Query<ProductoErpLite>("DELETE FROM ProductoErpLite Where Id > 0");
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }


        #region movimiento

        public int insertIntoSalidaProducto(SalidaProductoErpLite entidad)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Ventas.db")))
                {
                    connection.Insert(entidad);
                    return entidad.Id;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return 0;
            }
        }

        public List<SalidaProductoErpLite> selectTableSalidaProducto()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Ventas.db")))
                {
                    return connection.Table<SalidaProductoErpLite>().ToList();

                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }

        public bool updateSalidaProductoLite(int Id, string estado)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Ventas.db")))
                {
                    connection.Query<SalidaProductoErpLite>("UPDATE SalidaProductoErpLite Set estado = ? Where Id = ?", estado, Id);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool deleteTableSalidaProducto(SalidaProductoErpLite entidad)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Ventas.db")))
                {
                    connection.Delete(entidad);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool selectQueryTableSalidaProductoLite(int Id)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Ventas.db")))
                {
                    connection.Query<SalidaProductoErpLite>("SELECT * FROM SalidaProductoErpLite Where id_salida_almacen = ?", Id);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public bool deleteQueryTableSalidaProducto()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Ventas.db")))
                {
                    connection.Query<SalidaProductoErpLite>("DELETE FROM SalidaProductoErpLite Where Id > 0");
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public bool deleteQueryTableSalidaProductoByAlmacen(int idalmacen)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Ventas.db")))
                {
                    connection.Query<SalidaProductoErpLite>("DELETE FROM SalidaProductoErpLite Where fk_almacen = ?", idalmacen);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        #endregion

    }
}