using UnityEngine;
using System.Collections.Generic;

public class GUIManager : MonoBehaviour {
  private Player player;
  private static Dictionary<string, Texture> textures;

  public GUIStyle centerTextStyle;
  private string centerText;

	// Use this for initialization
	void Start () {
    player = gameObject.GetComponent<Player>();
    centerText = "";
  }
	
	// Update is called once per frame
	void Update () {
	}

  void OnGUI() {
    if (GUI.Button(new Rect(Screen.width - 50, 10, textures["pause"].width, textures["pause"].height), textures["pause"])) {
      switch (GameManager.state) {
        case GameManager.GameState.paused:
          GameManager.state = GameManager.GameState.running;
          break;
        case GameManager.GameState.running:
          GameManager.state = GameManager.GameState.paused;
          break;
      }
    }

    switch (GameManager.state) {
      case GameManager.GameState.paused:
        centerText = "PAUSED";
        break;
      case GameManager.GameState.gameOver:
        centerText = "GAME OVER";
        if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 20, 100, 20), "New Game")) { //TODO change the style
          GameManager.state = GameManager.GameState.restart;
        }
        
        break;
      default:
        centerText = "";
        break;
    }

    GUI.Label(new Rect(10, 10, 100, 20), "Score: " + player.Score); //TODO hard coded numbers
    GUI.Label(new Rect(10, 25, 100, 35), "Lives: " + player.Life);
    GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 200), centerText, centerTextStyle);
  }

  public static void InitStaticVars() {
    Texture2D texture = Resources.Load("Textures/pause") as Texture2D;
    textures = new Dictionary<string, Texture>();
    textures.Add("pause", texture);
  }
}
