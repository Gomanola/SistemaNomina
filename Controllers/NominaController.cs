using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Sistema_Nomina.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Sistema_Nomina.Controllers
{
    public class NominaController : Controller
    {
        static public Datos Dato;
        //LOGUEO
        [HttpGet]
        public IActionResult Logueo()
        {
            Dato = null;
            Validaciones cap = new Validaciones();
            ViewBag.Captcha = cap.GenerarCaptcha();
            ViewBag.RH = 0;
            return View();
        }
        [HttpPost]
        public IActionResult Logueo(Datos Parametros)
        {
            Validaciones cap = new Validaciones();
            ViewBag.Captcha = cap.GenerarCaptcha();
            string salida = SQLConnection.Logueo(Parametros);


            if (salida == "0")
            {
                ViewBag.Error = "Datos incorrectos";
                return View();
            }
            if (Parametros.captcha != Parametros.captchaGenerado)
            {
                ViewBag.Error = "Captcha incorrecto";
                return View();
            }
            if ((Parametros.captcha == Parametros.captchaGenerado) && (salida == "1"))
            {
                Dato = Parametros;
                ViewBag.cve_empleado = Dato.cve_emp;
                ViewBag.RH = 1;
                return View("Inicio", Parametros);
            }
            return View("Inicio", Parametros);
        }
        //Olvidar contraseña
        public IActionResult OlvidarContraseña()
        {
            return View();
        }

        // GET: NominaController/Inicio
        [HttpGet]
        public IActionResult Inicio()
        {
            if (Dato==null||Dato.cve_emp==0)
            {
                Validaciones cap = new Validaciones();
                ViewBag.Captcha = cap.GenerarCaptcha();
                return View("Logueo");
            }
            ViewBag.cve_empleado = Dato.cve_emp;
            ViewBag.RH = 1;
            return View();
        }
        [HttpPost]
        public IActionResult Inicio(Datos Dato)
        {
            return View(Dato);
        }

        // GET: NominaController/BusquedaEmpleados/
        [HttpGet]
        public ActionResult BusquedaEmpleados(bool baja)
        {
            ViewBag.RH = 1;

            //Si el baja no es true, el estado va a ser 0
            int estado = 0;

            if (baja)
            {
                ViewBag.baja = true;
                estado = 2;
            }

            // Buscar cantidad de áreas que hay e imprimirlas en una opción desplegable
            int no_areas = SQLConnection.consultaUltimoRegistro("Area");
            string[] area = new string[no_areas + 1];
            area[0] = "Todas las áreas";
            ViewBag.area = new string[no_areas + 1];
            ViewBag.area[0] = area[0];
            for (int i = 0; i < no_areas; i++)
            {
                area[i + 1] = SQLConnection.consultaBusquedaArea(i + 1,"");
                ViewBag.area[i + 1] = area[i + 1];
            }
            ViewBag.no_areas = no_areas;

            // Desplegar la lista de empleados
            SQLConnection Empleados = new SQLConnection();
            return View(Empleados.listaEmpleados("", 0, estado));
            //<label name="nombre_area" value="@Html.DisplayFor(modelItem => item.nombre_area)">@Html.DisplayFor(modelItem => item.nombre_area)</label>
        }
        [HttpPost]
        public ActionResult BusquedaEmpleados(int id_area, string nombre, int estado, bool baja)
        {
            ViewBag.RH = 1;
            ViewBag.baja = baja;

            // Buscar cantidad de áreas que hay e imprimirlas en una opción desplegable
            int no_areas = SQLConnection.consultaUltimoRegistro("Area");
            string[] area = new string[no_areas + 1];
            area[0] = "Todas las áreas";
            ViewBag.area = new string[no_areas + 1];
            ViewBag.area[0] = area[0];
            for (int i = 0; i < no_areas; i++)
            {
                area[i + 1] = SQLConnection.consultaBusquedaArea(i + 1, "");
                ViewBag.area[i + 1] = area[i + 1];
            }
            ViewBag.no_areas = no_areas;

            // Desplegar la lista de empleados
            if (nombre == null) { nombre = ""; }
            SQLConnection Empleados = new SQLConnection();
            return View(Empleados.listaEmpleados(nombre, id_area, estado));
        }
        // GET: NominaController/Details/5
        [HttpGet]
        public ActionResult AltasEmpleados()
        {
            ViewBag.RH = 1;

            //Buscar cantidad de areas que hay e imprimirlas en una opción plegable
            int no_areas = SQLConnection.consultaUltimoRegistro("Area");
            string[] area = new string[no_areas];
            ViewBag.area = new string[no_areas];
            for (int i = 0; i < no_areas; i++)
            {
                area[i] = SQLConnection.consultaBusquedaArea(i+1,"");
                ViewBag.area[i] = area[i];
            }
            ViewBag.no_areas = no_areas;


            //Buscar cantidad de clases de empleados que hay e imprimirlas en una opción plegable
            int no_clases = SQLConnection.consultaCantidadClases();
            string[] clase = new string[no_clases];
            ViewBag.clase = new string[no_clases];
            for (int i = 0; i < no_clases; i++)
            {
                clase[i] = SQLConnection.consultaBusquedaClase(i+1);
                ViewBag.clase[i] = clase[i];
            }
            ViewBag.no_clases = no_clases;
            return View();
        }
        [HttpPost]
        public ActionResult AltasEmpleados(Datos Parametros)
        {
            ViewBag.RH = 1;
            ViewBag.Error = "";
            ViewBag.Success = "";
            //Buscar cantidad de areas que hay e imprimirlas en una opción plegable
            
            
            int no_areas = SQLConnection.consultaUltimoRegistro("Area");
            string[] area = new string[no_areas];
            ViewBag.area = new string[no_areas];
            for (int i = 0; i < no_areas; i++)
            {
                area[i] = SQLConnection.consultaBusquedaArea(i+1, "");
                ViewBag.area[i] = area[i];
            }
            ViewBag.no_areas = no_areas;


            //Buscar cantidad de clases de empleados que hay e imprimirlas en una opción plegable
            int no_clases = SQLConnection.consultaCantidadClases();
            string[] clase = new string[no_clases];
            ViewBag.clase = new string[no_clases];
            for (int i = 0; i < no_clases; i++)
            {
                clase[i] = SQLConnection.consultaBusquedaClase(i+1);
                ViewBag.clase[i] = clase[i];
            }
            ViewBag.no_clases = no_clases;

            //Validar Area y Clase
            Parametros.id_area += 1;
            Parametros.id_clase += 1;

            //Validar Edad
            Parametros.edad_pers = Validaciones.Edad(Parametros.fech_nac);

            if (Parametros.edad_pers < 15)
            {
                ViewBag.Error = "No cumple con la menoría de edad";
                return View();
            }

            try
            {
                SQLConnection.altaEmpleado(Parametros);
            }
            catch
            {
                ViewBag.Error = "Empleado No Registrado";
                return View();
            }
            ViewBag.Success = "Empleado Registrado Con Éxito";

            return View();
        }


        // GET: NominaController/Detalles Empleado
        [HttpGet]
        public ActionResult DetallesEmpleado(Datos Parametros)
        {
            ViewBag.RH = 1;
            string motivo = "";

            //Buscar cantidad de areas que hay e imprimirlas en una opción plegable
            int no_areas = SQLConnection.consultaUltimoRegistro("Area");
            string[] area = new string[no_areas];
            ViewBag.area = new string[no_areas];
            for (int i = 0; i < no_areas; i++)
            {
                area[i] = SQLConnection.consultaBusquedaArea(i + 1, "");
                ViewBag.area[i] = area[i];
            }
            ViewBag.no_areas = no_areas;

            //Buscar cantidad de clases de empleados que hay e imprimirlas en una opción plegable
            int no_clases = SQLConnection.consultaUltimoRegistro("Clase");
            string[] clase = new string[no_clases];
            ViewBag.clase = new string[no_clases];
            for (int i = 0; i < no_clases; i++)
            {
                clase[i] = SQLConnection.consultaBusquedaClase(i + 1);
                ViewBag.clase[i] = clase[i];
            }
            ViewBag.no_clases = no_clases;

            //Consulta de datos del empleado e impresión en pantalla
            Parametros = SQLConnection.consultaDetallesEmpleado(Parametros);
            Parametros.motivo = SQLConnection.consultaMotivoBajaEmpleado(Parametros.cve_emp);
            ViewBag.estado = Parametros.estado;
            ViewBag.contraseña = Parametros.contraseña;

            Parametros.id_area -= 1;
            Parametros.id_clase -= 1;
            return View(Parametros);
        }
        [HttpPost]
        public ActionResult DetallesEmpleado(Datos Parametros, int hola)
        {
            ViewBag.RH = 1;
            ViewBag.Error = "";
            ViewBag.Success = "";
            ViewBag.estado = Parametros.estado;
            ViewBag.contraseña = Parametros.contraseña;

            //Buscar cantidad de areas que hay e imprimirlas en una opción plegable
            int no_areas = SQLConnection.consultaUltimoRegistro("Area");
            string[] area = new string[no_areas];
            ViewBag.area = new string[no_areas];
            for (int i = 0; i < no_areas; i++)
            {
                area[i] = SQLConnection.consultaBusquedaArea(i + 1,"");
                ViewBag.area[i] = area[i];
            }
            ViewBag.no_areas = no_areas;


            //Buscar cantidad de clases de empleados que hay e imprimirlas en una opción plegable
            int no_clases = SQLConnection.consultaCantidadClases();
            string[] clase = new string[no_clases];
            ViewBag.clase = new string[no_clases];
            for (int i = 0; i < no_clases; i++)
            {
                clase[i] = SQLConnection.consultaBusquedaClase(i + 1);
                ViewBag.clase[i] = clase[i];
            }
            ViewBag.no_clases = no_clases;

            //Validar Area y Clase
            Parametros.id_area += 1;
            Parametros.id_clase += 1;

            //Validar Edad
            Parametros.edad_pers = Validaciones.Edad(Parametros.fech_nac);

            if (Parametros.edad_pers < 15)
            {
                ViewBag.Error = "No cumple con la menoría de edad";
                return View(Parametros);
            }
            
            try
            {
                SQLConnection.modificarEmpleado(Parametros);
            }
            catch
            {
                ViewBag.Error = "Error De Modificación";
                return View(Parametros);
            }
            ViewBag.Success = "Empleado Modificado Con Éxito";

            return View(Parametros);
        }
        [HttpPost]
        public IActionResult BajasEmpleados(int[] bajaEmpleados,int confirmarBaja, string motivo)
        {
            if (bajaEmpleados != null && bajaEmpleados.Length > 0)
            {
                if (confirmarBaja == 0)
                {
                    Datos Parametros = new Datos();
                    List<Datos> tips = new List<Datos>();
                    foreach (var item in bajaEmpleados)
                    {
                        Parametros.cve_emp = item;
                        Parametros = SQLConnection.consultaDetallesEmpleado(Parametros);
                        tips.Add(new Datos()
                        {
                            cve_emp = item,
                            nombre = Parametros.nom_emp + " " + Parametros.app_emp + " " + Parametros.apm_emp,
                            nombre_area = SQLConnection.consultaBusquedaArea(Parametros.id_area,"")
                        });
                    }
                    return View(tips);
                }
                else if (confirmarBaja == 1)
                {
                    try
                    {
                        foreach (var cve_emp in bajaEmpleados)
                        {
                            // Implementar lógica de baja en la base de datos
                            if (motivo == null) { motivo = "Sin especificar"; }
                            SQLConnection.bajaEmpleado(cve_emp, motivo);
                        }
                        ViewBag.Success = "Baja completada con exito";
                        return RedirectToAction("BusquedaEmpleados", new { baja = false });
                    }
                    catch
                    {
                        ViewBag.Error = "Error al dar de baja";
                        return RedirectToAction("BajasEmpleados", new { baja = false });
                    }
                }
            }
            ViewBag.Error = "No se seleccionó ningún empleado";
            return RedirectToAction("BusquedaEmpleados", new { baja = true });
        }
        [HttpGet]
        public ActionResult BusquedaAreas(bool baja, string nueva_area, int boton)
        {
            if (nueva_area == null) { nueva_area = ""; }
            ViewBag.RH = 1;


            switch (boton)
            {
                case 1:
                    try
                    {
                        if (nueva_area == "" || nueva_area == null)
                        {
                            ViewBag.Error = "No has ingresado un area";
                        }
                        else if (nueva_area == SQLConnection.consultaBusquedaArea(0, nueva_area))
                        {
                            ViewBag.Error = "Ya existe esa area en el sistema";
                        }
                        else
                        {
                            SQLConnection.altaArea(nueva_area);
                            ViewBag.Success = "Area registrada con éxito";
                        }
                    }
                    catch
                    {
                        ViewBag.Error = "El area no pudo ser añadida";
                    }
                    break;
            }
                if (baja)
                {
                    ViewBag.baja = true;
                }
                // Desplegar la lista de empleados
                SQLConnection Areas = new SQLConnection();
                return View(Areas.listaAreas(""));
            
        }
        [HttpPost]
        public ActionResult BusquedaAreas(int id_area, string nombre_area, bool baja)
        {
            ViewBag.RH = 1;
            ViewBag.baja = baja;

            if (nombre_area == null) { nombre_area = ""; }
            SQLConnection Areas = new SQLConnection();
            return View(Areas.listaAreas(nombre_area));
        }
        [HttpPost]
        public IActionResult BajasAreas(int[] bajaAreas, int confirmarBaja)
        {
            if (bajaAreas != null && bajaAreas.Length > 0)
            {
                if (confirmarBaja == 0)
                {
                    Datos Parametros = new Datos();
                    List<Datos> tips = new List<Datos>();
                    foreach (var item in bajaAreas)
                    {
                        Parametros.id_area = item;
                        tips.Add(new Datos()
                        {
                            id_area = item,
                            nombre_area = SQLConnection.consultaBusquedaArea(Parametros.id_area, "")
                        });
                    }
                    return View(tips);
                }
                else if (confirmarBaja == 1)
                {
                    try
                    {
                        foreach (var item in bajaAreas)
                        {
                            // Implementar lógica de baja en la base de datos
                            SQLConnection.bajaArea(item);
                        }
                        ViewBag.Success = "Baja completada con exito";
                        return RedirectToAction("BusquedaAreas", new { baja = false });
                    }
                    catch
                    {
                        ViewBag.Error = "Error al dar de baja";
                        return RedirectToAction("BusquedaAreas", new { baja = false });
                    }
                }
            }
            ViewBag.Error = "No se seleccionó ningún area";
            return RedirectToAction("BusquedaAreas", new { baja = true });
        }
        [HttpGet]
        public ActionResult BusquedaConceptos(bool baja, string nuevo_concepto,string descripcion,float sueldoefecto, int boton)
        {
            if (nuevo_concepto == null) { nuevo_concepto = ""; }
            ViewBag.RH = 1;


            switch (boton)
            {
                case 1:
                    try
                    {
                        if (nuevo_concepto == "" || nuevo_concepto == null)
                        {
                            ViewBag.Error = "Ingresa el nombre del concepto";
                        }
                        else if (nuevo_concepto == SQLConnection.consultaBusquedaConcepto(0, nuevo_concepto))
                        {
                            ViewBag.Error = "Ya existe ese concepto en el sistema";
                        }
                        else
                        {
                            SQLConnection.altaConcepto(nuevo_concepto,descripcion,sueldoefecto);
                            ViewBag.Success = "Concepto registrado con éxito";
                        }
                    }
                    catch
                    {
                        ViewBag.Error = "El concepto no pudo ser añadido";
                    }
                    break;
            }
            if (baja)
            {
                ViewBag.baja = true;
            }
            // Desplegar la lista de empleados
            SQLConnection Conceptos = new SQLConnection();
            return View(Conceptos.listaConceptos(""));

        }
        [HttpPost]
        public ActionResult BusquedaConceptos(int id_concepto, string nom_concepto, bool baja)
        {
            ViewBag.RH = 1;
            ViewBag.baja = baja;

            if (nom_concepto == null) { nom_concepto = ""; }
            SQLConnection Conceptos = new SQLConnection();
            return View(Conceptos.listaConceptos(nom_concepto));
        }
        [HttpPost]
        public IActionResult BajasConceptos(int[] bajaConceptos, int confirmarBaja)
        {
            if (bajaConceptos != null && bajaConceptos.Length > 0)
            {
                if (confirmarBaja == 0)
                {
                    Datos Parametros = new Datos();
                    List<Datos> tips = new List<Datos>();
                    foreach (var item in bajaConceptos)
                    {
                        Parametros.id_concepto = item;
                        tips.Add(new Datos()
                        {
                            id_concepto= item,
                            nom_concepto = SQLConnection.consultaBusquedaConcepto(Parametros.id_concepto, "")
                        });
                    }
                    return View(tips);
                }
                else if (confirmarBaja == 1)
                {
                    try
                    {
                        foreach (var item in bajaConceptos)
                        {
                            // Implementar lógica de baja en la base de datos
                            SQLConnection.bajaConcepto(item);
                            Console.WriteLine(item);
                        }
                        ViewBag.Success = "Baja completada con exito";
                        return RedirectToAction("BusquedaConceptos", new { baja = false });
                    }
                    catch
                    {
                        ViewBag.Error = "Error al dar de baja";
                        return RedirectToAction("BusquedaConceptos", new { baja = false });
                    }
                }
            }
            ViewBag.Error = "No se seleccionó ningún concepto";
            return RedirectToAction("BusquedaConceptos", new { baja = true });
        }
    }
}
