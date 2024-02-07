using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    int activeGunIndex;
    [SerializeField] GameObject[] gunObjects;
    [SerializeField] GameObject gunPointer;

    GameObject ballObject;
    Vector3 gunOffset;

    float rotationSpeed = 100;

    Ball ball;

    GameObject currentGun;

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A)) RotateGunToTheSide(true);
        if (Input.GetKey(KeyCode.D)) RotateGunToTheSide(false);

    }

    void Update()
    {
        GunDirection();

        if (Input.GetKeyDown(KeyCode.Q)) ChangeActiveGun(-1);
        if (Input.GetKeyDown(KeyCode.E)) ChangeActiveGun(1);
    }

    public void GetBallObject(GameObject myBall)
    {
        ballObject = myBall;
        ball = ballObject.GetComponent<Ball>();
        ball.OnBallTurnOver += BallTurnOver;
    }

    public void ChangeActiveGun(int change)
    {
        activeGunIndex += change;

        if (activeGunIndex > gunObjects.Length - 1)
            activeGunIndex = 0;

        if (activeGunIndex < 0)
            activeGunIndex = gunObjects.Length - 1;

        foreach (GameObject gun in gunObjects)
        {
            gun.SetActive(false);
        }

        Debug.Log("new active gun: " +  activeGunIndex);
        currentGun = gunObjects[activeGunIndex];
        currentGun.SetActive(true);

        SaveGunRelativePosition();
    }

    public void SaveGunRelativePosition()
    {
        gunOffset = ballObject.transform.position - gunObjects[activeGunIndex].transform.position;
    }

    void RotateGunToTheSide(bool left)
    {
        int dir = left ? 1 : -1;
        transform.RotateAround(ballObject.transform.position, Vector3.up, dir * rotationSpeed * Time.deltaTime);
    }

    void GunDirection()
    {
        gunObjects[activeGunIndex].transform.LookAt(ballObject.transform.position);
    }

    public void GunWasShot()
    {
        currentGun.SetActive(false);
    }

    void BallTurnOver()
    {

    }
}
