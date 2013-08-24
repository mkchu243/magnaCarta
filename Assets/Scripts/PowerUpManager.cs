using UnityEngine;
using System.Collections.Generic;

public class PowerUpManager : MonoBehaviour {
  private static PowerUpManager instance;
  public enum powType{ projRad, explosionRad, damage, cooldown, rotSpeed, projSpeed, waterChange, fireChange,
    woodChange, earthChange, metalChange, holyChange, slowEnemy };

  public const float SpawnX = 30;
  private float spawnChance = 0.5f;
  private const float spawnInterval = 1;

  public PowerUp powPrefab;
  private Stack<PowerUp> inactivePow;
  private HashSet<PowerUp> activePow;
  private int numInEffect;
  private Vector3 powPosition;
  private Timer spawnTimer;

  private System.Random rng;

  void Awake() {
    instance = this; 
    inactivePow = new Stack<PowerUp>();
    activePow = new HashSet<PowerUp>();
    powPosition = new Vector3(0f,0f,0f);
    rng = new System.Random();
    spawnTimer = gameObject.AddComponent<Timer>();

  }

	// Use this for initialization
	void Start () {
	}

  public void Restart() {
    numInEffect = 0;
    spawnTimer.Restart(spawnInterval);
  }
	
	// Update is called once per frame
	void Update () {
    switch( GameManager.state ) {
      case GameManager.GameState.running:
        if( spawnTimer.TheTime <= 0 ) {
          float rand = (float)(rng.NextDouble());
          if( rand < spawnChance ) {
            SpawnPowUp();
          }
          spawnTimer.Restart(spawnInterval);
        }
        break;
    }
	}

  private void SpawnPowUp() {
    PowerUp powUp;
    if( inactivePow.Count > 0 ) {
      powUp = inactivePow.Pop();
    }
    else {
      powUp = (PowerUp)( Instantiate(powPrefab, new Vector3(0f, 0f, -100f), Quaternion.identity) );
    }
    
    activePow.Add(powUp);
    
    // TODO randomize these
    powType type = powType.projRad;
    int level = 2;
    bool isBuff = true;

    powUp.Spawn(type,
                level,
                Element.fire,
                PowData[type][level].speedMult,
                PowData[type][level].duration,
                new Vector3(SpawnX, (float)(39 * rng.NextDouble() - 18), 0f),
                isBuff);
  }

  public void Reload(PowerUp pow) {
    activePow.Remove(pow);
    inactivePow.Push(pow);
  }

  ///////////////////////////  Properties  //////////////////////////
  public static PowerUpManager Instance {
    get { return instance; }
    private set{}
  }

  public int NumInEffect {
    get { return numInEffect; }
    set { numInEffect = value; }
  }

  public Vector3 PowPosition {
    get { return powPosition; }
  }

  ///////////////////////////  Data  //////////////////////////////
  private struct PowAttributes{
    public float speedMult;
    public float duration;
    public float effectMult;

    public PowAttributes(float speedMult,
                   float duration,
                   float effectMult) {
      this.speedMult = speedMult;
      this.duration = duration;
      this.effectMult = effectMult;
    }
  }

  // TODO customize numbers
  private static Dictionary<int, PowAttributes> projRadLevels = new Dictionary<int, PowAttributes> {
    { 1, new PowAttributes(1f, 30f, 1.5f) },
    { 2, new PowAttributes(1.5f, 40f, 1.75f) },
    { 3, new PowAttributes(1.75f, 50f, 2f) }
  };

  private static Dictionary<int, PowAttributes> explosionRadLevels = new Dictionary<int, PowAttributes> {
    { 1, new PowAttributes(1f, 30f, 1.5f) },
    { 2, new PowAttributes(1.5f, 40f, 1.75f) },
    { 3, new PowAttributes(1.75f, 50f, 2f) }
  };

  private static Dictionary<int, PowAttributes> damageLevels = new Dictionary<int, PowAttributes> {
    { 1, new PowAttributes(1f, 30f, 1.5f) },
    { 2, new PowAttributes(1.5f, 40f, 1.75f) },
    { 3, new PowAttributes(1.75f, 50f, 2f) }
  };

  private static Dictionary<int, PowAttributes> cooldownLevels = new Dictionary<int, PowAttributes> {
    { 1, new PowAttributes(1f, 30f, 1.5f) },
    { 2, new PowAttributes(1.5f, 40f, 1.75f) },
    { 3, new PowAttributes(1.75f, 50f, 2f) }
  };

  private static Dictionary<int, PowAttributes> rotSpeedLevels = new Dictionary<int, PowAttributes> {
    { 1, new PowAttributes(1f, 30f, 1.5f) },
    { 2, new PowAttributes(1.5f, 40f, 1.75f) },
    { 3, new PowAttributes(1.75f, 50f, 2f) }
  };

  private static Dictionary<int, PowAttributes> projSpeedLevels = new Dictionary<int, PowAttributes> {
    { 1, new PowAttributes(1f, 30f, 1.5f) },
    { 2, new PowAttributes(1.5f, 40f, 1.75f) },
    { 3, new PowAttributes(1.75f, 50f, 2f) }
  };

  // Note: normal element changes do not need durations nor effect multipliers
  private static Dictionary<int, PowAttributes> waterChangeLevels = new Dictionary<int, PowAttributes> {
    { 1, new PowAttributes(1f, 30f, 1f) },
    { 2, new PowAttributes(1.5f, 0f, 1f) },
    { 3, new PowAttributes(1.75f, 50f, 1f) }
  };

  private static Dictionary<int, PowAttributes> fireChangeLevels = new Dictionary<int, PowAttributes> {
    { 1, new PowAttributes(1f, 30f, 1f) },
    { 2, new PowAttributes(1.5f, 0f, 1f) },
    { 3, new PowAttributes(1.75f, 50f, 1f) }
  };

  private static Dictionary<int, PowAttributes> woodChangeLevels = new Dictionary<int, PowAttributes> {
    { 1, new PowAttributes(1f, 30f, 1f) },
    { 2, new PowAttributes(1.5f, 0f, 1f) },
    { 3, new PowAttributes(1.75f, 50f, 1f) }
  };

  private static Dictionary<int, PowAttributes> earthChangeLevels = new Dictionary<int, PowAttributes> {
    { 1, new PowAttributes(1f, 30f, 1f) },
    { 2, new PowAttributes(1.5f, 0f, 1f) },
    { 3, new PowAttributes(1.75f, 50f, 1f) }
  };

  private static Dictionary<int, PowAttributes> metalChangeLevels = new Dictionary<int, PowAttributes> {
    { 1, new PowAttributes(1f, 30f, 1f) },
    { 2, new PowAttributes(1.5f, 0f, 1f) },
    { 3, new PowAttributes(1.75f, 50f, 1f) }
  };

  private static Dictionary<int, PowAttributes> holyChangeLevels = new Dictionary<int, PowAttributes> {
    { 1, new PowAttributes(1f, 30f, 1.5f) },
    { 2, new PowAttributes(1.5f, 40f, 1.75f) },
    { 3, new PowAttributes(1.75f, 50f, 2f) }
  };

  private static Dictionary<int, PowAttributes> slowEnemyLevels = new Dictionary<int, PowAttributes> {
    { 1, new PowAttributes(1f, 30f, 1.5f) },
    { 2, new PowAttributes(1.5f, 40f, 1.75f) },
    { 3, new PowAttributes(1.75f, 50f, 2f) }
  };

  private Dictionary< powType, Dictionary<int, PowAttributes> > PowData = new Dictionary<powType, Dictionary<int, PowAttributes>> {
    { powType.projRad, projRadLevels },
    { powType.explosionRad, explosionRadLevels },
    { powType.damage, damageLevels },
    { powType.cooldown, cooldownLevels },
    { powType.rotSpeed, rotSpeedLevels },
    { powType.projSpeed, projSpeedLevels },
    { powType.waterChange, waterChangeLevels },
    { powType.fireChange, fireChangeLevels },
    { powType.woodChange, woodChangeLevels },
    { powType.earthChange, earthChangeLevels },
    { powType.metalChange, metalChangeLevels },
    { powType.holyChange, holyChangeLevels },
    { powType.slowEnemy, slowEnemyLevels }
  };

}
