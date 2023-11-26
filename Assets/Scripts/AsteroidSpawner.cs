using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] Asteroid asteroidPrefab;

    [Header("Stats")]
    [SerializeField] float spawnRate = 2.0f;
    [SerializeField] float trajectoryVariance = 15.0f;

    [SerializeField] int spawnAmount = 1;
    [SerializeField] float spawnDistance = 15.0f; //атсероиды всегда будут спавниться за пределами камеры

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnRate)
        {
            Spawn();
            timer = 0f;
        }
    }

    private void Spawn()
    {
        for (int i = 0; i < this.spawnAmount; i++)
        {
            Vector3 spawnDirection = Random.insideUnitCircle.normalized * this.spawnDistance; //напрвление спавна астероидов на границе круга(normalized) (самого спавнера)
            Vector3 spawnPoint = this.transform.position + spawnDirection;
            float variance = Random.Range(-this.trajectoryVariance, this.trajectoryVariance);
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward/*вращение на угол variance вдоль оси z*/);

            //создаём астероид и устанавливаем его размеры, траекторию полёта
            Asteroid asteroid = Instantiate(this.asteroidPrefab, spawnPoint, rotation);
            asteroid.size = Random.Range(asteroid.minSize, asteroid.maxSize);
            asteroid.SetTrajectory(rotation * -spawnDirection);//отрицательное, т.к. нам нужно чтоб траектория была направлена внутрь области видимости камеры
        }


    }
}
