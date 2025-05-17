using UnityEngine;
using System.Collections.Generic;

public class RespawnObject : MonoBehaviour
{
    public GameObject objectPrefab;
    private List<GameObject> spawnedObjects = new List<GameObject>();

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject newObj = Instantiate(objectPrefab, mousePos, Quaternion.identity);
            spawnedObjects.Add(newObj);
        }

        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            foreach (GameObject obj in spawnedObjects)
            {
                if (obj != null)
                    obj.SetActive(false); 
            }

        
        }
    }
}
