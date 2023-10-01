using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadBlockManager : MonoBehaviour
{
    bool obsticalsSpawned = false;
    public Transform nextRoadSpawnPoint;
    public GameObject roadBlockPrefab;
    public List<Transform> spawnPointsParents;
    

    public GameObject obsticalPrefab;


    void Update()
    {
        //randomly spawning obsticals
        if (!obsticalsSpawned)
        {
            foreach (Transform pointsParent in spawnPointsParents)
            {
               Transform randomPoint = pointsParent.transform.GetChild((int)Random.Range(0,pointsParent.transform.childCount - 1));
               GameObject newObstical = Instantiate(obsticalPrefab, randomPoint.position, Quaternion.identity);
               newObstical.transform.SetParent(transform, true);
            }
            obsticalsSpawned = true;
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Instantiate(roadBlockPrefab, nextRoadSpawnPoint.position, Quaternion.identity);
        
    }
    private void OnTriggerExit2D(Collider2D other)
    {
       Destroy(gameObject);
    }
}
