using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common
{
    public static class SessionHandler
    {
        private static void SetSession<T>(string sessionId, T value)
        {
            HttpContext.Current.Session[sessionId] = value;
        }

        private static T GetSession<T>(string sessionId)
        {
            T val = default(T);
            var session = HttpContext.Current.Session;

            if (session[sessionId] != null)
            {
                val = (T)session[sessionId];
            }

            return val;
        }

        public static int? UserId
        {
            get
            {
                return GetSession<int>("UserId");
            }
            set
            {
                SetSession<int?>("UserId", value);
            }
        }
    }
}
