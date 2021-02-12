using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


// Created for Unity 2019.1.1f1
// This script manages scene transitions and scene loading
// OPTIMIZED I THINK ?
public class SceneManage : MonoBehaviour
{
    public static SceneManage Instance;


    [Header("SCENE CHANGE")]
    [Tooltip("Should this script automatically load a referenced scene after a certain amount of time ?")]
    [SerializeField] bool autoLoadSceneAfterDuration = false;
    [SerializeField] float durationBeforeAutoLoadScene = 0.5f;
    [SerializeField] int sceneToAutoLoadIndex = 1;
    [SerializeField] Animator sceneSwitchAnimator = null;

    Scene sceneToLoad;
    int index = 0;
    public bool proceedToLoadScene = false;
    bool quit = false;
    bool canLoadScene = true;
    bool loadWithIndex = false;



    [Header("RESTART SCENE")]
    [Tooltip("Choose which keys should be pressed to restart the scene")]
    [SerializeField] KeyCode[] pressSimultaneousKeysToRestart = null;











    #region FUNCTIONS
    #region BASE FUNCTIONS
    void Awake()                                                            // AWAKE
    {
        Instance = this;
        GetReferences();
    }


    void Start()                                                                // START
    {
        // If chosen, starts the coroutine that will load the indicated scene after the indicated duration
        if (autoLoadSceneAfterDuration)
            StartCoroutine(AutoLoadSceneAfterDuration());
    }


    void Update()                                                                   // UPDATE
    {
        if (isActiveAndEnabled && enabled)
        {
            // Checks if the inputs to restart the scene were pressed
            if (CheckIfAllKeysPressed(pressSimultaneousKeysToRestart))
                Restart();


            // Is called when the scene switch screen finished fading on
            if (proceedToLoadScene)
            {
                if (quit)
                    Application.Quit();
                else if (canLoadScene)
                {
                    canLoadScene = false;

                    if (loadWithIndex)
                        LoadScene(index);
                    else
                        LoadScene(sceneToLoad);
                }
            }
        }
    }
    #endregion




    // SCENE LOADING
    // Automaticly loads the indicated scene after the indicated duration, without transition animation
    IEnumerator AutoLoadSceneAfterDuration()
    {
        yield return new WaitForSecondsRealtime(durationBeforeAutoLoadScene);


        SceneManager.LoadSceneAsync(sceneToAutoLoadIndex);

        Invoke("SceneManager.LoadSceneAsync()", durationBeforeAutoLoadScene);
    }

    void LoadScene(Scene scene)
    {
        SceneManager.LoadSceneAsync(scene.name);
    }

    void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }


    // Set which scene should be loaded after the close scene anim
    public void SetLoadScene(int sceneIndex)
    {
        loadWithIndex = true;
        index = sceneIndex;
        sceneSwitchAnimator.SetTrigger("CloseScene");
    }


    // Set which scene should be loaded after the close scene anim
    public void SetLoadScene(Scene scene)
    {
        sceneToLoad = scene;
        sceneSwitchAnimator.SetTrigger("CloseScene");
    }

    // Triggers the restart of the current scene, called by the restart inputs
    public void Restart()
    {
        SetLoadScene(SceneManager.GetActiveScene());
    }








    // QUIT
    // Sets the instruction to quit the game after after the close scene anim
    public void Quit()
    {
        SetLoadScene(new Scene());
        quit = true;
    }













    // SECONDARY
    // Checks if the given keys are being pressed
    bool CheckIfAllKeysPressed(KeyCode[] keys)
    {
        bool notPressed = false;


        for (int i = 0; i < keys.Length; i++)
        {
            if (!Input.GetKey(keys[i]))
                notPressed = true;
        }


        return !notPressed;
    }



    private static string NameFromIndex(int index)
    {
        string scenePath = SceneUtility.GetScenePathByBuildIndex(index);
        
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
        return sceneName;
    }







    // If references not here, get them
    void GetReferences()
    {
        if (sceneSwitchAnimator == null)
            if (GetComponent<Animator>())
                sceneSwitchAnimator = GetComponent<Animator>();
    }

    // EDITOR
    private void OnDrawGizmosSelected()
    {
        GetReferences();
    }
    #endregion
}
