using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PotionScript : MonoBehaviour
{
    public int boostValue;

    public Material Material1;
    public Material Material2;
    public Material Material3;
    public Material Material4;

    PotionScript[] Potions;

    void Start() {
        if(gameObject.tag != "Potion") {
            System.Random rnd = new System.Random();
            boostValue = rnd.Next(0, 4);
        }
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(1)) {
            GameObject.Find("player").GetComponent<AdvancedMovement>().UpdateInventory();
            GameObject.Find("player").GetComponent<AdvancedMovement>().CollectedAudio.Play();
            Destroy(gameObject);
        }

        switch(boostValue) {
            case 0:
                if(Input.GetMouseButtonDown(1)) {
                    PlayerStats.PlayerHealth += (50); // Add RNG health from 30-80 heal
                    gameObject.name = "Health Potion";
                }
                transform.GetChild(0).GetComponent<MeshRenderer> ().material = Material1;
                break;
            case 1:
                if(Input.GetMouseButtonDown(1)) {
                    PlayerStats.PlayerSpeed += (5);
                    gameObject.name = "Speed Potion";
                }
                transform.GetChild(0).GetComponent<MeshRenderer> ().material = Material2;
                break;
            case 2:
                if(Input.GetMouseButtonDown(1)) {
                    PlayerStats.PlayerDefense += (10);
                    gameObject.name = "Defense Potion";
                }
                transform.GetChild(0).GetComponent<MeshRenderer> ().material = Material3;
                break;
            case 3:
                if(Input.GetMouseButtonDown(1)) {
                    PlayerStats.PlayerDefense += (10);
                    gameObject.name = "Defense Potion";
                }
                transform.GetChild(0).GetComponent<MeshRenderer> ().material = Material4;
                break;
            default:
                if(Input.GetMouseButtonDown(1)) {
                    PlayerStats.PlayerHealth += (50);
                    gameObject.name = "Health Potion";
                }
                transform.GetChild(0).GetComponent<MeshRenderer> ().material = Material1;
                break;
        }
    }

    void OnCollisionEnter(Collision collisionInfo) {
        if(collisionInfo.collider.tag == "Player"){
            int i = 0;
            //For every item in the inventoryItems; if it's empty, make the iterationCount of the inventoryItems to the game object.
            foreach(GameObject item in GameObject.Find("player").GetComponent<PlayerCombat>().inventoryItems) {
                if(item == null) {
                    GameObject.Find("player").GetComponent<PlayerCombat>().inventoryItems[i] = gameObject;
                    i=0;
                    GetComponent<CapsuleCollider>().enabled = false;
                    GetComponentInChildren<BoxCollider>().enabled = false;
                    gameObject.SetActive(false);
                    return;
                }
                i++;
            }
        }
    }
}
