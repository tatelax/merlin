using System.Collections.Generic;
using MerlinSDK;
using MerlinSDK.Component;
using MerlinSDK.Entity;
using Socket.Quobject.SocketIoClientDotNet.Client;
using UnityEngine;

[RequireComponent(typeof(SimulationController))]
public class Sync : MonoBehaviour
{
    private SimulationController _controller;

    private QSocket socket;

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

        socket = IO.Socket("http://localhost:3000");

        socket.On(QSocket.EVENT_CONNECT, () =>
        {
            Debug.Log("Connected");
            socket.Emit("chat", "test");
        });

        socket.On("chat", data => { Debug.Log("data : " + data); });
    }
    
    private void ComponentValueSet(Entity entity, IComponent component, bool setfromnetworkmessage)
    {
        int value = (int) component.Get();
        Debug.Log(value);
        // websocket.SendText(value.ToString());
    }
}