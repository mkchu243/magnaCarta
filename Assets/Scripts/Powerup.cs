using UnityEngine;
using System.Collections.Generic;

public struct BuffAttributes {
  public float duration;
  public bool affectEnemySpeed;
  public float speedMult;
  public bool affectDamage;
  public float damageMult;

  public BuffAttributes(
    float duration,
    bool affectEnemySpeed,
    float speedMult,
    bool affectDamage,
    float damageMult) {
      this.duration = duration;
      this.affectEnemySpeed = affectEnemySpeed;
      this.speedMult = speedMult;
      this.affectDamage = affectDamage;
      this.damageMult = damageMult;
  }
}

public class Powerup : MonoBehaviour {
  public enum PowerupType { slow, changeToFire };
  public static Dictionary<PowerupType, BuffAttributes> buffInfo = new Dictionary<PowerupType, BuffAttributes>{
    {PowerupType.slow, new BuffAttributes(10f, true, 0.5f, false, 0f)},
    {PowerupType.changeToFire, new BuffAttributes(0f, false, 0f, false, 0f)},
  };
  private const float baseSpeed = -5.0f; //TODO maybe health??

  private float speed;
  private Element element;

	// Use this for initialization
	void Start () {
    gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
    switch (GameManager.state) {
      case GameManager.GameState.running:
        //move, attack, etc
        transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0), Space.World);
        break;
    }
	}

  public void Spawn(Element elem, float speedMult, Vector3 pos) {
    element = elem;
    speed = speedMult * baseSpeed;
    transform.position = pos;
    gameObject.SetActive(true);
    setModel();
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
  }
}
