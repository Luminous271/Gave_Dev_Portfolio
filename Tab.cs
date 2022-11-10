using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tab : MonoBehaviour
{
  
    public GameObject optionsMenu;
    public GameObject Backpack;
    public GameObject controls;
    public GameObject map;

    // Update is called once per frame
    void Update () {
        // Reverse the active state every time escape is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Check whether it's active / inactive 
            bool isActive = optionsMenu.activeSelf;
            
            map.SetActive(false);
            optionsMenu.SetActive(!isActive);
            controls.SetActive(false);
        }
        if(Input.GetKeyDown(KeyCode.B)){
            bool bisActive = Backpack.activeSelf;
 
            Backpack.SetActive(!bisActive);
        }
        if(Input.GetKeyDown(KeyCode.M)){
            bool mapper = map.activeSelf;
            map.SetActive(!mapper);
            controls.SetActive(false);
            optionsMenu.SetActive(false);
        }
    }

}
