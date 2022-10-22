using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Threading;

public class MainUIScript : MonoBehaviour
{
    public Slider healthSlider;
    public Slider staminaSlider;
    public Slider enemyHealthSlider;
    PlayerCombat PlayerCombat;

    GameObject Player;

    public GameObject panel;
    public GameObject panel1;
    public GameObject fullInventoryUI;
    public TMPro.TextMeshProUGUI coinText;
    public GameObject dummy;

    public GameObject[] smitheryItems;
    public GameObject[] marketItems;

    public GameObject item;
    public GameObject item2;

    public static TMPro.TextMeshProUGUI ArmorText;




    void Start()
    {
        ArmorText = GameObject.Find("nameOfArmor").GetComponent<TMPro.TextMeshProUGUI>();
        Player = GameObject.Find("player");
        PlayerCombat = Player.GetComponent<PlayerCombat>();

        fullInventoryUI.SetActive(false);

        item = dummy;
        item2 = dummy;
    }

    void Update()
    {
        healthSlider.value = PlayerStats.PlayerHealth;
        if (healthSlider.value <= 0) {
            healthSlider.gameObject.SetActive(false);
            Application.Quit();
        }
        healthSlider.maxValue = PlayerStats.PlayerMaxHealth;

        staminaSlider.value = PlayerStats.PlayerStamina;
        staminaSlider.maxValue = 3;

        // For every item slot in the panel, the text of the slot is set to the name of the game object, which is 0,1,2,3,4.
        // So If the number's inventoryItems[item] is empty, set the text to nothing. Else, set it to the name of the inventoryItems[item]'s name
        foreach(Transform slot in panel.transform) {
            Transform tmp = slot.transform.GetChild(0);
            int ooo = Int32.Parse(tmp.gameObject.name);
            if(PlayerCombat.inventoryItems[ooo] == null) {
                tmp.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = " ";
            } else {
                tmp.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = PlayerCombat.inventoryItems[ooo].name;
            }
        }

        foreach(Transform sloti in panel1.transform) {
            Transform tmpi = sloti.transform.GetChild(0);
            int oooi = Int32.Parse(tmpi.gameObject.name);
            
            if(PlayerCombat.MatsItems[oooi] == null) {
                tmpi.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = " ";
            } else {
                tmpi.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = PlayerCombat.MatsItems[oooi].name;
            }
        }

        coinText.text = PlayerCombat.coins.ToString();

        RaycastHit ihit;
        if (Physics.Raycast(Player.GetComponent<AdvancedMovement>().cam.transform.position, Player.GetComponent<AdvancedMovement>().cam.transform.forward, out ihit, 10)){
            if (ihit.collider.tag == "Shootable") {
                enemyHealthSlider.value = ihit.collider.gameObject.GetComponent<EnemyCombat>().enemyHealth;
                enemyHealthSlider.maxValue = ihit.collider.gameObject.GetComponent<EnemyCombat>().enemyMaxHealth;
                Slider oo = Instantiate(enemyHealthSlider);
                oo.gameObject.transform.parent = dummy.transform;
                Destroy(oo.gameObject, 1f);
            }
        }

    }

    public void smitheryButton(int slot) {

        foreach(GameObject eo in GameObject.Find("player").GetComponent<PlayerCombat>().MatsItems) {
            if(eo != null) {
                if (eo.gameObject.tag == "Rock") {
                    item = eo;
                }
                if (eo.gameObject.tag == "Wood") {
                    item2 = eo;
                }
            }
        }
        try {
            List<GameObject> hehe = new List<GameObject>(GameObject.Find("player").GetComponent<PlayerCombat>().MatsItems);
            GameObject.Find("player").GetComponent<PlayerCombat>().MatsItems = hehe.ToArray();
        } catch {
            //play an error sound
        }

        if(slot == 0) {
            smitheryItems[slot].transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "Wooden Armor";
            try {
                int NumToDelete = 5;
                
                List<GameObject> hehe = new List<GameObject>(GameObject.Find("player").GetComponent<PlayerCombat>().MatsItems);

                if (Array.Exists(GameObject.Find("player").GetComponent<PlayerCombat>().MatsItems, element => element == item2)) {
    
                    foreach(GameObject eo in GameObject.Find("player").GetComponent<PlayerCombat>().MatsItems) {
                        if(eo != null) {
                            if (eo.gameObject.tag == "Rock") {
                                item = eo;
                            }
                            if (eo.gameObject.tag == "Wood") {
                                item2 = eo;
                                if (NumToDelete > 0) {
                                    
                                    hehe.Remove(item2);
                                    NumToDelete--;
                                } else {
                                    
                                    GameObject.Find("player").GetComponent<PlayerCombat>().MatsItems = hehe.ToArray();
                                    smitheryItems[slot].transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "Wooden Armor <";
                                    GameObject.Find("player").GetComponent<PlayerArmor>().woodenArmor();
                                    
                                }
                            }
                        }
                    }

                }
            } catch {
                Debug.Log("not enough mats");
                //play an error sound
            }
        }
        if(slot == 1) {
            smitheryItems[slot].transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "Stone Armor";
            try {
                int NumToDelete = 8;
                
                List<GameObject> hehe = new List<GameObject>(GameObject.Find("player").GetComponent<PlayerCombat>().MatsItems);

                if (Array.Exists(GameObject.Find("player").GetComponent<PlayerCombat>().MatsItems, element => element == item)) {
                    Debug.Log("o");
                    foreach(GameObject eo in GameObject.Find("player").GetComponent<PlayerCombat>().MatsItems) {
                        if(eo != null) {
                            Debug.Log("o");
                            if (eo.gameObject.tag == "Rock") {
                                item = eo;
                                if (NumToDelete > 0) {
                                    
                                    hehe.Remove(item);
                                    NumToDelete--;
                                    Debug.Log("Now on " + NumToDelete);
                                } else {
                                    
                                    GameObject.Find("player").GetComponent<PlayerCombat>().MatsItems = hehe.ToArray();
                                    smitheryItems[slot].transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "Stone Armor <";
                                    GameObject.Find("player").GetComponent<PlayerArmor>().stoneArmor();
                                    Debug.Log("e");
                                }
                            }
                            if (eo.gameObject.tag == "Wood") {
                                Debug.Log("o");
                                item2 = eo;
                                
                            }
                        }
                    }

                }
            } catch {
                Debug.Log("not enough mats");
                //play an error sound
            }
        }
        if(slot == 2) {
            
            smitheryItems[slot].transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "Metal Armor";
            try {
                int NumToDelete = 10;
                
                List<GameObject> hehe = new List<GameObject>(GameObject.Find("player").GetComponent<PlayerCombat>().MatsItems);

                if (Array.Exists(GameObject.Find("player").GetComponent<PlayerCombat>().MatsItems, element => element == item2) && Array.Exists(GameObject.Find("player").GetComponent<PlayerCombat>().MatsItems, element => element == item)) {
    
                    foreach(GameObject eo in GameObject.Find("player").GetComponent<PlayerCombat>().MatsItems) {
                        if(eo != null) {
                            if (eo.gameObject.tag == "Rock") {
                                item = eo;
                                if (NumToDelete > 0) {
                                    
                                    hehe.Remove(item);
                                    NumToDelete--;
                                } else {
                                    
                                    GameObject.Find("player").GetComponent<PlayerCombat>().MatsItems = hehe.ToArray();
                                    smitheryItems[slot].transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "Metal Armor <";
                                    GameObject.Find("player").GetComponent<PlayerArmor>().metalArmor();
                                    
                                }
                            }
                            if (eo.gameObject.tag == "Wood") {
                                item2 = eo;
                                if (NumToDelete > 0) {
                                    
                                    hehe.Remove(item2);
                                    NumToDelete--;
                                } else {
                                    
                                    GameObject.Find("player").GetComponent<PlayerCombat>().MatsItems = hehe.ToArray();
                                    smitheryItems[slot].transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "Metal Armor <";
                                    GameObject.Find("player").GetComponent<PlayerArmor>().metalArmor();
                                    
                                }
                            }
                        }
                    }

                }
            } catch {
                Debug.Log("not enough mats");
                //play an error sound
            }
        }

        if(slot == 3) {
            
            smitheryItems[slot].transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "Sharpen Sword";
            try {
                int NumToDelete = 5;
                
                List<GameObject> hehe = new List<GameObject>(GameObject.Find("player").GetComponent<PlayerCombat>().MatsItems);

                if (Array.Exists(GameObject.Find("player").GetComponent<PlayerCombat>().MatsItems, element => element == item2)) {
    
                    foreach(GameObject eo in GameObject.Find("player").GetComponent<PlayerCombat>().MatsItems) {
                        if(eo != null) {
                            if (eo.gameObject.tag == "Rock") {
                                item = eo;
                                if (NumToDelete > 0) {
                                    
                                    hehe.Remove(item);
                                    NumToDelete--;
                                } else {
                                    
                                    GameObject.Find("player").GetComponent<PlayerCombat>().MatsItems = hehe.ToArray();
                                    smitheryItems[slot].transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "Sharpen Sword <";
                                    PlayerCombat.boostDam += 5;
                                    
                                }
                            }
                            if (eo.gameObject.tag == "Wood") {
                                item2 = eo;
                                
                            }
                        }
                    }

                }
            } catch {
                Debug.Log("not enough mats");
                //play an error sound
            }
        }
        if(slot == 4) {
            
            smitheryItems[slot].transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "Better Grip";
            try {
                int NumToDelete = 5;
                
                List<GameObject> hehe = new List<GameObject>(GameObject.Find("player").GetComponent<PlayerCombat>().MatsItems);

                if (Array.Exists(GameObject.Find("player").GetComponent<PlayerCombat>().MatsItems, element => element == item)) {
    
                    foreach(GameObject eo in GameObject.Find("player").GetComponent<PlayerCombat>().MatsItems) {
                        if(eo != null) {
                            if (eo.gameObject.tag == "Rock") {
                                item = eo;
                                if (NumToDelete > 0) {
                                    
                                    hehe.Remove(item);
                                    NumToDelete--;
                                } else {
                                    
                                    GameObject.Find("player").GetComponent<PlayerCombat>().MatsItems = hehe.ToArray();
                                    smitheryItems[slot].transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "Better Grip <";
                                    PlayerCombat.megaChance = 4;
                                    
                                }
                            }
                            if (eo.gameObject.tag == "Wood") {
                                item2 = eo;
                                
                            }
                        }
                    }

                }
            } catch {
                Debug.Log("not enough mats");
                //play an error sound
            }
        }
        if(slot == 5) {
            
            smitheryItems[slot].transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "Stiffer Hilt";
            try {
                int NumToDelete = 10;
                
                List<GameObject> hehe = new List<GameObject>(GameObject.Find("player").GetComponent<PlayerCombat>().MatsItems);

                if (Array.Exists(GameObject.Find("player").GetComponent<PlayerCombat>().MatsItems, element => element == item2) && Array.Exists(GameObject.Find("player").GetComponent<PlayerCombat>().MatsItems, element => element == item)) {
    
                    foreach(GameObject eo in GameObject.Find("player").GetComponent<PlayerCombat>().MatsItems) {
                        if(eo != null) {
                            if (eo.gameObject.tag == "Rock") {
                                item = eo;
                                if (NumToDelete > 0) {
                                    
                                    hehe.Remove(item);
                                    NumToDelete--;
                                } else {
                                    
                                    GameObject.Find("player").GetComponent<PlayerCombat>().MatsItems = hehe.ToArray();
                                    smitheryItems[slot].transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "Stiffer Hilt <";
                                    PlayerCombat.megaChance = 2;
                                    
                                }
                            }
                            if (eo.gameObject.tag == "Wood") {
                                item2 = eo;
                                if (NumToDelete > 0) {
                                    
                                    hehe.Remove(item2);
                                    NumToDelete--;
                                } else {
                                    
                                    GameObject.Find("player").GetComponent<PlayerCombat>().MatsItems = hehe.ToArray();
                                    smitheryItems[slot].transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "Stiffer Hilt <";
                                    PlayerCombat.megaChance = 2;
                                    
                                }
                            }
                        }
                    }

                }
            } catch {
                Debug.Log("not enough mats");
                //play an error sound
            }
        }
    }

    public void marketButton(int slot) {
        marketItems[slot].transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text += " <";
    }

    public void workspaceChooser(int e) {
        if(e == 0) {
            foreach(GameObject i in smitheryItems) {
                i.SetActive(true);
            }
        }
    }
}