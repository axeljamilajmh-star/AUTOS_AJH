using AUTOS_AJH.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace AUTOS_AJH2.Models
{
    internal class DB_AUTO
    {
        // Cambia el Data Source por tu instancia de SQL Server
        private string cadenaConexion = @"Data Source=DESKTOP-B10TQP8\SQLEXPRESS;Initial Catalog=Autos_AJMH;Integrated Security=True";

        /// <summary>
        /// Obtiene todos los autos de la base de datos
        /// </summary>
        public DataTable ObtenerAutos()
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection con = new SqlConnection(cadenaConexion))
                {
                    string query = "SELECT * FROM vehiculo";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener autos: " + ex.Message);
            }

            return dt;
        }

        /// <summary>
        /// Inserta un nuevo auto en la base de datos
        /// </summary>
        public void InsertarAuto(Auto auto)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cadenaConexion))
                {
                    string query = @"
                        INSERT INTO vehiculo 
                        (Codigo, Marca, Modelo, Ano, Tipo, Precio, Color, Cilindraje, Transmision, Combustible, Puertas, Pasajeros, Descripcion, Disponible)
                        VALUES
                        (@Codigo, @Marca, @Modelo, @Ano, @Tipo, @Precio, @Color, @Cilindraje, @Transmision, @Combustible, @Puertas, @Pasajeros, @Descripcion, @Disponible)";

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Codigo", auto.Codigo);
                    cmd.Parameters.AddWithValue("@Marca", auto.Marca);
                    cmd.Parameters.AddWithValue("@Modelo", auto.Modelo);
                    cmd.Parameters.AddWithValue("@Ano", auto.Ano);
                    cmd.Parameters.AddWithValue("@Tipo", auto.Tipo);
                    cmd.Parameters.AddWithValue("@Precio", auto.Precio);
                    cmd.Parameters.AddWithValue("@Color", auto.Color);
                    cmd.Parameters.AddWithValue("@Cilindraje", auto.Cilindraje);
                    cmd.Parameters.AddWithValue("@Transmision", auto.Transmision);
                    cmd.Parameters.AddWithValue("@Combustible", auto.Combustible);
                    cmd.Parameters.AddWithValue("@Puertas", auto.Puertas);
                    cmd.Parameters.AddWithValue("@Pasajeros", auto.Pasajeros);
                    cmd.Parameters.AddWithValue("@Descripcion", auto.Descripcion);
                    cmd.Parameters.AddWithValue("@Disponible", auto.Disponible);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar auto: " + ex.Message);
            }
        }

        /// <summary>
        /// Actualiza un auto existente por su código
        /// </summary>
        public void ActualizarAuto(Auto auto)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cadenaConexion))
                {
                    string query = @"
                        UPDATE vehiculo
                        SET Marca=@Marca, Modelo=@Modelo, Ano=@Ano, Tipo=@Tipo, Precio=@Precio,
                            Color=@Color, Cilindraje=@Cilindraje, Transmision=@Transmision,
                            Combustible=@Combustible, Puertas=@Puertas, Pasajeros=@Pasajeros,
                            Descripcion=@Descripcion, Disponible=@Disponible
                        WHERE Codigo=@Codigo";

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Codigo", auto.Codigo);
                    cmd.Parameters.AddWithValue("@Marca", auto.Marca);
                    cmd.Parameters.AddWithValue("@Modelo", auto.Modelo);
                    cmd.Parameters.AddWithValue("@Ano", auto.Ano);
                    cmd.Parameters.AddWithValue("@Tipo", auto.Tipo);
                    cmd.Parameters.AddWithValue("@Precio", auto.Precio);
                    cmd.Parameters.AddWithValue("@Color", auto.Color);
                    cmd.Parameters.AddWithValue("@Cilindraje", auto.Cilindraje);
                    cmd.Parameters.AddWithValue("@Transmision", auto.Transmision);
                    cmd.Parameters.AddWithValue("@Combustible", auto.Combustible);
                    cmd.Parameters.AddWithValue("@Puertas", auto.Puertas);
                    cmd.Parameters.AddWithValue("@Pasajeros", auto.Pasajeros);
                    cmd.Parameters.AddWithValue("@Descripcion", auto.Descripcion);
                    cmd.Parameters.AddWithValue("@Disponible", auto.Disponible);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar auto: " + ex.Message);
            }
        }

        /// <summary>
        /// Elimina un auto por su código
        /// </summary>
        public void EliminarAuto(string codigo)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cadenaConexion))
                {
                    string query = "DELETE FROM vehiculo WHERE Codigo=@Codigo";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Codigo", codigo);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar auto: " + ex.Message);
            }
        }
    }
}
