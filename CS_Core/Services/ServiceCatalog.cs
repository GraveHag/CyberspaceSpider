using System.Collections.Concurrent;
using System.Reflection;

namespace CS_Core
{
    /// <summary>
    /// DI container
    /// </summary>
    public sealed class ServiceCatalog
    {
        static readonly ConcurrentDictionary<Type, object> Services = new ConcurrentDictionary<Type, object>();

        public static void RegisterService<Intefrace>(object service) => Services.TryAdd(typeof(Intefrace), service);

        public static void RegisterService<Intefrace, Service>() where Intefrace : class where Service : class
        {
            Type @interface = typeof(Intefrace);
            if (!@interface.IsInterface) throw new Exception($"Not interface [{@interface.FullName}]");


            Type @class = typeof(Service);

            ConstructorInfo[] constructors = @class.GetConstructors();

            if (constructors.Length == 0) throw new Exception($"Cannot activate object of type [{@class.FullName}] - no public constructor");

            ConstructorInfo? ctor = constructors.Where(p => p.GetParameters().Length == 0).FirstOrDefault();

            if (ctor == null) throw new Exception($"Cannot activate object of type [{@class.FullName}] - non parameterless constructor available");

            object? instance = Activator.CreateInstance<Service>();

            if (instance == null) throw new Exception($"Cannot activate object of type [{@class.FullName}] - activator cannot create instance");

            Services.TryAdd(@interface, instance);

        }

        public static void RegisterAllService()
        {
            IEnumerable<Type> serviceTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(IService).IsAssignableFrom(type) && type.IsClass);

            foreach (Type serviceType in serviceTypes)
            {
                Type? type = serviceType.GetInterfaces().FirstOrDefault();
                if (type is null) continue;

                object? instance = Activator.CreateInstance(serviceType) ?? throw new Exception($"Cannot activate object of type [{type.FullName}] - activator cannot create instance");
                Services.TryAdd(type, instance);
            }
                    
        }


        public static Service Mediate<Service>()
        {
            if (Services.TryGetValue(typeof(Service), out var appLogic)) return (Service)appLogic;

            throw new Exception($"Service [{typeof(Service).FullName}] is not registered");
        }

    }
}
