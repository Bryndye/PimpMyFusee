using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Script for the reactor module
public class ZReactor01 : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField] Module connectedModuleScript = null;
    [SerializeField] Rigidbody2D rigidbody2d = null;
    [SerializeField] Transform fuelSlider = null;


    [Header("PARAMETERS")]
    [SerializeField] float reactorpower = 10f;
    [SerializeField] Vector2 horizontalNoiseLimits = new Vector2(-10f, 10f);
    [SerializeField] float fuelAmount = 10f;
    float currentFuelAmount = 10f;
    bool reactorActivated = false;
    bool thrust = false;
    bool lastThrustState = false;


    [Header("FX")]
    [SerializeField] ParticleSystem fireFX = null;






    #region FUNCTIONS
    private void Awake()                                                            // AWAKE
    {
        GetComponentsIfNotReferenced();
    }

    void FixedUpdate()                                                              // FIXEDUPDATE
    {
        if (isActiveAndEnabled && enabled)
        {
            if (reactorActivated)
            {
                lastThrustState = thrust;
                thrust = Input.GetButton("Fire1");


                Debug.Log(thrust);

                // FX
                if (lastThrustState && !thrust)
                {
                    Debug.Log("End");
                    fireFX.Stop();
                }


                // FX
                if (!lastThrustState && thrust)
                {
                    Debug.Log("Start");
                    fireFX.Play();
                }


                // THRUST
                if (thrust && currentFuelAmount > 0)
                {
                    
                    // Unstable trajectory
                    Vector2 horizontalNoise = transform.right * Random.Range(horizontalNoiseLimits.x, horizontalNoiseLimits.y);


                    if (rigidbody2d != null)
                        rigidbody2d.AddForce((Vector2)transform.up * reactorpower + horizontalNoise, ForceMode2D.Force);

                    currentFuelAmount -= Time.fixedDeltaTime;
                    UpdateFuelVisual();

                    if (currentFuelAmount <= 0)
                    {
                        fireFX.Stop();
                        currentFuelAmount = 0;
                    }
                }
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
        currentFuelAmount = fuelAmount;
        UpdateFuelVisual();


        fireFX.Stop();
    }




    void UpdateFuelVisual()
    {
        fuelSlider.transform.localScale = new Vector3(currentFuelAmount / fuelAmount, transform.localScale.y);
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
