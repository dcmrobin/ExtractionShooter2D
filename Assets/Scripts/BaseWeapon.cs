using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    [Header("Gun params")]
    public int damage;
    public int spread;
    public int recoil;
    public int handling;
    public int ammunition;

    private UIController uiController;
    // Start is called before the first frame update
    void Start()
    {
        uiController = GameObject.FindGameObjectWithTag("UIcontroller").GetComponent<UIController>();
        uiController.damageUI.value = damage;
        uiController.spreadUI.value = spread;
        uiController.recoilUI.value = recoil;
        uiController.handlingUI.value = handling;
        uiController.ammunitionUI.text = ammunition.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
