using System;
using System.Collections.Generic;
using MerlinSDK;
using MerlinSDK.Component;
using MerlinSDK.Entity;
using UnityEngine;

[RequireComponent(typeof(SimulationController))]
public class Sync : MonoBehaviour
{
    private SimulationController _controller;


    private async void Awake()
    {
        _controller = GetComponent<SimulationController>();

        _controller.OnWorldsInitialized += (sender, args) =>
        {
            foreach (KeyValuePair<int, IWorld> world in _controller.Worlds)
            {
                world.Value.OnComponentSetOnEntityEvent += ComponentValueSet;
            }
        };
        
        var uri = new Uri("https://www.example.com");

    }
    
    private void ComponentValueSet(Entity entity, IComponent component, bool setfromnetworkmessage)
    {
        int value = (int) component.Get();
        Debug.Log(value);
        // websocket.SendText(value.ToString());
    }
}