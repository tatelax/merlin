1. Unity app sends JSON to websocket server with the change that happened: 
    ```{
        updateType: "createEntity",
        updateData: 
        {
            entityID: 1,
        }
    }```

2. Websocket relays this to the connected web client
3. Vue parses json to read updateType. Subsequent method is called with updateData params passed in
4. That method (createEntity) then adds a new entry to the entities map with the ID from the param (1)