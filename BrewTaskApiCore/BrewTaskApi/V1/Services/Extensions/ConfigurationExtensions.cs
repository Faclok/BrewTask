using System.Reflection;

namespace BrewTaskApi.V1.Services.Extensions
{

    /// <summary>
    /// config projects
    /// </summary>
    public static class ConfigurationExtensions
    {

        /// <summary>
        /// Add service business logic
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddBusinessServicesV1(this IServiceCollection services)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                foreach (var item in assembly.GetTypesWithHelpAttribute())
                    services.AddTransient(item);

            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetTypesWithHelpAttribute(this Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
                if (type.GetCustomAttributes(typeof(BusinessServiceAttribute), true).Length > 0)
                    yield return type;
        }
    }
}
