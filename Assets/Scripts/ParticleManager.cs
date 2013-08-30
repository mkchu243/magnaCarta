using System;
using System.Collections.Generic;
using UnityEngine;


public class ParticleManager : MonoBehaviour{
  private static ParticleManager instance;

  public GameObject ailBurnPrefab;

  private ObjectPool<GameObject> ailBurnPool;
  
  void Awake() {
    Instance = this;
  }

  public void Start() {
    ailBurnPool = new ObjectPool<GameObject>(ailBurnPrefab);
  }

  //setters and getters
  public static ParticleManager Instance { get; private set; }

  public GameObject CreateAilBurn() {
    GameObject ret = ailBurnPool.PoolCreate();
    ret.SetActive(true);
    ret.transform.rotation = ailBurnPrefab.transform.rotation;
    return ret;
  }

  public void ReleaseAilBurn(GameObject prefab) {
    prefab.SetActive(false);
    ailBurnPool.PoolRelease(prefab);
  }
}
