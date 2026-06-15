using MauiAppFit.Models;
using SQLite;

namespace MauiAppFit.Helpers
{
    public class SQLiteDatabaseHelper
    {
        readonly SQLiteAsyncConnection con;

        public SQLiteDatabaseHelper(string path)
        {
            con = new SQLiteAsyncConnection(path);
            con.CreateTableAsync<Atividade>().Wait();
        }

        public Task<List<Atividade>> GetAllRows()
        {
            return con.Table<Atividade>().OrderByDescending(i => i.Id).ToListAsync();
        }

        public Task<Atividade> GetById(int id)
        {
            return con.Table<Atividade>().FirstAsync(i => i.Id == id);
        }

        public Task<int> Insert(Atividade model)
        {
            return con.InsertAsync(model);
        }

        public Task<int> Delete(int id)
        {
            return con.Table<Atividade>().DeleteAsync(i => i.Id == id);
        }

        public Task<List<Atividade>> Search(string q)
        {
            string sql = $"SELECT * FROM Atividade WHERE Descricao LIKE '%{q}%' ";
            
            return con.QueryAsync<Atividade>(sql);
        }

        public Task<List<Atividade>> Update(Atividade model)
        {
            string sql = "UPDATE Atividade " +
                         "SET Descricao=?, Data=?, Peso=?, Observacoes=? " +
                         "WHERE Id=? ";

            return con.QueryAsync<Atividade>(
                sql,
                model.Descricao,
                model.Data,
                model.Peso,
                model.Observacoes,
                model.Id
             );
        }
    

    } // Fecha Classe

} // Fecha namespace
