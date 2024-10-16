using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Interactable : MonoBehaviour
{
    public enum Action {Craft, Inventory}
    public Action actionOnInteraction;
    public string interactionKey = "F";
    public GameObject UItoActivate;

    private bool canInteract;
    private bool interacting;
    private KeyCode interactionKeyCode;
    private PlayerController player;
    private UIController uiController;


    private void Start() {
        interactionKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), interactionKey.ToUpper());
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        uiController = UItoActivate.GetComponentInParent<UIController>();
    }

    private void Update() {
        if (canInteract)
        {
            HandleInteraction();
        }
    }

    public void HandleInteraction()
    {
        if (Input.GetKeyDown(interactionKeyCode))
        {
            UItoActivate.SetActive(true);
            interacting = true;

            player.interacting = true;

            uiController.craftedGun = GameObject.FindGameObjectWithTag("CraftedGun");
        }
    }

    public void StopInteracting()
    {
        Destroy(player.weapon.transform.GetChild(0).gameObject);
        GameObject newWeapon = Instantiate(uiController.gunPrefab);
        newWeapon.name = uiController.craftedGun.name;
        newWeapon.transform.SetParent(player.weapon.transform);
        newWeapon.transform.localPosition = Vector2.zero;

        for (int i = 0; i < uiController.craftedGun.transform.childCount; i++)
        {
            if (uiController.craftedGun.transform.GetChild(i).gameObject.activeSelf)
            {
                GameObject newAttatchment = new GameObject(uiController.craftedGun.transform.GetChild(i).name);
                newAttatchment.transform.SetParent(newWeapon.transform);
                newAttatchment.AddComponent<SpriteRenderer>().sprite = uiController.craftedGun.transform.GetChild(i).GetComponent<Image>().sprite;
                newAttatchment.GetComponent<SpriteRenderer>().sortingOrder = 2;
                newAttatchment.transform.localScale = Vector2.one;
                newAttatchment.transform.localPosition = newWeapon.transform.Find(uiController.craftedGun.transform.GetChild(i).name).localPosition;
            }
        }

        newWeapon.transform.localRotation = Quaternion.Euler(0, 0, 0);

        UItoActivate.SetActive(false);
        interacting = false;

        player.interacting = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!interacting && other.CompareTag("Player"))
        {
            other.transform.Find("TextStuff").gameObject.SetActive(true);
            other.transform.Find("TextStuff").Find("InteractionText").Find("InteractionKey").GetComponent<TMP_Text>().text = "[" + interactionKey.ToUpper() + "]";
            other.transform.Find("TextStuff").Find("InteractionText").GetComponent<TMP_Text>().text = actionOnInteraction.ToString();

            canInteract = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (!interacting && other.CompareTag("Player"))
        {
            other.transform.Find("TextStuff").gameObject.SetActive(false);

            canInteract = false;
        }
    }
}
