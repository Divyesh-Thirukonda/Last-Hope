using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerCombat : MonoBehaviour
{
    [Header("Combat Data")]
    public float attackTimer = 0;
    [Space]
    [Header("References")]
    public Camera cam;
    public EnemyCombat EnemyCombat;
    public AdvancedMovement AdvancedMovement;
    public Animator anim;
    public GameObject ParticleEffectHit;
    public GameObject FireGunParticle;
    public GameObject HoleForGun;
    public GameObject sword;
    public GameObject gun;
    [Space]
    [Header("Combat")]
    public GameObject Hand;
    public enum Weapons { Sword, Gun };
    public Weapons currentWeapon = Weapons.Sword;
    public GameObject[] itemsOnHand;
    [Space]
    [Header("UI")]
    public TextMesh damageTakenText;
    [Space]
    [Header("Inventory")]
    public GameObject[] inventoryItems;
    public int coins;
    public GameObject[] MatsItems;
    public static int megaChance = 6;
    public static int boostDam = 0;
    

    void Start()
    {
        itemsOnHand = new GameObject[Hand.transform.childCount];
        int i = 0;
        foreach (Transform child in Hand.transform) {
            itemsOnHand[i] = child.gameObject;
            if (i >= Hand.transform.childCount) {
                return;
            }
            i++;
            child.gameObject.SetActive(false);
        }

        sword = itemsOnHand[0];
        gun = itemsOnHand[1];

        EnemyCombat = GetComponent<EnemyCombat>();
        AdvancedMovement = GetComponent<AdvancedMovement>();
    }

    void Update()
    {
        System.Random rnd = new System.Random();
        PlayerStats.PlayerCritDamage = rnd.Next(5, 20);
        PlayerStats.PlayerCritChance = rnd.Next(0, megaChance);

        if(PlayerStats.PlayerHealth >= PlayerStats.PlayerMaxHealth) {
            PlayerStats.PlayerHealth = PlayerStats.PlayerMaxHealth;
        }
        if(PlayerStats.PlayerStamina >= 3) {
            PlayerStats.PlayerStamina = 3;
        }
        if (Input.GetKeyDown("t")) {
            if (AdvancedMovement.claimedSword == true && AdvancedMovement.claimedGun == false) {
                Debug.Log("nice one iiot");
            }
            if (AdvancedMovement.claimedSword == true && AdvancedMovement.claimedGun == true) {
                if (itemsOnHand[0].activeSelf == true) {
                    currentWeapon = Weapons.Gun;
                    gun.SetActive(true);
                    foreach(GameObject item in itemsOnHand){
                        if (item.name != "Gun"){
                            item.SetActive(false);
                        }
                    }
                } else if (itemsOnHand[1].activeSelf == true) {
                    currentWeapon = Weapons.Sword;
                    sword.SetActive(true);
                    foreach(GameObject item in itemsOnHand){
                        if (item.name != "Sword"){
                            item.SetActive(false);
                        }
                    }
                }
            } else if (SceneManager.GetActiveScene().buildIndex > 1) {
                if (itemsOnHand[0].activeSelf == true) {
                    currentWeapon = Weapons.Gun;
                    gun.SetActive(true);
                    foreach(GameObject item in itemsOnHand){
                        if (item.name != "Gun"){
                            item.SetActive(false);
                        }
                    }
                } else if (itemsOnHand[1].activeSelf == true) {
                    currentWeapon = Weapons.Sword;
                    sword.SetActive(true);
                    foreach(GameObject item in itemsOnHand){
                        if (item.name != "Sword"){
                            item.SetActive(false);
                        }
                    }
                }
            } 
        }
        attackTimer += Time.deltaTime;
        if(currentWeapon == Weapons.Sword) {
            PlayerStats.PlayerDamage = 20f + boostDam;
            PlayerStats.PlayerDamageRange = 5f;
            PlayerStats.PlayerDamageCooldown = 1f;

            if (Input.GetMouseButtonUp(0) && attackTimer >= PlayerStats.PlayerDamageCooldown) {
                attackTimer = 0;
                if(GameObject.Find("player").GetComponent<AdvancedMovement>().fullInventoryUI.activeInHierarchy) {
                    //hehe
                } else {
                    GameObject.Find("player").GetComponent<AdvancedMovement>().swordHitAudio.Play();
                }
                Attack();
            }
        }
        if(currentWeapon == Weapons.Gun) {
            PlayerStats.PlayerDamage = 5f + (boostDam/4);
            PlayerStats.PlayerDamageRange = 30f;
            PlayerStats.PlayerDamageCooldown = .1f;

            if (Input.GetMouseButtonUp(0) && attackTimer >= PlayerStats.PlayerDamageCooldown) {
                attackTimer = 0;
                Attack();
                GameObject.Find("player").GetComponent<AdvancedMovement>().gunHitAudio.Play();
                GameObject partGunFireInstance = Instantiate(FireGunParticle, HoleForGun.transform);
            }
       }


        

    }

    public void SetMoveUpAndDownEScript (GameObject newWeapon) {
        foreach(GameObject item in itemsOnHand){
            if (item.name == newWeapon.name && newWeapon.name == "Gun"){
                currentWeapon = Weapons.Gun;
            }
            if (item.name == newWeapon.name && newWeapon.name == "Sword"){
                currentWeapon = Weapons.Sword;
            }
        }
    }

    void Attack() {
        if(PlayerStats.PlayerCritChance == 1) {
            PlayerStats.PlayerDamage += (PlayerStats.PlayerCritDamage+ (PlayerStats.PlayerCritDamage*(PlayerStats.PlayerCritDamage/100)));
        }
        
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, PlayerStats.PlayerDamageRange) && sword.activeSelf == true | gun.activeSelf == true){
            if (hit.collider.tag == "Shootable") {
                
                hit.collider.gameObject.GetComponent<EnemyCombat>().TakeDamage();
                damageTakenText.text = PlayerStats.PlayerDamage.ToString();
                TextMesh oo = Instantiate(damageTakenText, hit.collider.gameObject.transform);
                oo.gameObject.transform.position += new Vector3(0, 2, 2);
                oo.gameObject.transform.parent = null;
                Destroy(oo.gameObject, 2f);
                
                GameObject partInstance = Instantiate(ParticleEffectHit, hit.collider.gameObject.transform.position, hit.collider.gameObject.transform.rotation);
                partInstance.gameObject.transform.position += new Vector3(0, 3, -3);
                partInstance.transform.parent = null;

                coins += 100;
            }
        }
        StartCoroutine(AttackAnim(""));
    }

    IEnumerator AttackAnim(string x)
    {
        anim.SetBool(x + "OneAttack", true);

        yield return new WaitForSeconds(2);

        anim.SetBool(x + "OneAttack", false);
    }

    /*
    float distance = Vector3.Distance(GameObject.Find("player").transform.position, gameObject.transform.position);
		if (distance <= radius)
		{
			PlayerStats.PlayerHealth -= (20- (20*(PlayerStats.PlayerDefense/100)));
		}
    */
}
