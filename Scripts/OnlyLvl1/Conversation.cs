using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Conversation : MonoBehaviour
{    
    public AudioSource one;

    void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1) {
            one = transform.GetChild(0).GetComponent<AudioSource>();
            one.Play();
            StartCoroutine(WaiterAudio(one.clip.length));
        }
    }

    void Update()
    {
        
    }

    IEnumerator WaiterAudio(float timeToWait)
    {
        GameObject.Find("player").GetComponent<AdvancedMovement>().enabled = false;

        yield return new WaitForSeconds(timeToWait);

        GameObject.Find("player").GetComponent<AdvancedMovement>().enabled = true;
        Application.Quit();
    }
}
