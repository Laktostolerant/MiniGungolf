using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper_Ability : WeaponAbility
{
    [SerializeField] LineRenderer lineRenderer;
    RaycastHit hit;

    int numberOfBounces;

    private void Start()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
    }

    private void FixedUpdate()
    {
        if (!isActiveWeapon) return;
    }

    public override void OnWeaponSelect()
    {
        isActiveWeapon = true;
        numberOfBounces = 0;
    }

    public override void OnWeaponDeselect()
    {
        isActiveWeapon = false;
        lineRenderer.positionCount = 0;
        numberOfBounces = 0;
    }

    public override void ActiveGunAbility(GameObject myBall)
    {
        myBall.GetComponent<Ball>().BallWasShot();
        lineRenderer.positionCount = 0;
    }

    //public override void PassiveGunAbility(GameObject myBall)
    //{
    //    PredictShot(myBall);
    //}

    //void PredictShot(GameObject myBall)
    //{
    //    Ball ball = myBall.GetComponent<Ball>();
    //    Vector3 origin = myBall.transform.position;
    //    Vector3 direction = ThePlayer.Instance.GetGunDirection(true);
    //    List<Vector3> hitPoints = new List<Vector3>();
    //    float remainingDistance = 2;

    //    hitPoints.Add(origin);

    //    while (remainingDistance > 0)
    //    {
    //        if (hitPoints.Count > 2) break;

    //        if (Physics.Raycast(origin, direction, out hit, remainingDistance, 1))
    //        {
    //            hitPoints.Add(hit.point);
    //            direction = Vector3.Reflect(direction, hit.normal);
    //            origin = hit.point;
    //            remainingDistance -= hit.distance;
    //        }
    //        else
    //        {
    //            hitPoints.Add(origin + direction * remainingDistance);
    //            break;
    //        }
    //    }

    //    lineRenderer.positionCount = hitPoints.Count;

    //    for (int i = 0; i < lineRenderer.positionCount; i++)
    //    {
    //        lineRenderer.SetPosition(i, hitPoints[i]);
    //    }
    //}

    //public override void OnBallCollisionEnter(Collision collision)
    //{
    //    if (!isActiveWeapon) return;

    //    numberOfBounces++;

    //    Debug.Log("BOUNCAH");

    //    if(numberOfBounces >= 2) myBall.GetBallRigidbody().velocity = Vector3.zero;
    //    myBall.GoNextPlayer();
    //}
}
