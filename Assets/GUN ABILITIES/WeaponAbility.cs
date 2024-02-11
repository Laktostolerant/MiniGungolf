using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAbility : MonoBehaviour
{
    public bool isActiveWeapon;

    public Ball myBall;

    public virtual void ConnectToMyBall(Ball ball)
    {
        myBall = ball;
        myBall.OnBallCollision += OnBallCollisionEnter;
    }

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

    public virtual void OnBallCollisionEnter(Collision collision)
    {
        if (!isActiveWeapon) return;
    }
}
