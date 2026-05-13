using System.Collections.Generic;

using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    [SerializeField] private Transform levelPart;
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 endPos;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 nextSpawnPos;

    [SerializeField] private float spawnDistance = 10f;
    [SerializeField] private float deleteDistance = 10f;

    [SerializeField] private List<Transform> platforms;
    [SerializeField] private int platformCount = 5;

    

    private void Start()
    {

        nextSpawnPos = platforms[0].Find("EndPos").transform.position;

        for(int i = 0; i < platformCount; i++)
        {
            Transform p=Instantiate(platforms[Random.Range(0,platforms.Count)],nextSpawnPos, Quaternion.identity,transform);
            nextSpawnPos = p.Find("EndPos").transform.position;
            //p.gameObject.SetActive(false);
        }

    }
    void Update()
    {
      
    }
}
