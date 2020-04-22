using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Linq;
using Assets.Scripts;
using Photon.Pun;
using System.IO;

/// <summary>
/// This generator class holds all the methods neccessary to dynamically generate a maze environment with bricks, minions and player.
/// </summary>
public class MazeGenerator : MonoBehaviour
{
    /// <summary>
    /// A PhotonView variable that will hold the other image of the maze. It is used in multiplayer gameplay mode.
    /// </summary>
    private PhotonView PV;

    /// <summary>
    /// A boolean variable that will help to determine is player in in multiplayer mode.
    /// </summary>
    public bool isMultiplayerMode;

    /// <summary>
    /// A variable that holds the MazeGenerator instance that will allow other scrips to access the methods defined in this class.
    /// </summary>
    public static MazeGenerator Instance;

    /// <summary>
    /// A variable that holds the brick prefab game object.
    /// </summary>
    public GameObject BrickPrefab;

    /// <summary>
    /// A variable that holds the coin prefab game object.
    /// </summary>
    public GameObject CoinPrefab;

    /// <summary>
    /// A variable that holds the chest prefab game object.
    /// </summary>
    public GameObject ChestPrefab;

    /// <summary>
    /// A variable that holds the player prefab game object.
    /// </summary>
    public GameObject PlayerPrefab;

    /// <summary>
    /// A variable that holds the minion prefab game object.
    /// </summary>
    public GameObject MinionPrefab;

    /// <summary>
    /// A variable that holds the game background sprite renderer object.
    /// </summary>
    public SpriteRenderer GameBackground;

    /// <summary>
    /// A variable that holds the x,y and z coordinates positon of maze.
    /// </summary>
    public Vector3 mazePos;

    /// <summary>
    /// A variable that holds maze row index.
    /// </summary>
    public int row; //An odd number

    /// <summary>
    /// A variable that holds maze column index.
    /// </summary>
    public int col; //An odd number

    /// <summary>
    /// A variable that holds number of coins in the maze.
    /// </summary>
    private int numOfCoins = 85; //Fix number of coins

    /// <summary>
    /// A variable that holds the max score attainable by gathering all the coins and chest boxes.
    /// </summary>
    private int maxScore = 85 * 10 + 6 * 30;

    //For coins(not real): real index (odd x, odd y)
    /// <summary>
    /// An array of booleans variable that will help to determine whether each position in the 50x50 maze is visited.
    /// </summary>
    public bool[,] visited = new bool[50, 50]; //[x][y]

    //Full maze
    //0: brick, 1: coin, 2: chest, 3: minion, 4: player
    /// <summary>
    /// A array of booleans variable that will help to determine whether each position in the 50x50 maze is marked.
    /// </summary>
    public int[,] mark = new int[50, 50]; //[x][y]

    /// <summary>
    /// A variable counter.
    /// </summary>
    public int count = 0;

    /// <summary>
    /// An array of chest occupied cells.
    /// </summary>
    public Cell[] chestCells;

    /// <summary>
    /// An array of minion occupied cells.
    /// </summary
    public Cell[] minionCells;

    /// <summary>
    /// An internal data structure to store the attributes of a cell such as the x and y coordinate.
    /// </summary>
    public struct Cell {
        public int x;  //column
        public int y;  //row

        public Cell(int x, int y) {
            this.x = x;
            this.y = y;
        }
    };

    /// <summary>
    /// This method is used to instantiate the MazeGenerator instance.
    /// </summary>
    void Awake()
    {
        Instance = this;
    }

        public int getMaxScore()
    {
        return maxScore;
    }

    /// <summary>
    /// This method is used to fixed the positions of minions and chest in their cell arrays.
    /// </summary>
    public void fixChestMinion() {
        chestCells = new Cell[]{
            new Cell(1, 1),
            new Cell(1, row - 2),
            new Cell(col - 2, 1),
            new Cell(col - 2, row - 2),
            new Cell(5, 3),
            new Cell(col - 1 - 3, row - 1 - 5)
        };
        minionCells = new Cell[] {
            new Cell(1, 7),
            new Cell(7, 1),
            new Cell(col - 2, 5),
            new Cell(9, row - 2),
            new Cell(5, 11),
            new Cell(7,9)
        };
    }

    /// <summary>
    /// This method is used to check if the cell is occupied.
    /// </summary>
    public bool CheckCell(Cell cell) {
        if (cell.x < 0 || cell.y < 0 || cell.x > col / 2 - 1 || cell.y > row / 2 - 1) {
            return false;
        }
        return true;
    }

    /// <summary>
    /// This method is used to check there are neighbouring cells.
    /// </summary>
    /// <param name="cell">A cell structure that represents a position in the maze.</param>
    public Cell CheckNeighbors(Cell cell) {
        ArrayList neighbors = new ArrayList();
        
        int x = cell.x;
        int y = cell.y;

        Cell top = new Cell(x, y - 1);
        Cell right = new Cell(x + 1, y);
        Cell bottom = new Cell(x, y + 1);
        Cell left = new Cell(x - 1, y);

        if (CheckCell(top) && !visited[top.x, top.y]) {
            neighbors.Add(top);  //top
        }
        if (CheckCell(right) && !visited[right.x, right.y]) {
            neighbors.Add(right);  //right
        }
        if (CheckCell(bottom) && !visited[bottom.x, bottom.y]) {
            neighbors.Add(bottom);  //bottom
        }
        if (CheckCell(left) && !visited[left.x, left.y]) {
            neighbors.Add(left);  //left
        }

        if (neighbors.Count > 0) {
            int r = Random.Range(0, neighbors.Count);
            return (Cell)neighbors[r];
        } else {
            return new Cell(-1, -1);
        }
    }

    /// <summary>
    /// A stack variable used to keep track on the cells to return to on DFS and Backtracking algorithm used to generator the maze environment. 
    /// </summary>
    public ArrayList stack = new ArrayList();

    /// <summary>
    /// This method is used along with all the methods defined above to create the maze environment using DFS and backtracking algorithm to dynamically generate the maze environment.
    /// </summary>
    /// <param name="cur">The current cell position.</param>
    public void DFS_Backtracking(Cell cur) {
        //Use algorithm on coins array then change to real index by (2x+1, 2y+1)
        
        visited[cur.x, cur.y] = true;
    
        // STEP 1
        Cell next = CheckNeighbors(cur);
        
        if (next.x != -1) {
            visited[next.x, next.y] = true;

            // STEP 2
            stack.Add(next);

            // STEP 3: mark wall as coin to go pass
            int realX = 2 * cur.x + 1;
            int realY = 2 * cur.y + 1;
            if (cur.x > next.x) {      //mark left cell as coin
                mark[realX - 1, realY] = 1;
            }
            else if (cur.x < next.x) { //mark right cell as coin
                mark[realX + 1, realY] = 1;
            }
            else if (cur.y > next.y) { //mark top cell as coin
                mark[realX, realY - 1] = 1;
            }
            else if (cur.y < next.y) { //mark bottom cell as coin
                mark[realX, realY + 1] = 1;
            }
            count++;  //1 more coin 

            // STEP 4
            cur = next;
            DFS_Backtracking(cur);
        } else if (stack.Count > 0) {
            cur = (Cell) stack[stack.Count - 1];
            stack.RemoveAt(stack.Count - 1);
            DFS_Backtracking(cur);
        }
    }

    /// <summary>
    /// This method is used to strategically break walls and place the minions or coins into the maze environment.
    /// </summary>
    /// <param name="time">A time variable to keep track of number of times the wall is broken.</param>
    public void BreakWall(int time) {
        //For each even row: break a brick that between 2 bricks
        for (int y = 2 ; y < row - 1; y += 2) {
            if (count == numOfCoins)
                break;

            if (y < row / 2) {
                //search from left
                for (int x = 1 ; x < col - 1 ; x++) {
                    if (mark[x - 1, y] == 0 && mark[x, y] == 0 && mark[x + 1, y] == 0) {
                        mark[x, y] = 1;  //mark as coin
                        count++;
                        break;
                    }
                }
            } else {
                //search from right
                for (int x = col - 2 ; x > 0 ; x--) {
                    if (mark[x - 1, y] == 0 && mark[x, y] == 0 && mark[x + 1, y] == 0) {
                        mark[x, y] = 1;  //mark as coin
                        count++;
                        break;
                    }
                }
            }
        }
        
        //For each even col: break a cell that between 2 cells
        for (int x = 2 ; x < col - 1; x += 2) {
            if (count == numOfCoins)
                break;
        
            if (x < col / 2) {
                //search from bottom
                for (int y = row - 2 ; y > 0 ; y--) {
                    if (mark[x, y - 1] == 0 && mark[x, y] == 0 && mark[x, y + 1] == 0) {
                        mark[x, y] = 1;  //mark as coin
                        count++;
                        break;
                    }
                }
            } else {
                //search from top
                for (int y = 1 ; y < row - 1 ; y++) {
                    if (mark[x, y - 1] == 0 && mark[x, y] == 0 && mark[x, y + 1] == 0) {
                        mark[x, y] = 1;  //mark as coin
                        count++;
                        break;
                    }
                }
            }
        }
        
        if (time == 2)  //just let it repeat 2 times
            return;
        
        if (count < numOfCoins) {
            BreakWall(2);
        }
    }

    /// <summary>
    /// This method is used to set the theme of the walls and background based on player's chosen level.
    /// </summary>
    /// <param name="chosenLevel">Player's selected difficulty level.</param>
    public void SetWalls_SetBackground(int chosenLevel)
    {
        int id;
        if (chosenLevel <= 5)
            id = chosenLevel;
        else
            id = chosenLevel - 5;

        GameBackground.sprite = Resources.Load<Sprite>("GameBg/game_bg_" + id);

        foreach (Transform child in this.transform)
        {
            if (child.tag == "Wall")
            {
                GameObject wall = child.gameObject;
                SpriteRenderer wallSprite = wall.GetComponent<SpriteRenderer>();
                wallSprite.sprite = Resources.Load<Sprite>("Wall/wall_" + id);
            }
        }
    }

    // Start is called before the first frame update

    /// <summary>
    /// This method is called before the first frame update, to initialize the maze environment with walls and background, if it is a multiplayer game, photonview will be generated to create a copy of the maze environment.
    /// </summary>
    void Start()
    {
        PV = GetComponent<PhotonView>();

        GenerateMaze();
        if (!isMultiplayerMode)
        {
            int chosenLevel = LevelController.Instance.getChosenLevel();
            SetWalls_SetBackground(chosenLevel);
        } 
    }

    /// <summary>
    /// This method is called to destroy the maze after every game ends.
    /// </summary>
    void DestroyMaze()
    {
      var components =   GameObject.FindGameObjectsWithTag("Page").Where(z => z.name == "PlayPage").First().GetComponents<Component>();
        foreach (var x in components)
        {
            if (!(x is Transform))
            {
                Destroy(x);
            }
        }
    }


    /// <summary>
    /// This method is called generate the maze environment everytime the player's presses on the play button.
    /// </summary>
    void GenerateMaze()
    {
        fixChestMinion();

        //walls and path to go + horizontal walls broken + vertical walls broken - no of chests - 1 player
        //numOfCoins = (2*(row/2)*(col/2) - 1) + (row/2 - 3) + (col/2 - 1) - chestCells.Length - 1;
        numOfCoins = 85;

        count = (row / 2) * (col / 2) - chestCells.Length - 1;  //1 player

        for (int x = 0; x < col; x++)
        {
            for (int y = 0; y < row; y++)
            {
                if (x % 2 == 1 && y % 2 == 1)
                {
                    mark[x, y] = 1;  //coin
                }
                else
                    mark[x, y] = 0;  //brick
            }
        }
        for (int i = 0; i < chestCells.Length; i++)
        {
            Cell chest = (Cell)chestCells[i];
            mark[chest.x, chest.y] = 2;  //chest
        }
        for (int i = 0; i < minionCells.Length; i++)
        {
            Cell minion = (Cell)minionCells[i];
            mark[minion.x, minion.y] = 3;  //minion
        }

        for (int x = 0; x < col / 2; x++)
        {
            for (int y = 0; y < row / 2; y++)
            {
                visited[x, y] = false;
            }
        }

        DFS_Backtracking(new Cell(0, 0));
        BreakWall(1);

        
        if (!isMultiplayerMode)
            mark[col / 2, row / 2] = 4;   //Player
        else
        {
            //Empty space for 2 players
            mark[3, 3] = 5;
            mark[col - 4, row - 4] = 5;
        }
        

        PlaceObjects();
    }

    /// <summary>
    /// This method is called to place all the objects in the maze such as bricks, coins, chestboxes, minions that will be created using the different methods defined in this class.
    /// </summary>
    void PlaceObjects() {
        
        if (!isMultiplayerMode)
        {
            int counter = 0;
            for (int x = 0; x < col; x++)
            {
                for (int y = 0; y < row; y++)
                {
                    GameObject gameObject = null;
                    GameObject minion = null;
                    switch (mark[x, y])
                    {
                        case 0:
                            gameObject = Instantiate(BrickPrefab) as GameObject;
                            break;
                        case 1:
                            gameObject = Instantiate(CoinPrefab) as GameObject;
                            break;
                        case 2:
                            gameObject = Instantiate(ChestPrefab) as GameObject;
                            break;
                        case 3:
                            if(counter < 4)
                            {
                                gameObject = Instantiate(CoinPrefab) as GameObject;
                                minion = Instantiate(MinionPrefab) as GameObject;
                            }
                            counter += 1;
                            break;
                        case 4:
                            gameObject = Instantiate(PlayerPrefab) as GameObject;
                            break;
                        default:
                            break;
                    }
                    if (gameObject != null)
                    {
                        Transform t = gameObject.transform;

                        //t is set a parent of whatever object the script is attached to
                        t.SetParent(transform);

                        Vector3 pos = new Vector3(x, -y, 0);

                        //Set position for t
                        t.position = mazePos + pos;

                        if (minion != null)
                        {
                            t = minion.transform;
                            t.SetParent(transform);
                            pos = new Vector3(x, -y, 0);

                            //Set position for t
                            t.position = mazePos + pos;
                        }
                    }
                }
            }
        } 
        else if (PhotonNetwork.IsMasterClient && PV.IsMine)
        {
            int counter = 0;
            for (int x = 0; x < col; x++)
            {
                for (int y = 0; y < row; y++)
                {
                    Vector3 pos = new Vector3(x, -y, 0);
                    switch (mark[x, y])
                    {
                        case 0:
                            PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Brick_wall_m"),
                                 mazePos + pos, Quaternion.identity, 0);   //Brick
                            break;
                        case 1:
                            PhotonNetwork.Instantiate(Path.Combine("Prefabs", "coin_m"),
                                mazePos + pos, Quaternion.identity, 0);   //coin
                            break;
                        case 2:
                            PhotonNetwork.Instantiate(Path.Combine("Prefabs", "chest_box_m"),
                                mazePos + pos, Quaternion.identity, 0);   //chest box
                            break;
                        case 3:
                            if (counter < 3)
                            {
                                PhotonNetwork.Instantiate(Path.Combine("Prefabs", "coin_m"),
                                        mazePos + pos, Quaternion.identity, 0);   //coin
                                PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Furry_em"),
                                        mazePos + pos, Quaternion.identity, 0);   //minion
                                counter += 1;
                            }
                            else
                            {
                                PhotonNetwork.Instantiate(Path.Combine("Prefabs", "coin_m"),
                                        mazePos + pos, Quaternion.identity, 0);   //coin
                                PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Furry_m"),
                                        mazePos + pos, Quaternion.identity, 0);   //minion
                                counter += 1;
                            }
                            break;
                        default:
                            break;
                    }    
                }
            }
        }
    }
}
