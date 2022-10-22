using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveUpAndDownE : MonoBehaviour
{
    [Header("Trigonometry Variables")]
    Vector3 startingPosition;
    [SerializeField] Vector3 movementVector;
    float movementFactor;
    [SerializeField] float period = 2f;

    [Space]
    [Header("Objects")]
    public GameObject newWeapon;
    public GameObject drop;
    GameObject Player;
    public PlayerCombat PlayerCombat;

    public bool doesOwn = false;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        Player = GameObject.Find("player");
        PlayerCombat = Player.GetComponent<PlayerCombat>();
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) {
            return;
        }
        float cycles = Time.time / period; 
        
        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * tau);

        movementFactor = (rawSinWave + 1f) / 2f;
        
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
    }

    public void OnCollisionEnter (Collision collisionInfo) {
        if (collisionInfo.collider.tag == "Player") {
            drop.SetActive(false);
            doesOwn = true;
            
            foreach (GameObject handItems in PlayerCombat.itemsOnHand) {
                handItems.SetActive(false);
            }
            newWeapon.SetActive(true);
            PlayerCombat.SetMoveUpAndDownEScript(newWeapon);
            
        }
    }
}