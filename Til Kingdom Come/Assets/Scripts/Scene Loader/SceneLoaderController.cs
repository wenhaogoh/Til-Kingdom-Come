using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoaderController : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 0.5f;
    public void LoadScene(string sceneName) {
        Debug.Log("Loading Scene: " + sceneName);
        StartCoroutine(Helper(sceneName));
    }
    private IEnumerator Helper(string sceneName) {
        // Play animation
        transition.SetTrigger("Start");

        // Wait for animation to finish running
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    // Audio helper functions
    public void PlayMusic(string name)
    {
        AudioController.instance.PlayMusic(name);
    }
}
