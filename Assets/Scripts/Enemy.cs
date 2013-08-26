using UnityEngine;
using System.Collections.Generic;

public abstract class Enemy : MonoBehaviour {
  //use this if we want the speed of basic enemy to vary based on element, similiar to how elemtn refence was done
  //public static Dictionary<Element, EnemyAttribute> attribs = new Dictionary<Element, EnemyAttributes> { };
  protected Element element;
  protected float speed;
  protected float health;
  protected float points;

	// Use this for initialization
	protected virtual void Start () {
	}
	
	// Update is called once per frame
	protected virtual void Update () {
    Vector3 rotationVelocity = new Vector3(45, 90, 1);
    transform.Rotate(rotationVelocity * Time.deltaTime);

    if (transform.position.x <= EnemyManager.KillX) { //TODO this is temporary for test
      EnemyManager.Instance.RemoveEnemy(this);
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
    gameObject.SetActive(true);
    setModel();
  }

  public void Die() {
    gameObject.SetActive(false);
  }
 
  //uses the physics clock, so it should be stable across all platforms
  void OnTriggerStay(Collider other) {
    if (other.gameObject.tag == "explosion") {
      float damageMult = 1;
      Element exploElem = other.gameObject.GetComponent<Explosion>().ExploElem;
      if( Reference.elements[element].weakness.Contains(exploElem)){ //it is my weakness
        damageMult = 2;
      }
      health -= Projectile.projData[exploElem].damage * damageMult;
      
      if(health <= 0){
        Die();
        //TODO update the score
      }
    }
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
}
