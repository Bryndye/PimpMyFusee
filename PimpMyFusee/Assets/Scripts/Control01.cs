using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Script for the reactor module
public class Control01 : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField] Module connectedModuleScript = null;
    [SerializeField] Rigidbody2D rigidbody2d = null;


    [Header("PARAMETERS")]
    [SerializeField] float controlForce = 10f;
    bool controlActivated = false;



    [Header("FX")]
    [SerializeField] ParticleSystem left = null;
    [SerializeField] ParticleSystem right = null;
    bool leftState = false;
    bool lastLeftState = false;
    bool rightState = false;
    bool lastRightState = false;






    #region FUNCTIONS
    private void Awake()                                                            // AWAKE
    {
        GetComponentsIfNotReferenced();
    }

    void FixedUpdate()                                                              // FIXEDUPDATE
    {
        if (isActiveAndEnabled && enabled)
        {
            if (controlActivated)
            {
                // Unstable trajectory
                Vector2 newForce = Input.GetAxis("Horizontal") * transform.right * controlForce;

                if (rigidbody2d != null)
                    rigidbody2d.AddForce(newForce, ForceMode2D.Force);






                // FX
                lastLeftState = leftState;
                lastRightState = rightState;
                leftState = Input.GetButton("Left");
                rightState = Input.GetButton("Right");


                if (!lastLeftState && leftState)
                    left.Play();
                else if (lastLeftState && !leftState)
                    left.Stop();

                if (!lastRightState && rightState)
                    right.Play();
                else if (lastRightState && !rightState)
                    right.Stop();
            }
        }
    }





    /// <summary>
    /// Turns the reactor propeling on or off
    /// </summary>
    /// <param name="state"></param>
    public void TriggerControl(bool state = false)
    {
        controlActivated = state;
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
