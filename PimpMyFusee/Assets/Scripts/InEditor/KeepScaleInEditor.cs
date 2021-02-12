using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Script to keep an object's global scale no matter his parent scale
public class KeepScaleInEditor : MonoBehaviour
{
    [SerializeField] Vector3 globalScaleToKeep = new Vector3(1, 1, 1);




    // EDITOR
    private void OnDrawGizmos()
    {
        if (transform.lossyScale != globalScaleToKeep)
            transform.lossyScale.Set(globalScaleToKeep.x, globalScaleToKeep.y, globalScaleToKeep.z);

        Debug.Log(transform.lossyScale);
    }
}
