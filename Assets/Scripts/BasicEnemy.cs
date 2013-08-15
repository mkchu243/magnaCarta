using UnityEngine;
using System.Collections.Generic;

public class BasicEnemy : Enemy {
  protected override void Update() {
    base.Update();
    switch (GameManager.state) {
      case GameManager.GameState.running:
        //move, attack, etc
        transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0), Space.World);
        break;
    }
  }

  protected override void setModel() {
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

  //setters and getter
  public override float GetBaseSpeed() { return -5; }
  public override float GetBaseHealth() { return 10; }
  public override float GetBasePoints() { return 100; }
}