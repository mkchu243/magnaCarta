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

  void OnTriggerEnter(Collider other) {
    if (other.gameObject.tag == "explosion") {
			
			//Tells current Enemy Element
	  Debug.Log(element);
	 
			//Should add 2x Damage for Weaknesses
	switch(other.gameObject.GetComponent<Explosion>().ExploElem){
	  case(Element.water):     //If Water projectile
		if(Reference.elements[element].weakness.Contains(Element.water)){
	      Debug.Log("This Enemy is weak to Water");
	      health = health - Projectile.BaseDamageWater * 2;
	      break;
		}
		health = health - Projectile.BaseDamageWater;  //norm attack
		break;
	  case(Element.fire):      //If Fire projectile
		if(Reference.elements[element].weakness.Contains(Element.fire)){
		  Debug.Log("This Enemy is weak to Fire");
	      health = health - Projectile.BaseDamageFire * 2;
		  break;	    
		}
		health = health - Projectile.BaseDamageFire;   //norm attack
		break;
      case(Element.wood):      //If Wood projectile
		if(Reference.elements[element].weakness.Contains(Element.wood)){
		  Debug.Log("This Enemy is weak to Wood");
		  health = health - Projectile.BaseDamageWood * 2;
		  break;	   
		}
		health = health - Projectile.BaseDamageWood;   //norm attack
		break;
	  case(Element.earth):     //If Earth projectile
		if(Reference.elements[element].weakness.Contains(Element.earth)){
		  Debug.Log("This Enemy is weak to Earth");
		  health = health - Projectile.BaseDamageEarth * 2;
		  break;
		}
		health = health - Projectile.BaseDamageEarth;  //norm attack
		break;
	  case(Element.metal):     //If Metal projectile
		if(Reference.elements[element].weakness.Contains(Element.metal)){
		  Debug.Log("This Enemy is weak to Metal");
		  health = health - Projectile.BaseDamageMetal * 2;
		  break;
		}
		health = health - Projectile.BaseDamageMetal;  //norm attack
		break;
	  case(Element.holy):     //If Holy projectile
		if(Reference.elements[element].weakness.Contains(Element.holy)){
		  Debug.Log("This Enemy is weak to Holy");
		  health = health - Projectile.BaseDamageHoly * 2;
		  break;
		}
		health = health - Projectile.BaseDamageHoly;  //norm attack
		break;
	  }			
			
	        //prints out health (testing purposes)   
	  Debug.Log ("health is "+health);
			
		    //Kills when dead, score++
	  if(health <= 0) {
	    Die();
//		Player.AddScore();
	  }
	
	  return;
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
