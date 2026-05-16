using System.Collections.Generic;

using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    [SerializeField] private Transform[] levelPart;
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 nextPartPosition;
    [SerializeField] private float distanceToSpawn = 30;
    [SerializeField] private float distanceToDelete=30;
    
    private void Start()
    {

        

    }
    void Update()
    {
        GeneratePlatform();
        DeletePlatform();

    }

    

    private void GeneratePlatform()
    {
        while (Vector3.Distance(player.position, nextPartPosition) < distanceToSpawn)
        {

            Transform part = levelPart[Random.Range(0, levelPart.Length)];

            Vector3 newPosition = new Vector3(nextPartPosition.x - part.Find("StartPos").position.x,0,0);

            Transform newPart = Instantiate(part, newPosition, transform.rotation, transform);

            nextPartPosition = newPart.Find("EndPos").position;
        }
    }


    private void DeletePlatform()
    {
        if (transform.childCount > 0)
        {
            Transform partToDelete=transform.GetChild(0);

            if (Vector3.Distance(player.position, partToDelete.Find("EndPos").position) > distanceToDelete)
                Destroy(partToDelete.gameObject);

        }
    }
}
