using MerlinSDK;
using UnityEngine;

public class TestUpdateSystem : IUpdateSystem, IInitializeSystem
{
    private float lastUpdate;
    private MainWorld mainWorld;
    
    public void Initialize()
    {
        Debug.Log("Initialized!");
        lastUpdate = 1f;
        mainWorld = (MainWorld)SimulationController.Instance.GetWorld<MainWorld>();
    }

    public void Update()
    {
        lastUpdate -= Time.deltaTime;
        
        if (lastUpdate <= 0f)
        {
            mainWorld.CreateEntity();
            lastUpdate = 10f;
        }
    }
}
