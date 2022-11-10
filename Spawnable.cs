using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnable : MonoBehaviour
{
    public GroundEnemyAI AI;
    public float deSpawnTime = 100f;
    void Start()
    {
        AI = GetComponent<GroundEnemyAI>();
        AI.target = GameObject.Find("Player").transform;
    }
    // Update is called once per frame
    void Update()
    {
        
        deSpawnTime = deSpawnTime - Time.deltaTime;
        if(deSpawnTime < 0)
            Destroy(gameObject);
    }

    
}
