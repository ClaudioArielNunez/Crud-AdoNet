using ProyectoCrud.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;


namespace ProyectoCrud.Controllers
{
    public class ContactoController : Controller
    {
        private static string conexion = ConfigurationManager.ConnectionStrings["cadena"].ToString();

        private List<Contacto> olista = new List<Contacto>();

        // GET: Contacto
        public ActionResult Inicio()
        {
            using (SqlConnection oconexion = new SqlConnection(conexion) )
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM CONTACTO", oconexion);
                cmd.CommandType = CommandType.Text;
                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Contacto nuevoContacto = new Contacto();
                        nuevoContacto.IdContacto = int.Parse(dr["IdContacto"].ToString());
                        //nuevoContacto.IdContacto = Convert.ToInt32(dr["IdContacto"]);
                        nuevoContacto.Nombres = dr["Nombres"].ToString();
                        nuevoContacto.Apellidos = dr["apellidos"].ToString();
                        nuevoContacto.Telefono = dr["Telefono"].ToString();
                        nuevoContacto.Correo = dr["Correo"].ToString();

                        olista.Add(nuevoContacto);
                    }
                }               
                                
            }

            return View(olista);
        }
    }
}