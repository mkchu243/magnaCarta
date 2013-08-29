using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;

public abstract class Enemy : MonoBehaviour {
  //use this if we want the speed of basic enemy to vary based on element, similiar to how elemtn refence was done
  //public static Dictionary<Element, EnemyAttribute> attribs = new Dictionary<Element, EnemyAttributes> { };
  protected Element element;
  protected float speed;
  protected float health;
  protected float points;
  
  protected float ailSpeedMult;

  protected Dictionary<ailmentType, Ailment> ailments;
	
	// Use this for initialization
  protected virtual void Start () {
    ailments = new Dictionary<ailmentType, Ailment>();
  }
	
	// Update is called once per frame
	protected virtual void Update () {
    Vector3 rotationVelocity = new Vector3(45, 90, 1);
    transform.Rotate(rotationVelocity * Time.deltaTime);

    switch( GameManager.state ){
      case GameManager.GameState.running:
        updateAilments();
        if (transform.position.x <= EnemyManager.KillX) { //TODO this is temporary for test
          EnemyManager.Instance.RemoveEnemy(this);
        }
        break;
    }
	}

  protected virtual void setModel() {
  }

  public void Spawn(Element elem, float speedMult, Vector3 pos) {
    element = elem;   
    speed = speedMult * GetBaseSpeed();
    health = GetBaseHealth(); //TODO health mult?, point Mult?
    points = GetBasePoints();
    transform.position = pos;

    ailSpeedMult = 1;

    gameObject.SetActive(true);
    setModel();
  }

  public void Die() {
    gameObject.SetActive(false);
  }
 
  //uses the physics clock, so it should be stable across all platforms
  void OnTriggerStay(Collider other) {
    if (other.gameObject.tag == "explosion") {
      Explosion explo = other.gameObject.GetComponent<Explosion>();
      Element exploElem = explo.ExploElem;
      
      if (explo.ApplyAilment) {
        ailmentType atype = Reference.elements[exploElem].ailment;
        addAilment(atype, 0); //TODO always level 0
      }

      float damageMult = 1;
      if (Reference.elements[element].weakness.Contains(exploElem)) { //it is my weakness
        damageMult = 2;
      }
      health -= Projectile.projData[exploElem].damage * damageMult;
      
      if(health <= 0){
        Die();
        //TODO update the score
      }
    }
  }

  private void addAilment(ailmentType atype, int level) {
    if (!ailments.ContainsKey(atype)) {
      ailments.Add(atype, new Ailment(atype, level)); //TODO level 1 always so dont need to update
    }

    activeDelegates[(int)atype](ailments[atype], this);
    ailments[atype].RestartClock();
  }

  private void updateAilments() {
    List<ailmentType> toRemove = new List<ailmentType>();
    foreach (ailmentType a in ailments.Keys) {
      if (!ailments[a].IsLive)
        toRemove.Add(a);
    }

    foreach (ailmentType a in toRemove) {
      UnityEngine.Debug.Log("removing");
      Ailment ailment = ailments[a];
      ailments.Remove(a);
      deactiveDelegates[(int)a](ailment, this);
    }
  }

  private static void RecalculateAilSpeedMult(Enemy e) {
    float speedMult = 1;
    foreach (ailmentType a in e.ailments.Keys) {
      if (Ailment.affectMovement.Contains(a)) {
        Ailment ailment = e.ailments[a];
        speedMult *= Ailment.ailmentData[a][ailment.Level].speedMult;
      }
    }
    e.ailSpeedMult = speedMult;
  }
	
  //setters and getter
  //done this way so it can inherit these stats and use generic spawn method
  public virtual float GetBaseSpeed() { return -5; }
  public virtual float GetBaseHealth() { return 10; }
  public virtual float GetBasePoints() { return 100; }

  public Element Element{
    get { return element; }
    set { element = value; }
  }

  /////////ailment delegates
  private delegate void AilDelegates(Ailment a, Enemy e);

  private static AilDelegates[] activeDelegates = {
    Freeze
  };

  private static AilDelegates[] deactiveDelegates = {
    FreezeDe
  };

  ////////activations////////
  private static void Freeze(Ailment a, Enemy e) {
    //add the particle model
    RecalculateAilSpeedMult(e);
  }

  ///////deactivations/////////
  private static void FreezeDe(Ailment a, Enemy e) {
    RecalculateAilSpeedMult(e);
  }
}
