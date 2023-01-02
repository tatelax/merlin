using LazyECS;
using UnityEngine;

public class GameObjectInitializeSystem : IInitializeSystem
{
    public void Initialize()
    {
        var world = MainSimulationController.Instance.GetWorld<MainWorld>();
        
        for (int i = 0; i < 1000000; i++)
        {
            var entity = world.CreateEntity();
            var obj = new GameObject();
            obj.transform.position = Random.insideUnitSphere * 100f;

            entity.Set<GameObjectComponent>(obj);
        }
    }
}
