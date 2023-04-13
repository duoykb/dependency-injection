namespace EasyDI.Interfaces;

public interface IDependencyInjection
{
    /// <summary>
    /// Registers a single instance of the service, that is the given instance.
    /// </summary>
    /// <param name="service"></param>
    /// <typeparam name="TService"></typeparam>
    void Inject<TService>(TService service);
    
    /// <summary>
    /// Registers TImplementation as TService.
    /// </summary>
    /// <param name="oneInstance">If set to true , one instance of the service will be used for all resolves.</param>
    /// <typeparam name="TImplementation">The actual implementation.</typeparam>
    /// <typeparam name="TService">The type that the implementation  will be registered as.</typeparam>
    void Inject<TImplementation, TService>(bool oneInstance = false);

    /// <summary>
    /// Registers TImplementation as TService.
    /// </summary>
    /// <param name="implementation">The actual implementation.</param>
    /// <typeparam name="TImplementation">The actual implementation.</typeparam>
    /// <typeparam name="TService">The type that the implementation  will be registered as.</typeparam>
    void Inject<TImplementation, TService>(TImplementation implementation);
    
    /// <summary>
    /// Registers a service of the given type.
    /// </summary>
    /// <param name="oneInstance">If set to true , one instance of the service will be used for all resolves.</param>
    /// <typeparam name="TService"></typeparam>
    void Inject<TService>(bool oneInstance = false);
    
    /// <summary>
    /// Registers a service of the given type.
    /// </summary>
    /// <param name="factory">a delegate that will return the service.</param>
    /// <typeparam name="TService"></typeparam>
    void Inject<TService>(Delegate factory);
    
    /// <summary>
    /// gets a service of the given type.
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <returns></returns>
    TService Resolve<TService>();
}