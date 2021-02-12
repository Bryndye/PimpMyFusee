using System.Collections;
using System.Collections.Generic;
using UnityEngine;




// Manages the game simulation
public class Manager : MonoBehaviour
{
    // SINGLETON
    public static Manager Instance = null;


    [Tooltip("List of all the instanciated modules")]
    [SerializeField] List<Module> modulesList = new List<Module>();
    [SerializeField] bool getAllModulesOnStart = false;






    private void Awake()                                                                                        // AWAKE
    {   
        // SINGLETON
        Instance = this;
    }



    // STARTS THE GAME
    public void StartSimulation()
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
                    modulesList[i].TriggerModule();
    }
}
