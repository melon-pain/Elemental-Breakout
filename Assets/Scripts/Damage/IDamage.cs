using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element 
{
    Fire,
    Wind,
    Lightning,
    Ice,
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

    public static Element GetResistance(Element defending)
    {
        switch (defending)
        {
            case Element.Fire:
                return Element.Wind;
            case Element.Ice:
                return Element.Fire;
            case Element.Lightning:
                return Element.Ice;
            case Element.Wind:
                return Element.Lightning;
            default:
                return Element.Fire;
        }
    }

    public static float GetModifier(Element attacking, Element defending)
    {
        switch (attacking)
        {
            case Element.Fire:
                switch (defending)
                {
                    case Element.Fire:
                        return 0.5f;
                    case Element.Wind:
                        return 0.5f;
                    case Element.Lightning:
                        return 1.0f;
                    case Element.Ice:
                        return 2.0f;
                }
                break;
            case Element.Wind:
                switch (defending)
                {
                    case Element.Fire:
                        return 2.0f;
                    case Element.Wind:
                        return 0.5f;
                    case Element.Lightning:
                        return 0.5f;
                    case Element.Ice:
                        return 1.0f;
                }
                break;
            case Element.Lightning:
                switch (defending)
                {
                    case Element.Fire:
                        return 1.0f;
                    case Element.Wind:
                        return 2.0f;
                    case Element.Lightning:
                        return 0.5f;
                    case Element.Ice:
                        return 0.5f;
                }
                break;
            case Element.Ice:
                switch (defending)
                {
                    case Element.Fire:
                        return 0.5f;
                    case Element.Wind:
                        return 1.0f;
                    case Element.Lightning:
                        return 2.0f;
                    case Element.Ice:
                        return 0.5f;
                }
                break;
        }
        return 1.0f;
    }
}