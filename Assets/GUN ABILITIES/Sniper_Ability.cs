using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper_Ability : WeaponAbility
{
    public override void ShootAbility(GameObject myBall)
    {
        
    }

    public override void IdleAbility(GameObject myBall)
    {
        PredictShot(myBall);
    }

    void PredictShot(GameObject myBall)
    {

    }
}
