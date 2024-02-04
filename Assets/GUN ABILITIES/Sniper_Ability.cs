using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper_Ability : WeaponAbility
{
    public override void ShootAbility()
    {
        
    }

    public override void IdleAbility()
    {
        PredictShot();
    }

    void PredictShot()
    {

    }
}
