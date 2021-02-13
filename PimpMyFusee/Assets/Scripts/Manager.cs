using System.Collections;
using System.Collections.Generic;
using UnityEngine;




// Manages the game simulation
public class Manager : MonoBehaviour
{
    // SINGLETON
    [HideInInspector] public static Manager Instance = null;
    private ManagerScore ms;

    [SerializeField] CameraMovements cameraScript = null;
    [SerializeField] GameObject playButton = null;
    [SerializeField] GameObject restartbutton = null;
    [SerializeField] List<GameObject> editorObjects = new List<GameObject>();


    List<Module> modulesList = new List<Module>();
    [SerializeField] bool getAllModulesOnStart = false;

    public bool simulationStarted = false;






    #region FUNCTIONS
    private void Awake()                                                                                        // AWAKE
    {   
        // SINGLETON
        Instance = this;
        GetReferences();

        ms = ManagerScore.Instance;

        // Set up display
        restartbutton.SetActive(false);
        EnableEditorUI(true);
    }




    void EnableEditorUI(bool on = false)
    {
        if (editorObjects != null && editorObjects.Count > 0)
            for (int i = 0; i < editorObjects.Count; i++)
                editorObjects[i].SetActive(on);
    }


    

    // STARTS THE GAME
    public void StartSimulation(bool on = false)
    {
        EnableEditorUI(!on);

        simulationStarted = on;

        //Put Score Into Manager
        if (!on)
            ms.GetTheScore();

        // Find modules in scene
        if (getAllModulesOnStart)
        {
            modulesList.Clear();

            Module[] modulesArray = GameObject.FindObjectsOfType<Module>();
            for (int i = 0; i < modulesArray.Length; i++)
                modulesList.Add(modulesArray[i]);
        }
            


        // Start modules
        if (modulesList != null && modulesList.Count > 0)
            for (int i = 0; i < modulesList.Count; i++)
                if (modulesList[i] != null)
                    modulesList[i].TriggerModule(on);


        // Start camera
        if (cameraScript != null)
            cameraScript.StartCamera(on);



        // GARAGE UI
        if (playButton != null)
            playButton.SetActive(!on);
        if (restartbutton != null)
            restartbutton.SetActive(on);
    }








    // Get references if they're not here
    void GetReferences()
    {
        if (cameraScript == null)
            if (Camera.main.GetComponent<CameraMovements>())
                cameraScript = Camera.main.GetComponent<CameraMovements>();
    }




    // EDITOR
    private void OnDrawGizmosSelected()                                                                             // ON DRAW GIZMOS SELECTED
    {
        GetReferences();
    }
    #endregion
}
