using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera gameCamera; 
    [SerializeField] GridManager grid; 
    [SerializeField] float panSpeed = 20f; 
    [SerializeField] float zoomSpeed = 10f; 
    [SerializeField] Vector2 zoomLimit = new Vector2(5f, 50f); 
    [SerializeField] bool isOrthographic = true; 

    
    private Vector2 panLimit; 

    void Start()
    {
        if (gameCamera == null)
        {
            gameCamera = Camera.main; 
        }

        if (grid == null)
        {
            Debug.LogError("GridSystem is not assigned. Please assign it in the inspector.");
            return;
        }

        SetupCamera();
    }

    void Update()
    {
        if (gameCamera == null || grid == null) return;

        HandleKeyboardInput();
       
        HandleZoom();
    }

  
    private void SetupCamera()
    {
       
        float gridCenterX = (grid.width - 1) * grid.tileSize / 2f;
        float gridCenterY = (grid.height - 1) * grid.tileSize / 2f;
        Vector3 gridCenter = new Vector3(gridCenterX, 0, gridCenterY);
  
        gameCamera.transform.position = new Vector3(gridCenter.x, 70f, gridCenter.z - 10f); 
        gameCamera.transform.LookAt(gridCenter);

       
        panLimit = new Vector2(grid.width * grid.tileSize, grid.height * grid.tileSize);
    }

    private void HandleKeyboardInput()
    {
        Vector3 position = gameCamera.transform.position;

        if (Input.GetKey("w") || Input.GetKey(KeyCode.UpArrow))
        {
            position.z += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow))
        {
            position.z -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow))
        {
            position.x -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow))
        {
            position.x += panSpeed * Time.deltaTime;
        }

       
        position.x = Mathf.Clamp(position.x, 0, panLimit.x);
        position.z = Mathf.Clamp(position.z, 0, panLimit.y);

        gameCamera.transform.position = position;
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (isOrthographic)
        {
           
            gameCamera.orthographicSize -= scroll * zoomSpeed;
            gameCamera.orthographicSize = Mathf.Clamp(gameCamera.orthographicSize, zoomLimit.x, zoomLimit.y);
        }
        else
        {
            
            gameCamera.fieldOfView -= scroll * zoomSpeed;
            gameCamera.fieldOfView = Mathf.Clamp(gameCamera.fieldOfView, zoomLimit.x, zoomLimit.y);
        }
    }


}
