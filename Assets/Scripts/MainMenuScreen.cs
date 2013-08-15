using UnityEngine;

public class MainMenuScreen : MonoBehaviour {

  void OnGUI() {
    GUI.BeginGroup(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 87.5f, 100, 175));
    GUI.Box(new Rect(0, 0, 100, 175), "Main Menu");

    if (GUI.Button(new Rect(10, 30, 80, 30), "Start Game")) {
      Application.LoadLevel("Level_01");
    }

    if (GUI.Button(new Rect(10, 100, 80, 30), "Exit")) {
      Application.Quit();
    }

    GUI.EndGroup();
  }
}