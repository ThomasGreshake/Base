# Base
A collection of basic classes of functions written by me (except for PriorityQueue) as a base for a small Unity game. The actual game files are not public.

"Architecture" features an Eventsystem and base functions for a basic data structure that is disconnected from the visual layer provided by Unity (A backend structure to Unitys frontend)
The Eventsystem is probably the only truely unique thing in there, as events are represented by interfaces with static Actions to distribute events. Event arguments are provided by functions on the interfaces.

"Functions" features some additional math functions (mostly geometry, like hitpoints and distances between lines, circles, ellipses, etc), random functions (random item from a list, weighted picks), LINQ-like extensions and other stuff.

"PriorityQueue" is not made by me. It is a Queue in which items are ordered via a priority value

"StatSystem" features a system that allows "modifications" of "stats". A stat is basically a number like 10, to which modifications of different types can be added, for example +20% -> number becomes 12. All stats of a specific thing (like a character, or whatever else can have its properties modified) are stored in a Statcontainer, which also keeps track of all current Stateffects, wich are collections of modifications to its stats. Modifications can be added to the container directily, via such Stateffects or can be created using the StatsUpgrader class, which allows the scaling of the strength of applied modifications depending on an upgrade value. Base data is expeceted to be in read from XML files.
