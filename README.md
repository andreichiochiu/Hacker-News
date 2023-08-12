# Hacker News

FAst API with in mem cache for simple implementation, in real production project we would use disitrbuted cache like REDIS, memcache, etc

### Logging

Classic console logging but inn real app it will be integrated with Azure AppInsights + healthckechs + monitoring  alerting mechanism

#### Execution

build project and press F5 or open exe file, then in borwser go to https://localhost:7255/stories?n=200 (n can have diferent values)

##### Notes

For simplicity, I considered the number of kids = number of reviews.

##### Improvement

This works well for a background service / worker, but in case of an app it can be improved in the following way:
- After starting up the service, have a background worker loading all stories into the cache of the service
- Or, when loading story details based on ID, use TPL to run multiple requests in the same time, so improve the time for leading data.

  Thanks for checking it!
