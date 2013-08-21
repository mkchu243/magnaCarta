using UnityEngine;
using System.Collections.Generic;

public class Cannon : MonoBehaviour {
  // Variables for rotating
  private const float zeroRotation = 90;  // the offset for where the cannon is along the x-axis
  private const float tolerance = 1.5f;     // the angle tolerance of the cannon
  private Vector3 pivotPoint; // the point where the cannon pivots around
  private float finalAngle;   // the end angle the cannon should be at
  private float rotSpeed = 95f;      // the rotation speed of the cannon
  private int rotDirection;   // the direction to rotate in.  1 = clockwise, -1 = counter-clockwise

  // Variables for objects
  public Projectile projectilePrefab; // The projectile prefab
  public Explosion explosionPrefab;   // the explosion prefab
  private Stack<Projectile> inactiveProj;
  private Dictionary<int, Projectile> activeProj;

  // Variables for firing
  private bool fireReady;             // Boolean to say if the cannon's ready to fire
  private Vector3 cannonEnd;          // the shooting end of the cannon
  private Vector3 target;             // the target to shoot towards
  private Timer coolTimer;               // the cooldown timer for shooting
  private const float maxCool = 1.2f;                // the max cooldown
  private Element elem;                  // the element to fire

  // Materials
  public static System.Random rng;
  private Material cannonMat;

  void Awake () {
    inactiveProj = new Stack<Projectile>();
    activeProj = new Dictionary<int, Projectile>();

    // calculates the pivot point equal to 1/4 the way down the cannon
	  pivotPoint = new Vector3(transform.position.x - (transform.localScale.x / 4f), transform.position.y, transform.position.z);
    coolTimer = gameObject.AddComponent<Timer>();

    rng = new System.Random();
    cannonMat = Resources.Load("Materials/cannonMat") as Material;
  }

  // Use this for initialization
  void Start(){
  }

  // Update is called once per frame
  void Update () {
    cannonEnd = CannonEnd;

    // Tests if the cannon is in the right rotation position and changes if not
    if(Mathf.Abs(finalAngle - transform.eulerAngles.z) < tolerance) {
      if(fireReady && coolTimer.TheTime <= 0) {  // If player chose to fire, will shoot
        finalAngle = transform.eulerAngles.z;
        Shoot(target, elem);
      }
    }
    else {
      transform.RotateAround(pivotPoint, Vector3.forward, rotDirection * rotSpeed * Time.deltaTime);
    }
  }

  public void Restart(){
    fireReady = false;
    coolTimer.Restart(0f);

    // Removes any active projectiles or explosions
    int count = activeProj.Count;
    Projectile proj;
    for(int i = 0; count > 0; i++) {
      if( activeProj.ContainsKey(i) ) {
        proj = activeProj[i];
        proj.Explosion.gameObject.SetActive(false);
        proj.gameObject.SetActive(false);
        activeProj.Remove(i);
        count--;
      }
    }

    transform.RotateAround( pivotPoint, Vector3.forward, zeroRotation - transform.eulerAngles.z );
    finalAngle = zeroRotation;  // set so the cannon doesn't rotate in the beginning
  }

  /**
   * Called from Player to fire a projectile.  First must rotate the cannon, then shoot.
   */
  public void Fire(Vector3 aim) {
    target = aim;  // sets the target attribute so that it can be used in Update()
    fireReady = true;
    Rotate(aim); // Rotates the cannon to "aim" the projectile

    // TODO include element into firing input
    float rand = (float)(6 * rng.NextDouble());
    if( rand < 1 ) {
      elem = Element.water;
      transform.renderer.material = Reference.elements[elem].mat;
    }
    else if( rand < 2 ) {
      elem = Element.fire;
      transform.renderer.material = Reference.elements[elem].mat;
    }
    else if( rand < 3 ) {
      elem = Element.wood;
      transform.renderer.material = Reference.elements[elem].mat;
    }
    else if( rand < 4 ) {
      elem = Element.earth;
      transform.renderer.material = Reference.elements[elem].mat;
    }
    else if( rand < 5 ) {
      elem = Element.metal;
      transform.renderer.material = Reference.elements[elem].mat;
    }
    else {
      elem = Element.holy;
      transform.renderer.material = Reference.elements[elem].mat;
    }
  }
  
  /**
   * Will actually launch the projectile.
   *
   * @param aim where to launch towards
   */
  private void Shoot(Vector3 aim, Element element){
    Projectile proj;
    int key = 0;

    if( inactiveProj.Count > 0 ) {
      proj = inactiveProj.Pop();
    }
    else {
      proj = (Projectile)(Instantiate( projectilePrefab, new Vector3(0f, 0f, -100f), Quaternion.identity ));
    }

    for(int i = 0; i < activeProj.Count + 1; i++) {
      if(!activeProj.ContainsKey(i)) {
        key = i;
        break;
      }
    }

    activeProj.Add(key, proj);

    transform.renderer.material = cannonMat;
    proj.Spawn(element, cannonEnd, Trajectory, this, key);
    fireReady = false;
    coolTimer.Restart(maxCool);
  }

  public void Reload(Projectile proj) {
    inactiveProj.Push(proj);
    activeProj.Remove(proj.Key);
  }
	
  /**
   * Rotates the cannon.  Intended to be used for firing projectiles in a different direction.
   * 
   * @param point the point to fire towards
   */
  private void Rotate(Vector3 point) {
  	 float angle = (float)(Mathf.Atan( (point.y - pivotPoint.y) / (point.x - pivotPoint.x) )); // Calculates the angle

    angle *= Mathf.Rad2Deg; // Change from radians to degrees
    angle += zeroRotation; // Sets the absolute rotation

    // Allows the cannon to shoot all angles.  Can be modified to be a restraint.
    if( point.x < pivotPoint.x ) {
     angle += 180f;
    }

    // Determines which direction to rotate in
    if( Mathf.Abs( angle - transform.eulerAngles.z ) <= 180 ) {
      if( angle < transform.eulerAngles.z )
        rotDirection = -1;
      else
        rotDirection = 1;
    }
    else {  // Happens if the rotation should occur across the 0 degree axis
      if( angle < transform.eulerAngles.z )
        rotDirection = 1;
      else
        rotDirection = -1;
    }

    finalAngle = angle;
  }

  //////////////////////////  Properties  ////////////////////////////////
  public Vector3 Pivot {
   get { return pivotPoint; }
  }

  public Vector3 CannonEnd {
    get {
      float x = (float)( transform.position.x + transform.localScale.y * Mathf.Cos((transform.eulerAngles.z - zeroRotation) * Mathf.Deg2Rad) );
      float y = (float)( transform.position.y + transform.localScale.y * Mathf.Sin((transform.eulerAngles.z - zeroRotation) * Mathf.Deg2Rad) );
      return new Vector3(x, y, transform.position.z);
    }
  }

  public Vector3 Trajectory {
    get {
      return new Vector3( cannonEnd.x - transform.position.x, cannonEnd.y - transform.position.y, 0f );
    }
  }
}
