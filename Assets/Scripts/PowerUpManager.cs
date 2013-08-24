using UnityEngine;
using System.Collections.Generic;

public class PowerUpManager : MonoBehaviour {
  private static PowerUpManager instance;
  // The PowerUp types
  public enum powType{ projRad, explosionRad, damage, cooldown, rotSpeed, projSpeed, waterChange, fireChange,
    woodChange, earthChange, metalChange, holyChange, slowEnemy };

  public const float SpawnX = 30;  // What X Value to spawn at
  private float spawnChance = 0.5f;  // The percentage chance to spawn, in decimal
  private const float spawnInterval = 1; // The Timer interval when another PowerUp may spawn

  // Object variables
  public PowerUp powPrefab;
  private Stack<PowerUp> inactivePow;
  private HashSet<PowerUp> activePow;
  private Timer spawnTimer;

  private int numInEffect;      // Number of powerups in play
  private Vector3 powPosition;  // Probably be moved later, for holding powerups

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
    bool isBuff = true;   // It's a buff not a debuff
    Element element = RandomElement();

    powUp.Spawn(type,
                level,
                element,
                PowData[type][level-1].speedMult,
                PowData[type][level-1].duration,
                new Vector3(SpawnX, (float)(39 * rng.NextDouble() - 18), 0f),
                isBuff);
  }

  public Element RandomElement() {
    float rand = (float)(6 * rng.NextDouble());
    Element element;
    if( rand < 1 ) {
      element = Element.water;
    }
    else if( rand < 2 ) {
      element = Element.fire;
    }
    else if( rand < 3 ) {
      element = Element.wood;
    }
    else if( rand < 4 ) {
      element = Element.earth;
    }
    else if( rand < 5 ) {
      element = Element.metal;
    }
    else {
      element = Element.holy;
    }

    return element;
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
    public float speedMult;      // Multiplier for movement speed
    public float duration;       // Duration of PowerUp effect
    public float effectMult;     // How much the PowerUp effects its respective area

    public PowAttributes(float speedMult,
                   float duration,
                   float effectMult) {
      this.speedMult = speedMult;
      this.duration = duration;
      this.effectMult = effectMult;
    }
  }

  // TODO customize numbers

  private static PowAttributes[] projRadLevels = {
    new PowAttributes(1f, 30f, 1.5f),
    new PowAttributes(1.5f, 40f, 1.75f),
    new PowAttributes(1.75f, 50f, 2f)
  };

  private static PowAttributes[] explosionRadLevels = {
    new PowAttributes(1f, 30f, 1.5f),
    new PowAttributes(1.5f, 40f, 1.75f),
    new PowAttributes(1.75f, 50f, 2f)
  };

  private static PowAttributes[] damageLevels = {
    new PowAttributes(1f, 30f, 1.5f),
    new PowAttributes(1.5f, 40f, 1.75f),
    new PowAttributes(1.75f, 50f, 2f)
  };

  private static PowAttributes[] cooldownLevels = {
    new PowAttributes(1f, 30f, 1.5f),
    new PowAttributes(1.5f, 40f, 1.75f),
    new PowAttributes(1.75f, 50f, 2f)
  };

  private static PowAttributes[] rotSpeedLevels = {
    new PowAttributes(1f, 30f, 1.5f),
    new PowAttributes(1.5f, 40f, 1.75f),
    new PowAttributes(1.75f, 50f, 2f)
  };

  private static PowAttributes[] projSpeedLevels = {
    new PowAttributes(1f, 30f, 1.5f),
    new PowAttributes(1.5f, 40f, 1.75f),
    new PowAttributes(1.75f, 50f, 2f)
  };

  // Note: normal element changes do not need durations nor effect multipliers
  private static PowAttributes[] waterChangeLevels = {
    new PowAttributes(1f, 30f, 1f),
    new PowAttributes(1.5f, 0f, 1f),
    new PowAttributes(1.75f, 50f, 1f)
  };

  private static PowAttributes[] fireChangeLevels = {
    new PowAttributes(1f, 30f, 1f),
    new PowAttributes(1.5f, 0f, 1f),
    new PowAttributes(1.75f, 50f, 1f)
  };

  private static PowAttributes[] woodChangeLevels = {
    new PowAttributes(1f, 30f, 1f),
    new PowAttributes(1.5f, 0f, 1f),
    new PowAttributes(1.75f, 50f, 1f)
  };

  private static PowAttributes[] earthChangeLevels = {
    new PowAttributes(1f, 30f, 1f),
    new PowAttributes(1.5f, 0f, 1f),
    new PowAttributes(1.75f, 50f, 1f)
  };

  private static PowAttributes[] metalChangeLevels = {
    new PowAttributes(1f, 30f, 1f),
    new PowAttributes(1.5f, 0f, 1f),
    new PowAttributes(1.75f, 50f, 1f)
  };

  private static PowAttributes[] holyChangeLevels = {
    new PowAttributes(1f, 30f, 1.5f),
    new PowAttributes(1.5f, 40f, 1.75f),
    new PowAttributes(1.75f, 50f, 2f)
  };

  private static PowAttributes[] slowEnemyLevels = {
    new PowAttributes(1f, 30f, 1.5f),
    new PowAttributes(1.5f, 40f, 1.75f),
    new PowAttributes(1.75f, 50f, 2f)
  };

  private Dictionary< powType, PowAttributes[] > PowData = new Dictionary<powType, PowAttributes[]> {
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
