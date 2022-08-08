using System.Collections.Generic;
using MerlinSDK;
using MerlinSDK.Component;
using MerlinSDK.Entity;
using UnityEngine;
using NativeWebSocket;
using System.Collections;
using System.Text;

[RequireComponent(typeof(SimulationController))]
public class Sync : MonoBehaviour
{
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
                world.Value.OnComponentSetOnEntityEvent += ComponentValueSet;
            }
        };
    }
    
    private async void Start()
    {
        websocket = new WebSocket("ws://localhost:5000/ws?userID=79&appID=111");

        websocket.OnOpen += () => {
            Debug.Log("CONNECTED!");
            websocket.SendText("Hello World!");
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

    private void Update() {
        if (trackedObj.transform.position == prevPos) {
            return;
        }

        prevPos = trackedObj.transform.position;

        if(websocket.State != WebSocketState.Open) {
            return;
        }

        string output = $"{trackedObj.transform.position.x}, {trackedObj.transform.position.y}, {trackedObj.transform.position.z}";
        websocket.Send(Encoding.ASCII.GetBytes(output));
    }

    private void ComponentValueSet(Entity entity, IComponent component, bool setfromnetworkmessage)
    {
        int value = (int) component.Get();
        Debug.Log(value);
    }
}