using System.Collections;
using System.Collections.Generic;
using UnityEngine;




// Manages the game
public class Manager : MonoBehaviour
{
    [Tooltip("List of all the instanciated modules")]
    [SerializeField] List<Module> modulesList = new List<Module>();






    // STARTS THE GAME
    public void StartSimulation()
    {
        if (modulesList != null && modulesList.Count > 0)
            for (int i = 0; i < modulesList.Count; i++)
                if (modulesList[i] != null)
                    modulesList[i].TriggerModule();
    }
}
