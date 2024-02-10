using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper_Ability : WeaponAbility
{
    [SerializeField] LineRenderer lineRenderer;
    RaycastHit hit;

    bool isActiveWeapon;

    private void Start()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
    }

    public override void OnWeaponDeselect()
    {
        isActiveWeapon = false;
        lineRenderer.positionCount = 0;
    }

    public override void ShootAbility(GameObject myBall)
    {
        lineRenderer.positionCount = 0;
    }

    public override void IdleAbility(GameObject myBall)
    {
        PredictShot(myBall);
    }

    void PredictShot(GameObject myBall)
    {
        Ball ball = myBall.GetComponent<Ball>();
        Vector3 origin = myBall.transform.position;
        Vector3 direction = Player.Instance.GetGunDirection(true);
        List<Vector3> hitPoints = new List<Vector3>();
        float remainingDistance = 3;

        hitPoints.Add(origin);

        while (remainingDistance > 0)
        {
            if (hitPoints.Count > 3) break;

            if (Physics.Raycast(origin, direction, out hit, remainingDistance, 1))
            {
                hitPoints.Add(hit.point);
                direction = Vector3.Reflect(direction, hit.normal);
                origin = hit.point;
                remainingDistance -= hit.distance;
            }
            else
            {
                hitPoints.Add(origin + direction * remainingDistance);
                break;
            }
        }

        lineRenderer.positionCount = hitPoints.Count;

        for(int i = 0; i < lineRenderer.positionCount; i++)
        {
            lineRenderer.SetPosition(i, hitPoints[i]);
        }
    }

    private void DrawReflectionPattern(Vector3 position, Vector3 direction, int reflectionsRemaining)
    {
        if (reflectionsRemaining == 0)
        {
            return;
        }

        Vector3 startingPosition = position;

        Ray ray = new Ray(position, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10))
        {
            direction = Vector3.Reflect(direction, hit.normal);
            position = hit.point;
        }
        else
        {
            position += direction * 10;
        }

        //Gizmos.color = Color.yellow;
        //Gizmos.DrawLine(startingPosition, position);

        Debug.DrawLine(startingPosition, position, Color.blue);

        DrawReflectionPattern(position, direction, reflectionsRemaining - 1);


    }
}
