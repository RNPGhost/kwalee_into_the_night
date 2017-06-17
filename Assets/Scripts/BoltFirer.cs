﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltFirer : MonoBehaviour {

  [SerializeField]
  private GameObject boltPrefab;
  [SerializeField]
  private Transform boltSpawn;
  [SerializeField]
  private bool fireOnStart;
  [SerializeField]
  private float boltCooldown;
  [SerializeField]
  private float boltSpeed;

  private float boltTimer;
  private bool fire;

  public void StartFiring() {
    fire = true;
  }

  public void StopFiring() {
    fire = false;
  }

  private void Start() {
    boltTimer = 0;
    fire = fireOnStart;
  }

  private void Update () {
    boltTimer -= Time.deltaTime;
    if (fire && (boltTimer <= 0)) {
      GameObject bolt = Instantiate(boltPrefab, boltSpawn.position, boltSpawn.rotation) as GameObject;
      bolt.GetComponent<SingleDirectionMover>().SetVelocity(boltSpeed * Vector3.forward);
      boltTimer = boltCooldown;
    }
	}
}
