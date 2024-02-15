using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * Generic weapon ability script.
 * This script is generally unused except for the pistol, as it lacks abilities.
 * It holds generic methods that are then overridden by custom inherited scripts.
 * An example of this is "Minigun_Ability" that overrides the ShootAbility() method with its own custom logic.
 */

public class WeaponAbility : MonoBehaviour
{
    public bool isActiveWeapon;

    public Ball myBall;

    //Get the information of the player's ball and subscribe to the collision event.
    public virtual void ConnectToMyBall(Ball ball)
    {
        myBall = ball;
        myBall.OnBallCollision += OnBallCollisionEnter;
    }

    //Set as active weapon.
    //Some weapons have passive abilities, they should only be used while the gun is actually active.
    public virtual void OnWeaponSelect() { isActiveWeapon = true; }

    //Reverse of previous.
    public virtual void OnWeaponDeselect() { isActiveWeapon = false; }

    //Active ability. This is called once the ball is shot.
    public virtual void ActiveGunAbility(GameObject myBall)
    {
        Ball ball = myBall.GetComponent<Ball>();
        ball.BallWasShot();
    }

    //Passive ability. This is used before the ball is shot.
    public virtual void PassiveGunAbility(GameObject myBall)
    {

    }

    public virtual void OnBallCollisionEnter(Collision collision)
    {
        if (!isActiveWeapon) return;
    }
}
