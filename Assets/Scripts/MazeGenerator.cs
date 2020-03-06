using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Linq;
using Assets.Scripts;

public class MazeGenerator : MonoBehaviour
{
    public static MazeGenerator Instance;
    public GameObject BrickPrefab; 
    public GameObject CoinPrefab; 
    public GameObject ChestPrefab;
    public GameObject PlayerPrefab;
    public GameObject MinionPrefab;
    public Vector3 mazePos; //positon of maze
    public int row; //An odd number
    public int col; //An odd number
    public int numOfCoins = 85; //Fix number of coins
    private int maxScore = 85 * 10 + 6 * 30;

    //For coins(not real): real index (odd x, odd y)
    public bool[,] visited = new bool[50, 50]; //[x][y]

    //Full maze
    //0: brick, 1: coin, 2: chest, 3: minion, 4: player
    public int[,] mark = new int[50, 50]; //[x][y]

    public int count = 0;

    public Cell[] chestCells;

    public Cell[] minionCells;

    public struct Cell {
        public int x;  //column
        public int y;  //row

        public Cell(int x, int y) {
            this.x = x;
            this.y = y;
        }
    };

    void Awake()
    {
        Instance = this;
    }

        public int getMaxScore()
    {
        return maxScore;
    }

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
            new Cell(9, row - 2)
        };
    }
    public bool CheckCell(Cell cell) {
        if (cell.x < 0 || cell.y < 0 || cell.x > col / 2 - 1 || cell.y > row / 2 - 1) {
            return false;
        }
        return true;
    }

    
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

    public ArrayList stack = new ArrayList();
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

    // Start is called before the first frame update
    
    void Start()
    {
        GenerateMaze();
        StartCoroutine(ConnectionManager.GetQuestions(10, 1));
    }
    
    /*
    private void OnEnable()
    {
        GenerateMaze();
    }
    */

    // Update is called once per frame
    void Update()
    {
        
    }
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
            mark[minion.x, minion.y] = 3;  //chest
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

        //Player
        mark[col / 2, row / 2] = 4;

        PlaceObjects();
    }
    void PlaceObjects() {
        for (int x = 0 ; x < col ; x++) {
            for (int y = 0 ; y < row ; y++) {
                GameObject gameObject = null;
                GameObject minion = null;
                switch (mark[x, y]) {
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
                        gameObject = Instantiate(CoinPrefab) as GameObject;
                        minion = Instantiate(MinionPrefab) as GameObject;
                        break;
                    case 4:
                        gameObject = Instantiate(PlayerPrefab) as GameObject;
                        break;
                }
                
                Transform t = gameObject.transform;

                //t is set a parent of whatever object the script is attached to
                t.SetParent(transform);

                Vector3 pos = new Vector3(x, -y, 0);

                //Set position for t
                t.position =  mazePos + pos;

                if (minion != null) {
                    t = minion.transform;
                    t.SetParent(transform);
                    pos = new Vector3(x, -y, 0);

                    //Set position for t
                    t.position =  mazePos + pos;
                }
            }
        }
    }
}
