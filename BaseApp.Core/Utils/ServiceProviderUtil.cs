using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApp.Core.Utils
{

    public class ServiceProviderUtil
    {
        private static IServiceProvider? ServiceProvider;

        public static void SetServiceProvider(IServiceProvider ServiceProvider) {
            if (ServiceProvider == null) return;
            ServiceProviderUtil.ServiceProvider = ServiceProvider;
        }

        public static T? GetService<T>() where T : class
        {
            if (ServiceProvider == null) return null;
            return ServiceProvider.GetService(typeof(T)) as T;
        }

        public static T GetRequiredService<T>() where T : class
        {
            if (ServiceProvider == null) throw new NullReferenceException();
            return ServiceProvider.GetRequiredService<T>();
        }
    }
}
