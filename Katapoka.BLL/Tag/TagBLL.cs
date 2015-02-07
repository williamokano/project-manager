using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Katapoka.BLL.Tag
{
    public class TagBLL : AbstractBLLModel<Katapoka.DAO.Tag_Tb>
    {
        public IList<Katapoka.DAO.Tag_Tb> GetAll()
        {
            return this.Context.Tag_Tb.OrderBy(p => p.DsTag).ToList();
        }
        public IList<Katapoka.DAO.Tag_Tb> GetTagsByName(string name)
        {
            return this.Context.Tag_Tb
                .Where(p => p.DsTag.Trim().ToLower().StartsWith(name.Trim().ToLower()))
                .ToList();
        }
    }
}
