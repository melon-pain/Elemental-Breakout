using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element 
{
    Fire,
    Ice,
    Lightning,
    Wind
};

public interface IDamage
{
    public void TakeDamage(Element attacking, float amount);
}

public partial class Damage
{
    public static Element GetWeakness(Element defending)
    {
        switch (defending)
        {
            case Element.Fire:
                return Element.Ice;
            case Element.Ice:
                return Element.Lightning;
            case Element.Lightning:
                return Element.Wind;
            case Element.Wind:
                return Element.Fire;
            default:
                return Element.Fire;
        }
    }
}