using UnityEngine;
using System.Collections.Generic;

public class Projectile : MonoBehaviour {
  private Timer explodeTimer;
  private Element element;
  private Vector3 traj;  // the trajectory of the projectile
  private Cannon cannon; //needs to know this so it can recycle projectile
  private Explosion explosion;
  private int key;  // needs to know to remove from active dictionary

  public const float KillX = 30f;

  void Awake() {
    explodeTimer = gameObject.AddComponent<Timer>();
  }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
    if( GameManager.state == GameManager.GameState.running ){
      transform.Translate( projData[element].speed * Time.deltaTime * traj, Space.World );

      if( transform.position.x > KillX ) {
        gameObject.SetActive(false);
        Reload();
      }
    }

    /*  May not use explode timer
    if (explodeTimer.time <= 0) {
      cannon.getExplosion().Spawn(element, transform.position);
      gameObject.SetActive(false);
    }
    */
	}

  void OnTriggerEnter(Collider other) {
    explosion.transform.renderer.material = Reference.elements[element].mat;
    explosion.Spawn(element, transform.position, this);
    gameObject.SetActive(false);
  }

  public void Spawn(Element e, Vector3 aim, Vector3 dir, Cannon c, int k) {
    gameObject.SetActive(true);
    explodeTimer.Restart(0.25f);
    element = e;
    transform.position = aim;
    traj = dir;
    cannon = c;
    key = k;
    transform.renderer.material = Reference.elements[element].mat;

    if(explosion == null) {
      explosion = (Explosion)Instantiate(cannon.explosionPrefab, new Vector3(0f, 0f, -100f), Quaternion.identity);
      explosion.gameObject.SetActive(false);
    }
  }

  public void Reload() {
    cannon.Reload(this);
  }

  public int Key {
    get { return key; }
  }

  public Explosion Explosion {
    get { return explosion; }
  }

  //Proj Info
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
      {Element.water, new ProjectileAttributes(BaseSpeedWater, BaseDamageWater, BaseExplosionRadius,     BaseExplosionDuration1)},
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
