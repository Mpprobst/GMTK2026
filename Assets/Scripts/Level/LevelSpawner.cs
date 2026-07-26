using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    [SerializeField] protected GameObject[] tilePrefabs;
    [SerializeField] private int rows, cols;
    [Range(0, 1)] [SerializeField] private float tilePct = 0.1f;
    [Range(0, 2)] [SerializeField] private float itemPct = 0.1f;
    [SerializeField] private float tileSize = 1;
    [SerializeField] private Transform tileContainer;

    [Header("WFC")]
    [SerializeField] protected TileData[] tileData;
    private List<TILE_TYPE>[,] grid;
    private bool[,] visited;
    Dictionary<TILE_TYPE, TileData> tileDataDict = new Dictionary<TILE_TYPE, TileData>();


    private List<GameObject> tiles = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tileDataDict.Clear();
        foreach (var t in tileData)
        {
            if (tileDataDict.ContainsKey(t.tileType))
            {
                Debug.LogWarning("duplicate tile data defined");
                continue;
            }
            tileDataDict.Add(t.tileType, t);
        }

        SpawnLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Debug.isDebugBuild)
            SpawnLevel();
    }

    public void SpawnLevel()
    {
        foreach (var t in tiles)
            Destroy(t);

        tiles.Clear();

        Vector3 corner = tileContainer.transform.position + new Vector3(-(rows-0.5f) / 2f * tileSize, 0, -(cols-0.5f) / 2f * tileSize);
        WaveFunctionCollapse();

        int cellct = rows * cols;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                // spawn the associated grid
                TILE_TYPE tileType = TILE_TYPE.FLAT;
                if (grid[i,j] != null)
                    tileType = grid[i, j][0];
                var tile = tileDataDict[tileType];

                // spawn a tile at random
                Vector3 pos = corner + new Vector3(i, 0, j) * tileSize;
                GameObject spawnedTile = (GameObject)Instantiate(tile.prefab, pos, Quaternion.identity, tileContainer);
                spawnedTile.transform.localScale = tileSize * Vector3.one;
                spawnedTile.name = tile.name;
                tiles.Add(spawnedTile);

                spawnedTile.GetComponent<Tile>().Initialize(itemPct);
            }
        }

        // TODO: spawn wall tiles around the outside
        // TODO: spawn an oasis
    }

    public void WaveFunctionCollapse()
    {
        // construct a grid of TILE_TYPE and collapse them one at a time using rules
        // use a lookup to the scriptable object to get the constraints when needed
        grid = new List<TILE_TYPE>[rows,cols];
        visited = new bool[rows,cols];
        
        int x = Random.Range(rows/2, rows);
        int y = Random.Range(rows/2, cols);
        int gridSize = rows * cols;
        // force first tile to be the oasis
        Collapse(x, y, TILE_TYPE.OASIS);

        // do this while all tiles have not been explored
        for (int i = 0; i < gridSize; i++)
        {
            // i could track min entropy to know what is the lowest thing I have seen, but when the last of that entropy level becomes 0 how do we know???
            // i don't think I actually care lets just make something super inefficient
            List<System.Tuple<int, int>> potentialCells = new List<System.Tuple<int, int>>();
            int minEntropy = int.MaxValue;
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    int entropy = 0;
                    if (grid[r, c] == null)
                        entropy = tileDataDict.Count;
                    else if (visited[r, c])
                        continue;

                    else
                        entropy = grid[r, c].Count;
                    if (entropy < minEntropy)
                    {
                        potentialCells.Clear();
                        minEntropy = entropy;
                    }

                    if (entropy == minEntropy)
                        potentialCells.Add(new System.Tuple<int, int>(r, c));
                }
            }

            if (potentialCells.Count > 0)
            {
                var cell = potentialCells[Random.Range(0, potentialCells.Count)];
                x = cell.Item1;
                y = cell.Item2;
            }
            else
            {
                x = Random.Range(0, rows);
                y = Random.Range(0, cols);
            }

            Collapse(x, y);
            
        }
        
    }

    private void Collapse(int x, int y, TILE_TYPE forceType=(TILE_TYPE)(-1))
    {
        if (grid[x, y] == null)
            grid[x, y] = tileDataDict.Keys.ToList();

        // when picking a tile, leave only one tile option left so we know what to spawn
        //Debug.Log($"{grid.GetLength(0)}x{grid.GetLength(1)}  {x},{y} = {grid[x,y].Count}");
        TILE_TYPE randTile = TILE_TYPE.FLAT;
        if (grid[x, y].Count > 0)
            randTile = grid[x, y][Random.Range(0, grid[x, y].Count)];
        if (grid[x, y].Contains(TILE_TYPE.FLAT) && Random.Range(0f, 1f) > tilePct)
            randTile = TILE_TYPE.FLAT;

        if (forceType >= 0)
            randTile = forceType;

        grid[x, y].Clear();
        grid[x, y].Add(randTile);
        visited[x, y] = true;

        // get all adjacent tiles and apply the constraints
        TileData.TileConstraint[] constraints = tileDataDict[randTile].constraints;
        foreach (var dir in TileData.DirectionVectors)
        {
            var constraint = constraints[(int)dir.Key];

            int adj_x = x + dir.Value.x;
            int adj_y = y + dir.Value.y;
            if (adj_x >= 0 && adj_x < rows && adj_y >= 0 && adj_y < cols)
            {
                // apply constraint
                if (grid[adj_x, adj_y] == null)
                {
                    grid[adj_x, adj_y] = constraint.allowedTypes.ToList();
                }
                else if (!visited[adj_x, adj_y]) // if we have 1 item we have selected our tile
                {
                    // logical AND this list with the constraint
                    for (int t = grid[adj_x, adj_y].Count - 1; t >= 0; t--)
                    {
                        if (!constraint.allowedTypes.Contains(grid[adj_x, adj_y][t]) && grid[adj_x, adj_y][t] != TILE_TYPE.FLAT)
                            grid[adj_x, adj_y].RemoveAt(t);
                    }
                }

            }
        }
    }
}
