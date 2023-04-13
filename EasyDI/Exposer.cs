using EasyDI.Interfaces;

namespace EasyDI;

public static class Exposer
{
    public static IDependencyInjection ExposeDependencyInjection() => new DependencyInjection();
}