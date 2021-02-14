using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] GameObject asteroidPrefab = null;
    [SerializeField] GameObject smallAsteroidPrefab = null;
    [SerializeField] Vector2 randomAsteroidsNumberLimits = new Vector2(10, 20);
    [SerializeField] Vector2 asteroidsZoneDimensions = new Vector2(50, 100);
    [SerializeField] float smallAsteroidSpawn = 20f;


    private void Start()
    {
        int numberOfAsteroids = (int)Random.Range(randomAsteroidsNumberLimits.x, randomAsteroidsNumberLimits.y);
        

        for (int i = 0; i < numberOfAsteroids; i++)
        {
            GameObject asteroidToInstantiate = asteroidPrefab;

            float randomAsteroidChoice = Random.Range(0, 100);
            if (randomAsteroidChoice < smallAsteroidSpawn)
                asteroidToInstantiate = smallAsteroidPrefab;

            float randomXLocation = Random.Range(- asteroidsZoneDimensions.x / 2, asteroidsZoneDimensions.x / 2);
            float randomYLocation = Random.Range(-asteroidsZoneDimensions.y / 2, asteroidsZoneDimensions.y / 2);
            Vector2 randomLocation = new Vector2(randomXLocation, randomYLocation);

            GameObject newAsteroid = Instantiate(asteroidToInstantiate, (Vector2)transform.position + randomLocation, transform.rotation);
        }

    }
}
