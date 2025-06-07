using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema_Nomina.Models
{
    public class Validaciones
    {
        public String GenerarCaptcha()
        {
            string Captcha = "";
            for (int i = 0; i <= 2; i++)
            {
                var guid = Guid.NewGuid();
                var justNumbers = new String(guid.ToString().Where(Char.IsDigit).ToArray());
                var seed = int.Parse(justNumbers.Substring(0, 4));
                var random = new Random(seed);
                var value = random.Next(0, 9);
                Captcha = Captcha + value.ToString();
                int numero = random.Next(26);
                char letra = (char)(((int)'a') + numero);
                Captcha = Captcha + letra;
            }
            return Captcha;
        }

        static public int Edad(DateTime FechaNacimiento)
        {
            DateTime FechaActual = DateTime.Now;
            int edad = FechaActual.Year - FechaNacimiento.Year;
            if ((FechaActual.Month < FechaNacimiento.Month) || ((FechaActual.Month == FechaNacimiento.Month) && FechaActual.Day < FechaNacimiento.Day))
            {
                edad = edad - 1;
            }
            return edad;
        }
    }
}
