using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Script for the reactor module
public class Reactor01 : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField] Module connectedModuleScript = null;
    [SerializeField] Rigidbody2D rigidbody2d = null;
    bool reactorActivated = false;








    private void Awake()                                                            // AWAKE
    {
        GetComponentsIfNotReferenced();
    }

    void FixedUpdate()
    {
        if (isActiveAndEnabled && enabled)
        {
            if (reactorActivated)
                rigidbody2d.AddForce(Vector2.up);
        }
    }





    /// <summary>
    /// Turns the reactor propeling on or off
    /// </summary>
    /// <param name="state"></param>
    public void TriggerReactor(bool state = false)
    {
        reactorActivated = state;
    }







    // EDITOR
    private void GetComponentsIfNotReferenced()
    {
        // GET COMPONENTS IS NOT REFERENCED
        if (rigidbody2d == null)
            if (GetComponent<Rigidbody2D>())
                rigidbody2d = GetComponent<Rigidbody2D>();

        if (connectedModuleScript == null)
            if (GetComponent<Module>())
                connectedModuleScript = GetComponent<Module>();
    }


    private void OnDrawGizmosSelected()                                                         // ON DRAW GIZMOS SELECTED
    {
        GetComponentsIfNotReferenced();
    }
}
