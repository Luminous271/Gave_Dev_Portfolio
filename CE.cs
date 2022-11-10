using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CE : MonoBehaviour
{
    public Vector2 damageForce;
    float damageDirectionX;
    float damageDirectionY;
    public Movement playercontroller;
    void Start(){
        playercontroller = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        //Hello I am asd 
        if(playercontroller.gameObject.transform.position.x - transform.position.x > 0){
            damageDirectionX = 1;
        }
        else{
            damageDirectionX = -1;
        }
        if(playercontroller.gameObject.transform.position.y - transform.position.y > 0){
            damageDirectionY = 1;
        }
        else{
            damageDirectionY = -1;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if(collision.gameObject.name.Equals("Player"))
        {
            damageForce = damageForce * new Vector2(damageDirectionX, damageDirectionY);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(damageForce, ForceMode2D.Impulse);
        }
    }
}
