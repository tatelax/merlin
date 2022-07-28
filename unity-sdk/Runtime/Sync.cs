using System.Collections.Generic;
using MerlinSDK;
using MerlinSDK.Component;
using MerlinSDK.Entity;
using UnityEngine;
using NativeWebSocket;

[RequireComponent(typeof(SimulationController))]
public class Sync : MonoBehaviour
{
    private SimulationController _controller;
    private WebSocket websocket;

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
    }
    
    private async void Start()
    {
        websocket = new WebSocket("ws://localhost:5000/ws");

        websocket.OnOpen += () => {
            Debug.Log("CONNECTED!");
            SendTestMessage();
        };

        await websocket.Connect();
    }

    private async void SendTestMessage()
    {
        await websocket.SendText("Hello World!");
    }

    private void ComponentValueSet(Entity entity, IComponent component, bool setfromnetworkmessage)
    {
        int value = (int) component.Get();
        Debug.Log(value);
    }
}