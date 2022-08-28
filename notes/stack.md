1. Unity app sends JSON to websocket server with the change that happened:

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

2. Websocket relays this to the connected web client
3. Vue parses json to read updateType. Subsequent method is called with updateData params passed in
4. That method (createEntity) then adds a new entry to the entities map with the ID from the param (1)