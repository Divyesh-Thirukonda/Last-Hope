using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MaterialScript : MonoBehaviour
{
    void OnCollisionEnter(Collision collisionInfo) {
        if(collisionInfo.collider.tag == "Player"){
            int i = 0;
            //For every itemslot in the MatsItems; if it's empty, make the iterationCount of the MatsItems to the game object.
            foreach(GameObject item in GameObject.Find("player").GetComponent<PlayerCombat>().MatsItems) {
                if(item == null) {
                    GameObject.Find("player").GetComponent<PlayerCombat>().MatsItems[i] = gameObject;
                    i=0;
                    GetComponent<BoxCollider>().enabled = false;
                    GetComponentInChildren<BoxCollider>().enabled = false;
                    gameObject.SetActive(false);
                    return;
                }
                i++;
            }
        }
    }
}
