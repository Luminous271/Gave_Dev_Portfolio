using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovemet : MonoBehaviour
{
    public float speed = 10f;
    public float deletionTime;
    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * Time.deltaTime * speed;
        deletionTime--;
        if(deletionTime < 0)
            Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            
            Destroy(collision.gameObject);
        }
        
    }
}
