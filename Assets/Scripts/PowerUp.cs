﻿using UnityEngine;
using System.Collections.Generic;

public class PowerUp : MonoBehaviour {
  // Constants
  private const float baseSpeed = -5.0f;
  private const float rotSpeed = 150f;

  private PowerUpManager powManager;
  private Timer powTimer;

  // PowerUp information
  private float duration;                  // Duration of effect
  private int level;                       // Level of PowerUp
  private PowerUpManager.powType type;  // Type of PowerUp
  private bool isBuff;                     // Tells whether it is a buff or debuff
  private Element element;                 // Element needed to obtain it?
  
  private float speed;                     // Movement speed of the PowerUp
  private bool isMoving;                   // Says whether it is moving or hanging in the player's GUI

  private int key;

  void Awake() {
    powManager = PowerUpManager.Instance;
    powTimer = gameObject.AddComponent<Timer>();
  }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
    switch( GameManager.state ) {
      case GameManager.GameState.running:
        transform.Rotate(new Vector3( 0f, rotSpeed * Time.deltaTime, 0f ), Space.World );

        if( isMoving ) {
          transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0), Space.World);

          if(transform.position.x <= PowerUpManager.KillX) {
            Reload();
          }
        }
        else {
          if( powTimer.TheTime <= 0 ) {
            Deactivate();
          }
        }
        break;
    }
	}

  void OnTriggerEnter() {
    if( isMoving ) {
      Activate();
    }
  }

  public void Spawn(PowerUpManager.powType t, int lev, Element elem, float speedMult, float dur, Vector3 pos, bool buff) {
    type = t;
    level = lev;
    element = elem;
    speed = speedMult * baseSpeed;
    duration = dur;
    transform.position = pos;
    isBuff = buff;
    gameObject.SetActive(true);
    isMoving = true;
    setModel();

    if( transform.eulerAngles.z == 0 ) {
      transform.Rotate( new Vector3(0f, 0f, 30f), Space.World );
    }
  }

  
  private void Activate() {
    isMoving = false;
    powTimer.Restart(duration);
    powManager.Activate(this);
  }

  public void Deactivate() {
    powManager.Deactivate(this);
    Reload();
  }

  public void ResetTimer() {
    powTimer.Restart(duration);
  }

  public void Move(Vector3 pos) {
    transform.Translate( pos - transform.position, Space.World );
  }

  private void setModel() {
    transform.FindChild("waterModel").gameObject.SetActive(false);
    transform.FindChild("fireModel").gameObject.SetActive(false);
    transform.FindChild("woodModel").gameObject.SetActive(false);
    transform.FindChild("earthModel").gameObject.SetActive(false);
    transform.FindChild("metalModel").gameObject.SetActive(false);
    transform.FindChild("holyModel").gameObject.SetActive(false);
    switch (element) { //TODO the other elements
      case Element.water:
        transform.FindChild("waterModel").gameObject.SetActive(true);
        break;
      case Element.fire:
        transform.FindChild("fireModel").gameObject.SetActive(true);
        break;
      case Element.wood:
        transform.FindChild("woodModel").gameObject.SetActive(true);
        break;
      case Element.earth:
        transform.FindChild("earthModel").gameObject.SetActive(true);
        break;
      case Element.metal:
        transform.FindChild("metalModel").gameObject.SetActive(true);
        break;
      case Element.holy:
        transform.FindChild("holyModel").gameObject.SetActive(true);
        break;
    }
  }

  public void Reload() {
    gameObject.SetActive(false);
    powManager.Reload(this);
  }

  ////////////////////////////  Properties  /////////////////////////////////
  public float Duration {
    get { return duration; }
  }

  public int Level {
    get { return level; }
  }

  public PowerUpManager.powType Type {
    get { return type; }
  }

  public bool IsBuff {
    get { return isBuff; }
  }

  public int Key {
    get { return key; }
    set { key = value; }
  }

  public float Speed {
    get { return Speed; }
    set { Speed = value; }
  }
}
