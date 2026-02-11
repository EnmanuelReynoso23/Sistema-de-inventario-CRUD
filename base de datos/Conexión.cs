using System;
using Npgsql;

namespace Proyecto.base_de_datos
{
    public class Conexion
    {
        private string conexionString;

        public Conexion()
        {
            conexionString = "Host=localhost;Port=5432;Username=postgres;Password=22022006;Database=";
        }

        public NpgsqlConnection GetConnection() 
        {
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(conexionString);
                return conn;
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo conectar con la base de datos:" + ex.Message);
            }
        }
    }
}
