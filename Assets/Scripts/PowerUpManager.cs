using UnityEngine;
using System.Collections.Generic;

public class PowerUpManager : MonoBehaviour {
  private static PowerUpManager instance;
  public enum powType{ projRad, explosionRad, damage, cooldown, rotSpeed, projSpeed, waterChange, fireChange,
    woodChange, earthChange, metalChange, holyChange, slowEnemy };
  public PowerUp powPrefab;
  private Stack<PowerUp> inactivePow;
  private Dictionary<int, PowerUp> activePow;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
    switch( GameManager.state ) {
      case GameManager.GameState.running:
        break;
    }
	}

  public void Reload(PowerUp pow) {
    activePow.Remove(pow.Key);
    inactivePow.Push(pow);
  }

  ///////////////////////////  Properties  //////////////////////////
  public static PowerUpManager Instance {
    get { return instance; }
    private set{}
  }
}
