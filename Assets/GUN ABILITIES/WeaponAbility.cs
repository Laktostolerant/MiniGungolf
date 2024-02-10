using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAbility : MonoBehaviour
{
    bool isActiveWeapon;

    public virtual void OnWeaponSelect()
    {
        isActiveWeapon = true;
    }

    public virtual void OnWeaponDeselect()
    {
        isActiveWeapon = false;
    }

    public virtual void ShootAbility(GameObject myBall)
    {
        Ball ball = myBall.GetComponent<Ball>();
        ball.BallWasShot();
    }

    public virtual void IdleAbility(GameObject myBall)
    {

    }
}
