using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;

public class Projectile : MonoBehaviour {
  private Stopwatch explodeTimer;
  private float explodeTime;
  private Element element;
  private float speed;
  private Cannon cannon; //needs to know this so it can recycle explosion

  void Awake() {
    explodeTimer = new Stopwatch();
  }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
    if (explodeTimer.Elapsed.Seconds >= explodeTime) {
      cannon.getExplosion().Spawn(element, transform.position);
      gameObject.SetActive(false);
    }
	}

  public void Spawn(Element e, Vector3 aim, Cannon c) {
    gameObject.SetActive(true);
    explodeTime = 0.25f; //TODO hard coded number
    explodeTimer.Reset();
    explodeTimer.Start();
    element = e;
    transform.position = aim;
    cannon = c;
  }

  //Proj Info
  public const float BaseSpeedWater = -5.0f;
  public const float BaseSpeedFire = -5.0f;
  public const float BaseSpeedWood = -5.0f;
  public const float BaseSpeedEarth = -5.0f;
  public const float BaseSpeedMetal = -5.0f;
  public const float BaseSpeedHoly = -5.0f;

  public const float BaseDamageWater = 2;
  public const float BaseDamageFire = 3;
  public const float BaseDamageWood = 2;
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
      {Element.earth, new ProjectileAttributes(BaseSpeedEarth, BaseDamageEarth, BaseExplosionRadius,     BaseExplosionDuration1)},
      {Element.metal, new ProjectileAttributes(BaseSpeedMetal, BaseDamageMetal, BaseExplosionRadius,     BaseExplosionDuration1)},
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
