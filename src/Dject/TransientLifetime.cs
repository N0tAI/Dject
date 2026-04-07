using System.Reflection;

namespace Dject;

public class TransientLifetime(Type instanceType) : InstanceLifetime(instanceType)
{
    public override object GetInstance()
    {
        throw new NotImplementedException();
    }
}
