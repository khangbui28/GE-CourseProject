using UnityEngine;

public class TowerPlacement : ICommand
{
    private Vector3 position;
    private GameObject tower;
    private GridTile tile;
    private string towerType;
    private int towerCost;

    public TowerPlacement(Vector3 position, GridTile tile, string towerType, int towerCost)
    {
        this.position = position;
        this.tile = tile;
        this.towerType = towerType;
        this.towerCost = towerCost;
    }

    public void Execute()
    {
        if (tile.tileType == TileType.Empty)
        {
            tower = Manager.Instance.towerFactory.CreateTower(towerType, position);
            tile.SetTileType(TileType.Tower);
        }
    }
    
    public void Undo()
    {
        if (tower != null)
        {
            GameObject.Destroy(tower);
            tile.SetTileType(TileType.Empty); 
            UIManager.Instance.AddMoney(towerCost); 
        }
    }
}

public interface ICommand
{
    void Execute();
    void Undo();

    
}
