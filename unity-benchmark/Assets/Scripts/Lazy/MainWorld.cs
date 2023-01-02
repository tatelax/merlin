using LazyECS;

public class MainWorld : World
{
    public override void Initialize()
    {
        base.Initialize();

        Features = new Feature[]
        {
            new TestFeature()
        };
    }
}