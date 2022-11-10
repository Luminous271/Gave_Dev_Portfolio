using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraController : MonoBehaviour
{

    private Movement playerController;
    public BulletMovemet prefab;
    [SerializeField]private GameObject aura = null;
    [SerializeField]private GameObject arrow = null;
    [SerializeField]private Transform fireLocation = null;
    public float defaultAlpha;
    public float maxAlpha = 255f; 
    public bool showAura = false;
    public float alphFade;
    Color e;
    public float currAlpha;
    private Camera mainCam;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<Movement>();
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }   
    // Update is called once per frame
    void Update()
    {
        e = aura.GetComponent<SpriteRenderer>().color;
        currAlpha = e.a;
        if(Input.GetKeyDown(KeyCode.E))
            showAura = !showAura;
        if(showAura)
            UpdateAura(defaultAlpha);
        else if(!showAura)
            UpdateAura(0);
        if(Input.GetKeyDown(KeyCode.Mouse1)){
            Fire();
        }
        aura.GetComponent<SpriteRenderer>().color = e;
        //arrow.GetComponent<SpriteRenderer>().color = e; 
    }
    void Breeched(){
        UpdateAura(255);
    }
    public void UpdateAura(float finalp){
        float currA = e.a;
        if(currA > finalp - 0.05f){
            e.a = currA - alphFade * Time.deltaTime;
        }
        else if(currA < finalp + 0.05f){
            e.a = currA + alphFade * Time.deltaTime;
        }
    }
    void Fire(){
        Instantiate(prefab, fireLocation.position, fireLocation.rotation);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            UpdateAura(maxAlpha);
        }
    }

}
