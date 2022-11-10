using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyController : MonoBehaviour
{   
    private Movement playerController;
    public float playerEnergy = 100.0f;
    [SerializeField] private float maxEnergy = 100.0f;
    [SerializeField] public float fireCost = 20;
    [HideInInspector]public bool hasRegenerated = true;
    [Range(0,50)][SerializeField]private float regen = 0.5f;
    [SerializeField] private Image energyProgessUI = null;
    [SerializeField]private CanvasGroup sliderCanvasGroup = null;
    
    void Start(){
        playerController = GetComponent<Movement>();
    }
    void Update(){
        UpdateEnergy(1);
        if(playerEnergy >= maxEnergy){
            hasRegenerated = true;
            UpdateEnergy(0);
        }
        else
            playerEnergy += regen * Time.deltaTime;
        
    }
    public void UpdateEnergy(int value){
            energyProgessUI.fillAmount = playerEnergy/maxEnergy;
            if(value == 0){
                sliderCanvasGroup.alpha = 0;
            }
            else{
                sliderCanvasGroup.alpha = 1; 
            }
    }
}
