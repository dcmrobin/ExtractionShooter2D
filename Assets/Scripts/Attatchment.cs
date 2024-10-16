using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Attatchment : MonoBehaviour
{
    public enum TypeOfAttachment {Stock, Grip, Magazine, Scope, Barrel}
    public TypeOfAttachment attachmentType;
    public UIController uiController;
    public Transform installTarget;
    bool installed;
    Transform originalPos;
    public Attributes attributes = new Attributes();

    private void Start() {
        uiController = GameObject.FindGameObjectWithTag("UIcontroller").GetComponent<UIController>();
        name = GetComponent<Image>().sprite.name;
        originalPos = transform.parent;
    }

    public void Install()
    {
        if (!installed && installTarget.childCount <= 0)
        {
            uiController.damageUI.value += attributes.damageIncrease;
            uiController.spreadUI.value += attributes.spreadIncrease;
            uiController.recoilUI.value += attributes.recoilIncrease;
            uiController.handlingUI.value += attributes.handlingIncrease;
            if (attachmentType == TypeOfAttachment.Magazine)
            {
                uiController.ammunitionUI.text = attributes.ammunition.ToString();
            }

            for (int i = 0; i < uiController.craftedGun.transform.childCount; i++)
            {
                if (uiController.craftedGun.transform.GetChild(i).name == attachmentType.ToString())
                {
                    uiController.craftedGun.transform.GetChild(i).gameObject.SetActive(true);
                    uiController.craftedGun.transform.GetChild(i).GetComponent<Image>().sprite = GetComponent<Image>().sprite;
                    uiController.craftedGun.transform.GetChild(i).GetComponent<Image>().SetNativeSize();
                }
            }

            transform.SetParent(installTarget);
            transform.position = installTarget.position;
            installed = true;
        }
        else if (installed)
        {
            uiController.damageUI.value -= attributes.damageIncrease;
            uiController.spreadUI.value -= attributes.spreadIncrease;
            uiController.recoilUI.value -= attributes.recoilIncrease;
            uiController.handlingUI.value -= attributes.handlingIncrease;
            if (attachmentType == TypeOfAttachment.Magazine)
            {
                uiController.ammunitionUI.text = "0";
            }

            for (int i = 0; i < uiController.craftedGun.transform.childCount; i++)
            {
                if (uiController.craftedGun.transform.GetChild(i).GetComponent<Image>().sprite.name == GetComponent<Image>().sprite.name)
                {
                    uiController.craftedGun.transform.GetChild(i).gameObject.SetActive(false);
                }
            }

            transform.SetParent(originalPos);
            transform.position = originalPos.position;
            installed = false;
        }
        else if (installTarget.GetChild(0) != gameObject)
        {
            return;
        }
    }
}

[Serializable]
public class Attributes
{
    public int damageIncrease;
    public int spreadIncrease;
    public int recoilIncrease;
    public int handlingIncrease;
    public int ammunition;
}
