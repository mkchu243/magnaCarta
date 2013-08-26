using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
  private Timer lifeTimer;
  private Element element;
  private float initRad = 1;
  private float maxRad;
  private float duration;
  private float speed;
  private Cannon cannon; // Needs to know so explosion can recycle

  // PowerUp Variables
  private static float explosionRadPowUp;

  void Awake() {
    transform.Rotate(new Vector3(90, 0, 0));
    lifeTimer = gameObject.AddComponent<Timer>();
  }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
    switch( GameManager.state ) {
      case GameManager.GameState.running:
        if (lifeTimer.TheTime <= 0) {
          Reload();
        }
        else if (transform.localScale.x < maxRad) {
          float rad = (1f - (lifeTimer.TheTime / speed) / duration) * (maxRad - initRad) + initRad;
          gameObject.transform.localScale = new Vector3( rad, 0.25f, rad);
        }
        break;
    }
	}

  public void Spawn(Element e, Vector3 pos, Projectile p) {
    gameObject.SetActive(true);
    transform.position = pos;

    duration = Projectile.projData[e].explosionDuration;
    lifeTimer.Restart(duration);

    initRad = p.transform.localScale.x;
    maxRad = Projectile.projData[e].explosionRadius * explosionRadPowUp;
    gameObject.transform.localScale = new Vector3(initRad, 0.25f, initRad);
    transform.renderer.material = Reference.elements[e].mat;
    cannon = p.Cannon;

    speed = 1f;  // TODO edit for element speed of growth
  }

  public void Reload() {
    gameObject.SetActive(false);
    cannon.Reload(this);
  }

  ////////////////////////////  Properties  /////////////////////////////
  public static float ExplosionRadPowUp {
    get { return explosionRadPowUp; }
    set { explosionRadPowUp = value; }
  }
}
