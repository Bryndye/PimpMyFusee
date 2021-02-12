using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Camera movements script
public class CameraMovements : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField] Rigidbody2D rigidbody2d = null;

    [Header("REFERENCES")]
    [SerializeField] GameObject moduleToFollow = null;

    
    [Header("STATE")]
    [SerializeField] public CameraMode cameraMode = CameraMode.garage;
    public enum CameraMode
    {
        garage,
        game,
    }




    




    #region FUNCTIONS
    private void Awake()                                                                    // AWAKE
    {
        GetReferences();
    }



    public void StartCamera(bool on = false)
    {
        if (on)
            SwitchState(CameraMode.game);
        else
            SwitchState(CameraMode.garage);
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
    }


    private void OnDrawGizmosSelected()                                                 // ON DRAW GIZMOS SELECTED
    {
        GetReferences();
    }
    #endregion
}
