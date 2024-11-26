using UnityEngine;

public enum TileType { Empty, Path, Tower }

public class GridTile : MonoBehaviour
{
    public int x;
    public int y;
    public TileType tileType;

    private Renderer tileRenderer;
    private GameObject tower; 

    public Color emptyColor = Color.white;
    public Color pathColor = Color.gray;
    public Color towerColor = Color.green;

    private void Awake()
    {
        tileRenderer = GetComponent<Renderer>();
    }


    public void Initialize(int x, int y, TileType type)
    {
        this.x = x;
        this.y = y;
        SetTileType(type);
    }

   
    public void SetTileType(TileType type)
    {
        tileType = type;

        switch (tileType)
        {
            case TileType.Empty:
                tileRenderer.material.color = emptyColor;
                break;
            case TileType.Path:
                tileRenderer.material.color = pathColor;
                break;
            case TileType.Tower:
                tileRenderer.material.color = towerColor;
                break;
        }
    }


    public void SetTower(GameObject newTower)
    {
        tower = newTower;
    }

    public GameObject GetTower()
    {
        return tower;
    }
}