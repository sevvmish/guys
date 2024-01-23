using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public AbilityTypes CurrentAbility { get; private set; }
    private PlayerControl currentPlayerControl;
    private EffectsControl effectsControl;
    private GameManager gm;

    public void SetData(PlayerControl pc, EffectsControl ec)
    {
        gm = GameManager.Instance;
        currentPlayerControl = pc;
        effectsControl = ec;
        CurrentAbility = AbilityTypes.none;
        gm.GetUI().SetMainPlayerAbilityManager(this);        
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (CurrentAbility != AbilityTypes.none) { return; }

        if (other.TryGetComponent(out AbilityProvider ap))
        {
            print(ap.CurrentAbility + " !!!!!!!!!!!!!!!!!!!!!!");

            CurrentAbility = ap.CurrentAbility;

            if (currentPlayerControl.IsItMainPlayer)
            {
                gm.GetUI().ShowAbilityButton(CurrentAbility);
            }

            ap.Deactivate();
        }
    }

    public void ActivateAbility()
    {
        if (CurrentAbility == AbilityTypes.none) return;

        switch (CurrentAbility)
        {
            case AbilityTypes.Acceleration:

                break;

            case AbilityTypes.RocketBack:

                break;
        }

        CurrentAbility = AbilityTypes.none;
        gm.GetUI().HideAbilityButton();
    }

}

public enum AbilityTypes
{
    none,
    Acceleration,
    RocketBack
}
