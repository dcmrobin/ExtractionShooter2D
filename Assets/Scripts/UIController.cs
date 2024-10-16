using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIController : MonoBehaviour
{
    [Header("Crafting UI")]
    public Slider damageUI;
    public Slider spreadUI;
    public Slider recoilUI;
    public Slider handlingUI;
    public TMP_Text ammunitionUI;
    public GameObject craftedGun;
    public GameObject gunPrefab;

    //[Header("Inventory UI")]
    //

    private void Update() {
        damageUI.transform.Find("Value").GetComponent<TMP_Text>().text = damageUI.value.ToString();
        spreadUI.transform.Find("Value").GetComponent<TMP_Text>().text = spreadUI.value.ToString();
        recoilUI.transform.Find("Value").GetComponent<TMP_Text>().text = recoilUI.value.ToString();
        handlingUI.transform.Find("Value").GetComponent<TMP_Text>().text = handlingUI.value.ToString();
    }
}
