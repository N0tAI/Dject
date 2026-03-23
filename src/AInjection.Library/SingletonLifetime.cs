namespace AInjection;

public class SingletonLifetime(Type instanceType) : InstanceLifetime(instanceType)
{
    public override object GetInstance()
    {
        var initializer = Initializer.Value;

        throw new NotImplementedException();
    }
}
