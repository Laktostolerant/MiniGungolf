using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAbility : MonoBehaviour
{
    public virtual void UseWeaponAbilities(GameObject myBall)
    {
        ShootAbility(myBall);
        IdleAbility(myBall);
    }

    public virtual void ShootAbility(GameObject myBall)
    {

    }

    public virtual void IdleAbility(GameObject myBall)
    {

    }
}
