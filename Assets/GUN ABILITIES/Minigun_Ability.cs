using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigun_Ability : WeaponAbility
{
    public override void ActiveGunAbility(GameObject myBall)
    {
        StartCoroutine(RapidFire(myBall));
    }

    public override void PassiveGunAbility(GameObject myBall)
    {

    }

    IEnumerator RapidFire(GameObject myBall)
    {
        Ball ball = myBall.GetComponent<Ball>();

        for (int i = 0; i < 20; i++)
        {
            ball.AddForceToBall(Player.Instance.GetGunDirection(true), 0.5f, false);
            yield return new WaitForSeconds(0.1f);
        }

        ball.BallWasShot();

        while (ball.GetBallRigidbody().velocity.magnitude > 0)
            yield return null;

        ball.GoNextPlayer();
    }
}
