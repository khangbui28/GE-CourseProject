using UnityEngine;

public class TowerFactory : MonoBehaviour
{
    [SerializeField] GameObject basicTowerPrefab;
    [SerializeField] GameObject advancedTowerPrefab;

    public GameObject CreateTower(string towerType, Vector3 position)
    {
        GameObject tower = null;

        switch (towerType)
        {
            case "Basic":
                tower = Instantiate(basicTowerPrefab, position, Quaternion.identity);
                break;
            case "Advanced":
                tower = Instantiate(advancedTowerPrefab, position, Quaternion.identity);
                break;
        }

        return tower;
    }
}
