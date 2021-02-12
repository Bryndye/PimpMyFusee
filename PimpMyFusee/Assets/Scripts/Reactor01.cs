using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Script for the reactor module
public class Reactor01 : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField] Module connectedModuleScript = null;
    [SerializeField] Rigidbody2D rigidbody2d = null;


    [Header("PARAMETERS")]
    [SerializeField] float reactorpower = 10f;
    [SerializeField] Vector2 horizontalNoiseLimits = new Vector2(-10f, 10f);
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
            {
                // Unstable trajectory
                Vector2 horizontalNoise = transform.right * Random.Range(horizontalNoiseLimits.x, horizontalNoiseLimits.y);


                if (rigidbody2d != null)
                    rigidbody2d.AddForce((Vector2)transform.up * reactorpower + horizontalNoise, ForceMode2D.Force);
            }
        }
    }





    /// <summary>
    /// Turns the reactor propeling on or off
    /// </summary>
    /// <param name="state"></param>
    public void TriggerReactor(bool state = false)
    {
        reactorActivated = state;
        Debug.Log(state);
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
