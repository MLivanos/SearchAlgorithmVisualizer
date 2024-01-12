<div align="center">

# Search Algorithm Visualizer

<img width="767" alt="Screenshot 2024-01-12 at 10 27 12 AM" src="https://github.com/MLivanos/SearchAlgorithmVisualizer/assets/59032623/8729b0a0-78cb-4a6c-976b-12999efe6c62">

![StaticBadge](https://img.shields.io/badge/PLAY-100000?style=for-the-badge&logo=unity&logoColor=white&link=mlivanos.github.io/SearchAlgorithmVisualizer)

</div>

Hello Fellow AI Educators and Learners!

The goal of this repo is to provide a visual understanding of different search algorithms using Unity to visualize the process. This project is inspired by [Clement Mihailescu's Similar Algorithm Visualizer](https://clementmihailescu.github.io/Pathfinding-Visualizer/#), but with a few important distinctions. Namely, this is in Unity. This allows you to download the project and modify it however you'd like with your own art assets to draw a narrative, modify the codebase to add new algorithms, and styles of mazes, or extend the project however you'd like. Further, I also added some additional options that are useful for my purposes and may be useful for yours - namely:

* A choice of A* heuristics
* A field to input the weight for weighted A*
* Detailed feedback on the path cost and efficiency of an algorithm after it is run

## INSTRUCTIONS:

Click the Play button above to get started! If you receive an error about the stack, open the simulation in Incognito mode. After selecting your maze, algorithm, and parameters, click "Run" to begin the simulation. Notice the "Nodes Explored" and "Path Cost" entries above, which can give you insights into the strengths and weaknesses of each algorithm.

Hold "s" and click a node to change the start node. Hold "g" and click a node to change the goal node. Click any free space to turn it into a barrier, and any barrier to turn it into a free space.

<img width="766" alt="Screenshot 2024-01-12 at 9 10 18 AM" src="https://github.com/MLivanos/SearchAlgorithmVisualizer/assets/59032623/349f7dcd-ee60-443c-9213-925aa55a91e2">

Take a look at the marked image above. The remainder of this section will discuss each item in turn.

i - Maze type. The random maze algorithm used to generate the maze.

ii - Search algorithm. The search algorithm that will be used to traverse the maze

iii - Run button. Click here to start the simulation

iv - Clear path button. Click here to remove the currently discovered path and nodes explored.

v - Refresh. Clears and new maze with the specified parameters

vi - Simulation speed. How fast nodes will be explored. Sliding this to max will reveal one node every frame, and sliding it to the minimum will reveal one node every 0.3 seconds.

vii - Random maze obstacle proportion. If you make a random maze, this is the percent of it that will be randomly covered in barriers.

viii - A* Heuristic. Your choice of heuristic if you are choosing to simulate A*. Note that the "Zero" heuristic simply returns zero for all nodes (Dijkstra's algorithm).

ix - Map size. Size of the map to be generated. Each axis must be >8. The default values for length and width are 70 and 30, respectively.

x - Efficency display. Displays the number of nodes explored by the algorithm, and the path cost returned. The prior deals with the efficiency of the algorithm, and the latter with optimality.

xi - A* Weight. Parameter to use weighted A*, defaulted to 1 (vanilla A*).

xii - Zoom and pan. Change the view of the simulation.

xiii - Start node. In the dark blue color. To change this, hold "s" and click somewhere on the screen.

xiv - Goal node. In the light blue-green color. To change this, hold "g" and click somewhere on the screen.
