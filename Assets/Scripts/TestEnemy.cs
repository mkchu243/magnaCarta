using UnityEngine;
using System.Collections.Generic;

public class TestEnemy : Enemy {
  protected override void Start() {
  }

  protected override void Update() {
    base.Update();
    switch (GameManager.state) {
      case GameManager.GameState.running:
        //move, attack, etc
        transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0), Space.World);
        break;
    }
  }

  //setters and getter
  public override float GetBaseSpeed() { return -5; }
  public override float GetBaseHealth() { return 10; }
  public override float GetBasePoints() { return 100; }
}