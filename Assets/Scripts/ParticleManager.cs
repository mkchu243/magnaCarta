using System;
using System.Collections.Generic;
using UnityEngine;


public class ParticleManager : MonoBehaviour{
  private static ParticleManager instance;

  public GameObject ailFreezePrefab;
  public GameObject ailBurnPrefab;
  public GameObject ailRootPrefab;
  public GameObject ailDamPrefab;
  public GameObject ailCutPrefab;

  private ObjectPool<GameObject> ailFreezePool;
  private ObjectPool<GameObject> ailBurnPool;
  private ObjectPool<GameObject> ailRootPool;
  private ObjectPool<GameObject> ailDamPool;
  private ObjectPool<GameObject> ailCutPool;

  public static GameObject[] ailPrefabs;
  public static ObjectPool<GameObject>[] ailPools;
  
  void Awake() {
    Instance = this;
  }

  public void Start() {
    ailFreezePool = new ObjectPool<GameObject>(ailFreezePrefab);
    ailBurnPool   = new ObjectPool<GameObject>(ailBurnPrefab);
    ailRootPool   = new ObjectPool<GameObject>(ailRootPrefab);
    ailDamPool = new ObjectPool<GameObject>(ailDamPrefab);
    ailCutPool = new ObjectPool<GameObject>(ailCutPrefab);

    ailPrefabs = new GameObject[]{
      ailFreezePrefab,
      ailBurnPrefab,
      ailRootPrefab,
      ailDamPrefab,
      ailCutPrefab
    };

    ailPools = new ObjectPool<GameObject>[]{
      ailFreezePool,
      ailBurnPool,
      ailRootPool,
      ailDamPool,
      ailCutPool
    };
  }

  //setters and getters
  public static ParticleManager Instance { get; private set; }

  public GameObject CreateAilParticle(ailmentType atype) {
    GameObject ret = ailPools[(int) atype].PoolCreate();
    ret.SetActive(true);
    ret.transform.rotation = ailPrefabs[(int) atype].transform.rotation;
    return ret;
  }

  public void ReleaseAilParticle(ailmentType atype, GameObject prefab) {
    prefab.SetActive(false);
    ailPools[(int) atype].PoolRelease(prefab);
  }
}
