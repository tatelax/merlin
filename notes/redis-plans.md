# Redis Plans

## Schema

```
{appID}:{sessionID}:components
{appID}:{sessionID}:stream
{appID}:{sessionID}:worlds
{appID}:{sessionID}:{worldID}:entity:{id}
{appID}:{sessionID}:{worldID}:group:{id}
```

## Hierarchy

```
appID (0)
    |
     --> sessionID (0)
                 |
                  --> 0:0:sessionResults
                                       |
                                        --> entitiesCreated : 14292
                                        --> entitiesDestroyed : 212
                                        --> runTime : 1h52m01s
                 |
                  --> 0:0:components
                                   |
                                    --> 0 : HelloWorldComponent
                                    --> 1 : ExampleComponent
                 |
                  --> 0:0:stream
                               |
                                --> EntityCreated : {worldID}:{entityID}
                                --> EntityDestroyed : {worldID}:{entityID}
                                --> EntityUpdated : {worldID}:{entityID}:{componentID}:'Hello World!'
                 |
                  --> 0:0:worlds
                               |
                                --> 0 : HelloWorldComponent
                                --> 1 : ExampleComponent
                 |
                  --> worldID (0)
                             |
                              --> 0:0:0:entity:0 (0)
                                             |
                                              --> componentID : value
                                              --> componentID : value
                                              --> componentID : value
                             |
                              --> 0:0:0:entity:1 (1)
                                             |
                                              --> componentID : value
                             |
                              --> 0:0:0:0:group (0)
                                             |
                                              --> count : 0
```

## Notes

* After a stream ends, we process the data into another hashset with various data points from that stream

## Commands

Get all component IDs and their names in appID 0 in sessionID 0
* ```HGETALL 0:0:components```

Get all entities in appID 0 sessionID 0 worldID 0
* ```SCAN 0 MATCH 0:0:0:entity:*```