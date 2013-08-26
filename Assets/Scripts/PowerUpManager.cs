﻿using UnityEngine;
using System.Collections.Generic;

public class PowerUpManager : MonoBehaviour {
  // Singleton
  private static PowerUpManager instance;

  // The PowerUp types
  public enum powType{ projRad, explosionRad, damage, cooldown, rotSpeed, projSpeed, waterChange, fireChange,
    woodChange, earthChange, metalChange, holyChange, slowEnemy };

  private delegate void PowDelegates( PowerUp powUp );
  private static PowDelegates[] activeDelegates = {
    ProjRad,
    ExplosionRad,
    Damage,
    Cooldown,
    RotSpeed,
    ProjSpeed,
    WaterChange,
    FireChange,
    WoodChange,
    EarthChange,
    MetalChange,
    HolyChange,
    SlowEnemy
  };

  private static PowDelegates[] deactiveDelegates = {
    ProjRadDe,
    ExplosionRadDe,
    DamageDe,
    CooldownDe,
    RotSpeedDe,
    ProjSpeedDe,
    WaterChangeDe,
    FireChangeDe,
    WoodChangeDe,
    EarthChangeDe,
    MetalChangeDe,
    HolyChangeDe,
    SlowEnemyDe
  };

  public const float KillX = -27f;
  public const float MaxY = 14.5f;
  public const float SpawnX = 30;  // What X Value to spawn at
  private float spawnChance = 0.5f;  // The percentage chance to spawn, in decimal
  private const float spawnInterval = 1; // The Timer interval when another PowerUp may spawn

  // Object variables
  public PowerUp powPrefab;
  private Stack<PowerUp> inactivePow;
  private HashSet<PowerUp> activePow;
  private HashSet<PowerUp> effectivePow;
  private Timer spawnTimer;

  private int numInEffect;      // Number of powerups in play
  private Vector3 powPosition;  // Probably be moved later, for holding powerups
  private const float powRotZ = 30;
  private float powWidth;

  private System.Random rng;

  void Awake() {
    instance = this; 
    inactivePow = new Stack<PowerUp>();
    activePow = new HashSet<PowerUp>();
    effectivePow = new HashSet<PowerUp>();
    powPosition = new Vector3(-25f, 18f, 10f );
    rng = new System.Random();
    spawnTimer = gameObject.AddComponent<Timer>();
    powWidth = 2 * Mathf.Cos(powRotZ * Mathf.Deg2Rad);

    InitializePowUp();
  }

	// Use this for initialization
	void Start () {
	}

  public void Restart() {
    numInEffect = 0;
    spawnTimer.Restart(spawnInterval);

    // Reloads active PowerUps
    foreach( PowerUp powUp in activePow ) {
      powUp.Reload();
    }
    activePow.Clear();

    // Resets effective PowerUps
    foreach(PowerUp powUp in effectivePow) {
      Deactivate(powUp);
    }
  }

  /**
   * Initializes all PowerUp Variables
   */
  private void InitializePowUp() {
    Projectile.RadPowUp = 1f;
    Explosion.RadPowUp = 1f;
    Cannon.CoolPowUp = 1f;
    Cannon.RotPowUp = 1f;
    Projectile.SpeedPowUp = 1f;
  }
	
	/**
   * Update is called once per frame.  Update will spawn a PowerUp depending on
   * the spawnTimer and chance of spawning one
   */
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

  /**
   * Spawns a PowerUp on the map.  Type, buff status, and element should be
   * randomized
   */
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
    powType type = RandPowType();
    int level = 2;
    bool isBuff = true;   // It's a buff not a debuff
    Element element = RandomElement();

    powUp.Spawn(type,
                level,
                element,
                PowData[type][level-1].speedMult,
                PowData[type][level-1].duration,
                new Vector3(SpawnX, (float)( MaxY * (2 * rng.NextDouble() - 1) ), 0f),
                isBuff);
  }

  /**
   * Activates a PowerUp
   *
   * @param powUp the PowerUp to activate
   */
  public void Activate( PowerUp powUp ) {
    PowDelegates ad = activeDelegates[(int)powUp.Type];
    powUp.Move( (powPosition + numInEffect * (new Vector3(powWidth, 0, 0))) );
    ad( powUp );
    effectivePow.Add(powUp);
  }

  /**
   * Deactivates a PowerUp
   *
   * @param powUp the PowerUp to deactivate
   */
  public void Deactivate( PowerUp powUp ) {
    PowDelegates dd = deactiveDelegates[(int)powUp.Type];
    dd( powUp );
    if( GameManager.state != GameManager.GameState.restart ) {
      effectivePow.Remove(powUp);
    }
  }

  /**
   * Returns a random element
   */
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

  /**
   * Generates a random PowerUp type
   */
  private powType RandPowType() {
    float rand = (float)(13 * rng.NextDouble());
    powType type;

    if( rand < 1 ) {
      type = powType.projRad;
    }
    else if( rand < 2 ) {
      type = powType.explosionRad;
    }
    else if( rand < 3 ) {
      type = powType.damage;
    }
    else if( rand < 4 ) {
      type = powType.cooldown;
    }
    else if( rand < 5 ) {
      type = powType.rotSpeed;
    }
    else if( rand < 6 ) {
      type = powType.projSpeed;
    }
    else if( rand < 7 ) {
      type = powType.waterChange;
    }
    else if( rand < 8 ) {
      type = powType.fireChange;
    }
    else if( rand < 9 ) {
      type = powType.woodChange;
    }
    else if( rand < 10 ) {
      type = powType.earthChange;
    }
    else if( rand < 11 ) {
      type = powType.metalChange;
    }
    else if( rand < 12 ) {
      type = powType.holyChange;
    }
    else {
      type = powType.slowEnemy;
    }

    return type;
  }

  public void Reload(PowerUp powUp) {
    if( GameManager.state != GameManager.GameState.restart ) {
      activePow.Remove(powUp);
    }
    inactivePow.Push(powUp);
  }
  
  ///////////////////////////  PowerUp Activations /////////////////////////
  private static void ProjRad( PowerUp powUp ) {
    Projectile.RadPowUp = PowData[powUp.Type][powUp.Level-1].effectMult;
  }

  private static void ExplosionRad( PowerUp powUp ) {
    Explosion.RadPowUp = PowData[powUp.Type][powUp.Level-1].effectMult;
  }

  private static void Damage( PowerUp powUp ) {
    Debug.Log("TODO handle this");
  }

  private static void Cooldown( PowerUp powUp ) {
    Cannon.CoolPowUp = PowData[powUp.Type][powUp.Level-1].effectMult;
  }

  private static void RotSpeed( PowerUp powUp ) {
    Cannon.RotPowUp = PowData[powUp.Type][powUp.Level-1].effectMult;
  }

  private static void ProjSpeed( PowerUp powUp ) {
    Projectile.SpeedPowUp = PowData[powUp.Type][powUp.Level-1].effectMult;
  }

  private static void WaterChange( PowerUp powUp ) {
    Debug.Log("TODO handle this");
  }

  private static void FireChange( PowerUp powUp ) {
    Debug.Log("TODO handle this");
  }

  private static void WoodChange( PowerUp powUp ) {
    Debug.Log("TODO handle this");
  }

  private static void EarthChange( PowerUp powUp ) {
    Debug.Log("TODO handle this");
  }

  private static void MetalChange( PowerUp powUp ) {
    Debug.Log("TODO handle this");
  }

  private static void HolyChange( PowerUp powUp ) {
    Debug.Log("TODO handle this");
  }

  private static void SlowEnemy( PowerUp powUp ) {
    Debug.Log("TODO handle this");
  }

  ///////////////////////////  PowerUp Deactivations /////////////////////////
  private static void ProjRadDe( PowerUp powUp ) {
    Projectile.RadPowUp = 1f;
  }

  private static void ExplosionRadDe( PowerUp powUp ) {
    Explosion.RadPowUp = 1f;
  }

  private static void DamageDe( PowerUp powUp ) {
    Debug.Log("TODO handle this");
  }

  private static void CooldownDe( PowerUp powUp ) {
    Cannon.CoolPowUp = 1f;
  }

  private static void RotSpeedDe( PowerUp powUp ) {
    Cannon.RotPowUp = 1f;
  }

  private static void ProjSpeedDe( PowerUp powUp ) {
    Projectile.SpeedPowUp = 1f;
  }

  private static void WaterChangeDe( PowerUp powUp ) {
    Debug.Log("TODO handle this");
  }

  private static void FireChangeDe( PowerUp powUp ) {
    Debug.Log("TODO handle this");
  }

  private static void WoodChangeDe( PowerUp powUp ) {
    Debug.Log("TODO handle this");
  }

  private static void EarthChangeDe( PowerUp powUp ) {
    Debug.Log("TODO handle this");
  }

  private static void MetalChangeDe( PowerUp powUp ) {
    Debug.Log("TODO handle this");
  }

  private static void HolyChangeDe( PowerUp powUp ) {
    Debug.Log("TODO handle this");
  }

  private static void SlowEnemyDe( PowerUp powUp ) {
    Debug.Log("TODO handle this");
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

  private static Dictionary< powType, PowAttributes[] > PowData = new Dictionary<powType, PowAttributes[]> {
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
