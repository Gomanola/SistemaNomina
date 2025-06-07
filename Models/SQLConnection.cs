using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Sistema_Nomina.Models
{
    public class SQLConnection
    {
        //Logueo
        public static string conexion = "Data Source=DESKTOP-JOFMVR5\\SQLEXPRESS; Initial Catalog=Nominas; Integrated Security=True";
        public static string Logueo(Datos Dato)
        {
            SqlConnection conn = new SqlConnection(conexion);
            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_logueo", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@cve_emp", Dato.cve_emp);
            cmd.Parameters.AddWithValue("@contraseña", Dato.contraseña);

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                return "1";
                //AlumnosInfo.numero=1;
            }
            else //AlumnosInfo.numero=0;
                return "0";
        }

        //Consulta listado de empleados
        public List<Datos> listaEmpleados(string nombre, int id_area, int estado)
        {
            List<Datos> tips = new List<Datos>();
            SqlConnection conn = new SqlConnection(conexion);
            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_consultaEmpleados", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@nombre", nombre);
            cmd.Parameters.AddWithValue("@id_area", id_area);
            cmd.Parameters.AddWithValue("@estado", estado);

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                tips.Add(new Datos()
                {
                    numeroLista = Convert.ToInt32(dr["numero"]),
                    cve_emp = Convert.ToInt32(dr["cve_emp"]),
                    nombre = Convert.ToString(dr["nombre"]),
                    nombre_area = Convert.ToString(dr["nombre_area"]),
                    estado = Convert.ToBoolean(dr["estado"])
                });
            }
            conn.Close();
            return tips;
        }

        //Consulta cantidad de areas
        public static int consultaCantidadAreas()
        {
            int cantidad=0;
            SqlConnection conn = new SqlConnection(conexion);
            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_CantidadAreas", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read()) {
                cantidad = dr.GetInt32(0);
            }
            
            conn.Close();
            return cantidad;
        }

        //Consulta para buscar un area
        public static string consultaBusquedaArea(int id_area,string nombre_area)
        {
            string nombre_a="";
            SqlConnection conn = new SqlConnection(conexion);
            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_busquedaArea", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@id_area", id_area);
            cmd.Parameters.AddWithValue("@nombre_area", nombre_area);

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                nombre_a = dr.GetString(0);
            }

            conn.Close();
            return nombre_a;
        }

        //Consulta cantidad de clases
        public static int consultaCantidadClases()
        {
            int cantidad = 0;
            SqlConnection conn = new SqlConnection(conexion);
            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_CantidadClases", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                cantidad = dr.GetInt32(0);
            }

            conn.Close();
            return cantidad;
        }

        //Consulta para buscar un area
        public static string consultaBusquedaClase(int id_clase)
        {
            string nombre_clase = "";
            SqlConnection conn = new SqlConnection(conexion);
            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_busquedaClase", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@id_clase", id_clase);

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                nombre_clase = dr.GetString(0);
            }

            conn.Close();
            return nombre_clase;
        }
        public static void altaEmpleado(Datos Parametros)
        {
            SqlConnection conn = new SqlConnection(conexion);
            SqlCommand cmd = new SqlCommand("sp_altaEmpleados", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            conn.Open();

            /*string query = "Insert into tbAlumno(boleta,nombre,correo,grupo,edad) values(@boleta,@nombre,@correo,@grupo,@edad)";
            SqlCommand cmd = new SqlCommand(query, conn);*/

            cmd.Parameters.AddWithValue("@nom_emp", Parametros.nom_emp);
            cmd.Parameters.AddWithValue("@app_emp", Parametros.app_emp);
            cmd.Parameters.AddWithValue("@apm_emp", Parametros.apm_emp);
            cmd.Parameters.AddWithValue("@contraseña", Parametros.contraseña);
            cmd.Parameters.AddWithValue("@fech_nac", Parametros.fech_nac);
            cmd.Parameters.AddWithValue("@edad_pers", Parametros.edad_pers);
            cmd.Parameters.AddWithValue("@id_clase", Parametros.id_clase);
            cmd.Parameters.AddWithValue("@id_area", Parametros.id_area);

            int resultado = cmd.ExecuteNonQuery();
            conn.Close();
        }

        //Consulta para deplegar los datos de un empleado
        public static Datos consultaDetallesEmpleado(Datos Parametros)
        {
            SqlConnection conn = new SqlConnection(conexion);
            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_detallesEmpleado", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@cve_emp", Parametros.cve_emp);

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                Parametros.nom_emp = dr.GetString(1);
                Parametros.app_emp = dr.GetString(2);
                Parametros.apm_emp= dr.GetString(3);
                Parametros.contraseña = dr.GetString(4);
                Parametros.fech_nac = dr.GetDateTime(5);
                Parametros.edad_pers = dr.GetInt32(6);
                Parametros.habilitado = dr.GetBoolean(7);
                Parametros.estado = dr.GetBoolean(8);
                Parametros.id_clase = dr.GetInt32(9);
                Parametros.id_area = dr.GetInt32(10);
            }

            conn.Close();
            return Parametros;
        }

        //Modificar datos de los empleados
        public static void modificarEmpleado(Datos Parametros)
        {
            SqlConnection conn = new SqlConnection(conexion);
            SqlCommand cmd = new SqlCommand("sp_modificarEmpleado", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            conn.Open();

            /*string query = "Insert into tbAlumno(boleta,nombre,correo,grupo,edad) values(@boleta,@nombre,@correo,@grupo,@edad)";
            SqlCommand cmd = new SqlCommand(query, conn);*/

            cmd.Parameters.AddWithValue("@cve_emp", Parametros.cve_emp);
            cmd.Parameters.AddWithValue("@nom_emp", Parametros.nom_emp);
            cmd.Parameters.AddWithValue("@app_emp", Parametros.app_emp);
            cmd.Parameters.AddWithValue("@apm_emp", Parametros.apm_emp);
            cmd.Parameters.AddWithValue("@contraseña", Parametros.contraseña);
            cmd.Parameters.AddWithValue("@fech_nac", Parametros.fech_nac);
            cmd.Parameters.AddWithValue("@edad_pers", Parametros.edad_pers);
            cmd.Parameters.AddWithValue("@habilitado", Parametros.habilitado);
            cmd.Parameters.AddWithValue("@estado", Parametros.estado);
            cmd.Parameters.AddWithValue("@id_clase", Parametros.id_clase);
            cmd.Parameters.AddWithValue("@id_area", Parametros.id_area);

            int resultado = cmd.ExecuteNonQuery();
            conn.Close();
        }

        //Consultar cantidad de empleados en el sistema
        public static int consultaCantidadEmpleados()
        {
            int cantidad = 0;
            SqlConnection conn = new SqlConnection(conexion);
            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_CantidadEmpleados", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                cantidad = dr.GetInt32(0);
            }

            conn.Close();
            return cantidad;
        }
        //Dar de baja un empleado
        public static void bajaEmpleado(int cve_emp,string motivo)
        {
            SqlConnection conn = new SqlConnection(conexion);
            SqlCommand cmd = new SqlCommand("sp_bajaEmpleado", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            conn.Open();

            /*string query = "Insert into tbAlumno(boleta,nombre,correo,grupo,edad) values(@boleta,@nombre,@correo,@grupo,@edad)";
            SqlCommand cmd = new SqlCommand(query, conn);*/

            cmd.Parameters.AddWithValue("@cve_emp", cve_emp);
            cmd.Parameters.AddWithValue("@motivo", motivo);
            cmd.Parameters.AddWithValue("@Fecha_C", DateTime.Today);

            int resultado = cmd.ExecuteNonQuery();
            conn.Close();
        }
        //Consulta listado de Areas
        public List<Datos> listaAreas(string nombre_area)
        {
            List<Datos> tips = new List<Datos>();
            SqlConnection conn = new SqlConnection(conexion);
            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_consultaAreas", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@nombre_area", nombre_area);

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                tips.Add(new Datos()
                {
                    id_area = Convert.ToInt32(dr["id_area"]),
                    nombre_area = Convert.ToString(dr["nombre_area"]),
                    CantidadEmpleados = Convert.ToInt32(dr["CantidadEmpleados"]),
                    CantidadEmpleadosConContrato = Convert.ToInt32(dr["CantidadEmpleadosConContrato"]),
                    CantidadEmpleadosSinContrato = Convert.ToInt32(dr["CantidadEmpleadosSinContrato"])
                });
            }
            conn.Close();
            return tips;
        }
        public static void altaArea(string nombre_area)
        {
            SqlConnection conn = new SqlConnection(conexion);
            SqlCommand cmd = new SqlCommand("sp_altaAreas", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            conn.Open();

            /*string query = "Insert into tbAlumno(boleta,nombre,correo,grupo,edad) values(@boleta,@nombre,@correo,@grupo,@edad)";
            SqlCommand cmd = new SqlCommand(query, conn);*/

            cmd.Parameters.AddWithValue("@nombre_area", nombre_area);

            int resultado = cmd.ExecuteNonQuery();
            conn.Close();
        }
        public static void bajaArea(int id_area)
        {
            SqlConnection conn = new SqlConnection(conexion);
            SqlCommand cmd = new SqlCommand("sp_bajaArea", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            conn.Open();

            /*string query = "Insert into tbAlumno(boleta,nombre,correo,grupo,edad) values(@boleta,@nombre,@correo,@grupo,@edad)";
            SqlCommand cmd = new SqlCommand(query, conn);*/

            cmd.Parameters.AddWithValue("@id_area", id_area);

            int resultado = cmd.ExecuteNonQuery();
            conn.Close();
        }
        public static int consultaUltimoRegistro(string opcion)
        {
            int cantidad = 0;
            SqlConnection conn = new SqlConnection(conexion);
            SqlCommand cmd = new SqlCommand("sp_consultaUltimoRegistro", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            conn.Open();
            cmd.Parameters.AddWithValue("@opcion", opcion);

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                cantidad = dr.GetInt32(0);
            }

            conn.Close();
            return cantidad;
        }
        public static string consultaMotivoBajaEmpleado(int cve_emp)
        {
            string motivo = "";
            SqlConnection conn = new SqlConnection(conexion);
            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_consultaMotivoBajaEmpleado", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@cve_emp", cve_emp);

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                motivo = dr.GetString(0);
            }

            conn.Close();
            return motivo;
        }
        //Consulta listado de Conceptos
        public List<Datos> listaConceptos(string nom_concepto)
        {
            List<Datos> tips = new List<Datos>();
            SqlConnection conn = new SqlConnection(conexion);
            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_consultaConceptos", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@nom_concepto", nom_concepto);

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                tips.Add(new Datos()
                {
                    id_concepto = Convert.ToInt32(dr["id_concepto"]),
                    nom_concepto = Convert.ToString(dr["nom_concepto"]),
                    descripcion = Convert.ToString(dr["descripcion"]),
                    sueldoefecto = Convert.ToInt32(dr["sueldoefecto"])
                });
            }
            conn.Close();
            return tips;
        }
        //Consulta para buscar un concepto
        public static string consultaBusquedaConcepto(int id_concepto, string nom_concepto)
        {
            string nombre_a = "";
            SqlConnection conn = new SqlConnection(conexion);
            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_busquedaConcepto", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@id_concepto", id_concepto);
            cmd.Parameters.AddWithValue("@nom_concepto", nom_concepto);

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                nombre_a = dr.GetString(0);
            }

            conn.Close();
            return nombre_a;
        }
        public static void altaConcepto(string nom_concepto,string descripcion, float sueldoefecto)
        {
            SqlConnection conn = new SqlConnection(conexion);
            SqlCommand cmd = new SqlCommand("sp_altaConceptos", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            conn.Open();

            /*string query = "Insert into tbAlumno(boleta,nombre,correo,grupo,edad) values(@boleta,@nombre,@correo,@grupo,@edad)";
            SqlCommand cmd = new SqlCommand(query, conn);*/

            cmd.Parameters.AddWithValue("@nom_concepto", nom_concepto);
            cmd.Parameters.AddWithValue("@descripcion", descripcion);
            cmd.Parameters.AddWithValue("@sueldoefecto", sueldoefecto);

            int resultado = cmd.ExecuteNonQuery();
            conn.Close();
        }
        public static void bajaConcepto(int id_concepto)
        {
            SqlConnection conn = new SqlConnection(conexion);
            SqlCommand cmd = new SqlCommand("sp_bajaConcepto", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            conn.Open();

            /*string query = "Insert into tbAlumno(boleta,nombre,correo,grupo,edad) values(@boleta,@nombre,@correo,@grupo,@edad)";
            SqlCommand cmd = new SqlCommand(query, conn);*/

            cmd.Parameters.AddWithValue("@id_concepto", id_concepto);

            int resultado = cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
