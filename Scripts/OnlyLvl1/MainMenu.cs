using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class MainMenu : MonoBehaviour
{
    
    public Slider slider;
    public GameObject sliderObject;

    public void PlayTutorial()
    {
        StartCoroutine(LoadAsynchronouslyTutorial());

                
    }

    IEnumerator LoadAsynchronouslyTutorial () {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        sliderObject.SetActive(true);   

        while (!operation.isDone) {

            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            slider.value = progress;

            yield return null;
        }
    }
    
}
