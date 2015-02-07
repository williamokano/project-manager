using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Katapoka.BLL.Utilitarios
{
    public class Utilitario
    {

        public static TimeSpan ConvertStringToTime(string time)
        {

            // TODO : Criar um método que a partir de uma string ele retorne um TimeSpan corretamente preenchido.
            Regex regexHora = new Regex("(\\d+):([0-5]\\d)");
            if (regexHora.IsMatch(time))
            {
                Match match = regexHora.Match(time);
                int horas = Convert.ToInt32(match.Groups[1].Value);
                int minutos = Convert.ToInt32(string.Format("{0:00}", match.Groups[2].Value));
                int segundos = 0;
                TimeSpan ts = new TimeSpan(0, horas, minutos, segundos, 0);
                return ts;
            }
            throw new Exception("Não é um formato de hora válido.");
        }

        public static Decimal ConvertTimeStringToDecimal(string time)
        {
            return (Decimal)ConvertStringToTime(time).TotalHours;
        }
    }
}
