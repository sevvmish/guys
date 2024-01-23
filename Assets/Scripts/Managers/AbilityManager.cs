using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public AbilityTypes CurrentAbility { get; private set; }
    private PlayerControl currentPlayerControl;
    private EffectsControl effectsControl;

    public void SetData(PlayerControl pc, EffectsControl ec)
    {
        currentPlayerControl = pc;
        effectsControl = ec;
        CurrentAbility = AbilityTypes.none;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CurrentAbility != AbilityTypes.none) { return; }

        if (other.TryGetComponent(out AbilityProvider ap))
        {
            print(ap.CurrentAbility + " !!!!!!!!!!!!!!!!!!!!!!");
            ap.Deactivate();
        }
    }

}

public enum AbilityTypes
{
    none,
    Acceleration,
    RocketBack
}
