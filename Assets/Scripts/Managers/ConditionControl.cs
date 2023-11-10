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

    public bool MakePainted(float koeff, float timer, Color color)
    {
        if (conditions.Contains(Conditions.painted) || pc.IsDead || pc.IsRagdollActive) return false;

        conditions.Add(Conditions.painted);
        ec.MakePainted(color, timer);
        StartCoroutine(playPainted(timer));
        pc.ChangeSpeed(koeff, timer);

        return true;
    }
    private IEnumerator playPainted(float timer)
    {
        for (float i = 0; i < timer; i+=0.1f)
        {
            yield return new WaitForSeconds(0.1f);
            if (pc.IsDead && pc.IsRagdollActive) break;
        }

        conditions.Remove(Conditions.painted);
    }
}

public enum Conditions
{
    enslowed,
    painted,
    stunned
}
