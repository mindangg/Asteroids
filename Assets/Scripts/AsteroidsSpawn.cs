using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidsSpawn : MonoBehaviour
{
    public Asteroids asteroidsPrefab;
    private float spawnRate = 2;
    private float spawnDistance = 15;
    private float trajectotyVariance = 15;

    private void Start()
    {
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
    }

    private void Spawn()
    {
        Vector3 spawnDirection = Random.insideUnitCircle.normalized * spawnDistance;
        Vector3 spawnPoint = transform.position + spawnDirection;

        float variance = Random.Range(-trajectotyVariance, trajectotyVariance);
        Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);

        Asteroids asteroids = Instantiate(asteroidsPrefab, spawnPoint, rotation);
        asteroids.size = Random.Range(asteroids.minSize, asteroids.maxSize);
        asteroids.SetTrajectory(rotation * -spawnDirection);
    }
}
