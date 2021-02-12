using System.Collections;
using System.Collections.Generic;
using UnityEngine;




// Main script for the modules
public class Module : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField] Rigidbody2D rigidbody2d = null;


    bool activated = false;
    [SerializeField] int cost = 20;

    [Header("SUB MODULES")]
    [SerializeField] Reactor01 reactor01 = null;



    [Header("EDITOR")]
    [SerializeField] GameObject editorObjects = null;









    #region FUNCTIONS
    private void Awake()                                                            // AWAKE
    {
        GetComponentsIfNotReferenced();
        DisableEditorInfo();
    }

    private void Start()                                                             // START
    {
        // When the module appears on screen it is not activated yet
        TriggerModule(false);

        // COST
        if (ManagerShop.Instance != null)
            ManagerShop.Instance.AddEspace(cost);
    }


    private void OnDestroy()                                                        // ON DESTROY
    {
        // COST
        if (ManagerShop.Instance != null)
            ManagerShop.Instance.AddEspace(-cost);
    }








    // MODULE
    /// <summary>
    /// Triggers all of the module's functions to start the simulation
    /// </summary>
    /// <param name="state">Should the module be turned on or off</param>
    public void TriggerModule(bool state = true)
    {
        activated = state;
        rigidbody2d.isKinematic = !state;


        // Sub modules
        if (reactor01 != null)
            reactor01.TriggerReactor(true);
    }











    // EDITOR
    private void GetComponentsIfNotReferenced()
    {
        // GET COMPONENTS IS NOT REFERENCED
        if (rigidbody2d == null)
            if (GetComponent<Rigidbody2D>())
                rigidbody2d = GetComponent<Rigidbody2D>();
    }

    private void DisableEditorInfo()
    {
        if (editorObjects != null)
            editorObjects.SetActive(false);
    }

    private void OnDrawGizmosSelected()                                                         // ON DRAW GIZMOS SELECTED
    {
        GetComponentsIfNotReferenced();
    }
    #endregion
}
