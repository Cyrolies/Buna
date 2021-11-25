using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using DALEFModel;
using Common;

namespace DSDBLL
{

	public sealed class AuditManager : BaseManager,IDisposable
	{

		public AuditManager()
		{
            base.Model = Enumerations.ModalContext.EzFloManagerEntities;
		}


		#region Audit Methods

		public Audit GetAudit(int id,bool includeRelations)
		{
			try
			{
				if (includeRelations)
				{

					//Add Includes here example below
					//List<Expression<Func<StpData, object>>> includepaths = new List<Expression<Func<StpData, object>>>();
					//includepaths.Add(p => p.StcData);

					// Add includepaths into method here if used and not null
					return Repository.Get<Audit>(null, p => p.AuditID == id);

				}
				else
				{

					return Repository.Get<Audit>(null,p => p.AuditID == id);

				}

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public IQueryable<Audit> GetAudit(bool includeRelations)
		{

			try
			{
				if (includeRelations)
				{

					//Add Includes here example below
					//STARTCUSTOMCODE
					//List<Expression<Func<StpData, object>>> includepaths = new List<Expression<Func<StpData, object>>>();
					//includepaths.Add(p => p.StpDataType);

					// Add includepaths into method here if used and not null
					return Repository.GetList<Audit>();
					//ENDCUSTOMCODE

				}
				else
				{

					return Repository.GetList<Audit>();

				}

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int AddAudit(Audit newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Add<Audit>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int UpdateAudit(Audit editEntity)
		{
			try
			{
				editEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Update, editEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Update<Audit>(editEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int DeleteAudit(Audit deleteEntity)
		{
			try
			{
				 deleteEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Delete, deleteEntity.setToPendingUsable);//Sets to Pending Deletion if supervision On else Approved if Off
				return Repository.UoW.Delete<Audit>(deleteEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}
		#endregion

		#region Dispose
		public void Dispose()
		{
		}
		#endregion
	}
}
