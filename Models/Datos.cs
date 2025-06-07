namespace Sistema_Nomina.Models
{
    public class Datos
    {
        //Area
        public int id_area { get; set; }
        public string nombre_area { get; set; }

        //Prestaciones
        public int id_prestacion { get; set; }
        public string nombre_prest { get; set; }
        public float sueldo_extra { get; set; }

        //Clases
        public int id_clase { get; set; }
        public string nombre_clase { get; set; }

        //Asignación prestaciones

        //Conceptos
        public int id_concepto { get; set; }
        public string nom_concepto { get; set; }
        public string descripcion { get; set; }
        public float sueldoefecto { get; set; }

        //Empleados
        public int cve_emp { get; set; }
        public string nom_emp { get; set; }
        public string app_emp { get; set; }
        public string apm_emp { get; set; }
        public string nombre { get; set; }
        public string contraseña { get; set; }
        public DateTime fech_nac { get; set; }
        public int edad_pers { get; set; }
        public bool habilitado { get; set; }
        public bool estado { get; set; }

        //Asignación Conceptos
        public DateOnly Fecha_C { get; set; }
        public string motivo {  get; set; }

        //Histórico Nomina
        public int cve_nomina { get; set; }
        public DateOnly Fecha_subida { get; set; }
        public float Sueldo_calc { get; set; }

        //Otros
        public string captcha { get; set; }
        public string captchaGenerado { get; set; }
        public int numeroLista { get; set; }
        public int[,] bajaEmpleados {  get; set; }
        public int CantidadEmpleados { get; set; }
        public int CantidadEmpleadosConContrato { get; set; }
        public int CantidadEmpleadosSinContrato { get; set; }
        //public int prueba { get; set; }
    }
}
