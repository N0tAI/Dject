using System.Reflection;

namespace AInjection;

public class TransientLifetime(Type instanceType) : InstanceLifetime(instanceType)
{
    public override object GetInstance()
    {
        throw new NotImplementedException();
    }
}
