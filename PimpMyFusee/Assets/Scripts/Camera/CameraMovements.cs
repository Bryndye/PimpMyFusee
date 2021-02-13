using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Camera movements script
public class CameraMovements : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField] Rigidbody2D rigidbody2d = null;
    [SerializeField] Camera cameraa = null;

    [Header("REFERENCES")]
    [SerializeField] GameObject moduleToFollow = null;

    
    [Header("STATE")]
    [SerializeField] public CameraMode cameraMode = CameraMode.garage;
    public enum CameraMode
    {
        garage,
        game,
    }

    [Header("PARAMETERS")]
    [SerializeField] float speedMultiplier = 5f;
    [SerializeField] float cameraSpeedLerpSpeedMultiplier = 5f;
    [SerializeField] float upperSizeLimit = 20;
    [SerializeField] float upperSpeedLimit = 50;
    [SerializeField] Vector3 maxOffset = new Vector3(0, 50, 0);
    Vector3 currentOffset = new Vector3(0, 0, 0);
    float baseSize = 5;
    Vector3 basePosition = new Vector3(0, 0, -10);




    




    #region FUNCTIONS
    private void Awake()                                                                    // AWAKE
    {
        GetReferences();
        baseSize = cameraa.orthographicSize;
        basePosition = transform.position;
    }



    private void FixedUpdate()
    {
        if (enabled & isActiveAndEnabled)
            switch (cameraMode)
            {
                case CameraMode.game:
                    // Movements
                    if (moduleToFollow != null)
                        CameraMovementsHandle(moduleToFollow.transform.position);
                    else
                        moduleToFollow = FindObjectOfType<Module>().gameObject;


                    // SIZE
                    float newSize = baseSize;
                    float speed = rigidbody2d.velocity.magnitude;
                    if (speed < 1)
                        newSize = baseSize;
                    else if (speed >= upperSpeedLimit)
                        newSize = upperSizeLimit;
                    else
                        newSize = baseSize + (upperSizeLimit - baseSize) * (speed / upperSpeedLimit);
                    cameraa.orthographicSize = Mathf.Lerp(cameraa.orthographicSize, newSize, Time.deltaTime);
                    break;



                case CameraMode.garage:
                    // Movements
                    CameraMovementsHandle(basePosition);
                    cameraa.orthographicSize = Mathf.Lerp(cameraa.orthographicSize, baseSize, Time.deltaTime * 10);
                    break;
            }
    }





    void CameraMovementsHandle(Vector3 positionToFollow, float movementSPeed = 5f)
    {
        // OFFSET
        Vector3 newOffset = new Vector3(0, 0, 0);


        float speed = rigidbody2d.velocity.magnitude;
        if (speed < 1)
            newOffset = Vector3.zero;
        else if (speed >= upperSpeedLimit)
            newOffset = maxOffset;
        else
            newOffset = maxOffset * (speed / upperSpeedLimit);

        currentOffset = Vector3.Lerp(currentOffset, newOffset, Time.deltaTime);



        // MOVEMENTS
        Vector3 newCameraVelocity = new Vector3((positionToFollow.x + currentOffset.x) - transform.position.x, (positionToFollow.y + currentOffset.y) - transform.position.y, 0) * speedMultiplier;
        rigidbody2d.velocity = Vector3.Lerp(rigidbody2d.velocity, newCameraVelocity, Time.fixedDeltaTime * cameraSpeedLerpSpeedMultiplier);

        //Vector3 newPosition = Vector3.Lerp(transform.position, positionToFollow + currentOffset, Time.deltaTime * movementSPeed);
        //transform.position = newPosition;
    }









    /// <summary>
    /// Sets the camera game behaviour
    /// </summary>
    /// <param name="on"></param>
    public void StartCamera(bool on = false)
    {
        // Behaviour
        if (on)
        {
            baseSize = cameraa.orthographicSize;
            SwitchState(CameraMode.game);
        }
        else
            SwitchState(CameraMode.garage);


        // Physics
        /*
        if (rigidbody2d)
            rigidbody2d.simulated = on;
            */
    }



    /// <summary>
    /// Switched the camera mode on which its behaviour depends
    /// </summary>
    /// <param name="newMode"></param>
    public void SwitchState(CameraMode newMode = CameraMode.garage)
    {
        cameraMode = newMode;

        switch (newMode)
        {
            case CameraMode.garage:
                break;

            case CameraMode.game:
                break;
        }
    }






    // EDITOR
    // Check if needed references are here
    void GetReferences()
    {
        if (rigidbody2d == null)
            if (GetComponent<Rigidbody2D>())
                rigidbody2d = GetComponent<Rigidbody2D>();

        if (cameraa == null)
            if (GetComponent<Camera>())
                cameraa = GetComponent<Camera>();
    }


    private void OnDrawGizmosSelected()                                                 // ON DRAW GIZMOS SELECTED
    {
        GetReferences();
    }
    #endregion
}
