using ProyectoCrud.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics.Contracts;


namespace ProyectoCrud.Controllers
{
    public class ContactoController : Controller
    {
        private static string conexion = ConfigurationManager.ConnectionStrings["cadena"].ToString();

        private static List<Contacto> olista = new List<Contacto>();
        /*
         Problema: Cada vez que se realizaba una nueva solicitud HTTP a tu controlador, se creaba una nueva instancia de ContactoController, y con ella, una nueva olista. Esto significaba que cualquier cambio o adición a la lista se perdía entre solicitudes, a menos que esos datos se almacenaran y recuperaran de una fuente persistente como una base de datos.
         Solución: Al hacer olista estática, ahora la lista es compartida por todas las instancias del controlador. Esto significa que cualquier contacto añadido a la lista permanece disponible en solicitudes HTTP subsiguientes, simulando un estado persistente.         
         */

        // GET: Contacto
        public ActionResult Inicio()
        {
            //Evita q se agregen elementos repetidos a la lista
            olista = new List<Contacto>();
            

            using (SqlConnection oconexion = new SqlConnection(conexion))
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

        [HttpGet]
        public ActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(Contacto contacto)
        {

            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("SP_Registrar", oconexion);
                cmd.Parameters.AddWithValue("@Nombres", contacto.Nombres);
                cmd.Parameters.AddWithValue("@Apellidos", contacto.Apellidos);
                cmd.Parameters.AddWithValue("@Telefono", contacto.Telefono);
                cmd.Parameters.AddWithValue("@Correo", contacto.Correo);

                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();

            }

                return RedirectToAction("Inicio");
        }

        [HttpGet]
        public ActionResult Actualizar(int? id)
        {
            //la lista debe ser static para persistir los datos

            if(id == null) return RedirectToAction("Inicio");

            Contacto contacto = olista.Where(x => x.IdContacto == id).FirstOrDefault();

            return View(contacto);
        }

        [HttpPost]
        public ActionResult Actualizar(Contacto contacto)
        {
            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
                SqlCommand cdm = new SqlCommand("SP_Editar", oconexion);
                cdm.Parameters.AddWithValue("@IdContacto", contacto.IdContacto);
                cdm.Parameters.AddWithValue("@Nombres", contacto.Nombres);
                cdm.Parameters.AddWithValue("@Apellidos", contacto.Apellidos);
                cdm.Parameters.AddWithValue("@Telefono", contacto.Telefono);
                cdm.Parameters.AddWithValue("@Correo", contacto.Correo);

                cdm.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cdm.ExecuteNonQuery();
            }

                return RedirectToAction("Inicio");
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null) return RedirectToAction("Inicio");

            Contacto contacto = olista.FirstOrDefault(x => x.IdContacto == id);

            return View(contacto);
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("SP_Eliminar", oconexion);
                cmd.Parameters.AddWithValue("@IdContacto", id);

                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }
                return RedirectToAction("Inicio");
        }
    }
}