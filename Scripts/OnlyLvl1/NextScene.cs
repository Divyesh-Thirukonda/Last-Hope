using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GG());
    }

    void Update()
    {
        
    }

    IEnumerator GG () {
        yield return new WaitForSeconds(30);
        SceneManager.LoadScene(3);
    }
}
