namespace EasyDI.Exceptions;

public class ServiceNotFoundException : Exception
{
    public ServiceNotFoundException(Type serviceType)
    :base($"couldn't resolve a service of type {serviceType.Name}.") { }
}