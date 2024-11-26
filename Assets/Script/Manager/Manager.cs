using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager instance;

    public TowerFactory towerFactory;
    public Camera mainCamera;

    private Stack<ICommand> commandStack = new Stack<ICommand>();
    private Stack<ICommand> redoStack = new Stack<ICommand>();

    private string selectedTowerType = "Basic";

    public static Manager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<Manager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject(nameof(Manager));
                    instance = obj.AddComponent<Manager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        HandleMouseInput();
        HandleUndoRedo();
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GridTile tile = hit.collider.GetComponent<GridTile>();
                if (tile != null && tile.tileType == TileType.Empty)
                {
                    int towerCost = UIManager.Instance.GetTowerCost(selectedTowerType);
                    if (UIManager.Instance.playerMoney >= towerCost)
                    {
                        Vector3 towerPosition = tile.transform.position + Vector3.up * 0.5f;
                        ICommand placeTowerCommand = new TowerPlacement(towerPosition, tile, selectedTowerType, towerCost);
                        ExecuteCommand(placeTowerCommand);
                        UIManager.Instance.DeductMoney(towerCost);
                    }
                    else
                    {
                        Debug.Log("Not enough money to place this tower!");
                    }
                }
            }
        }
    }

 
    private void HandleUndoRedo()
    {
        if (Input.GetKeyDown(KeyCode.Z)) UndoLastCommand();
        if (Input.GetKeyDown(KeyCode.Y)) RedoLastCommand();
       
    }

 
    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        commandStack.Push(command);
        redoStack.Clear();
    }

  
    public void UndoLastCommand()
    {
        if (commandStack.Count > 0)
        {
            ICommand command = commandStack.Pop();
            command.Undo();
            redoStack.Push(command);
        }
    }

   
    public void RedoLastCommand()
    {
        if (redoStack.Count > 0)
        {
            ICommand command = redoStack.Pop();
            command.Execute();
            commandStack.Push(command);
        }
    }

  
    public void SetSelectedTowerType(string towerType)
    {
        selectedTowerType = towerType;
    }

}
