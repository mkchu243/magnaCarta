using UnityEngine;
using System.Collections.Generic;

public class Cannon : MonoBehaviour {
  public Projectile projectilePrefab;
  public Explosion explosionPrefab;
  private Projectile proj;
  private Explosion explosion;

  void Awake () {
    proj = (Projectile)Instantiate(projectilePrefab, new Vector3(0f, 0f, -100f), Quaternion.identity);
    proj.gameObject.SetActive(false);
    explosion = (Explosion)Instantiate(explosionPrefab, new Vector3(0f, 0f, -100f), Quaternion.identity);
    explosion.gameObject.SetActive(false);
  }

  // Use this for initialization
  void Start(){
  }

  // Update is called once per frame
  void Update () {
  }

  public void Restart(){
    proj.gameObject.SetActive(false);
    explosion.gameObject.SetActive(false);
  }

  public void Fire(Vector3 aim) {
    proj.Spawn(Element.fire, new Vector3(aim.x, aim.y, 0), this);
  }

  public Explosion getExplosion() {
    return explosion;
  }
}