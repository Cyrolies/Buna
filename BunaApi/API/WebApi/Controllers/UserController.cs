using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DSDBLL;
using Common;
using DALEFModel;
using System.Text;

namespace Controllers
{
	/// <summary>
	/// All web administration functionality User/Roles/Permissions/Activity/UserNotifications
	/// </summary>
	/// <seealso cref="Controllers.BaseApiController" />
	[RoutePrefix("UserController")]
	public class UserController : BaseApiController
	{
		UserManager manager = new UserManager();

		#region "User"
		/// <summary>
		/// Gets all Users.
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns></returns>
		[Route("GetUsers")]
		[HttpPost]
		public HttpResponseMessage GetUsers([FromBody] GridParam filter)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				GridResult<User> users = manager.GetUserList(filter);
				response = Request.CreateResponse(HttpStatusCode.OK, users);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}
		/// <summary>
		/// GET api/User/5
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="includerelations">The includerelations.</param>
		/// <param name="includeDropDownData"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("GetUser/{id}/{includerelations}")]
		public HttpResponseMessage GetUser(string id, string includerelations)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				var user = manager.GetUser(Convert.ToInt32(id), Convert.ToBoolean(includerelations));
				if (user == null)
				{
					response = Request.CreateResponse(HttpStatusCode.NotFound);
				}
				response = Request.CreateResponse(HttpStatusCode.OK, user);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		/// <summary>
		/// GET api/User/5
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="includerelations">The includerelations.</param>
		/// <returns></returns>
		[HttpGet]
		[Route("Login/{name}/{includerelations}")]
		public HttpResponseMessage Login(string name, string includerelations)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				var user = manager.GetUser(name, System.Convert.ToBoolean(includerelations));
				if (user == null)
				{
					response = Request.CreateResponse(HttpStatusCode.NotFound);
				}
				response = Request.CreateResponse(HttpStatusCode.OK, user);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		/// <summary>
		/// Puts the specified User.
		/// </summary>
		/// <param name="user">The user.</param>
		/// <returns></returns>
		[Route("User/add")]
		[HttpPost]
		public HttpResponseMessage AddUser([FromBody] User user)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (user != null)
				{
					User newuser = manager.AddReturnUser(user);
					response = Request.CreateResponse<User>(HttpStatusCode.Created, newuser);

				}
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;

		}

		/// <summary>
		/// Updates the User.
		/// </summary>
		/// <param name="user">The user.</param>
		/// <returns></returns>
		
		[Route("User/update")]
		[HttpPost]
		public HttpResponseMessage UpdateUser([FromBody] User user)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (user != null)
				{
					int newuser = manager.UpdateUser(user);
					response = Request.CreateResponse(HttpStatusCode.Created, user.UserID.ToString());
				}
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "hello id =" + user.UserID.ToString() + ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;

		}

		/// <summary>
		/// Deletes the activity.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		[Route("User/delete/{id?}")]
		[HttpPost]
		public HttpResponseMessage DeleteUser(int id)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				int row = manager.DeleteUser(id);
				response = Request.CreateResponse(HttpStatusCode.OK);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		/// <summary>
		/// Reset user passwords.
		/// </summary>
		/// <param name="userIds">The new item list.</param>
		/// <returns></returns>
		[Route("ResetUsers")]
		[HttpPost]
		public HttpResponseMessage ResetUsers(string[] userIds)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (userIds != null)
				{
					PasswordResetList userNames = new PasswordResetList();
					Random rnd = new Random();
					string arbPwd = rnd.Next(10000,99999).ToString();
					userNames.defaultPwd = arbPwd;
					userNames.usersReset = new List<User>();
					foreach (string userId in userIds)
					{
						User user = manager.GetUser(Convert.ToInt32(userId), false);
						if (user != null)
						{
							userNames.usersReset.Add(user);
							user.UserPWDHash = Common.PasswordHash.CreateHash(arbPwd);
							user.ChangeDateTime = DateTime.Now;
							user.IsReset = true;
							int newId = manager.UpdateUser(user);
						}

					}
					response = Request.CreateResponse(HttpStatusCode.Created, userNames);
				}
				else
				{
					response = Request.CreateResponse(HttpStatusCode.NotFound, "User list is empty");
				}
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}
			return response;
		}

		#endregion

		#region "UserRole"
		/// <summary>
		/// Gets all UserRole.
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns></returns>
		[Route("UserRole")]
		[HttpPost]
		public HttpResponseMessage GetUserRoleList([FromBody] GridParam filter)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				GridResult<UserRole> activities = manager.GetUserRoleList(filter);
				response = Request.CreateResponse(HttpStatusCode.OK, activities);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}
		/// <summary>
		/// GET api/UserRole/5
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="includerelations">The includerelations.</param>
		/// <returns></returns>
		[Route("UserRole/{id?}/{includerelations?}")]
		public HttpResponseMessage GetUserRole(int id, string includerelations)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				var user = manager.GetUserRole(id, System.Convert.ToBoolean(includerelations));
				if (user == null)
				{
					response = Request.CreateResponse(HttpStatusCode.NotFound);
				}
				response = Request.CreateResponse(HttpStatusCode.OK, user);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		/// <summary>
		/// Puts the specified User.
		/// </summary>
		/// <param name="userrole">The userrole.</param>
		/// <returns></returns>
		[Route("UserRole/add")]
		[HttpPost]
		public HttpResponseMessage AddUserRole([FromBody] UserRole userrole)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (userrole != null)
				{
					UserRole newuser = manager.AddReturnUserRole(userrole);
					response = Request.CreateResponse<UserRole>(HttpStatusCode.Created, newuser);
				}
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;

		}


		/// <summary>
		/// Updates the User.
		/// </summary>
		/// <param name="userrole">The userrole.</param>
		/// <returns></returns>
		[Route("UserRole/update")]
		[HttpPost]
		public HttpResponseMessage UpdateUserRole([FromBody] UserRole userrole)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (userrole != null)
				{
					int newuser = manager.UpdateUserRole(userrole);
					response = Request.CreateResponse(HttpStatusCode.Created, userrole.UserRoleID.ToString());
				}
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "hello id =" + userrole.UserRoleID.ToString() + ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;

		}

		/// <summary>
		/// Deletes the activity.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		[Route("UserRole/delete/{id?}")]
		[HttpPost]
		public HttpResponseMessage DeleteUserRole(int id)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				int row = manager.DeleteUserRole(id);
				response = Request.CreateResponse(HttpStatusCode.OK);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}
		#endregion

		#region "UserRoleActivity"
		/// <summary>
		/// Gets all UserRoleActivity.
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns></returns>
		[Route("UserRoleActivity")]
		[HttpPost]
		public HttpResponseMessage GetUserRoleActivityList([FromBody] GridParam filter)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				GridResult<UserRoleActivity> result = new GridResult<UserRoleActivity>();
				result = manager.GetUserRoleActivityList(filter);
				response = Request.CreateResponse(HttpStatusCode.OK, result);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}
		/// <summary>
		/// GET api/UserRoleActivity/5
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="includerelations">The includerelations.</param>
		/// <returns></returns>
		[Route("UserRoleActivity/{id?}/{includerelations?}")]
		public HttpResponseMessage GetUserRoleActivity(int id, string includerelations)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				var user = manager.GetUserRoleActivity(id, System.Convert.ToBoolean(includerelations));
				if (user == null)
				{
					response = Request.CreateResponse(HttpStatusCode.NotFound);
				}
				response = Request.CreateResponse(HttpStatusCode.OK, user);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		/// <summary>
		/// Puts the specified User.
		/// </summary>
		/// <param name="userroleactivity">The userroleactivity.</param>
		/// <returns></returns>
		[Route("UserRoleActivity/add")]
		[HttpPost]
		public HttpResponseMessage AddUserRoleActivity([FromBody] UserRoleActivity userroleactivity)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (userroleactivity != null)
				{
					UserRoleActivity newuserroleactivity = manager.AddReturnUserRoleActivity(userroleactivity);
					response = Request.CreateResponse<UserRoleActivity>(HttpStatusCode.Created, newuserroleactivity);
				}
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;

		}

		/// <summary>
		/// Updates the User.
		/// </summary>
		/// <param name="newuserroleactivity">The newuserroleactivity.</param>
		/// <returns></returns>
		[Route("UserRoleActivity/update")]
		[HttpPost]
		public HttpResponseMessage UpdateUserRoleActivity([FromBody] UserRoleActivity newuserroleactivity)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (newuserroleactivity != null)
				{
					int newuser = manager.UpdateUserRoleActivity(newuserroleactivity);
					response = Request.CreateResponse(HttpStatusCode.Created, newuserroleactivity.UserRoleActivityID.ToString());
				}
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "hello id =" + newuserroleactivity.UserRoleActivityID.ToString() + ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;

		}

		/// <summary>
		/// Deletes the activity.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		[Route("UserRoleActivity/delete/{id?}")]
		[HttpPost]
		public HttpResponseMessage DeleteUserRoleActivity(int id)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				int row = manager.DeleteUserRoleActivity(id);
				response = Request.CreateResponse(HttpStatusCode.OK);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		#endregion

		#region "Activity"
		/// <summary>
		/// Gets all activities.
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns></returns>
		[Route("Activity")]
		[HttpPost]
		public HttpResponseMessage GetActivityList([FromBody] GridParam filter)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				GridResult<Activity> result = new GridResult<Activity>();
				result = manager.GetActivityList(filter);
				response = Request.CreateResponse(HttpStatusCode.OK, result);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		/// <summary>
		/// GET api/activity/5
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="includerelations">The includerelation.</param>
		/// <param name="includedata">The includedata this includes all drop down lists for the foreign keys for editing.</param>
		/// <returns></returns>
		[Route("Activity/{id?}/{includerelations?}")]
		public HttpResponseMessage GetActivity(int id, string includerelations)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				var activity = manager.GetActivity(id, System.Convert.ToBoolean(includerelations));
				if (activity == null)
				{
					response = Request.CreateResponse(HttpStatusCode.NotFound);
				}
				response = Request.CreateResponse(HttpStatusCode.OK, activity);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		/// <summary>
		/// Puts the specified activity.
		/// </summary>
		/// <param name = "activity" > The activity.</param>
		/// <returns></returns>
		[Route("Activity/add")]
		[HttpPost]
		public HttpResponseMessage AddActivity([FromBody] Activity activity)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (activity != null)
				{
					Activity newactivity = manager.AddReturnActivity(activity);
					response = Request.CreateResponse<Activity>(HttpStatusCode.Created, newactivity);
				}
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}
			return response;

		}


		/// <summary>
		/// Updates the activity.
		/// </summary>
		/// <param name="activity">The activity.</param>
		/// <returns></returns>
		[Route("Activity/update")]
		[HttpPost]
		public HttpResponseMessage UpdateActivity([FromBody] Activity activity)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (activity != null)
				{
					int newactivity = manager.UpdateActivity(activity);
					response = Request.CreateResponse(HttpStatusCode.Created, activity.ActivityID.ToString());
				}
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}
			return response;

		}

		/// <summary>
		/// Deletes the activity.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		[Route("Activity/delete/{id?}")]
		[HttpPost]
		public HttpResponseMessage DeleteActivity(int id)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				int row = manager.DeleteActivity(id);
				response = Request.CreateResponse(HttpStatusCode.OK);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		#endregion

	}
}
