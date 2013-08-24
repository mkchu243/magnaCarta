using UnityEngine;
using System.Collections.Generic;

public class Cannon : MonoBehaviour {
  // Constants
  private const float zeroRotation = 90;  // the offset for where the cannon is along the x-axis
  private const float tolerance = 1.5f;     // the angle tolerance of the cannon
  private const float fullCircle = 360f;  // A full circle
  private float rotSpeed = 95f;      // the rotation speed of the cannon
  private const float maxCool = 1.2f;                // the max cooldown

  // Variables for rotating
  private Vector3 pivotPoint; // the point where the cannon pivots around
  private float finalAngle;   // the end angle the cannon should be at
  private float angleDiff;    // The angle difference between current angle and the final angle
  private int rotDirection;   // the direction to rotate in.  1 = clockwise, -1 = counter-clockwise

  // Variables for objects
  public Projectile projectilePrefab; // The projectile prefab
  public Explosion explosionPrefab;   // the explosion prefab
  private Stack<Projectile> inactiveProj;
  private HashSet<Projectile> activeProj;
  private Stack<Explosion> inactiveExplosion;
  private HashSet<Explosion> activeExplosion;

  // Variables for firing
  private bool fireReady;             // Boolean to say if the cannon's ready to fire
  private Vector3 cannonEnd;          // the shooting end of the cannon
  private Vector3 target;             // the target to shoot towards
  private Timer coolTimer;               // the cooldown timer for shooting
  private Element elem;                  // the element to fire
  private float projInitRad;

  // Materials
  public static System.Random rng;
  private Material cannonMat;

  void Awake () {
    inactiveProj = new Stack<Projectile>();
    activeProj = new HashSet<Projectile>();
    inactiveExplosion = new Stack<Explosion>();
    activeExplosion = new HashSet<Explosion>();

    // calculates the pivot point equal to 1/4 the way down the cannon
	  pivotPoint = new Vector3(transform.position.x - transform.localScale.y, transform.position.y, transform.position.z);
    coolTimer = gameObject.AddComponent<Timer>();

    rng = new System.Random();
    cannonMat = Resources.Load("Materials/cannonMat") as Material;
  }

  // Use this for initialization
  void Start(){
  }

  // Update is called once per frame
  void Update () {
    switch( GameManager.state ) {
      case GameManager.GameState.running:
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
          if( AngleAdjust(Mathf.Abs(finalAngle - transform.eulerAngles.z)) > angleDiff ) {
            rotDirection *= -1;
          }
        }
        angleDiff = AngleAdjust(Mathf.Abs(finalAngle - transform.eulerAngles.z)); // used to make sure the cannon is rotating in the correct direction
        break;
    }
  }


  public void Restart(){
    fireReady = false;
    coolTimer.Restart(0f);

    // Removes any active projectiles
    foreach( Projectile proj in activeProj ) {
      proj.Reload();
    }
    activeProj.Clear();
    // Removes any active explosions
    foreach( Explosion ex in activeExplosion ) {
      ex.Reload();
    }
    activeExplosion.Clear();

    // Reset mats
    ChangeMat(cannonMat);
    // Reset angles
    transform.RotateAround( pivotPoint, Vector3.forward, zeroRotation - transform.eulerAngles.z );
    finalAngle = zeroRotation;  // set so the cannon doesn't rotate in the beginning
    projInitRad = 1f;
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
    }
    else if( rand < 2 ) {
      elem = Element.fire;
    }
    else if( rand < 3 ) {
      elem = Element.wood;
    }
    else if( rand < 4 ) {
      elem = Element.earth;
    }
    else if( rand < 5 ) {
      elem = Element.metal;
    }
    else {
      elem = Element.holy;
    }

    ChangeMat(Reference.elements[elem].mat);
  }
  

  /**
   * Will actually launch the projectile.
   *
   * @param aim where to launch towards
   */
  private void Shoot(Vector3 aim, Element element){
    Projectile proj;

    // Obtains an available Projectile
    if( inactiveProj.Count > 0 ) {
      proj = inactiveProj.Pop();
    }
    else {
      proj = (Projectile)( Instantiate(projectilePrefab, new Vector3(0f, 0f, -100f), Quaternion.identity) );
    }

    activeProj.Add(proj);

    proj.transform.localScale = new Vector3(projInitRad, projInitRad, projInitRad);
    ChangeMat(cannonMat);

    proj.Spawn(element, cannonEnd, Trajectory, this);
    fireReady = false;
    coolTimer.Restart(maxCool);
  }


  public void CreateExplosion(Element elem, Vector3 pos) {
    Explosion ex;

    // Obtains an available Explosion
    if( inactiveExplosion.Count > 0 ) {
      ex = inactiveExplosion.Pop();
    }
    else {
      ex = (Explosion)( Instantiate(explosionPrefab, new Vector3(0f, 0f, -100f), Quaternion.identity) );
    }
    
    activeExplosion.Add(ex);  // Key needed to keep track of active explosions
    ex.Spawn(elem, pos, this);
  }


  // Pushes Projectiles onto the stack
  public void Reload(Projectile proj) {
    inactiveProj.Push(proj);
    if( !(GameManager.state == GameManager.GameState.restart) ) {
      activeProj.Remove(proj);
    }
  }

  // Pushes Explosions onto the stack
  public void Reload(Explosion ex) {
    inactiveExplosion.Push(ex);
    if( !(GameManager.state == GameManager.GameState.restart) ) {
      activeExplosion.Remove(ex);
    }
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
    angleDiff = AngleAdjust(Mathf.Abs( angle - transform.eulerAngles.z ));

    finalAngle = angle;
  }

  private void ChangeMat(Material mat) {
    transform.renderer.material = mat;
    foreach(Transform child in transform) {
      child.transform.renderer.material = mat;
    }
  }

  /*
   * If the Angle is greater than half a circle, adjusts around 360 so angleDiff can
   * correctly calculate the angle difference between current and finalAngle
   */
  private float AngleAdjust(float num) {
    if( num > fullCircle / 2)
      num = Mathf.Abs( num - 360 );
    return num;
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
