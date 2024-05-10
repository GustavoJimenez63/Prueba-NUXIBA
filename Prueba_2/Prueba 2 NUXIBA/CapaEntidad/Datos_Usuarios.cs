using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace CapaEntidad
{
    public class Datos_Usuarios
    {
        public string cadenaConexion = @"Data Source=GUSTAVO;Initial Catalog=NUXIBA_Prueba_Tecnica;User ID=sa;Password=12345678;";


        public List<Usuarios> obtenerUsuarios()
        {
            List<Usuarios> listaUsuarios = new List<Usuarios>();


            using (SqlConnection conection = new SqlConnection(cadenaConexion))
            {
                string query = "SELECT u.userid,u.Login, u.Nombre, u.Paterno, u.Materno, e.sueldo, e.FechaIngreso FROM usuarios u JOIN empleados e ON u.userid = e.userId; ";

                SqlCommand command = new SqlCommand(query, conection);
                conection.Open();

                SqlDataReader lectura = command.ExecuteReader();

                while (lectura.Read())
                {

                    // Convertir la fecha de entrada de formato JSON a una fecha legible

                    listaUsuarios.Add(
                        new Usuarios()
                        {
                            userid= Convert.ToInt32(lectura["userid"]),
                            Login = lectura["Login"].ToString(),
                            Nombre = lectura["Nombre"].ToString(),
                            Paterno = lectura["Paterno"].ToString(),
                            Materno = lectura["Materno"].ToString(),
                            oEmpledos = new Empleados()
                            {
                                Sueldo = Convert.ToSingle(lectura["sueldo"]), 
                                FechaIngreso = Convert.ToDateTime(lectura["FechaIngreso"])
                               
                            }
                        }
                        );
                }

                conection.Close();
            }
            return listaUsuarios;
        }




        public int Registrar(Usuarios obj, out string Mensaje)
        {
            int IdAutogenerado = 0;
            Mensaje = string.Empty;


            if (string.IsNullOrEmpty(obj.Nombre))
            {
                Mensaje = "No puedes dejar el campo nombre vacio";
            }
            else if (string.IsNullOrEmpty(obj.Paterno))
            {
                Mensaje += "No puedes dejar el campo Apellido Paterno vacio";
            }
            else if (string.IsNullOrEmpty(obj.Materno))
            {
                Mensaje += "No puedes dejar el campo Apellido materno vacio";
            }
            else if (string.IsNullOrEmpty(obj.Login))
            {
                Mensaje += "No puedes dejar el campo Login vacio";
            }
            else if (obj.oEmpledos.Sueldo == 0)
            {
                Mensaje += "No puedes dejar el campo Sueldo vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                try
                {
                    using (SqlConnection oConexion = new SqlConnection(cadenaConexion))
                    {
                        string consultaInsertarUsuario = @"INSERT INTO Usuarios (Login, Nombre, Paterno, Materno) 
                                   VALUES (@Login, @Nombre, @Paterno, @Materno);
                                   SELECT SCOPE_IDENTITY();"; // Obtener el ID autogenerado

                        SqlCommand cmdInsertarUsuario = new SqlCommand(consultaInsertarUsuario, oConexion);

                        cmdInsertarUsuario.Parameters.AddWithValue("@Login", obj.Login);
                        cmdInsertarUsuario.Parameters.AddWithValue("@Nombre", obj.Nombre);
                        cmdInsertarUsuario.Parameters.AddWithValue("@Paterno", obj.Paterno);
                        cmdInsertarUsuario.Parameters.AddWithValue("@Materno", obj.Materno);

                        oConexion.Open();
                        IdAutogenerado = Convert.ToInt32(cmdInsertarUsuario.ExecuteScalar());
                        oConexion.Close();

                        string consultaInsertarEmpleado = @"INSERT INTO Empleados (userId, sueldo, FechaIngreso) 
                                    VALUES (@IdUsuario, @Sueldo, GETDATE());";


                        SqlCommand cmdInsertarEmpleado = new SqlCommand(consultaInsertarEmpleado, oConexion);

                        cmdInsertarEmpleado.Parameters.AddWithValue("@IdUsuario", IdAutogenerado);
                        cmdInsertarEmpleado.Parameters.AddWithValue("@Sueldo", obj.oEmpledos.Sueldo);



                        oConexion.Open();
                        cmdInsertarEmpleado.ExecuteNonQuery();
                        oConexion.Close();

                    }
                }
                catch (Exception ex)
                {
                    IdAutogenerado = 0;
                    Mensaje = ex.Message;

                }
            }

            
            return IdAutogenerado;
        }




        public bool Editar(Usuarios obj, out string Mensaje)
        {
            bool Resultado = false;
            Mensaje = string.Empty;

            if(string.IsNullOrEmpty(obj.Nombre))
            {
                Mensaje = "No puedes dejar el campo nombre vacio";
            }else if( string.IsNullOrEmpty(obj.Paterno))
            {
                Mensaje += "No puedes dejar el campo Apellido Paterno vacio";
            }
            else if (string.IsNullOrEmpty(obj.Materno))
            {
                Mensaje += "No puedes dejar el campo Apellido materno vacio";
            }
            else if (string.IsNullOrEmpty(obj.Login))
            {
                Mensaje += "No puedes dejar el campo Login vacio";
            }
            else if (obj.oEmpledos.Sueldo==0)
            {
                Mensaje += "No puedes dejar el campo Sueldo vacio";
            }


            if(string.IsNullOrEmpty(Mensaje))
            {
                try
                {
                    using (SqlConnection oConexion = new SqlConnection(cadenaConexion))
                    {
                        string consultaActualizarUsuario = @"UPDATE Usuarios
                                     SET Login = @Login,
                                         Nombre = @Nombre,
                                         Paterno = @Paterno,
                                         Materno = @Materno
                                     WHERE userid = @userid;";

                        SqlCommand cmdActualizarUsuario = new SqlCommand(consultaActualizarUsuario, oConexion);

                        cmdActualizarUsuario.Parameters.AddWithValue("@Login", obj.Login);
                        cmdActualizarUsuario.Parameters.AddWithValue("@Nombre", obj.Nombre);
                        cmdActualizarUsuario.Parameters.AddWithValue("@Paterno", obj.Paterno);
                        cmdActualizarUsuario.Parameters.AddWithValue("@Materno", obj.Materno);
                        cmdActualizarUsuario.Parameters.AddWithValue("@userid", obj.userid);

                        oConexion.Open();
                        cmdActualizarUsuario.ExecuteNonQuery();
                        oConexion.Close();

                        string consultaActualizarEmpleado = @"UPDATE empleados
                                      SET Sueldo = @sueldo
                                      WHERE userid = @userid;";

                        SqlCommand cmdActualizarEmpleado = new SqlCommand(consultaActualizarEmpleado, oConexion);

                        cmdActualizarEmpleado.Parameters.AddWithValue("@sueldo", obj.oEmpledos.Sueldo);
                        cmdActualizarEmpleado.Parameters.AddWithValue("@userid", obj.userid); // Utilizar el ID del usuario

                        oConexion.Open();
                        cmdActualizarEmpleado.ExecuteNonQuery();
                        oConexion.Close();

                        Resultado = true;
                    }
                }
                catch (Exception ex)
                {
                    Resultado = false;
                    Mensaje = ex.Message;
                }

            }



            
            return Resultado;
        }




    }
}
