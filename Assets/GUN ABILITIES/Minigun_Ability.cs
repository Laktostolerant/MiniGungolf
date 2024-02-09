using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigun_Ability : WeaponAbility
{
    public override void ShootAbility(GameObject myBall)
    {

    }

    public override void IdleAbility(GameObject myBall)
    {
        StartCoroutine(RapidFire(myBall));
    }

    IEnumerator RapidFire(GameObject myBall)
    {
        Ball ball = myBall.GetComponent<Ball>();

        for (int i = 0; i < 10; i++)
        {
            ball.AddForceToBall(Vector3.left, 0.1f, false);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
