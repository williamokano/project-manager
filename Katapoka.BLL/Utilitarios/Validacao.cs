using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Katapoka.BLL.Utilitarios
{
    public abstract class Validacao
    {
        public static bool IsValidCnpj(string cnpj)
        {
            string tempCnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (tempCnpj.Length != 14)
                return false;
            int[] fatorMultiplicadorDigito1 = new int[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] fatorMultiplicadorDigito2 = new int[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            string dv = string.Empty;
            int iTemp = 0;
            //Find first DV
            for (int i = 0; i < fatorMultiplicadorDigito1.Length; i++)
                iTemp += Int32.Parse(tempCnpj[i].ToString()) * fatorMultiplicadorDigito1[i];
            if (iTemp % 11 < 2)
                dv = "0";
            else
                dv = (11 - iTemp % 11).ToString();

            //Find second DV
            iTemp = 0;
            for (int i = 0; i < fatorMultiplicadorDigito2.Length; i++)
                iTemp += Int32.Parse(tempCnpj[i].ToString()) * fatorMultiplicadorDigito2[i];
            if (iTemp % 11 < 2)
                dv = dv + "0";
            else
                dv = dv + (11 - iTemp % 11).ToString();
            return tempCnpj.Substring(12, 2) == dv;
        }
        public static bool IsValidCpf(string cpf)
        {
            string tempCpf = cpf.Replace(".", "").Replace("-", "");
            if (tempCpf.Length != 11)
                return false;

            string dv = string.Empty;
            int[] ftMt1 = new int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2};
            int[] ftMt2 = new int[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int iTemp = 0;
            //Calculate first DV
            for (int i = 0; i < ftMt1.Length; i++)
                iTemp += Int32.Parse(tempCpf[i].ToString()) * ftMt1[i];
            if (iTemp % 11 < 2)
                dv = "0";
            else
                dv = (11 - iTemp % 11).ToString();

            //Calculate second DV
            iTemp = 0;
            for (int i = 0; i < ftMt2.Length; i++)
                iTemp += Int32.Parse(tempCpf[i].ToString()) * ftMt2[i];
            if (iTemp % 11 < 2)
                dv = dv + "0";
            else
                dv = dv + (11 - iTemp % 11).ToString();

            return tempCpf.Substring(9, 2) == dv;
        }

        // TODO Melhorar o método de validaçao de URL, está dando OK para http://www.facebook, não deveria.
        public static bool IsValidUrl(string url)
        {
            Regex regexUrl = new Regex("^((https?|ftp)://|(www|ftp)\\.)[a-z0-9-]+(\\.[a-z0-9-]+)+([/?].*)?$");
            return regexUrl.IsMatch(url.Trim());
        }
        public static bool IsValidEmail(string email)
        {
            Regex regexEmail = new Regex("^(([^<>()[\\]\\\\.,;:\\s@\\\"]+(\\.[^<>()[\\]\\\\.,;:\\s@\\\"]+)*)|(\\\".+\\\"))@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\])|(([a-zA-Z\\-0-9]+\\.)+[a-zA-Z]{2,}))$");
            return regexEmail.IsMatch(email.Trim());
        }
    }
}