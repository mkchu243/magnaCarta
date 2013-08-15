using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
  private Timer lifeTimer;
  private Element element;

  void Awake() {
    transform.Rotate(new Vector3(90, 0, 0));
    lifeTimer = gameObject.AddComponent<Timer>();
  }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
    if (lifeTimer.time <= 0) {
      gameObject.SetActive(false);
    }
	}

  public void Spawn(Element e, Vector3 pos) {
    gameObject.SetActive(true);
    transform.position = pos;
    lifeTimer.Restart(Projectile.projData[e].explosionDuration);
    gameObject.transform.localScale = new Vector3(Projectile.projData[e].explosionRadius, 0.25f, Projectile.projData[e].explosionRadius);
  }
}
