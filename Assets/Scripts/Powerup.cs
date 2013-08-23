using UnityEngine;
using System.Collections.Generic;

public class PowerUp : MonoBehaviour {
  // Constants
  private const float baseSpeed = -5.0f;
  private const float KillX = -27f;

  private PowerUpManager powManager = PowerUpManager.Instance;

  // PowerUp information
  private float duration;
  private int level;
  private PowerUpManager.powType powType;
  private bool isBuff;
  private Element element;
  
  private float speed;
  private int key;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
    switch( GameManager.state ) {
      case GameManager.GameState.running:
        transform.Translate(new Vector3(baseSpeed * Time.deltaTime, 0, 0));

        if(transform.position.x <= KillX) {
          Die();
        }
        break;
    }
	}


  public void Spawn(Element elem, float speedMult, Vector3 pos, int k) {
    element = elem;
    speed = speedMult * baseSpeed;
    transform.position = pos;
    gameObject.SetActive(true);
    setModel();
    key = k;
  }

  private void setModel() {
    switch (element) { //TODO the other elements
      case Element.water:
        transform.FindChild("waterModel").gameObject.SetActive(true);
        transform.FindChild("fireModel").gameObject.SetActive(false);
        break;
      case Element.fire:
        transform.FindChild("waterModel").gameObject.SetActive(false);
        transform.FindChild("fireModel").gameObject.SetActive(true);
        break;
    }
  }

  public void Die() {
    gameObject.SetActive(false);
    Reload();
  }

  private void Reload() {
    powManager.Reload(this);
  }

  ////////////////////////////  Properties  /////////////////////////////////
  public float Duration {
    get { return duration; }
  }

  public int Level {
    get { return level; }
  }

  public PowerUpManager.powType PowType {
    get { return powType; }
  }

  public bool IsBuff {
    get { return isBuff; }
  }

  public float Speed {
    get { return Speed; }
    set { Speed = value; }
  }

  public int Key {
    get { return key; }
  }
}
