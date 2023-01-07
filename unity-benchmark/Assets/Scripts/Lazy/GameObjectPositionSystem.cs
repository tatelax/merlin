using System;
using System.Collections.Generic;
using System.Diagnostics;
using LazyECS;
using LazyECS.Entity;
using UnityEngine;
using Debug = UnityEngine.Debug;
using EventType = LazyECS.EventType;

public class GameObjectPositionSystem : IUpdateSystem, IInitializeSystem
{
    private Group gameObjectGroup;
    private MainWorld mainWorld;
    
    public void Update()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        
        foreach (var entity in mainWorld.GetEntities<GameObjectComponent>())
        {
            Transform gameObjectTrans = entity.Get<GameObjectComponent>().Value.transform;
            gameObjectTrans.position = Vector3.Lerp(gameObjectTrans.position, Vector3.zero, Time.deltaTime * 0.5f);
        }
        
        stopwatch.Stop();
        
        Debug.Log(stopwatch.ElapsedMilliseconds.ToString());
    }

    public void Initialize()
    {
        mainWorld = MainSimulationController.Instance.GetWorld<MainWorld>() as MainWorld;
        
        mainWorld.CreateGroup(GroupType.All, new HashSet<Type>
            {typeof(GameObjectComponent)}, EventType.Set, OnEntityUpdate);
    }

    private void OnEntityUpdate(EventType _eventtype, Entity entity, Type component)
    {

    }
}
