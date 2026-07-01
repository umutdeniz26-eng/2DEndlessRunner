using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] levelPrefabs;

    private Vector3 lastEndPos;
    [SerializeField] private Transform startPos;
    [SerializeField] private float spawnDistance=20f;
    [SerializeField] Transform player;

    void Start()
    {
        lastEndPos = startPos.position;

        SpawnLevelPart();
            SpawnLevelPart();
    }

    
    void Update()
    {
        if (Vector3.Distance(player.transform.position,lastEndPos)<spawnDistance)
        {
            SpawnLevelPart();
        }
    }

    private void SpawnLevelPart()
    {
        int randomIndex = Random.Range(0, levelPrefabs.Length);

        GameObject levelToCreate = levelPrefabs[randomIndex];

        GameObject newPart=Instantiate(levelToCreate, lastEndPos, Quaternion.identity);

        Transform endPos = newPart.transform.Find("EndPoint");
        lastEndPos = endPos.position;
    }
}
