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

    bool isMyTurn = true;

    private void Start()
    {
        currentGun = gunObjects[0];
    }

    void FixedUpdate()
    {
        if (!isMyTurn) return;

        if (Input.GetKey(KeyCode.A)) RotateGunToTheSide(true);
        if (Input.GetKey(KeyCode.D)) RotateGunToTheSide(false);

    }

    void Update()
    {
        if (!isMyTurn) return;

        GunDirection();

        if (Input.GetKeyDown(KeyCode.Q)) ChangeActiveGun(-1);
        if (Input.GetKeyDown(KeyCode.E)) ChangeActiveGun(1);
    }

    public void GetBallObject(GameObject myBall)
    {
        ballObject = myBall;
        ball = ballObject.GetComponent<Ball>();
        ball.OnBallTurnOver += BallTurnOver;
        SaveGunRelativePosition();
    }

    public void ChangeActiveGun(int change)
    {
        SaveGunRelativePosition();

        activeGunIndex += change;

        if (activeGunIndex > gunObjects.Length - 1)
            activeGunIndex = 0;

        if (activeGunIndex < 0)
            activeGunIndex = gunObjects.Length - 1;

        foreach (GameObject gun in gunObjects)
        {
            ToggleGunMeshes(gun, false);
        }

        Debug.Log("new active gun: " +  activeGunIndex);
        currentGun = gunObjects[activeGunIndex];
        ToggleGunMeshes(currentGun, true);
        SetWeaponOffset();
    }

    public void SaveGunRelativePosition()
    {
        gunOffset = ballObject.transform.position - transform.position;
    }
    
    void SetWeaponOffset()
    {
        transform.position = ballObject.transform.position - gunOffset;
        gunObjects[activeGunIndex].transform.position = transform.position + new Vector3(0, gunObjects[activeGunIndex].GetComponent<GunProperties>().properties.GunHeight, 0);
        gunPointer.transform.position = ballObject.transform.position + new Vector3(gunOffset.x, -gunOffset.y, gunOffset.z) + new Vector3(0, gunObjects[activeGunIndex].GetComponent<GunProperties>().properties.GunHeight, 0);
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
        isMyTurn = false;
        ToggleGunMeshes(currentGun, false);
        WeaponAbility ability = currentGun.GetComponent<WeaponAbility>();
        ability.UseWeaponAbilities(ballObject);
    }

    void BallTurnOver()
    {
        SetWeaponOffset();
        isMyTurn = true;
    }

    public GunProperties GetActiveGunStats()
    {
        return gunObjects[activeGunIndex].GetComponent<GunProperties>();
    }

    void ToggleGunMeshes(GameObject weapon, bool setEnabled)
    {
        foreach(MeshRenderer mesh in weapon.GetComponentsInChildren<MeshRenderer>())
        {
            mesh.enabled = setEnabled;
        }
    }
}
