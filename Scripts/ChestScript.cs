using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    public GameObject possiblity0;
    public GameObject possiblity1;
    public GameObject possiblity2;
    System.Random randDrop = new System.Random();

    private AdvancedMovement AdvancedMovement;
    private GameObject Player;

    public GameObject ParticleEffectChest;
    
    void Start()
    {
        
        Player = GameObject.Find("player");
        AdvancedMovement = Player.GetComponent<AdvancedMovement>();
    }

    public void Interact() {
        GameObject[] chestDrop = {possiblity0, possiblity1, possiblity2};
        chestDrop[0] = chestDrop[randDrop.Next(0,3)];
        GameObject gg = Instantiate(chestDrop[0], gameObject.transform);
        gg.name = gg.name.Replace("(Clone)", "");
        gg.transform.parent = null;
        gg.transform.position += new Vector3(0, 1, 0);
        gameObject.SetActive(false);
        Destroy(gameObject);


        GameObject partInstance = Instantiate(ParticleEffectChest, gameObject.transform.position, gameObject.transform.rotation);
        partInstance.gameObject.transform.position += new Vector3(0, 3, -3);
        partInstance.transform.parent = null;
    }
}
