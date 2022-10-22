using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    bool gameHasEnded = false;


    public void EndGame()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            Restart();
        }
    }
    public void WinGame()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            NEXT();
        }
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NEXT()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
