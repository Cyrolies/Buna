using System.ComponentModel.Composition;
using System.Configuration;
//using System.Data.Objects;
using System.Data.Entity;
using System;
using DALBase;
using Common;

namespace DALEFBase
{
    /// <summary>
    /// This factory class instantiates a unit of work object with the object context selected
    /// </summary>
    [Export(typeof(IDALFactory))]
    public class DALFactory : IDALFactory 
    {
        private IUnitOfWork uow;
        

        #region IDALFactory Members

        /// <summary>
        /// Initializes a new instance of the <see cref="DALFactory"/> class.
        /// </summary>
        /// <param name="dataModal">The data modal.</param>
        public DALFactory(Enumerations.ModalContext dataModal)
        {
            DbContext datacontext = null;
            switch (dataModal)
            {
                //Robin you can have more than one database here to point to
                case Enumerations.ModalContext.EzFloManagerEntities:
                    {
                        datacontext = new DALModelContext("name=EzFloManagerEntities");
                        break;
                    }
               case Enumerations.ModalContext.EzFloDSDEntities:
                    {
                        datacontext = new DALModelContext("name=EzFloDSDEntities");
                        break;
                    }
               case Enumerations.ModalContext.EzFloOTIEntities:
                    {
                        datacontext = new DALModelContext("name=EzFloOTIEntities");
                        break;
                    }
               case Enumerations.ModalContext.EzFloAglEntities:
                    {
                        datacontext = new DALModelContext("name=EzFloAGLEntities");
                        break;
                    }   
                default:
                    {
                        break;
                    }
            }

            this.CurrentUoW = new UnitOfWork(datacontext);
        }
       
        /// <summary>
        /// Gets the current uo W.
        /// </summary>
        /// <value>
        /// The current uo W.
        /// </value>
        public IUnitOfWork CurrentUoW
        {
            get
            {
                if (uow != null)
                {
                    return uow;
                }
                else
                {
                    throw new Exception("The data model is null");
                }

               
            }

            set
            {
                uow = value;
            }
        }

        #endregion

        
    }
}
