using System.Reflection;
using EasyDI.Exceptions;
using EasyDI.Interfaces;

namespace EasyDI;

internal class DependencyInjection : IDependencyInjection
{
    private readonly Dictionary<Type, Delegate> services = new();

    // Todo resolverHelperFix
    private object ResolverHelper(Type type)    
    {
        if (services.ContainsKey(type) is false)
            throw new ServiceNotFoundException(type);

        var method = services[type].Method;

        var args = method.GetParameters().Select(parameter => ResolverHelper(parameter.GetType())).ToArray();

        return services[type].DynamicInvoke(args) ?? throw new Exception();
    }

    // Todo InstanciateFix
    private object Instantiate<T>()
    {
        var ctor = typeof(T)
            .GetTypeInfo()
            .DeclaredConstructors
            .MaxBy(ctor => ctor.GetParameters().Length);

        if (ctor is null) throw new Exception();

        var args = ctor.GetParameters().Select(info => ResolverHelper(info.ParameterType)).ToArray();

        return ctor.Invoke(args);
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

    // Todo InjectFix
    public void Inject<TService>(Delegate factory)
    {
        var type = typeof(TService);

        if (type != factory.Method.ReturnType)
            throw new ArgumentException(
                "The return type of the factory method must be the same with the given type argument");

        var args = factory.Method.GetParameters().Select(info => ResolverHelper(type)).ToArray();
        var service = factory.DynamicInvoke(args);

        if (service is null)
            throw new Exception();

        services[type] = ()=> service;
    }


    public TService Resolve<TService>() =>
        (TService)ResolverHelper(typeof(TService));
}