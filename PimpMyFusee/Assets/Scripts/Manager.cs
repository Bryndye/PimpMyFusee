using System.Collections;
using System.Collections.Generic;
using UnityEngine;




// Manages the game simulation
public class Manager : MonoBehaviour
{
    // SINGLETON
    [HideInInspector] public static Manager Instance = null;

    [SerializeField] CameraMovements cameraScript = null;
 

    [Tooltip("List of all the instanciated modules")]
    List<Module> modulesList = new List<Module>();
    [SerializeField] bool getAllModulesOnStart = false;

    bool simulationStarted = false;






    #region FUNCTIONS
    private void Awake()                                                                                        // AWAKE
    {   
        // SINGLETON
        Instance = this;
        GetReferences();
    }



    // STARTS THE GAME
    public void StartSimulation(bool on = false)
    {
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
