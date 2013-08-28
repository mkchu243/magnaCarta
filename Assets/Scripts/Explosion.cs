using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class Explosion : MonoBehaviour {
  private Stopwatch lifeTimer;
  private float lifeTime;
  private Element element;
	
  void Awake() {
    transform.Rotate(new Vector3(90, 0, 0));
    lifeTimer = new Stopwatch();
  }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
    if (lifeTimer.Elapsed.Seconds >= lifeTime) {
      gameObject.SetActive(false);
    }
	}

  public void Spawn(Element e, Vector3 pos) {	
    gameObject.SetActive(true);
    transform.position = pos;
    gameObject.transform.localScale = new Vector3(Projectile.projData[e].explosionRadius, 0.25f, Projectile.projData[e].explosionRadius);
  	element = e;     //sets private element var
    
    lifeTimer.Reset();
    lifeTimer.Start();
    lifeTime = Projectile.projData[e].explosionDuration;
  }
		
  public Element ExploElem{
	  get{return element;}
  }
	
}
