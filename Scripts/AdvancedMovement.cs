using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class AdvancedMovement : MonoBehaviour
{
    [Header("Propoerty Links")]
    private Rigidbody rb;
    public Camera cam;
    private Animator anim;
    [Space]
    [Header("Movement")]
	public static float mouseSensitivityX = 1.0f;
	public static float mouseSensitivityY = 1.0f;
	public float walkSpeed = 15.0f;
    public float sprintSpeed = 25.0f;
    Vector3 moveDir;
	Vector3 targetMoveAmount;
	Vector3 moveAmount;
	Vector3 smoothMoveVelocity;
	Transform cameraT;
	float verticalLookRotation;
	float jumpForce = 500.0f;
	bool grounded;
	public LayerMask groundedMask;
    [Space]
    [Header("Animations")]
    public const float locomationAnimationSmoothTime = .1f;
    [Space]
    [Header("Respawn Variables")]
    [SerializeField] GameObject SpawnPoint01;
    GameObject currentCheckpoint;
    [Space]
    [Header("Combat")]
    public bool claimedSword = false;
    public bool claimedGun = false;
    private PlayerCombat PlayerCombat;
    private GameObject Player;
    
    private ChestScript ChestScript;
    public GameObject Inventory;
    public GameObject fullInventoryUI;
    public TextMesh DispText;

    public AudioSource walkAudio;
    bool isWalking = false;
    public AudioSource windAudio;
    public AudioSource chestOpenAudio;
    public AudioSource CollectedAudio;
    public AudioSource swordHitAudio;
    public AudioSource gunHitAudio;
    public AudioSource UIclickaudio;
    public AudioSource diedAudio;
    public AudioSource enemyHitAudio;


    public GameObject verificationUI;
    bool funibool = false;

    public GameObject[] InventorySlots;

    public static float timeSpentOnGame;
    public TextMeshProUGUI timerText;
    

    
    

    void Start() {
        Inventory = GameObject.Find("Inventory");
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        cameraT = Camera.main.transform;


        Player = GameObject.Find("player");
        PlayerCombat = Player.GetComponent<PlayerCombat>();
        ChestScript = GetComponent<ChestScript>();
    }

    private void FixedUpdate() {
        if(grounded) {
            moveDir = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical")).normalized;
		    targetMoveAmount = moveDir * PlayerStats.PlayerSpeed;
		    moveAmount = Vector3.SmoothDamp (moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);
            rb.MovePosition (rb.position + transform.TransformDirection (moveAmount) * Time.fixedDeltaTime);
        } else {
            moveDir = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical")).normalized;
		    targetMoveAmount = moveDir * PlayerStats.PlayerSpeed;
		    moveAmount = Vector3.SmoothDamp (moveAmount, targetMoveAmount, ref smoothMoveVelocity, .55f);
            rb.MovePosition (rb.position + transform.TransformDirection (moveAmount) * Time.fixedDeltaTime);
        }
    }

    public void Update()
    {
        timeSpentOnGame += Time.deltaTime;
        timerText.text = Math.Floor(timeSpentOnGame).ToString() + " Seconds Into Game";

        if(fullInventoryUI.activeInHierarchy) {
            Cursor.lockState = CursorLockMode.None;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
        }

		transform.Rotate (Vector3.up * Input.GetAxis ("Mouse X") * mouseSensitivityX);
		verticalLookRotation += Input.GetAxis ("Mouse Y") * mouseSensitivityY;
		verticalLookRotation = Mathf.Clamp (verticalLookRotation, -60, 60);
		cameraT.localEulerAngles = Vector3.left * verticalLookRotation;


		// jump
		if (Input.GetButtonDown ("Jump")) {
			if (grounded) {
				rb.AddForce (transform.up * jumpForce);
			}
		}

        if (Input.GetKey(KeyCode.LeftShift) && PlayerStats.PlayerStamina > 0) {
            PlayerStats.PlayerStamina -= 1 * Time.deltaTime;
            PlayerStats.PlayerSpeed = sprintSpeed;
        } else {
            if (PlayerStats.PlayerStamina <= 5) {
                PlayerStats.PlayerStamina += 1 * Time.deltaTime;
            }
            PlayerStats.PlayerSpeed = walkSpeed;
        }



        if (rb.velocity.y >= 20 | rb.velocity.y <= -20) {
            windAudio.Play();
            windAudio.volume = 100;
        } else {
            windAudio.Play();
            windAudio.volume -= 1 * Time.deltaTime;
        }

        if (Input.GetKeyDown("w") | Input.GetKeyDown(KeyCode.UpArrow) && grounded) {
            isWalking = true;
        } else if (Input.GetKeyUp("w") | Input.GetKeyUp(KeyCode.UpArrow)) {
            isWalking = false;
        }
        if(isWalking) {
        } else {
            walkAudio.Play();
        }

        float SpeedPercentOfPlayer = rb.velocity.magnitude / 10f;
        anim.SetFloat("SpeedPercentOfPlayer", SpeedPercentOfPlayer, locomationAnimationSmoothTime, Time.deltaTime);
        
        

        RaycastHit ihit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out ihit, 10)){
            if (ihit.collider.tag == "PowerGrid") {
                InteractMessage(ihit);
            }
            if (ihit.collider.tag == "Encryptor") {
                InteractMessage(ihit);
            }
            if (ihit.collider.tag == "Chest") {
                InteractMessage(ihit);
            }
        }

        if(Input.GetKeyDown("e")) {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 10)){
                if (hit.collider.tag == "Chest") {
                    hit.collider.gameObject.GetComponent<ChestScript>().Interact();
                    chestOpenAudio.Play();
                    PlayerCombat.coins += 50;
                }
                if (hit.collider.tag == "PowerGrid") {
                    PlayerCombat.coins += 50;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
                if (hit.collider.tag == "Encryptor") {
                    funibool = true;
                    verificationUI.SetActive(true);
                    PlayerCombat.coins += 50;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Return) && funibool) {
            verificationUI.SetActive(false);
        }

        InventSwapper(1);
        InventSwapper(2);
        InventSwapper(3);
        InventSwapper(4);
        InventSwapper(5);
        if(Input.GetKeyDown("escape")) {
            PlayerCombat.Hand.SetActive(true);
            Inventory.SetActive(false);
            UIclickaudio.Play();
        }
        if(Input.GetKeyDown("q")) {
            PlayerCombat.Hand.SetActive(false);
            Inventory.SetActive(true);
            UIclickaudio.Play();
        }

        if(Input.GetKeyDown("r")) {
            fullInventoryUI.SetActive(true);
            UIclickaudio.Play();
        }
        if(Input.GetKeyDown("t")) {
            fullInventoryUI.SetActive(false);
            UIclickaudio.Play();
        }
    }

    void InteractMessage(RaycastHit ihit) {
        DispText.text = "Press 'E' to Interact";
        TextMesh oo = Instantiate(DispText, ihit.collider.gameObject.transform);
        oo.gameObject.transform.position += new Vector3(0, 2, 2);
        oo.gameObject.transform.parent = null;
        Destroy(oo.gameObject, 2f);
    }

    //  If I press 1; as long as it's less than 5 and the 1st slot isn't empty; spawn the item in the first inentory slot of PlayerCombat at the Inventory game object 
    int dummyNum = 0;
    void InventSwapper(int i) {
        if (i <= 5) {//***** The 5 should be replaced with #ofSLots in inventory
            if(Input.GetKeyDown(i.ToString())) {
                try {
                    dummyNum = i;
                    GameObject gg = Instantiate(PlayerCombat.inventoryItems[i-1], Inventory.transform.GetChild(i-1));
                    gg.name = gg.name.Replace("(Clone)", "");
                    if (Inventory.transform.GetChild(i-1) == null) {}
                    gg.transform.SetParent(Inventory.transform.GetChild(i-1));
                    gg.SetActive(true);
                    PlayerCombat.Hand.SetActive(false);
                    gg.transform.localPosition = Vector3.zero;
                    PlayerCombat.inventoryItems[i-1] = null;

                    InventorySlots = new GameObject[Inventory.transform.childCount];
                    int e = 0;
                    foreach (Transform child in Inventory.transform) {
                        //InventorySlots[e] = child.gameObject;
                        if (e > Inventory.transform.childCount) {
                            return;
                        }
                        e++;
                        child.gameObject.SetActive(false);
                    }
                    Inventory.transform.GetChild(i-1).gameObject.SetActive(true);
                }
                catch { //mental reminder put this in method
                    InventorySlots = new GameObject[Inventory.transform.childCount];
                    int e = 0;
                    foreach (Transform child in Inventory.transform) {
                        //InventorySlots[e] = child.gameObject;
                        if (e > Inventory.transform.childCount) {
                            return;
                        }
                        e++;
                        child.gameObject.SetActive(false);
                    }
                    Inventory.transform.GetChild(i-1).gameObject.SetActive(true);
                }
            }
        }
    }

    public void UpdateInventory() {
        PlayerCombat.inventoryItems[dummyNum-1] = null;
    }


    public void Respawn (GameObject playerCurrentCheckpoint) {
        if(playerCurrentCheckpoint != null) {
            transform.position = playerCurrentCheckpoint.transform.position + new Vector3(0, 2, 0);
        } else {
            transform.position = SpawnPoint01.transform.position;
        }
    }



    public void OnCollisionEnter (Collision collisionInfo) {
        if(collisionInfo.collider.tag == "Checkpoint"){
            currentCheckpoint = collisionInfo.gameObject;
        }

        if (collisionInfo.collider.tag == "MegaBouncepad") {
            rb.AddForce(Vector3.up * 2350);
        }
        if (collisionInfo.collider.tag == "Bouncepad") {
            rb.AddForce(Vector3.up * 1000);
        }
        if (collisionInfo.collider.tag == "Wallrun") {
            rb.AddForce(transform.forward * 2000);
        }

        if (collisionInfo.collider.tag == "Respawn") {
            Respawn(currentCheckpoint);
            PlayerStats.PlayerHealth -= 10;
            diedAudio.Play();
        }
        if (collisionInfo.collider.tag == "Void") {
            FindObjectOfType<GameManager>().EndGame();
        }
        if (collisionInfo.collider.tag == "Sword") {
            CollectedAudio.Play();
            claimedSword = true;
            PlayerCombat.coins += 50;
            GameObject.Find("Second").GetComponent<AudioSource>().Play();
        }
        if (collisionInfo.collider.tag == "Gun") {
            CollectedAudio.Play();
            claimedGun = true;
            PlayerCombat.coins += 500;
        }

        /*
        switch (collisionInfo.collider.tag) {
            case "Gun":
        }
        */
    }

    void OnCollisionStay(Collision collision) {
        if(collision.collider.gameObject.layer == 8) {
            grounded = true;
        } else {
            grounded = false;
        }
    }
    void OnCollisionExit(Collision collision) {
        if(collision.collider.gameObject.layer == 8) {
            grounded = false;
        }
    }
    
}
