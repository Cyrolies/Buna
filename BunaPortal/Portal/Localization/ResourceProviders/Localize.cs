using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BunaPortal
{
    public class Localizer
    {
        private static ILocalizationResourceProvider _instance;
        private static Func<ILocalizationResourceProvider> _initializer;
        private static Func<string> _cultureNameResolver;
        private static object _aLock;

        public static ILocalizationResourceProvider Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (_aLock)
                    {
                        if (_instance == null)
                        {
                            if (_initializer == null)
                            {
                                throw new ArgumentNullException("_initializer");
                            }
                            _instance = _initializer.Invoke();
                        }
                    }
                }
                return _instance;
            }
        }

        public static string CultureName
        {
            get
            {
                return _cultureNameResolver.Invoke();
            }
        }

        static Localizer()
        {
            _aLock = new object();
        }

        private Localizer()
        {
        }

        internal static void RegisterCultureNameResolver(Func<string> cultureNameResolver)
        {
            _cultureNameResolver = cultureNameResolver;
        }

        internal static void RegisterResourceProvider(Func<ILocalizationResourceProvider> initializer)
        {
            _initializer = initializer;
        }
    }
}
