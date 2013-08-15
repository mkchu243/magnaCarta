using UnityEngine;
using System.Collections.Generic;
using System;

public class EnemyManager : MonoBehaviour {
  private static EnemyManager instance; 
  //game vars const
  private const float SpawnX = 30;
  public const float KillX = -27;
  public const float MaxY = 18;
  public const float MinY = -18;
  
  private const int NumEnemies = 20;
  
  private const float InitialSpeedMult = 1;
  private const float FinalSpeedMult = 5;
  private const float InitialSpawnTime = 3;
  private const float FinalSpawnTime = 1;
  
  //game vars
  private float spawnAttributeRatio = 0.5f;
  private float speedMult;
  private float spawnTime;

  //prefabs
  public BasicEnemy basicEnemyPrefab;
  public TestEnemy testEnemyPrefab;

  //vars
  private Stack<BasicEnemy> inactiveBasicEnemies;
  private Stack<TestEnemy> inactiveTestEnemies;
  private HashSet<Enemy> activeEnemies;

  private System.Random rng;
  private Timer spawnTimer;

  void Awake() {
    Instance = this;
  }

	// Use this for initialization
	void Start() {    
    activeEnemies = new HashSet<Enemy>();
    inactiveBasicEnemies = new Stack<BasicEnemy>();
    inactiveTestEnemies = new Stack<TestEnemy>();
    
    spawnTimer = gameObject.AddComponent<Timer>();
    rng = new System.Random();
	}
	
	// Update is called once per frame
	void Update() {
    switch (GameManager.state){
      case GameManager.GameState.running:
        //spawn new enemies
        if (spawnTimer.time <= 0){
          SpawnEnemies();
          spawnTimer.Restart(spawnTime);
        }
        break;
    }
	}
  
  public void Restart() {
    //reclaim enemies
    foreach (Enemy e in activeEnemies) {
      reclaimEnemy(e);
    }
    activeEnemies.Clear();

    spawnTimer.Restart(0);
    speedMult = InitialSpeedMult;
    spawnTime = InitialSpawnTime;
  }

  public void RemoveEnemy(Enemy e) {
    Player.Instance.handleEnemy(e);
    reclaimEnemy(e);
    activeEnemies.Remove(e);
  }
    
  private void SpawnEnemies() {
    Enemy e;
    //generate enemy
    if (rng.NextDouble() > 0.5f)
      e = popBasicEnemy();
    else
      e = popTestEnemy();

    //generate behavior
    Element elem;
    if (rng.NextDouble() > spawnAttributeRatio)
      elem = Element.water;
    else
      elem = Element.fire;

    e.Spawn(elem, speedMult, new Vector3(SpawnX, UnityEngine.Random.Range(MinY, MaxY), 0));
    activeEnemies.Add(e);
  }

////RECYCLE ENEMY METHODS
  //push the enemy to the right stack
  private void reclaimEnemy(Enemy e) {
    e.Die();
    if (e.GetType() == typeof(BasicEnemy)) {
      inactiveBasicEnemies.Push((BasicEnemy)e);
    } else if (e.GetType() == typeof(TestEnemy)) {
      inactiveTestEnemies.Push((TestEnemy)e);
    }
  }

  //get a basic enemy or create one if there are no inactive ones
  private BasicEnemy popBasicEnemy() {
    BasicEnemy ret;
    if (inactiveBasicEnemies.Count > 0) //pop if you can
      ret = inactiveBasicEnemies.Pop();
    else {                              //if you can't just make one
      ret = (BasicEnemy)Instantiate(basicEnemyPrefab, new Vector3(0f, 0f, -100f), Quaternion.identity);
      ret.transform.parent = transform;
    }
    return ret;
  }

  private TestEnemy popTestEnemy() {
    TestEnemy ret;
    if (inactiveTestEnemies.Count > 0) {
      ret = inactiveTestEnemies.Pop();
    }  else {
      ret = (TestEnemy)Instantiate(testEnemyPrefab, new Vector3(0f, 0f, -100f), Quaternion.identity);
      ret.transform.parent = transform;
    }
    return ret;
  }

  //setters and getters
  public static EnemyManager Instance { get; private set; }
}
