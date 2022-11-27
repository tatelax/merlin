# v1

1. Unity app sends JSON to websocket server with the change that happened:
```
    setWorlds
    {
        "world": [
            {"0": "Hello World"},
            {"1": "ExampleWorld"}
        ]
    }

    createEntity
    {
        updateType: "0",
        worldID: 1,
        updateData: 
        {
            entityID: 1
        }
    }

    destroyEntity
    {
        updateType: "1",
        worldID: 1,
        updateData:
        {
            entityID: 1
        }
    }

    updateEntity
    {
        updateType: '2',
        worldID: 1,
        updateData:
        {
            entityID: 1,
            componentID: 1,
            dataType: string --? maybe needed
            value: "Hello World!"
        }
    }
```

2. Websocket relays this to the connected web client
3. Vue parses json to read updateType. Subsequent method is called with updateData params passed in
4. That method (createEntity) then adds a new entry to the entities map with the ID from the param (1)

# v2

1. On the frontend the user will select the world that they give a shit about. Entity updates will only happen to the selected world. Unselected worlds will only be sent their current entity count.
2. The frontend user will also select the entity they give a shit about so only selected entities will have their component values sent to the frontend via websocket

# v3

1. Use Mirage on client (Unity) and server (.net core app) to send RPCs

# v4

1. Mirage client on Unity app communicates with .net core app server. .net core app server receives certain RPCs for creating and updating and destroying entities then just relays that to all connected websocket clients. So 2 websocket servers. One for observers and one for game clients.