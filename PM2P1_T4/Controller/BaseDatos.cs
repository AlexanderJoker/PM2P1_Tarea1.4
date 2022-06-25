using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PM2P1_T4.Model;
using SQLite;

namespace PM2P1_T4.Controller
{
    public class BaseDatos
    {
        readonly SQLiteAsyncConnection database;

        public BaseDatos(string path)
        {
            database = new SQLiteAsyncConnection(path);

            database.CreateTableAsync<Imagen>();
        }

        #region OperacionesImagen
        //Metodos CRUD - CREATE
        public Task<int> insertUpdateImagen(Imagen img)
        {
            if (img.id != 0)
            {
                return database.UpdateAsync(img);
            }
            else
            {
                return database.InsertAsync(img);
            }
        }

        //Metodos CRUD - READ
        public Task<List<Imagen>> getListImagen()
        {
            return database.Table<Imagen>().ToListAsync();
        }

        public Task<Imagen> getImagen(int id)
        {
            return database.Table<Imagen>()
                .Where(i => i.id == id)
                .FirstOrDefaultAsync();
        }

        //Metodos CRUD - DELETE
        public Task<int> deleteImagen(Imagen img)
        {
            return database.DeleteAsync(img);
        }

        #endregion OperacionesImagen
    }
}
