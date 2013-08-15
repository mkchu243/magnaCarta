using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour {
  private static Player instance;
  private const float initialLife = 5;

  private Cannon cannon;
  private float score;
  private float life;

  void Awake() {
    Instance = this;
    cannon = gameObject.GetComponentInChildren<Cannon>();
  }

	// Use this for initialization
	void Start() {
    Restart();
	}

  public void Restart() {
    score = 0;
    life = initialLife;
    cannon.Restart();
  }

  // Update is called once per frame
  void Update() {
    switch( GameManager.state ){
      case GameManager.GameState.running:
        if (life <= 0) {
          GameManager.state = GameManager.GameState.gameOver;
        }
        if (Input.GetMouseButtonDown(0)) { //press left click
          Vector3 clickPos = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
          cannon.Fire(clickPos);
        }
        break;
    }
	}

  public void handleEnemy(Enemy enemy) {
    life -= 1; //TODO fix this
  }

  //setters and getters
  public static Player Instance { get; private set; }

  public float Score {
    get { return score; }
  }

  public float Life {
    get { return life; }
  }
}
