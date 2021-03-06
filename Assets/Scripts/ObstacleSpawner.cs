﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour {

  [SerializeField]
  private ScoreController scoreController;
  [SerializeField]
  private float maxOffset;
  [SerializeField]
  private GameObject asteroid;
  [SerializeField]
  private GameObject enemyFighter;
  [SerializeField]
  private float spawnFighterProbability;
  [SerializeField]
  private float delayBeforeAnySpawns;
  [SerializeField]
  private float initialDelayBetweenSpawns;
  [SerializeField]
  private float minSpawnDelay;
  [SerializeField]
  private float delayDecayRate;
  [SerializeField]
  private Transform target;
  [SerializeField]
  private float initialObstacleSpeed;
  [SerializeField]
  private float obstacleSpeedIncreaseRate;
  [SerializeField]
  private float maxObstacleSpeed;
  [SerializeField]
  private float enemyFighterSpeed;
  [SerializeField]
  private GameObject player;

  private float spawnDelay;
  private float spawnTimer;
  private float obstacleSpeed;
  private bool allowSpawning;

  private void Start() {
    spawnTimer = 0;
  }

  public void ResetSpawning() {
    StopSpawning();
    StartSpawning();
  }

  public void StopSpawning() {
    allowSpawning = false;
  }

  private void StartSpawning() {
    spawnDelay = initialDelayBetweenSpawns;
    obstacleSpeed = initialObstacleSpeed;
    spawnTimer = delayBeforeAnySpawns;
    allowSpawning = true;
  }
  

  private void Update() {
    spawnTimer -= Time.deltaTime;
    if (allowSpawning && (spawnTimer <= 0)) {
      spawnDelay = Mathf.Max(minSpawnDelay, spawnDelay - delayDecayRate);
      spawnTimer = spawnDelay;

      ChooseObstacleToSpawn();
    }
  }

  private void ChooseObstacleToSpawn() {
    if (Random.Range(0.0f, 1.0f) <= spawnFighterProbability) {
      SpawnFighter();
    } else {
      SpawnAsteroid();
    }

  }

  private void SpawnFighter() {
    SetFighterTarget(SpawnObstacle(enemyFighter));
  }

  private void SetFighterTarget(GameObject fighter) {
    AutoTargetMovementController autoTargetMovementController = fighter.GetComponent<AutoTargetMovementController>();
    if (autoTargetMovementController != null) {
      autoTargetMovementController.SetSpeed(enemyFighterSpeed);
      autoTargetMovementController.SetTarget(player);
    }
  }

  private void SpawnAsteroid() {
    SetAsteroidTarget(SpawnObstacle(asteroid));
  }

  private void SetAsteroidTarget(GameObject spawnedObstacle) {
    SingleDirectionMover singleDirectionMover = spawnedObstacle.GetComponent<SingleDirectionMover>();
    if (singleDirectionMover != null) {
      obstacleSpeed = Mathf.Min(maxObstacleSpeed, obstacleSpeed + obstacleSpeedIncreaseRate);
      float targetOffset = maxOffset * Random.Range(-1.0f, 1.0f);
      Vector3 targetPosition = new Vector3(target.position.x + targetOffset, target.position.y, target.position.z);
      singleDirectionMover.SetVelocity(obstacleSpeed * (targetPosition - spawnedObstacle.transform.position).normalized);
    }
  }

  private GameObject SpawnObstacle(GameObject obstacle) {
    Vector3 objectPosition = gameObject.transform.position;
    float offset = maxOffset * Random.Range(-1.0f, 1.0f);
    Vector3 spawnPosition = new Vector3(objectPosition.x + offset, objectPosition.y, objectPosition.z);
    GameObject spawnedObstacle = Instantiate(obstacle, spawnPosition, gameObject.transform.rotation) as GameObject;

    spawnedObstacle.GetComponent<ObstacleHealth>().SetScoreController(scoreController);

    return spawnedObstacle;
  }
}
