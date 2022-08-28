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
appID:{appID}:sessionID:{sessionID}:sessionResults
                                                    --> isComplete : true
                                                    --> entitiesCreated : 14292
                                                    --> entitiesDestroyed : 212
                                                    --> runTime : 1h52m01s

appID:{appID}:sessionID:{sessionID}:components
                                                    --> {componentID} : HelloWorldComponent
                                                    --> {componentID} : ExampleComponent

appID:{appID}:sessionID:{sessionID}:stream
                                                    --> EntityCreated : {worldID}:{entityID}
                                                    --> EntityDestroyed : {worldID}:{entityID}
                                                    --> EntityUpdated : {worldID}:{entityID}:{componentID}:'Hello World!'

appID:{appID}:sessionID:{sessionID}:worlds
                                                    --> {worldID} : HelloWorld
                                                    --> {worldID} : ExampleWorld

appID:{appID}:sessionID:{sessionID}:worldID:{worldID}
                                                    --> appID:{appID}:sessionID:{sessionID}:worldID:{worldID}:entityID:{entityID}
                                                                                                                                    --> {componentID} : value
                                                                                                                                    --> {componentID} : value
                                                                                                                                    --> {componentID} : value

                                                    --> appID:{appID}:sessionID:{sessionID}:worldID:{worldID}:entityID:{entityID}
                                                                                                                                    --> {componentID} : value

                                                    --> appID:{appID}:sessionID:{sessionID}:worldID:{worldID}:groupID:{groupID}
                                                                                                                                    --> count : 0
```

## Notes

* After a stream ends, we process the data into another hashset with various data points from that stream

## Commands

Get all component IDs and their names in appID 0 in sessionID 0
* ```HGETALL appID:123:sessionID:111:components```
  
Get all worlds for a sessionID
* ```HGETALL appID:123:sessionID:111:worlds```

Get all entities in appID 123 sessionID 456 worldID 789
* ```SCAN 0 match appID:123:sessionID:456:worldID:789:entityID:*```

Get all groups for a particular worldID
* ```SCAN 0 match appID:123:sessionID:111:worldID:33:groupID:*```