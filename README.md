# A* vs Dijkstra - 2D Pathfinding Game

This Unity 2D game serves two main purposes:

**Gameplay Objective:** The player must collect as many bonuses as possible while avoiding obstacles and competing against an opponent doing the same. The opponent uses pathfinding to reach bonuses efficiently, and both players move in turns.

**Algorithm Comparison:** The game is also used for analyzing and comparing the performance of A* and Dijkstra algorithms in terms of efficiency, memory usage, and behavior on maps with varying complexity.

## Highlights
- Turn-based game on a 2D grid: player vs opponent
- The opponent uses A* or Dijkstra for movement
- The player chooses:
  - the algorithm the opponent uses
  - the bonus distribution: uniform/normal

## Level Variety
- 5 levels with increasing difficulty
- Maps vary in size, obstacle density and layout complexity
- Number of collectible bonuses differs by level
- Bonus placement follows the selected distribution model
