using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// The script for the asteroid's behaviour
public class Asteroid01 : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField] Rigidbody2D rigidbody2d = null;


    [Header("PARAMETERS")]
    [SerializeField] Vector2 randomRotationSpeedLimits = new Vector2(-5f, 5f);
    [SerializeField] Vector2 randomForceXLimits = new Vector2(-5f, 5f);
    [SerializeField] Vector2 randomForceYLimits = new Vector2(-5f, 5f);
    [SerializeField] Vector2 randomScaleLimits = new Vector2(0.5f, 5f);





    private void Awake()                                                // AWAKE
    {
        GetReferences();
    }


    void Start()                                                                    // START
    {
        if (rigidbody2d != null)
        {
            // LOOK
            Vector3 randomRotation = new Vector3(0, 0, Random.Range(0f, 360f));
            transform.eulerAngles = randomRotation;
            transform.localScale = transform.localScale * Random.Range(randomScaleLimits.x, randomScaleLimits.y);


            // PHYSICS
            float randomTorque = Random.Range(randomRotationSpeedLimits.x, randomRotationSpeedLimits.y);
            float randomForceX = Random.Range(randomForceXLimits.x, randomForceXLimits.y);
            float randomForceY = Random.Range(randomForceYLimits.x, randomForceYLimits.y);

            rigidbody2d.AddTorque(randomTorque);
            rigidbody2d.AddForce(new Vector2(randomForceX, randomForceY), ForceMode2D.Impulse);
        }
    }






    void GetReferences()
    {
        if (rigidbody2d == null)
            if (GetComponent<Rigidbody2D>())
                rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // EDITOR
    private void OnDrawGizmosSelected()
    {
        GetReferences();
    }
}
