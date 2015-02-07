using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace Katapoka.BLL
{
    public abstract class AbstractBLLPersistence<TObjectContext, TEntityObject> : AbstractBLLContext<TObjectContext>
        where TObjectContext : ObjectContext, new()
        where TEntityObject : EntityObject, IEntityWithKey, new()
    {

        /// <summary>
        /// Default constructor which is required to the AbstractBLLContext
        /// </summary>
        public AbstractBLLPersistence() { }
        /// <summary>
        /// Recovery an object by their integer primary key
        /// </summary>
        /// <param name="key">the integer primary key</param>
        /// <returns>An TEntityObject</returns>
        public TEntityObject GetById(int key)
        {
            try
            {
                var set = this.Context.CreateObjectSet<TEntityObject>().EntitySet;
                var pk = set.ElementType.KeyMembers[0];
                System.Data.EntityKey entityKey = new System.Data.EntityKey(set.EntityContainer.Name + "." + set.Name, pk.Name, key);
                return (TEntityObject)this.Context.GetObjectByKey(entityKey);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Recovery an object by their string primary key
        /// </summary>
        /// <param name="key">the string primary key</param>
        /// <returns>An TEntityObject</returns>
        public TEntityObject GetById(string key)
        {
            try
            {
                var set = this.Context.CreateObjectSet<TEntityObject>().EntitySet;
                var pk = set.ElementType.KeyMembers[0];
                System.Data.EntityKey entityKey = new System.Data.EntityKey(set.EntityContainer.Name + "." + set.Name, pk.Name, key);
                return (TEntityObject)this.Context.GetObjectByKey(entityKey);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Save the object changes
        /// </summary>
        /// <param name="pEntity"></param>
        public virtual void Save(TEntityObject pEntity)
        {
            Save(pEntity, null);
        }
        /// <summary>
        /// Save the object changes and set if it's still active
        /// </summary>
        /// <param name="pEntity"></param>
        /// <param name="flagActive"></param>
        public void Save(TEntityObject pEntity, bool? flagActive)
        {
            switch (pEntity.EntityState)
            {
                case System.Data.EntityState.Detached:
                case System.Data.EntityState.Added:
                    this.Add(pEntity, flagActive);
                    break;
                case System.Data.EntityState.Modified:
                    this.Update(pEntity, flagActive);
                    break;
                case System.Data.EntityState.Deleted:
                    this.Delete(pEntity);
                    break;
                case System.Data.EntityState.Unchanged:
                    if (this.ControlsTransaction)
                        this.Update(pEntity);
                    break;
            }
        }
        public virtual void Add(TEntityObject pEntity)
        {
            Add(pEntity, null);
        }
        public virtual void Add(TEntityObject pEntity, bool? flagAtivo)
        {
            Context.AddObject(pEntity.GetType().Name, pEntity);
            if (ControlsTransaction && AutoSaveChanges)
                Context.SaveChanges();
        }
        public virtual void Update(TEntityObject pEntity)
        {
            Update(pEntity, null);
        }
        public virtual void Update(TEntityObject pEntity, bool? flagAtivo)
        {
            Context.ApplyCurrentValues<TEntityObject>(pEntity.GetType().Name, pEntity);
            if (ControlsTransaction && AutoSaveChanges)
                Context.SaveChanges();
        }
        public virtual void TryDelete(TEntityObject pEntity)
        {
            if (!this.ControlsTransaction)
                throw new Exception("This method should used only by the BLL that controls the transaction.");
            try
            {
                Context.DeleteObject(pEntity);
                if (ControlsTransaction && AutoSaveChanges)
                    Context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public virtual void Delete(TEntityObject pEntity)
        {
            Context.DeleteObject(pEntity);
            if (ControlsTransaction && AutoSaveChanges)
                Context.SaveChanges();
        }
        public virtual void DeleteRange(IList<TEntityObject> listEntity)
        {
            bool autoSaveChangesTemp = AutoSaveChanges;
            AutoSaveChanges = false;

            foreach (TEntityObject entity in listEntity)
                this.Delete(entity);

            AutoSaveChanges = autoSaveChangesTemp;

            if (ControlsTransaction && AutoSaveChanges)
                Context.SaveChanges();
        }
        public virtual void SaveRange(IList<TEntityObject> listEntity)
        {
            bool tempAutoSaveChanges = AutoSaveChanges;
            AutoSaveChanges = false;

            foreach (TEntityObject entity in listEntity)
                this.Save(entity);

            AutoSaveChanges = tempAutoSaveChanges;

            if (ControlsTransaction && AutoSaveChanges)
                Context.SaveChanges();
        }
        public virtual TEntityObject Detach(TEntityObject pEntity)
        {
            Context.ContextOptions.LazyLoadingEnabled = false;
            Context.Detach(pEntity);
            return pEntity;
        }
        public virtual IList<TEntityObject> DetachRange(IList<TEntityObject> listEntity)
        {
            foreach (TEntityObject entity in listEntity)
                Context.Detach(entity);
            return listEntity;
        }
    }
}
