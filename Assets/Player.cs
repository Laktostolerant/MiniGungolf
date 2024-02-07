using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [SerializeField] GameObject ballObject;
    [SerializeField] GameObject gunLocation;
    [SerializeField] GameObject directionPointer;
    [SerializeField] MeshRenderer ballDirectionRenderer;

    [SerializeField] GameObject[] allGunModels;
    [SerializeField] GunStats[] allGunScripts;
    [SerializeField] LayerMask groundMask;

    int currentGunIndex;
    int shootStrength;
    public float gunHeight;

    bool isMyTurn;
    bool shootCooldown;

    bool isRespawning;
    bool gunsIsVisible = true;

    [SerializeField] Ball ball;
    GunManager gunManager;

    private void Awake() { Instance = this; }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gunManager = GetComponentInChildren<GunManager>();
        gunManager.GetBallObject(ballObject);

        ball = GetComponentInChildren<Ball>();
        ball.OnBallTurnOver += SetShootable;

        isMyTurn = true;
        shootStrength = allGunScripts[0].GunStrength;
    }

    private void Update()
    {
        if (isMyTurn && Input.GetKeyDown(KeyCode.O) && !shootCooldown)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        gunManager.SaveGunRelativePosition();
        gunManager.GunWasShot();

        ball.FireBall(directionPointer.transform.position - ballObject.transform.position, shootStrength);

        isMyTurn = false;
    }

    void SetShootable()
    {
        Debug.Log("ball is land lmao");
        isMyTurn = true;
    }
}
