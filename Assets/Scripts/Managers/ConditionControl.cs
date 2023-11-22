using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionControl : MonoBehaviour
{
    private PlayerControl pc;
    private EffectsControl ec;
    private HashSet<Conditions> conditions = new HashSet<Conditions>();

    // Start is called before the first frame update
    public void SetData(PlayerControl control, EffectsControl effects)
    {
        pc = control;
        ec = effects;
    }

    public bool HasCondition(Conditions _type)
    {
        return conditions.Contains(_type);
    }
        
    public void SetEnslowedState(bool isEnsl)
    {
        if (!conditions.Contains(Conditions.enslowed) && isEnsl)
        {
            conditions.Add(Conditions.enslowed);
        }
        else if (conditions.Contains(Conditions.enslowed) && !isEnsl)
        {
            conditions.Remove(Conditions.enslowed);
        }
    }

    public bool MakeFrozen(float timer)
    {
        if (conditions.Contains(Conditions.frozen) || pc.IsDead || pc.IsRagdollActive) return false;

        conditions.Add(Conditions.frozen);
        StartCoroutine(playAnyState(timer, Conditions.frozen));
        pc.ChangeJumpPermission(timer);
        ec.MakeFrozen(timer);
        pc.ChangeWalkPermission(timer);
        pc.ChangeSpeed(0.01f, timer);
        return true;
    }

    public bool MakePainted(float koeff, float timer, Color color)
    {
        if (conditions.Contains(Conditions.painted) || pc.IsDead || pc.IsRagdollActive) return false;

        conditions.Add(Conditions.painted);
        ec.MakePainted(color, timer);
        StartCoroutine(playAnyState(timer, Conditions.painted));
        pc.ChangeSpeed(koeff, timer);

        return true;
    }
    private IEnumerator playAnyState(float timer, Conditions cond)
    {
        for (float i = 0; i < timer; i+=0.1f)
        {
            yield return new WaitForSeconds(0.1f);
            if (pc.IsDead && pc.IsRagdollActive) break;
        }

        conditions.Remove(cond);
    }
}

public enum Conditions
{
    enslowed,
    painted,
    stunned,
    frozen
}
