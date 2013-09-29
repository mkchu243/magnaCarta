using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class Explosion : MonoBehaviour {
  private Stopwatch lifeTimer;
  private int lifeTime;
  private Element element;
  private bool applyAilment;
  private float initRad = 1;
  private float maxRad;
  private float duration;
  private float speed;
  private Cannon cannon; // Needs to know so explosion can recycle

  // PowerUp Variables
  private static float radPowUp;
	
  void Awake() {
    transform.Rotate(new Vector3(90, 0, 0));
    lifeTimer = new Stopwatch();
  }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
    switch( GameManager.state ) {
      case GameManager.GameState.running:
        if (lifeTimer.ElapsedMilliseconds >= lifeTime) {
          Reload();
        }
        else if (transform.localScale.x < maxRad) {
          //float rad = (1f - (lifeTimer.TheTime / speed) / duration) * (maxRad - initRad) + initRad;
          float rad = ((lifeTimer.ElapsedMilliseconds / speed) / lifeTime) * (maxRad - initRad) + initRad;
          gameObject.transform.localScale = new Vector3( rad, 0.25f, rad);
        }
        break;
    }
	}

  public void Spawn(Element e, Vector3 pos, Projectile p) {
    gameObject.SetActive(true);
    transform.position = pos;

    element = e;     //sets private element var
    applyAilment = false;

    lifeTimer.Reset();
    lifeTimer.Start();
    lifeTime = Projectile.projData[e].explosionDuration;

    initRad = p.transform.localScale.x;
    maxRad = Projectile.projData[e].explosionRadius * radPowUp;
    gameObject.transform.localScale = new Vector3(initRad, 0.25f, initRad);
    transform.renderer.material = Reference.elements[e].mat;
    cannon = p.Cannon;

    speed = 1f;  // TODO edit for element speed of growth
  }

  public void Reload() {
    gameObject.SetActive(false);
    cannon.Reload(this);
  }
    
  void OnTriggerEnter(Collider other) {
    if (other.gameObject.tag == "enemy") {
      Enemy e = other.gameObject.GetComponent<Enemy>();
      if (Reference.elements[e.Element].creates.Contains(element)) {
        applyAilment = true;
      }
    }
  }

  ////////////////////////////  Properties  /////////////////////////////
  public static float RadPowUp {
    get { return radPowUp; }
    set { radPowUp = value; }
  }
		
  public Element ExploElem{
	  get{return element;}
  }

  public bool ApplyAilment {
    get { return applyAilment; }
  }
	
}
