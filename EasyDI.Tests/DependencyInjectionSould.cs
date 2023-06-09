﻿using EasyDI.Interfaces;

namespace EasyDI.Tests;

public class DependencyInjectionShould
{
    private class EmployeeService: IEmployee { }
    private interface IEmployee { }
    
    private readonly IDependencyInjection dependencyInjection;

    public DependencyInjectionShould() => 
        dependencyInjection = Exposer.ExposeDependencyInjection();

    [Fact]
    public void AddServiceInstance()
    {
        var empService = new EmployeeService();
        
        dependencyInjection.Inject(empService);
        
        Assert.True(empService == dependencyInjection.Require<EmployeeService>());
    }

    [Fact]
    public void AddServiceAsTransient()
    {
        dependencyInjection.Inject<EmployeeService, IEmployee>(oneInstance:false);

        var emp1 = dependencyInjection.Require<IEmployee>();
        var emp2 = dependencyInjection.Require<IEmployee>();
        
        Assert.True(emp1 != emp2 );
    }

    [Fact]
    public void AddServiceAsSingleton()
    {
        dependencyInjection.Inject<EmployeeService, IEmployee>(oneInstance:true);

        var emp1 = dependencyInjection.Require<IEmployee>();
        var emp2 = dependencyInjection.Require<IEmployee>();
        
        Assert.True(emp1 == emp2 );
    }
}