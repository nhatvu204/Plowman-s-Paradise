using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MainMenu : MonoBehaviour
{
    public Button loadGameButton;

    public void NewGame()
    {
        StartCoroutine(LoadGameAsync(SceneTransitionManager.Location.PlayerHome, null));
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadGameAsync(SceneTransitionManager.Location.PlayerHome, LoadGame));
    }

    //To be called after the scene is loaded
    void LoadGame()
    {
        //Confirm if the GameStateMnager is there after the scene is loaded
        if(GameStateManager.Instance == null)
        {
            Debug.Log("No GSM");
            return;
        }
        GameStateManager.Instance.LoadSave();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadGameAsync(SceneTransitionManager.Location scene, Action onFirstFrameLoad)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene.ToString());
        //Make this persistent so it can continue to run
        DontDestroyOnLoad(gameObject);

        //Wait for the scene to load
        while (!asyncLoad.isDone)
        {
            yield return null;
            Debug.Log("loading");
        }

        //Scene loaded
        Debug.Log("loaded");

        yield return new WaitForEndOfFrame();
        Debug.Log("First loaded");
        //If there is an Action assigned, call it
        onFirstFrameLoad?.Invoke();

        //Destroy after running
        Destroy(gameObject);

    }

    // Start is called before the first frame update
    void Start()
    {
        //Disable or enable the load game button based on whether there is a save file
        loadGameButton.interactable = SaveManager.HasSave();
    }
}
