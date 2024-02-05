using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;

public class ShootManager : MonoBehaviour
{
    Vector3 previousPosition;
    Transform camRespawningTarget;

    public static ShootManager Instance;

    [SerializeField] GameObject ballObject;
    [SerializeField] GameObject gunLocation;
    [SerializeField] GameObject directionPointer;
    [SerializeField] MeshRenderer ballDirectionRenderer;

    [SerializeField] GameObject[] allGunModels;
    [SerializeField] GunStats[] allGunScripts;
    [SerializeField] LayerMask groundMask;

    Vector3 gunOffset;

    int currentGunIndex;
    int shootStrength;
    float rotationSpeed = 100;
    public float gunHeight;

    bool canShoot;
    bool shootCooldown;

    bool isRespawning;
    bool gunsIsVisible = true;

    [SerializeField] Ball ball;
    GunManager guns;

    bool isMyTurn;

    private void Awake() { Instance = this; }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        guns = GetComponentInChildren<GunManager>();  
        ball = GetComponentInChildren<Ball>();
        ball.OnBallFinalDestination += BallRespawned;

        canShoot = true;
        shootStrength = allGunScripts[0].GunStrength;
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A)) SideScroller(true);
        if (Input.GetKey(KeyCode.D)) SideScroller(false);
    }

    private void Update()
    {
        if (canShoot)
        {
            if (Input.GetKeyDown(KeyCode.Q)) SwapWeapon(-1);
            if (Input.GetKeyDown(KeyCode.E)) SwapWeapon(1);
            if (Input.GetKeyDown(KeyCode.O) && !shootCooldown) Shoot();
        }

        gunLocation.transform.position = new Vector3(gunLocation.transform.position.x, ballObject.transform.position.y + allGunModels[currentGunIndex].GetComponent<GunProperties>().properties.GunHeight, gunLocation.transform.position.z);

        //AlterObjHeight(gunLocation);
        GunDirection();
    }

    void Shoot()
    {
        previousPosition = ballObject.transform.position;
        SetRelativePosition();

        ToggleGunVisibility();

        ball.FireBall(directionPointer.transform.position - ballObject.transform.position, shootStrength);

        canShoot = false;
        StartCoroutine(ShootCooldown());
    }

    void Land()
    {
        ToggleGunVisibility();

        canShoot = true;
        gunLocation.transform.position = ballObject.transform.position - gunOffset;
    }

    void SetRelativePosition()
    {
        gunOffset = ballObject.transform.position - gunLocation.transform.position;
    }

    void SideScroller(bool left)
    {
        int dir = left ? 1 : -1;

        gunLocation.transform.RotateAround(ballObject.transform.position, Vector3.up, dir * rotationSpeed * Time.deltaTime);
    }

    void GunDirection()
    {
        gunLocation.transform.LookAt(ballObject.transform.position);
    }

    IEnumerator ShootCooldown()
    {
        shootCooldown = true;
        yield return new WaitForSeconds(0.1f);
        shootCooldown = false;
    }

    void SwapWeapon(int val)
    {
        currentGunIndex += val;
        if (currentGunIndex > allGunModels.Length - 1)
            currentGunIndex = 0;

        if (currentGunIndex < 0)
            currentGunIndex = allGunModels.Length - 1;
        SetCurrentGunVisible(currentGunIndex);
    }

    void SetCurrentGunVisible(int index)
    {
        foreach (GameObject gun in allGunModels)
        {
            gun.SetActive(false);
        }

        allGunModels[index].SetActive(true);
        shootStrength = allGunScripts[index].GunStrength;
    }

    void ToggleGunVisibility()
    {
        gunsIsVisible = !gunsIsVisible;

        MeshRenderer[] gunRenderers = allGunModels[currentGunIndex].GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer rend in gunRenderers)
            rend.enabled = gunsIsVisible;
        ballDirectionRenderer.enabled = gunsIsVisible;
    }

    void BallRespawned()
    {
        Debug.Log("ball is land lmao");
        ToggleGunVisibility();
        canShoot = true;

        gunLocation.transform.position = ballObject.transform.position - gunOffset;
    }
}
