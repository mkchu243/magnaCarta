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
  
  //ailment variables
  protected float ailSpeedMult; //the total speed modifier of all the ailments
  private int burnTime;
  private Stopwatch burnClock;

  protected Dictionary<ailmentType, Ailment> ailments;
  protected Dictionary<ailmentType, GameObject> particles; //ailment particles
	
	// Use this for initialization
  protected virtual void Start () {
    ailments = new Dictionary<ailmentType, Ailment>();
    particles = new Dictionary<ailmentType, GameObject>();
  }
	
	// Update is called once per frame
	protected virtual void Update () {
    //Vector3 rotationVelocity = new Vector3(45, 90, 1);
    //transform.Rotate(rotationVelocity * Time.deltaTime);

    switch( GameManager.state ){
      case GameManager.GameState.running:
        updateAilments();
        if (health <= 0) {
          EnemyManager.Instance.RemoveEnemy(this);
        } else if (transform.position.x <= EnemyManager.KillX) { //TODO this is temporary for test
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

  //set object to no longer render
  //calling EnemyManager.Instance.RemoveEnemy modifies player life, etc
  //and also calls this method
  public void Die() {
    gameObject.SetActive(false);

    //TODO probably a better way of doing this
    List<ailmentType> toRemove = new List<ailmentType>();
    foreach (ailmentType a in ailments.Keys) {
      toRemove.Add(a);
    }

    foreach (ailmentType a in toRemove) {
      removeAilment(a);
    }
  }
 
  //uses the physics clock, so it should be stable across all platforms
  void OnTriggerStay(Collider other) {
    if (GameManager.state != GameManager.GameState.running)
      return;

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
    }
  }

  private void addAilment(ailmentType atype, int level) {
    if (!ailments.ContainsKey(atype)) {
      ailments.Add(atype, new Ailment(atype, level)); //TODO level 1 always so dont need to update
      activeDelegates[(int)atype](ailments[atype], this);
      ailments[atype].RestartClock();

      //add the particle
      GameObject part = ParticleManager.Instance.CreateAilParticle(atype);
      part.transform.position = transform.position;
      part.transform.parent = transform;
      particles.Add(atype, part);
    } else if (Ailment.resetTimer.Contains(atype)) { //if it already contains and needs timer reset
      ailments[atype].RestartClock();
    }
  }

  private void removeAilment(ailmentType atype) {
    ailments.Remove(atype);
    deactiveDelegates[(int)atype](atype, this);

    //remove particle
    GameObject part = particles[atype];
    ParticleManager.Instance.ReleaseAilParticle(atype, part);
    particles.Remove(atype);
  }

  //periodically try to remove the ailments
  private void updateAilments() {
    List<ailmentType> toRemove = new List<ailmentType>();
    foreach (ailmentType a in ailments.Keys) {
      if (!ailments[a].IsLive)
        toRemove.Add(a);
      else if (a == ailmentType.burn && burnClock.ElapsedMilliseconds >= burnTime) {
        health -= Ailment.ailmentData[ailmentType.burn][ailments[a].Level].effectMult;
        burnClock.Reset();
        burnClock.Start();
      }
    }

    foreach (ailmentType a in toRemove) {
      removeAilment(a);
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
  private delegate void AilDelegates(Ailment a, Enemy e); //TODO prob don't need ailment
  private delegate void DeAilDelegates(ailmentType a, Enemy e); //TODO prob dont need atype

  private static AilDelegates[] activeDelegates = {
    Freeze,
    Burn,
    Root,
    Dam,
    Cut
  };

  private static DeAilDelegates[] deactiveDelegates = {
    FreezeDe,
    BurnDe,
    RootDe,
    DamDe,
    CutDe
  };

  ////////activations////////
  //a is the ailment being applied, e is the enemy it is being applied to
  private static void Freeze(Ailment a, Enemy e) {
    RecalculateAilSpeedMult(e);
  }
  
  private static void Burn(Ailment a, Enemy e) {
    if (e.burnClock == null) {
      e.burnClock = new Stopwatch();
    }
    e.burnClock.Start();
    e.burnTime = (int) Ailment.ailmentData[a.Type][a.Level].speedMult * 1000;
  }
  
  private static void Root(Ailment a, Enemy e) {
    RecalculateAilSpeedMult(e);
  }
  
  private static void Dam(Ailment a, Enemy e) {
    RecalculateAilSpeedMult(e);
  }

  private static void Cut(Ailment a, Enemy e) {
  }

  ///////deactivations/////////
  //probably only need the type
  private static void FreezeDe(ailmentType a, Enemy e) {
    RecalculateAilSpeedMult(e);
  }
  
  private static void BurnDe(ailmentType a, Enemy e) {
    e.burnClock.Reset();
  }

  private static void RootDe(ailmentType a, Enemy e) {
    RecalculateAilSpeedMult(e);
  }

  private static void DamDe(ailmentType a, Enemy e) {
    RecalculateAilSpeedMult(e);
  }

  private static void CutDe(ailmentType a, Enemy e) {
    e.health = e.health * Ailment.ailmentData[a][0].effectMult;
  }
}
