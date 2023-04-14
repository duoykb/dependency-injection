using System.Reflection;
using EasyDI.Exceptions;
using EasyDI.Interfaces;

namespace EasyDI;

internal class DependencyInjection : IDependencyInjection
{
    private readonly Dictionary<Type, Delegate> services = new();
    
    private object RequireHelper(Type type)    
    {
        if (services.ContainsKey(type) is false)
            throw new ServiceNotFoundException(type);

        var method = services[type].Method;

        var args = method.GetParameters().Select(parameter => RequireHelper(parameter.GetType())).ToArray();

        return services[type].DynamicInvoke(args) ?? throw new Exception();
    }
    private object Instantiate<T>()
    {
        var constructors = typeof(T).GetTypeInfo().DeclaredConstructors
            .OrderBy(c => c.GetParameters().Length)
            .Reverse();

        foreach (var c in constructors)
        {
            try
            {
                var args = c.GetParameters().Select(pInfo => RequireHelper(pInfo.ParameterType)).ToArray();
                var instance = c.Invoke(args);

                return instance;
            }catch (ServiceNotFoundException){}
        }

        throw new Exception($"could not create instance of {typeof(T).Name}.");
    }

    public void Inject<TService>(TService service) => 
        services[typeof(TService)] = () => service;
    
    public void Inject<TImplementation, TService>(TImplementation implementation) => 
        services[typeof(TService)] = () => implementation;

    public void Inject<TImplementation, TService>(bool oneInstance = false)
    {
        var implementation = Instantiate<TImplementation>();
        var type = typeof(TService);

        if (oneInstance)
            services[type] = () => implementation;
        else
            services[type] = Instantiate<TImplementation>;
    }
    
    public void Inject<TService>(bool oneInstance = false)
    {
        var service = Instantiate<TService>();
        var type = typeof(TService);

        if (oneInstance)
            services[type] = () => service;
        else
            services[type] = Instantiate<TService>;
    }
    
    public void Inject<TService>(Delegate factory)
    {
        var type = typeof(TService);

        if (type != factory.Method.ReturnType)
            throw new ArgumentException(
                "The return type of the factory method must be the same with the given type argument.");

        var args = factory.Method.GetParameters().Select(info => RequireHelper(type)).ToArray();
        var service = factory.DynamicInvoke(args);

        if (service is null)
            throw new Exception($"Could not create service of {typeof(TService).Name}");

        services[type] = ()=> service;
    }


    public TService Require<TService>() =>
        (TService)RequireHelper(typeof(TService));
}