using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;

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
  private const float InitialSpawnTime = 1;//3;
  private const float FinalSpawnTime = 1;
  
  //game vars
  private bool startSpawn;
  private float spawnAttributeRatio = 0.5f;
  private float speedMult;
  private float spawnTime;

  //prefabs
  public BasicEnemy basicEnemyPrefab;
  public TestEnemy testEnemyPrefab;

  //vars
  private ObjectPool<BasicEnemy> basicPool;

  private System.Random rng;
  private Stopwatch spawnTimer;

  void Awake() {
    Instance = this;
  }

	// Use this for initialization
	void Start() {
    basicPool = new ObjectPool<BasicEnemy>(basicEnemyPrefab);
    
    spawnTimer = new Stopwatch();
    rng = new System.Random();
	}
	
	// Update is called once per frame
	void Update() {
    switch (GameManager.state){
      case GameManager.GameState.running:
        //spawn new enemies
        if (spawnTimer.Elapsed.Seconds >= spawnTime || startSpawn){
          SpawnEnemies();
          spawnTimer.Reset();
          spawnTimer.Start();
          startSpawn = false;
        }
        break;
    }
	}
  
  public void Restart() {
    //reclaim enemies
    foreach (BasicEnemy e in basicPool.ActivePool) {
      e.Die();
    }
    basicPool.Clear();

    startSpawn = true;
    spawnTimer.Reset();
    speedMult = InitialSpeedMult;
    spawnTime = InitialSpawnTime;
  }

  public void RemoveEnemy(Enemy e) {
    Player.Instance.handleEnemy(e);
    e.Die();
    reclaimEnemy(e);
  }
    
  private void SpawnEnemies() {
    Enemy e;
    //generate enemy
//    if (rng.NextDouble() > 0.5f)
      e = basicPool.PoolCreate();
 //   else
 //     e = popTestEnemy();

    //generate behavior
    Element elem;
   // if (rng.NextDouble() > spawnAttributeRatio)
   //   elem = Element.water;
   // else
      elem = Element.fire;

    if(e)
      e.Spawn(elem, speedMult, new Vector3(SpawnX, UnityEngine.Random.Range(MinY, MaxY), 0));
  }

////RECYCLE ENEMY METHODS
  //push the enemy to the right stack
  private void reclaimEnemy(Enemy e) {
    if (e.GetType() == typeof(BasicEnemy)) {
      basicPool.PoolRelease((BasicEnemy)e);
    } else if (e.GetType() == typeof(TestEnemy)) {
      //inactiveTestEnemies.Push((TestEnemy)e);
    }
  }

  //setters and getters
  public static EnemyManager Instance { get; private set; }
}
