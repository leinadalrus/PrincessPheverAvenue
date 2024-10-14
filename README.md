## A Small personal (pet) project for a game-jam. Based on a late 1990s Japanese Car RPG.

`A light racing soap-box cart RPG`
`-built-to-scale`

# To-do

1. Scale media (images.) to 320-by-224 pixels.
2. Implement "Easing" functions.
3. Circle buffer context-scene buildings (flyweights.)
    
4. Group buildings into a disjoin union set
    1. setting ID for the group to use in a Cyclic LRU storage cache.

- Linear arrayed groupings for cyclic buffer data-structure.

As the player entity reaches high RPM and speed, velocity is split between momentum and directional-input.
As the player progresses, give them choices and chances to:

1. Change vehicle(s).
2. Modify their vehicle's capabilities.
3. Swap tunings between vehicles.
4. Re/Un-cover won/winning parts.
    1. Therefore, we'll need Inventory and Inventory Management Systems.
    2. So there are choices and chances between items/parts won and, or lost.

#### Game design philosophy
> Fast is Slow.
> Slow is Smooth.
> Smooth is Fast.

#### Deliverables

- Sprite-sheet artwork
- User interface
- Foley art, sound effects
- Soap box kart
- Potato

#### Mission Criticalities

- Develop Differential Equation - Functions
- Newton's laws as functions
- Director class
- RPG Inventory Manager class
- Time-Graph database system
- Input handler and command design pattern
- Scan-line polygon fill
- Magnitude discriminators
- Vector tuples
    - In essence, Squares are in-a-way Triangles that make up Squares.
- A-Star Algorithm
    - Complement Marching Squares results
- Confine with bounding boxes
- Render corners affected by marched rays/shapes

#### Algorithms to employ

- Adjacency List
- Binary Search Tree
- Breadth First Search
- Matrix Breadth First Search.
- Depth First Search

#### Roadmap

1. LRU (LFU) Cache
    1. with Flyweights
2. Breadth-first Search
    1. or Dijkstra's Shortest-path algorithm
