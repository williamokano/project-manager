using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Katapoka.BLL
{
    public class Constantes
    {
        private const string TEMPO_VALIDADE_LOGIN = "TempoValidadeLogin";
        private const string NOME_SITE = "NomeSite";
        
        public static int TempoValidadeLogin
        {
            get
            {
                try
                {
                    return (int)(new System.Configuration.AppSettingsReader().GetValue(TEMPO_VALIDADE_LOGIN, typeof(int)));
                }
                catch
                {
                    return 60;
                }
            }
        }

        public static string NomeSite
        {
            get
            {
                try
                {
                    return (string)(new System.Configuration.AppSettingsReader().GetValue(NOME_SITE, typeof(string)));
                }
                catch
                {
                    return "Quântica Networks Project Manager";
                }
            }
        }

    }
}
