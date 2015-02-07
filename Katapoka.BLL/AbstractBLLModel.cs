using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects.DataClasses;

namespace Katapoka.BLL
{
    public abstract class AbstractBLLModel<TEntityObject> : AbstractBLLPersistence<Katapoka.DAO.Entities, TEntityObject>
        where TEntityObject : EntityObject, new()
    {
    }
}
