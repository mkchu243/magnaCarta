using System.Diagnostics;
using System.Collections.Generic;

public enum ailmentType { freeze, burn, root };

public class Ailment{
  private ailmentType type;
  private int level;
  private Stopwatch lifeTimer;
  private float lifeTime;

  public Ailment(ailmentType type, int level) {
    this.type = type;
    this.level = level;
    lifeTimer = new Stopwatch();
    lifeTime = ailmentData[type][level].duration; 
  }

  public void RestartClock(){
    lifeTimer.Reset();
    lifeTimer.Start();
  }

  //////setters and getters
  public ailmentType Type { get { return type; } }
  public int Level { get { return level; } }
  public bool IsLive { get { return lifeTimer.Elapsed.Seconds < lifeTime; } }
  
  ///////ailment dictionary////////
  public struct AilmentAttributes {
    public float speedMult;
    public float duration;
    public float effectMult;

    public AilmentAttributes(float speedMult, float duration, float effectMult) {
      this.speedMult = speedMult;
      this.duration = duration;
      this.effectMult = effectMult;
    }
  }

  public static HashSet<ailmentType> affectMovement = new HashSet<ailmentType>{
    ailmentType.freeze, ailmentType.root
  };

  private static AilmentAttributes[] freezeData = {
    new AilmentAttributes(0.5f , 1, 0),
    new AilmentAttributes(0.25f, 2, 0),
    new AilmentAttributes(0    , 3, 0),
  };

  private static AilmentAttributes[] burnData = {
    new AilmentAttributes(0, 1, 0),
    new AilmentAttributes(0, 2, 0),
    new AilmentAttributes(0, 3, 0),
  };

  //TODO the rest of the ailments
  public static Dictionary<ailmentType, AilmentAttributes[]> ailmentData =
    new Dictionary<ailmentType, AilmentAttributes[]> {
      {ailmentType.freeze, freezeData},
      {ailmentType.burn,   burnData}
    };
}

