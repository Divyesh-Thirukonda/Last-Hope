using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntelScript : MonoBehaviour
{
    public bool ObtainedIntel = false;
    public GameObject elevatedBridge;

    void Start()
    {
        elevatedBridge = GameObject.Find("ELEVATEDBRIDGE");
        elevatedBridge.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown("e") && ObtainedIntel == true) {
            RaycastHit hit;
            if (Physics.Raycast(GameObject.Find("player").GetComponent<AdvancedMovement>().cam.transform.position, GameObject.Find("player").GetComponent<AdvancedMovement>().cam.transform.forward, out hit, 10)){
                if (hit.collider.tag == "Encryptor") {
                    Debug.Log("eeeeeee");
                    GameObject.Find("player").GetComponent<AdvancedMovement>().CollectedAudio.Play(); // Replace collected audio with instructions
                    GameObject.Find("player").GetComponent<PlayerCombat>().coins += 50;
                    elevatedBridge.SetActive(true);
                }
            }
        }
    }

    void OnCollisionEnter(Collision collisionInfo) {
        if(collisionInfo.collider.tag == "Player"){
            ObtainedIntel = true;
            gameObject.transform.position += new Vector3(0, -100, 0);
            GameObject.Find("player").GetComponent<AdvancedMovement>().CollectedAudio.Play(); // Replace collected audio with instructions
        }
    }
}
