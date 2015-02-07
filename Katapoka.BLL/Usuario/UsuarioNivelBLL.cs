using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Katapoka.BLL.Usuario
{
    public class UsuarioNivelBLL : AbstractBLLModel<Katapoka.DAO.UsuarioNivel_Tb>
    {
        public IList<Katapoka.DAO.UsuarioNivel_Tb> GetAll()
        {
            return this.Context.UsuarioNivel_Tb
                .OrderBy(p => p.DsNivel)
                .ToList();
        }
    }
}
