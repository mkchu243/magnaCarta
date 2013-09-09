using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {
	
	private float time;
	private bool on;
	
	// Use this for initialization
	void Start() {
	}
	
	// Update is called once per frame
	void Update() {
    if(on && GameManager.state == GameManager.GameState.running){
      time -= Time.deltaTime;
    }
	}
  
  public void Restart(float time) {
    this.time = time;
    on = true;
  }

  public float TheTime {
    get { return time; }
  }
}
