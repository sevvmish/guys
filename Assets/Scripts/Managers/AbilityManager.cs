using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public AbilityTypes CurrentAbility { get; private set; }
    private PlayerControl currentPlayerControl;
    private EffectsControl effectsControl;
    private GameManager gm;
    private WaitForSeconds ZeroOne = new WaitForSeconds(0.1f);


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

        if (other.TryGetComponent(out AbilityProvider ap))
        {
            //print(ap.CurrentAbility + " !!!!!!!!!!!!!!!!!!!!!!");

            CurrentAbility = ap.CurrentAbility;

            if (currentPlayerControl.IsItMainPlayer)
            {
                gm.GetUI().ShowAbilityButton(CurrentAbility);
                StartCoroutine(playTimerForAbility(Globals.ABILITY_DURATION));
            }
            else
            {
                ActivateAbility();
            }

            ap.Deactivate();
        }
    }

    public void ActivateAbility()
    {
        if (CurrentAbility == AbilityTypes.none || !currentPlayerControl.IsCanAct || currentPlayerControl.IsRagdollActive) return;

        switch (CurrentAbility)
        {
            case AbilityTypes.Acceleration:
                currentPlayerControl.ChangeSpeed(1.5f, Globals.ACCELERATION_DURATION);
                effectsControl.MakeFastEffect(Globals.ACCELERATION_DURATION);
                break;

            case AbilityTypes.RocketBack:
                effectsControl.MakeRocketPackView(Globals.ROCKETPACK_DURATION);
                break;
        }

        if (currentPlayerControl.IsItMainPlayer)
        {
            gm.ShakeScreen(0.2f, 2f, 30);
        }

        HideAbility(true);
    }

    public void HideAbility(bool isPressed)
    {        
        if (CurrentAbility == AbilityTypes.none) { return; }

        CurrentAbility = AbilityTypes.none;
        
        if (currentPlayerControl.IsItMainPlayer)
        {
            gm.GetUI().HideAbilityButton(isPressed);
        }        
    }

    private IEnumerator playTimerForAbility(float duration)
    {
        for (float i = 0; i < duration; i += 0.1f)
        {
            gm.GetUI().SetFillAmountAbilityTimer(1f - (i/duration));
            yield return ZeroOne;
            if (currentPlayerControl.IsDead || CurrentAbility == AbilityTypes.none) yield break;
        }

        HideAbility(false);
    }

}

public enum AbilityTypes
{
    none,
    Acceleration,
    RocketBack
}
