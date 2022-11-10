using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GripController : MonoBehaviour
{
    private Movement playerController;
    [SerializeField] public float playerGrip = 100.0f;
    [SerializeField] private float maxGrip = 100.0f;
    [HideInInspector]public bool isGripping = false;
    [HideInInspector]public bool hasRegenerated = true;
    [Range(0,50)][SerializeField]private float drain = 0.5f;
    [Range(0,50)][SerializeField]private float regen = 0.5f;
    [SerializeField]private Image gripProgessUI = null;
    [SerializeField]private CanvasGroup gripCanvasGroup = null;

    void Start(){
        playerController = GetComponent<Movement>();
    }
    void Update(){
        this.isGripping = playerController.isGripping;
        if(isGripping == false && playerController.canRegen){            
            if(playerGrip <= maxGrip){
                hasRegenerated = false;
                UpdateGrip(1);
                if(playerController.grounded)
                    playerGrip += regen * Time.deltaTime; 
            }
            else{
                hasRegenerated = true;
                UpdateGrip(0);
            }
        }
        if(isGripping == true){
            hasRegenerated = false;
            UpdateGrip(1);
            Gripping();
        }
    }
    public void Gripping(){
        UpdateGrip(1);
        if(playerGrip >= 0.1){
            playerGrip -= drain * Time.deltaTime;
        }
        if(playerGrip <= 0.1){
            playerController.LetGo();
        }
    }
    public void UpdateGrip(int value){
            gripProgessUI.fillAmount = playerGrip/maxGrip;
            if(value == 0){
                gripCanvasGroup.alpha = 0;
            }
            else{
                gripCanvasGroup.alpha = 1;
            }
    }
}
