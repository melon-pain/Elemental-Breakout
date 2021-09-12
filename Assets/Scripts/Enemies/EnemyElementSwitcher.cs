using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyElementSwitcher : MonoBehaviour
{
    
    public enum SwitchType
    {
        HP,
        Time
    };
    private Enemy enemy;
    [SerializeField] private SwitchType switchType;

    private void Start()
    {
        enemy = this.GetComponent<Enemy>();
        switch (switchType)
        {
            case SwitchType.HP:
                StartCoroutine(CheckHP());
                break;
            case SwitchType.Time:
                break;
        }
    }

    private IEnumerator CheckHP()
    {
        while (!enemy.isDead)
        {
            float ratio = enemy.currentHP / enemy.maxHP;

            if (ratio > 0.75f)
            {
                enemy.ChangeElement(Element.Fire);
            }
            else if (ratio > 0.5f)
            {
                enemy.ChangeElement(Element.Ice);
            }
            else if (ratio > 0.25f)
            {
                enemy.ChangeElement(Element.Lightning);
            }
            else if (ratio > 0.0f)
            {
                enemy.ChangeElement(Element.Wind);
            }
            Debug.Log(ratio);
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }
}
