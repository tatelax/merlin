using MerlinSDK;
using MerlinSDK.Entity;
using UnityEngine;

public class TestInitializeSystem : IInitializeSystem
{
    public void Initialize()
    {
        Debug.Log("Initialized!");

        MainWorld mainWorld = (MainWorld)SimulationController.Instance.GetWorld<MainWorld>();

        Entity newEntity = mainWorld.CreateEntity();
        newEntity.Set<TestComponent>(1);
    }
}
