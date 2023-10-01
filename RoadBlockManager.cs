using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadBlockManager : MonoBehaviour
{
    bool obsticalsSpawned = false;
    public Transform nextRoadSpawnPoint;
    public GameObject roadBlockPrefab;
    public 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!obsticalsSpawned)
        {
            //spawn obsticals here
            obsticalsSpawned = true;
        }

    }
}
