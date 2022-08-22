using System;
using System.Collections.Generic;
using MerlinSDK;
using MerlinSDK.Component;
using MerlinSDK.Entity;
using UnityEngine;
using NativeWebSocket;
using System.Text;

[RequireComponent(typeof(SimulationController))]
public class Sync : MonoBehaviour
{
    public Action WebsocketOpened;
    
    [SerializeField] private string appID;

    public GameObject trackedObj;

    private SimulationController _controller;
    private WebSocket websocket;

    private Vector3 prevPos;

    private void Awake()
    {
        _controller = GetComponent<SimulationController>();

        _controller.OnWorldsInitialized += (sender, args) =>
        {
            foreach (KeyValuePair<int, IWorld> world in _controller.Worlds)
            {
                world.Value.OnEntityCreatedEvent += ValueOnOnEntityCreatedEvent;
                world.Value.OnComponentSetOnEntityEvent += ComponentValueSet;
            }
        };
    }

    private void ValueOnOnEntityCreatedEvent(Entity entity, bool entitycreatedfromnetworkmessage)
    {
        websocket.SendText($"{{ \"updateType\": \"CreateEntity\", \"updateData\":  {{\"entityID\": {entity.id} }}}}");
    }

    private async void Start()
    {
        websocket = new WebSocket($"ws://localhost:2414/?userID=79&appID={appID}", "StateUpdate");

        websocket.OnOpen += () =>
        {
            Debug.Log("CONNECTED!");
            websocket.SendText("Hello World!");
            
            WebsocketOpened?.Invoke();
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
        };

        websocket.OnMessage += bytes =>
        {
            Debug.Log("OnMessage!");
            Debug.Log(bytes);

            // getting the message as a string
            // var message = System.Text.Encoding.UTF8.GetString(bytes);
            // Debug.Log("OnMessage! " + message);
        };

        await websocket.Connect();
    }

    private void Update()
    {
        if (trackedObj.transform.position == prevPos)
        {
            return;
        }

        prevPos = trackedObj.transform.position;

        if (websocket.State != WebSocketState.Open)
        {
            return;
        }

        string output = $"{trackedObj.transform.position.x}, {trackedObj.transform.position.y}, {trackedObj.transform.position.z}";
        websocket.Send(Encoding.ASCII.GetBytes(output));
    }

    private void ComponentValueSet(Entity entity, IComponent component, bool setfromnetworkmessage)
    {
        int value = (int)component.Get();
        Debug.Log(value);
    }
}