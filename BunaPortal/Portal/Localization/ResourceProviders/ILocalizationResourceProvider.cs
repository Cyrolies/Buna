using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BunaPortal
{
    public interface ILocalizationResourceProvider
    {
        string GetString(string cultureName, string key);

        string GetString(string key);

        IHtmlString GetHtmlString(string cultureName, string key);

        IHtmlString GetHtmlString(string key);
    }
}
