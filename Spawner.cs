using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Spawnable spawn;
    public float spawnRate;
    public int maxSpawns;
    float save;
    public float spawnableRange;
    public float spawnerCountDown;
    public LayerMask playerLayer;
    bool enter;
    void Start(){
        save = spawnRate;
        enter = false;
    }
    void Update()
    {
        Collider2D Player = Physics2D.OverlapCircle(transform.position, spawnableRange, playerLayer);
        if (Player)
        {
            enter = true;
        }
        if (enter == true && spawnerCountDown >= 0)
            spawnerCountDown = spawnerCountDown - Time.deltaTime;
        if(spawnerCountDown <= 0.5f)
            spawnRate = spawnRate - Time.deltaTime;
        if (spawnRate < 0 && maxSpawns > 0){
            Instantiate(spawn, transform.position, transform.rotation);   
            spawnRate = save; 
            maxSpawns--;
        }
        if(maxSpawns == 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (spawnableRange == null)
            return;
        Gizmos.DrawWireSphere(transform.position, spawnableRange);
    }
}
