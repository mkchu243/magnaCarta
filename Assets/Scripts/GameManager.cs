using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
 
  public enum GameState{restart, running, paused, gameOver};
  public static GameState state;
  
	// Use this for initialization
	void Start() {
    state = GameState.restart;

    GUIManager.InitStaticVars();
	}
	
	// Update is called once per frame
	void Update() {
    switch( state ){
    case GameState.restart:
      EnemyManager.Instance.Restart();
      Player.Instance.Restart();
      state = GameState.running;
      break;
    //case GameState.running:
    //  break;
    }
	}
}
