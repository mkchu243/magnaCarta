//mwoo_dev
using UnityEngine;
using System.Collections.Generic;

public enum Element { water, wood, fire, earth, metal, holy };

public struct ElementAttributes{
  public HashSet<Element> weakness;
  public HashSet<Element> strength; //NOTE do we even need this?
  public HashSet<Element> creates;
  public ailmentType ailment;
  public Material mat;

  public ElementAttributes(HashSet<Element> weakness,
                           HashSet<Element> strength,
                           HashSet<Element> creates,
                           ailmentType ailment,
                           Material mat){
    this.weakness = weakness;
    this.strength = strength;
    this.creates = creates;
    this.ailment = ailment;
    this.mat = mat;
  }
}

static class Reference {
  //the weakness of that element
  public static HashSet<Element> WeaknessWater = new HashSet<Element> { Element.earth, Element.holy };
  public static HashSet<Element> WeaknessFire  = new HashSet<Element> { Element.water, Element.holy };
  public static HashSet<Element> WeaknessWood  = new HashSet<Element> { Element.metal, Element.holy };
  public static HashSet<Element> WeaknessEarth = new HashSet<Element> { Element.wood,  Element.holy };
  public static HashSet<Element> WeaknessMetal = new HashSet<Element> { Element.fire,  Element.holy };
  public static HashSet<Element> WeaknessHoly  = new HashSet<Element> {};

  public static HashSet<Element> StrengthWater = new HashSet<Element> { Element.fire };
  public static HashSet<Element> StrengthFire  = new HashSet<Element> { Element.metal };
  public static HashSet<Element> StrengthWood  = new HashSet<Element> { Element.earth };
  public static HashSet<Element> StrengthEarth = new HashSet<Element> { Element.water };
  public static HashSet<Element> StrengthMetal = new HashSet<Element> { Element.wood };
  public static HashSet<Element> StrengthHoly  = new HashSet<Element> { Element.water, Element.wood, Element.fire, Element.metal, Element.earth};
	
  //what that element creates
  public static HashSet<Element> CreateWater = new HashSet<Element> { Element.wood  };
  public static HashSet<Element> CreateFire  = new HashSet<Element> { Element.earth };
  public static HashSet<Element> CreateWood  = new HashSet<Element> { Element.fire  };
  public static HashSet<Element> CreateEarth = new HashSet<Element> { Element.metal };
  public static HashSet<Element> CreateMetal = new HashSet<Element> { Element.water };
  public static HashSet<Element> CreateHoly  = new HashSet<Element> { };

  public static Material waterMat = Resources.Load("Materials/waterMat") as Material;
  public static Material fireMat = Resources.Load("Materials/fireMat") as Material;
  public static Material woodMat = Resources.Load("Materials/woodMat") as Material;
  public static Material earthMat = Resources.Load("Materials/earthMat") as Material;
  public static Material metalMat = Resources.Load("Materials/metalMat") as Material;
  public static Material holyMat = Resources.Load("Materials/holyMat") as Material;
  public static Material normalMat = Resources.Load("Materials/normalMat") as Material;

  public static Dictionary<Element, ElementAttributes> elements =
    new Dictionary<Element, ElementAttributes>{
    { Element.water , new ElementAttributes(WeaknessWater, StrengthWater, CreateWater, ailmentType.freeze, waterMat) }, //TODO make ailments match
    { Element.fire  , new ElementAttributes(WeaknessFire,  StrengthFire,  CreateFire,  ailmentType.burn,   fireMat) },
    { Element.wood  , new ElementAttributes(WeaknessWood,  StrengthWood,  CreateWood,  ailmentType.root,   woodMat) },
    { Element.earth , new ElementAttributes(WeaknessEarth, StrengthEarth, CreateEarth, ailmentType.burn,   earthMat) },
    { Element.metal , new ElementAttributes(WeaknessMetal, StrengthMetal, CreateMetal, ailmentType.dam,    metalMat) },
    { Element.holy  , new ElementAttributes(WeaknessHoly,  StrengthHoly,  CreateHoly,  ailmentType.freeze, holyMat) } //TODO this shouldnt freeze, but it will never happen
  };
}
