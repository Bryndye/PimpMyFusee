using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Script for the reactor module
public class Mother01 : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField] Module connectedModuleScript = null;
    [SerializeField] Rigidbody2D rigidbody2d = null;


    [Header("PARAMETERS")]
    [SerializeField] float controlForce = 10f;
    bool motherActivated = false;






    #region FUNCTIONS
    private void Awake()                                                            // AWAKE
    {
        GetComponentsIfNotReferenced();
    }

    void FixedUpdate()                                                              // FIXEDUPDATE
    {
        if (isActiveAndEnabled && enabled)
        {
            if (motherActivated)
            {
                // Unstable trajectory
                Vector2 newForce = Input.GetAxis("Horizontal") * transform.right * controlForce;


                if (rigidbody2d != null)
                    rigidbody2d.AddForce(newForce, ForceMode2D.Force);
            }
        }
    }





    /// <summary>
    /// Turns the reactor propeling on or off
    /// </summary>
    /// <param name="state"></param>
    public void TriggerMother(bool state = false)
    {
        motherActivated = state;
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
    #endregion
}
