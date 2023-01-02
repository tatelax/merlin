using LazyECS;

public class TestFeature : Feature
{
    public TestFeature()
    {
        Systems = new Systems()
            .Add(new GameObjectInitializeSystem())
            .Add(new GameObjectPositionSystem());
    }
}