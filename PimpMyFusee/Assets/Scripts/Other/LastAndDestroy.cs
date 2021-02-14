using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Allow to give  gameobject a max life duration
// OPTIMIZED
public class LastAndDestroy : MonoBehaviour
{
    //Wait amount
    [SerializeField] public float lastDuration;
    [SerializeField] bool activate = true;
    float spawnTimecode;

    //Game object to spawn
    [SerializeField] public List<GameObject> gameObjectsToSpawnOnDestroy = new List<GameObject>();









	void Start ()                                               // START
    {
        spawnTimecode = Time.time;
	}
	

	void Update ()                                                      // UPDATE
    {
        if (enabled && isActiveAndEnabled)
            if (activate && Time.time >= spawnTimecode + lastDuration)
                Destroy(gameObject);
	}

    private void OnDestroy()                                                        // ON DESTROY
    {
        if (gameObjectsToSpawnOnDestroy != null && gameObjectsToSpawnOnDestroy.Count > 0)
        {
            for (int i = 0; i < gameObjectsToSpawnOnDestroy.Count; i++)
                if (gameObjectsToSpawnOnDestroy[i] != null)
                Instantiate(gameObjectsToSpawnOnDestroy[i], gameObject.transform.position, gameObject.transform.rotation);
        }
    }
}
