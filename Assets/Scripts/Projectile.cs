using UnityEngine;
using System.Collections.Generic;

public class Projectile : MonoBehaviour {
  private Timer explodeTimer;
  private Element element;
  private Vector3 traj;  // the trajectory of the projectile
  private Cannon cannon; //needs to know this so it can recycle projectile

  public const float KillX = 30f;
  private const float initRad = 1f;

  // PowerUp Variables
  private static float radPowUp;
  private static float speedPowUp;

  void Awake() {
    explodeTimer = gameObject.AddComponent<Timer>();
  }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
    switch( GameManager.state ) {
      case GameManager.GameState.running:
        transform.Translate( projData[element].speed * speedPowUp * Time.deltaTime * traj, Space.World );

        if( transform.position.x > KillX ) {
          Reload();
        }
        break;
    }

    /*  May not use explode timer
    if (explodeTimer.time <= 0) {
      cannon.CreateExplosion(element, transform.position);
      gameObject.SetActive(false);
    }
    */
	}

  void OnTriggerEnter(Collider other) {
    if( other.gameObject.GetType() != typeof( Explosion ) ) {
      cannon.CreateExplosion(element, transform.position, this);
      Reload();
    }
  }

  public void Spawn(Element e, Vector3 aim, Vector3 dir, Cannon c) {
    gameObject.SetActive(true);
    explodeTimer.Restart(0.25f);
    element = e;
    transform.position = aim;
    traj = dir;
    cannon = c;
    transform.renderer.material = Reference.elements[element].mat;

    float rad = initRad * radPowUp;
    SetRad(rad);
  }

  private void SetRad(float rad) {
    transform.localScale = new Vector3(rad, rad, rad);
  }

  public void Reload() {
    gameObject.SetActive(false);
    cannon.Reload(this);
  }

  //////////////////////////  Properties  ////////////////////////////
  public Cannon Cannon {
    get { return cannon; }
  }

  public static float RadPowUp {
    get { return radPowUp; }
    set { radPowUp = value; }
  }
  public static float SpeedPowUp {
    get { return speedPowUp; }
    set { speedPowUp = value; }
  }

  ////////////////////////////  Projectile Data  ////////////////////////////
  public const float BaseSpeedWater = 7.0f;
  public const float BaseSpeedFire = 7.0f;
  public const float BaseSpeedWood = 7.0f;
  public const float BaseSpeedEarth = 7.0f;
  public const float BaseSpeedMetal = 7.0f;
  public const float BaseSpeedHoly = 7.0f;

  public const float BaseDamageWater = 2;
  public const float BaseDamageFire = 3;
  public const float BaseDamageWood = 3;
  public const float BaseDamageEarth = 3;
  public const float BaseDamageMetal = 3;
  public const float BaseDamageHoly = 7;

  public const float BaseExplosionRadius = 4;
  public const float BaseExplosionRadiusHoly = 5;

  public const float BaseExplosionDuration1 = 2;
  public static Dictionary<Element, ProjectileAttributes> projData =
  new Dictionary<Element, ProjectileAttributes>{
      {Element.water, new ProjectileAttributes(BaseSpeedWater, BaseDamageWater,  BaseExplosionRadius,     BaseExplosionDuration1)},
      {Element.fire,  new ProjectileAttributes(BaseSpeedFire,  BaseDamageFire,  BaseExplosionRadius,     BaseExplosionDuration1)},
      {Element.wood,  new ProjectileAttributes(BaseSpeedWood,  BaseDamageWood,  BaseExplosionRadius,     BaseExplosionDuration1)},
      {Element.earth,  new ProjectileAttributes(BaseSpeedEarth,  BaseDamageEarth,  BaseExplosionRadius,     BaseExplosionDuration1)},
      {Element.metal,  new ProjectileAttributes(BaseSpeedMetal,  BaseDamageMetal,  BaseExplosionRadius,     BaseExplosionDuration1)},
      {Element.holy,  new ProjectileAttributes(BaseSpeedHoly,  BaseDamageHoly,  BaseExplosionRadiusHoly, BaseExplosionDuration1)}
    };
}


public struct ProjectileAttributes {
  public float speed;
  public float damage;
  public float explosionRadius;
  public float explosionDuration;

  public ProjectileAttributes(float speed,
                              float damage,
                              float explosionRadius,
                              float explosionDuration) {
    this.speed = speed;
    this.damage = damage;
    this.explosionRadius = explosionRadius;
    this.explosionDuration = explosionDuration;
  }
}
