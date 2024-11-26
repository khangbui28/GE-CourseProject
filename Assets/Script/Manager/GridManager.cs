using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public int width = 10;
    public int height = 5;
    public float tileSize = 1f;

  
    public GameObject tilePrefab;
    public Transform tilesParent;
    public List<Vector2Int> pathCoordinates;

    private GridTile[,] gridTiles;

    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        GenerateGrid();
        HighlightPath();
    }

    private void GenerateGrid()
    {
        if (tilesParent == null)
        {
            GameObject parentObject = new GameObject("GridTilesParent");
            tilesParent = parentObject.transform;
        }

        gridTiles = new GridTile[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x * tileSize, 0, y * tileSize);
                GameObject tileObject = Instantiate(tilePrefab, position, Quaternion.identity, tilesParent);
                tileObject.name = $"Tile_{x}_{y}";

                GridTile gridTile = tileObject.GetComponent<GridTile>();
                gridTile.Initialize(x, y, TileType.Empty);

                gridTiles[x, y] = gridTile;
            }
        }
    }

 
    private void HighlightPath()
    {
        foreach (Vector2Int coord in pathCoordinates)
        {
            if (IsWithinBounds(coord.x, coord.y))
            {
                gridTiles[coord.x, coord.y].SetTileType(TileType.Path);
            }
        }
    }

    public bool IsWithinBounds(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

  
    public GridTile GetTile(int x, int y)
    {
        if (IsWithinBounds(x, y))
        {
            return gridTiles[x, y];
        }
        return null;
    }

    
    public Vector3 GetTileWorldPosition(int x, int y)
    {
        if (IsWithinBounds(x, y))
        {
            return new Vector3(x * tileSize, 0, y * tileSize);
        }
        return Vector3.zero;
    }

    public List<GridTile> GetAllTiles()
    {
        List<GridTile> allTiles = new List<GridTile>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                allTiles.Add(gridTiles[x, y]);
            }
        }
        return allTiles;
    }

}
