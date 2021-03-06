﻿using System.Diagnostics;
using System.Collections.Generic;

                         //water fire   wood earth metal
public enum ailmentType { freeze, burn, root, dam, cut };

public class Ailment{
  private ailmentType type;
  private int level;
  private Stopwatch lifeTimer;
  private int lifeTime;

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
  public bool IsLive { get { return lifeTimer.ElapsedMilliseconds < lifeTime; } }
  
  ///////ailment dictionary////////
  public struct AilmentAttributes {
    public float speedMult;
    public int duration;
    public float effectMult;

    public AilmentAttributes(float speedMult, int duration, float effectMult) {
      this.speedMult = speedMult;
      this.duration = duration;
      this.effectMult = effectMult;
    }
  }

  public static HashSet<ailmentType> resetTimer = new HashSet<ailmentType> {
    ailmentType.freeze, ailmentType.root, ailmentType.dam, ailmentType.burn
  };

  public static HashSet<ailmentType> affectMovement = new HashSet<ailmentType>{
    ailmentType.freeze, ailmentType.root, ailmentType.dam
  };

  private static AilmentAttributes[] freezeData = {
    new AilmentAttributes(0.5f , 2000,    0),
    new AilmentAttributes(0.25f, 2500,    0),
    new AilmentAttributes(0    , 3000,    0),
  };

  private static AilmentAttributes[] burnData = {
    //                    burnClock duration damage
    new AilmentAttributes(0.20f, 2000,    0.20f),
    new AilmentAttributes(0.10f, 2500,    0.25f),
    new AilmentAttributes(0.10f, 3000,    0.30f),
  };

  private static AilmentAttributes[] cutData = {
    new AilmentAttributes(0, 2000,    0.75f),
    new AilmentAttributes(0, 2500,    0.6f),
    new AilmentAttributes(0, 1000,    0.5f),
  };

  public static Dictionary<ailmentType, AilmentAttributes[]> ailmentData =
    new Dictionary<ailmentType, AilmentAttributes[]> {
      {ailmentType.freeze, freezeData},
      {ailmentType.burn,   burnData},
      {ailmentType.root,   freezeData},
      {ailmentType.cut,    cutData},
      {ailmentType.dam,    freezeData}
    };
}

