using System.Collections.Generic;
using LazyECS;
using LazyECS.Component;
using LazyECS.Entity;
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
            foreach (KeyValuePair<int,IWorld> world in _controller.Worlds)
            {
                world.Value.OnComponentSetOnEntityEvent += ComponentValueSet;
            }
        };
        
        websocket = new WebSocket("ws://localhost:3000");

        websocket.OnOpen += () =>
        {
            Debug.Log("Connection open!");
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
        };

        websocket.OnMessage += (bytes) =>
        {
            Debug.Log("OnMessage!");
            Debug.Log(bytes);

            // getting the message as a string
            // var message = System.Text.Encoding.UTF8.GetString(bytes);
            // Debug.Log("OnMessage! " + message);
        };

        // Keep sending messages at every 0.3s
        //InvokeRepeating("SendWebSocketMessage", 0.0f, 0.3f);

        // waiting for messages
        await websocket.Connect();
    }

    private void ComponentValueSet(Entity entity, IComponent component, bool setfromnetworkmessage)
    {
        int value = (int)component.Get();
        Debug.Log(value);
        websocket.SendText(value.ToString());
    }
}
