using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using DALEFModel;
using Common;

namespace DSDBLL
{

	//public sealed class CheckListManager : BaseManager,IDisposable
	//{

	//	public CheckListManager()
	//	{
 //           base.Model = Enumerations.ModalContext.EzFloDSDEntities;
	//	}

 //       #region viewInCheckList

 //       public IQueryable<viewInCheckList> GetExceptionCheckList(bool includeRelations, Expression<Func<viewInCheckList, bool>> where = null, Dictionary<string, string> listOrderBy = null, int page = 0, int size = 0)
 //       {
 //           try
 //           {

 //               IQueryable<viewInCheckList> list = null;
 //               if (includeRelations)// Add includepaths into method 
 //               {

 //               }
 //               else
 //               {
 //                   if (where != null)
 //                   {
 //                       list = Repository.GetList<viewInCheckList>(where).Where(m => m.Passed == false).OrderBy(m => m.VehicleNumber).ThenBy(m => m.ShipmentNumber);
 //                   }
 //                   else
 //                   {
 //                       list = Repository.GetList<viewInCheckList>().Where(m => m.Passed == false).OrderBy(m => m.VehicleNumber).ThenBy(m => m.ShipmentNumber);
 //                   }
 //               }
 //               if (shippingPointID > 0)
 //               {
 //                   list = list.Where(o => o.ShippingPoint == shippingPoint.ToString());
 //               }
 //               //APPLY ALL SORTING
 //               if (listOrderBy != null && listOrderBy.Count() > 0)
 //               {
 //                   foreach (var sort in listOrderBy)
 //                   {
 //                       list = list.OrderBy(sort.Key, sort.Value);
 //                   }
 //               }

 //               //APPLY PAGE SIZE
 //               if (page > 0 && size > 0)
 //               {
 //                   list = list.Skip((page - 1) * size).Take(size);
 //               }
 //               return list;

 //           }
 //           catch (Exception ex)
 //           {
 //               throw ex;
 //           }
 //       }

 //       public IQueryable<viewInCheckList> GetViewInCheckList(bool includeRelations, Expression<Func<viewInCheckList, bool>> where = null, Dictionary<string, string> listOrderBy = null, int page = 0, int size = 0)
 //       {
 //           try
 //           {

 //               IQueryable<viewInCheckList> list = null;
 //               if (includeRelations)// Add includepaths into method 
 //               {

 //               }
 //               else
 //               {
 //                   if (where != null)
 //                   {
 //                       list = Repository.GetList<viewInCheckList>(where).OrderBy(m => m.VehicleNumber).ThenBy(m => m.ShipmentNumber);
 //                   }
 //                   else
 //                   {
 //                       list = Repository.GetList<viewInCheckList>().OrderBy(m => m.VehicleNumber).ThenBy(m => m.ShipmentNumber);
 //                   }
 //               }
 //               if (shippingPointID > 0)
 //               {
 //                   list = list.Where(o => o.ShippingPoint == shippingPoint.ToString());
 //               }
 //               //APPLY ALL SORTING
 //               if (listOrderBy != null && listOrderBy.Count() > 0)
 //               {
 //                   foreach (var sort in listOrderBy)
 //                   {
 //                       list = list.OrderBy(sort.Key, sort.Value);
 //                   }
 //               }

 //               //APPLY PAGE SIZE
 //               if (page > 0 && size > 0)
 //               {
 //                   list = list.Skip((page - 1) * size).Take(size);
 //               }
 //               return list;

 //           }
 //           catch (Exception ex)
 //           {
 //               throw ex;
 //           }
 //       }

 //       #endregion

 //       #region InCheckList Methods

 //       public InCheckList GetInCheckList(int id,bool includeRelations)
	//	{
	//		try
	//		{
	//			if (includeRelations)
	//			{

	//				//Add Includes here example below
	//				//List<Expression<Func<StpData, object>>> includepaths = new List<Expression<Func<StpData, object>>>();
	//				//includepaths.Add(p => p.StcData);

	//				// Add includepaths into method here if used and not null
	//				return Repository.Get<InCheckList>(null, p => p.Id == id);

	//			}
	//			else
	//			{

	//				return Repository.Get<InCheckList>(null,p => p.Id == id);

	//			}

	//		}catch(Exception ex)
	//		{
	//			throw ex;
	//		}
	//	}

	//	public IQueryable<InCheckList> GetInCheckList(bool includeRelations, Expression<Func<InCheckList, bool>> where = null, Dictionary<string, string> listOrderBy = null, int page = 0, int size = 0)
	//	{
	//		try
	//		{

	//			IQueryable<InCheckList> list = null;
	//			if (includeRelations)// Add includepaths into method 
	//			{

	//				//Add Includes here example below
	//				//STARTCUSTOMCODE
	//				//List<Expression<Func<StpData, object>>> includepaths = new List<Expression<Func<StpData, object>>>();
	//				//includepaths.Add(p => p.StpDataType);

	//				//APPLY FILTERS
	//				//if (where != null)
	//				//{
	//				//list = Repository.GetList<InCheckList, string>(includepaths, where);
	//				//}
	//				//else
	//				//{
	//				//list = Repository.GetList<InCheckList, string>(includepaths);
	//			//}
	//		}
	//		else
	//		{
	//			if (where != null)
	//			{
 //                   list = Repository.GetList<InCheckList>(where).OrderByDescending(m => m.CreateDateTime).OrderByDescending(m => m.ChangeDateTime);
	//			}
	//			else
	//			{
 //                   list = Repository.GetList<InCheckList>().OrderByDescending(m => m.CreateDateTime).OrderByDescending(m => m.ChangeDateTime);
	//			}
	//		}

	//		//APPLY ALL SORTING
	//		if (listOrderBy != null && listOrderBy.Count() > 0)
	//		{
 //               foreach (var sort in listOrderBy)
 //               {
 //                   list = list.OrderBy(sort.Key, sort.Value);
 //               }
	//		}

	//		//APPLY PAGE SIZE
	//		if (page > 0 && size > 0)
	//		{
	//			list = list.Skip((page - 1) * size).Take(size);
	//		}
	//		return list;

	//		}catch(Exception ex)
	//		{
	//			throw ex;
	//		}
	//	}
	//	public IQueryable<InCheckList> GetInCheckList(bool includeRelations)
	//	{

	//		try
	//		{
	//			if (includeRelations)
	//			{

	//				//Add Includes here example below
	//				//STARTCUSTOMCODE
	//				//List<Expression<Func<StpData, object>>> includepaths = new List<Expression<Func<StpData, object>>>();
	//				//includepaths.Add(p => p.StpDataType);

	//				// Add includepaths into method here if used and not null
	//				return Repository.GetList<InCheckList>();
	//				//ENDCUSTOMCODE

	//			}
	//			else
	//			{

	//				return Repository.GetList<InCheckList>();

	//			}

	//		}catch(Exception ex)
	//		{
	//			throw ex;
	//		}
	//	}

	//	public int AddInCheckList(InCheckList newEntity)
	//	{
	//		try
	//		{
	//			newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
	//			return Repository.UoW.Add<InCheckList>(newEntity);

	//		}catch(Exception ex)
	//		{
	//			throw ex;
	//		}
	//	}

	//	public int UpdateInCheckList(InCheckList editEntity)
	//	{
	//		try
	//		{
	//			editEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Update, editEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
	//			return Repository.UoW.Update<InCheckList>(editEntity);

	//		}catch(Exception ex)
	//		{
	//			throw ex;
	//		}
	//	}

	//	public int DeleteInCheckList(InCheckList deleteEntity)
	//	{
	//		try
	//		{
	//			 deleteEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Delete, deleteEntity.setToPendingUsable);//Sets to Pending Deletion if supervision On else Approved if Off
	//			return Repository.UoW.Delete<InCheckList>(deleteEntity);

	//		}catch(Exception ex)
	//		{
	//			throw ex;
	//		}
	//	}
	//	#endregion

	//	#region Dispose
	//	public void Dispose()
	//	{
	//	}
	//	#endregion

 //   }
}
