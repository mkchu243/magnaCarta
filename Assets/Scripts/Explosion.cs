using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
  private Timer lifeTimer;
  private Element element;
  private const float initRad = 1;
  private float maxRad;
  private float duration;
  private float speed;
  private Projectile proj; // Needs to know so projectile can know when to reload

  void Awake() {
    transform.Rotate(new Vector3(90, 0, 0));
    lifeTimer = gameObject.AddComponent<Timer>();
  }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
    if (lifeTimer.TheTime <= 0) {
      gameObject.SetActive(false);
      proj.Reload();
    }
    else if (transform.localScale.x < maxRad) {
      float rad = (1f - (lifeTimer.TheTime / speed) / duration) * (maxRad - initRad) + initRad;
      gameObject.transform.localScale = new Vector3( rad, 0.25f, rad);
    }
	}

  public void Spawn(Element e, Vector3 pos, Projectile p) {
    gameObject.SetActive(true);
    transform.position = pos;
    duration = Projectile.projData[e].explosionDuration;
    lifeTimer.Restart(duration);
    maxRad = Projectile.projData[e].explosionRadius;
    gameObject.transform.localScale = new Vector3(initRad, 0.25f, initRad);
    proj = p;

    speed = 1f;  // TODO edit for element speed of growth
  }
}
