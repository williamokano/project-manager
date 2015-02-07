using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace Katapoka.BLL
{
    public abstract class AbstractBLLContext<TObjectContext> : IDisposable
        where TObjectContext : ObjectContext, new()
    {
        private bool controlsTransaction = true;
        protected bool AutoSaveChanges { get; set; }
        private TObjectContext context;
        private IList<AbstractBLLContext<TObjectContext>> dependetsBLL;
        protected AbstractBLLContext<TObjectContext> ParentBLL { get; set; }
        private bool disposed = false;

        /// <summary>
        /// Return the entity framework context object
        /// </summary>
        protected TObjectContext Context
        {
            get
            {
                if (disposed)
                    throw new ObjectDisposedException(this.GetType().FullName);
                return context;
            }
            set
            {
                if (disposed)
                    throw new ObjectDisposedException(this.GetType().FullName);
                controlsTransaction = false;
                dependetsBLL = null;
                this.context = value;
            }
        }

        protected bool ControlsTransaction
        {
            get
            {
                if (disposed)
                    throw new ObjectDisposedException(this.GetType().FullName);
                return controlsTransaction;
            }
        }

        /// <summary>
        /// Just the constructor
        /// </summary>
        public AbstractBLLContext()
        {
            this.context = new TObjectContext();
            controlsTransaction = true;
            AutoSaveChanges = true;
            dependetsBLL = new List<AbstractBLLContext<TObjectContext>>();
        }

        protected void ReciclarContext()
        {
            if (disposed)
                throw new ObjectDisposedException(this.GetType().FullName);
            if (controlsTransaction)
            {
                this.context = new TObjectContext();
                for (int i = 0; i < dependetsBLL.Count; i++)
                    dependetsBLL[i].Context = this.context;
            }
        }

        /// <summary>
        /// Create an BLL with the same context with this BLL
        /// This should be done to a correct transaction control
        /// </summary>
        /// <typeparam name="T">The new BLL type</typeparam>
        /// <returns>The new BLL with the same context</returns>
        public T CriarObjetoNoMesmoContexto<T>()
            where T : AbstractBLLContext<TObjectContext>, new()
        {
            if (disposed)
                throw new ObjectDisposedException(this.GetType().FullName);
            IList<AbstractBLLContext<TObjectContext>> bllDependentes = null;
            T obj = null;
            if (this.dependetsBLL != null)
                bllDependentes = this.dependetsBLL;
            else
                bllDependentes = this.ParentBLL.dependetsBLL;

            obj = (T)bllDependentes.Where(p => p.GetType() == typeof(T)).FirstOrDefault();

            if (obj == null)
            {
                obj = new T();
                obj.Context = this.Context;
                obj.ParentBLL = this;
                obj.dependetsBLL = null;
                bllDependentes.Add(obj);
            }
            return obj;
        }

        /// Implement IDisposable.
        /// Do not make this method virtual.
        /// A derived class should not be able to override this method.
        public void Dispose()
        {
            Dispose(true);
            /// This object will be cleaned up by the Dispose method.
            /// Therefore, you should call GC.SupressFinalize to
            /// take this object off the finalization queue
            /// and prevent finalization code for this object
            /// from executing a second time.
            GC.SuppressFinalize(this);
        }

        /// Dispose(bool disposing) executes in two distinct scenarios.
        /// If disposing equals true, the method has been called directly
        /// or indirectly by a user's code. Managed and unmanaged resources
        /// can be disposed.
        /// If disposing equals false, the method has been called by the
        /// runtime from inside the finalizer and you should not reference
        /// other objects. Only unmanaged resources can be disposed.
        protected virtual void Dispose(bool disposing)
        {
            /// Check to see if Dispose has already been called.
            if(!this.disposed)
            {
                /// If disposing equals true, dispose all managed
                /// and unmanaged resources.
                if(disposing)
                {
                    if(this.ParentBLL != null)
                        this.ParentBLL.Dispose();
                    this.ParentBLL = null;
                    if(this.dependetsBLL != null)
                        for(int i=0; i<this.dependetsBLL.Count; i++)
                            this.dependetsBLL[i].ParentBLL = null;
                    this.dependetsBLL = null;

                    /// Dispose managed resources.
                    this.context.Dispose();
                }

                /// Note disposing has been done.
                disposed = true;
            }
        }

        // Use C# destructor syntax for finalization code.
        // This destructor will run only if the Dispose method
        // does not get called.
        // It gives your base class the opportunity to finalize.
        // Do not provide destructors in types derived from this class.
        ~AbstractBLLContext()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }

    }
}
