using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltPopulateScene : MonoBehaviour
{
    [Header("Population Settings")]
    [SerializeField] private GameObject prefabToPopulate;
    [SerializeField] private int numberOfObjects = 10;
    [SerializeField] private Vector3 spawnAreaSize = new Vector3(10f, 0f, 10f);
    
    [Header("Ground Detection")]
    [SerializeField] private bool useGroundDetection = true;
    [SerializeField] private LayerMask groundLayerMask = -1;
    [SerializeField] private float raycastDistance = 100f;
    [SerializeField] private float heightOffset = 0f;
    [SerializeField] private Transform groundReference;
    
    [Header("Population Pattern")]
    [SerializeField] private PopulationPattern pattern = PopulationPattern.Random;
    [SerializeField] private float gridSpacing = 2f;
    [SerializeField] private bool populateOnStart = true;
    
    [Header("Runtime Controls")]
    [SerializeField] private KeyCode populateKey = KeyCode.P;
    [SerializeField] private KeyCode clearKey = KeyCode.C;
    
    private List<GameObject> populatedObjects = new List<GameObject>();
    
    public enum PopulationPattern
    {
        Random,
        Grid,
        Circle,
        Line
    }

    void Start()
    {
        if (populateOnStart)
        {
            PopulateScene();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(populateKey))
        {
            PopulateScene();
        }
        
        if (Input.GetKeyDown(clearKey))
        {
            ClearPopulatedObjects();
        }
    }
    
    /// <summary>
    /// Populates the scene with duplicated objects based on the selected pattern
    /// </summary>
    public void PopulateScene()
    {
        if (prefabToPopulate == null)
        {
            Debug.LogWarning("No prefab assigned to populate!");
            return;
        }
        
        ClearPopulatedObjects();
        
        switch (pattern)
        {
            case PopulationPattern.Random:
                populateRandomly();
                break;
            case PopulationPattern.Grid:
                populateInGrid();
                break;
            case PopulationPattern.Circle:
                populateInCircle();
                break;
            case PopulationPattern.Line:
                populateInLine();
                break;
        }
        
        Debug.Log($"Populated scene with {populatedObjects.Count} objects using {pattern} pattern");
    }
    
    /// <summary>
    /// Populates objects randomly within the spawn area
    /// </summary>
    private void populateRandomly()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            Vector3 randomPosition = getRandomPosition();
            createObjectAtPosition(randomPosition, i);
        }
    }
    
    /// <summary>
    /// Populates objects in a grid pattern
    /// </summary>
    private void populateInGrid()
    {
        int gridSize = Mathf.CeilToInt(Mathf.Sqrt(numberOfObjects));
        int objectsCreated = 0;
        
        for (int x = 0; x < gridSize && objectsCreated < numberOfObjects; x++)
        {
            for (int z = 0; z < gridSize && objectsCreated < numberOfObjects; z++)
            {
                Vector3 gridPosition = transform.position + new Vector3(
                    (x - gridSize / 2f) * gridSpacing,
                    0f,
                    (z - gridSize / 2f) * gridSpacing
                );
                
                createObjectAtPosition(gridPosition, objectsCreated);
                objectsCreated++;
            }
        }
    }
    
    /// <summary>
    /// Populates objects in a circular pattern
    /// </summary>
    private void populateInCircle()
    {
        float radius = Mathf.Max(spawnAreaSize.x, spawnAreaSize.z) / 2f;
        
        for (int i = 0; i < numberOfObjects; i++)
        {
            float angle = (360f / numberOfObjects) * i * Mathf.Deg2Rad;
            Vector3 circlePosition = transform.position + new Vector3(
                Mathf.Cos(angle) * radius,
                0f,
                Mathf.Sin(angle) * radius
            );
            
            createObjectAtPosition(circlePosition, i);
        }
    }
    
    /// <summary>
    /// Populates objects in a line pattern
    /// </summary>
    private void populateInLine()
    {
        Vector3 startPos = transform.position - (Vector3.right * spawnAreaSize.x / 2f);
        Vector3 endPos = transform.position + (Vector3.right * spawnAreaSize.x / 2f);
        
        for (int i = 0; i < numberOfObjects; i++)
        {
            float t = numberOfObjects > 1 ? (float)i / (numberOfObjects - 1) : 0f;
            Vector3 linePosition = Vector3.Lerp(startPos, endPos, t);
            
            createObjectAtPosition(linePosition, i);
        }
    }
    
    /// <summary>
    /// Creates an object at the specified position with optional randomization
    /// </summary>
    private void createObjectAtPosition(Vector3 position, int index)
    {
        // Adjust position based on ground detection
        Vector3 finalPosition = getGroundPosition(position);
        
        GameObject newObject = Instantiate(prefabToPopulate, finalPosition, Quaternion.identity);
        
        // Set parent and name
        newObject.transform.SetParent(this.transform);
        newObject.name = $"{prefabToPopulate.name}_Populated_{index:000}";
        
        populatedObjects.Add(newObject);
    }
    
    /// <summary>
    /// Gets a random position within the spawn area
    /// </summary>
    private Vector3 getRandomPosition()
    {
        return transform.position + new Vector3(
            Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f),
            Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f),
            Random.Range(-spawnAreaSize.z / 2f, spawnAreaSize.z / 2f)
        );
    }
    
    /// <summary>
    /// Determines the ground position for object placement
    /// </summary>
    private Vector3 getGroundPosition(Vector3 desiredPosition)
    {
        if (!useGroundDetection)
        {
            return desiredPosition;
        }
        
        // If ground reference is assigned, use its Y position
        if (groundReference != null)
        {
            return new Vector3(desiredPosition.x, groundReference.position.y + heightOffset, desiredPosition.z);
        }
        
        // Use raycast to find ground
        Vector3 raycastStart = new Vector3(desiredPosition.x, desiredPosition.y + raycastDistance / 2f, desiredPosition.z);
        RaycastHit hit;
        
        if (Physics.Raycast(raycastStart, Vector3.down, out hit, raycastDistance, groundLayerMask))
        {
            return new Vector3(desiredPosition.x, hit.point.y + heightOffset, desiredPosition.z);
        }
        
        // If no ground found and ground reference exists, use that
        if (groundReference != null)
        {
            return new Vector3(desiredPosition.x, groundReference.position.y + heightOffset, desiredPosition.z);
        }
        
        // Fallback to original position
        Debug.LogWarning($"No ground found for position {desiredPosition}. Consider assigning a Ground Reference or adjusting Ground Layer Mask.");
        return desiredPosition;
    }
    
    /// <summary>
    /// Clears all previously populated objects
    /// </summary>
    public void ClearPopulatedObjects()
    {
        foreach (GameObject obj in populatedObjects)
        {
            if (obj != null)
            {
                if (Application.isPlaying)
                {
                    Destroy(obj);
                }
                else
                {
                    DestroyImmediate(obj);
                }
            }
        }
        populatedObjects.Clear();
    }
    
    /// <summary>
    /// Draws gizmos to visualize the spawn area in the Scene view
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, spawnAreaSize);
        
        // Draw different pattern previews
        switch (pattern)
        {
            case PopulationPattern.Circle:
                Gizmos.color = Color.cyan;
                float radius = Mathf.Max(spawnAreaSize.x, spawnAreaSize.z) / 2f;
                Gizmos.DrawWireSphere(transform.position, radius);
                break;
        }
    }
}
