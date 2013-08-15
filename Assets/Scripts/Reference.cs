using UnityEngine;
using System.Collections.Generic;

public enum Element { water, wood, fire, earth, metal, holy };

public struct ElementAttributes{
  public HashSet<Element> weakness; //NOTE do we even need this?
  public HashSet<Element> strength;
  public HashSet<Element> creates;

  public ElementAttributes(HashSet<Element> weakness, HashSet<Element> strength, HashSet<Element> creates){
    this.weakness = weakness;
    this.strength = strength;
    this.creates = creates;
  }
}

static class Reference {
  ////////////////TODO put your comment here/////////////////
	// mwoo_dev
  public static HashSet<Element> WeaknessWater = new HashSet<Element> { Element.earth, Element.holy };
  public static HashSet<Element> WeaknessFire  = new HashSet<Element> { Element.water, Element.holy };
  public static HashSet<Element> WeaknessWood  = new HashSet<Element> { Element.metal, Element.holy };
  //public static HashSet<Element> WeaknessEarth = new HashSet<Element> { Element.wood,  Element.holy };
  //public static HashSet<Element> WeaknessMetal = new HashSet<Element> { Element.fire,  Element.holy };
  public static HashSet<Element> WeaknessHoly  = new HashSet<Element> {};

  public static HashSet<Element> StrengthWater = new HashSet<Element> { Element.fire };
  public static HashSet<Element> StrengthFire  = new HashSet<Element> { Element.metal };
  public static HashSet<Element> StrengthWood  = new HashSet<Element> { Element.earth };
  //public static HashSet<Element> StrengthEarth = new HashSet<Element> { Element.water };
  //public static HashSet<Element> StrengthMetal = new HashSet<Element> { Element.wood };
  public static HashSet<Element> StrengthHoly  = new HashSet<Element> { Element.water, Element.wood, Element.fire, Element.metal, Element.earth};
	
  public static HashSet<Element> CreateWater = new HashSet<Element> { Element.wood  };
  public static HashSet<Element> CreateFire  = new HashSet<Element> { Element.earth };
  public static HashSet<Element> CreateWood  = new HashSet<Element> { Element.fire  };
  //public static HashSet<Element> CreateEarth = new HashSet<Element> { Element.metal };
  //public static HashSet<Element> CreateMetal = new HashSet<Element> { Element.water };
  public static HashSet<Element> CreateHoly  = new HashSet<Element> { };
	
  public static Dictionary<Element, ElementAttributes> elements =
    new Dictionary<Element, ElementAttributes>{
    { Element.water , new ElementAttributes(WeaknessWater, StrengthWater, CreateWater) },
    { Element.fire  , new ElementAttributes(WeaknessFire,  StrengthFire,  CreateFire) },
    { Element.wood  , new ElementAttributes(WeaknessWood,  StrengthWood,  CreateWood) },
    //{ Element.earth , new ElementAttributes(WeaknessEarth, StrengthEarth, CreateEarth) },
    //{ Element.metal , new ElementAttributes(WeaknessMetal, StrengthMetal, CreateMetal) },
    { Element.holy  , new ElementAttributes(WeaknessHoly,  StrengthHoly,  CreateHoly) }
  };

  //TODO the other elements
}