This application contains my soluton for a thread safe hotel booking system.

I've just used a static list as a database, as there was no requirement to implement persistent storage. However, I've tried to architecture the application so that a persistant database could be used in place.

For thread safety, the main concern is during the read write required to add a new booking. Multiple reads are generally safe so I have implemented this by wrapping the repository methods in a reader writer lock.

For convenience a Main method is provided, but more detail of my considerations can be found in the test project.