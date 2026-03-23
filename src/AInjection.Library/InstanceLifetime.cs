using System.Reflection;

namespace AInjection;

public abstract class InstanceLifetime
{
    protected Type _instanceType;
    protected Lazy<MethodBase> _instanceFactory { get; }

    public InstanceLifetime(Type instanceType, MethodBase? factory = null)
    {
        _instanceType = instanceType;
        if(factory is null)
            _instanceFactory = new Lazy<MethodBase>(FindBestContructor);
        else
            _instanceFactory = new Lazy<MethodBase>(factory);
    }
    private MethodBase FindBestContructor()
    {
        List<ConstructorInfo> validConstructors = [];
        var constructors = _instanceType.GetConstructors();

        // Find best matching ctor (ctor with the most parameters that matches those in the mapping)
        foreach (var ctor in constructors)
        {
            var parameters = ctor.GetParameters();
            var hasAllParameters = true;
            foreach (var param in parameters)
            {
                // Parameter is not registered in the DI container thus cannot be handled
                if (!_serviceMapping.ContainsKey(param.ParameterType))
                {
                    hasAllParameters = false;
                    break;
                }
                // Parameter cannot be the type being instantiated as would be a cyclical dependency (bad practice + stack overflow by default)
                if (param.ParameterType == serviceType || param.ParameterType == _instanceType)
                {
                    hasAllParameters = false;
                    break;
                }
            }
            if (hasAllParameters)
                validConstructors.Add(ctor);
        }

        if (validConstructors.Count == 0)
            throw new MissingMethodException($"Service instance {_instanceType.FullName} does not have parameterless constructor or constructor that can be fulfilled");

        var sortedConstructors = validConstructors.OrderByDescending(ctor => ctor.GetParameters().Count());
        return sortedConstructors.First();
    }
    public abstract object GetInstance();
    protected Type[] GetDependencies()
    {
        List<Type> dependencies = new();
        return [];
    }
}
