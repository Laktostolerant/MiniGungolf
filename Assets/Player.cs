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
    [SerializeField] GameObject directionPointer;

    int shootStrength;
    public float gunHeight;

    bool isMyTurn;

    [SerializeField] Ball ball;
    GunManager gunManager;

    private void Awake() { Instance = this; }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gunManager = GetComponentInChildren<GunManager>();
        gunManager.ConnectBallToPlayer(ballObject);

        ball = GetComponentInChildren<Ball>();
        ball.OnBallFinalPosition += PlayerBallFinalDestination;

        isMyTurn = true;
        shootStrength = gunManager.GetActiveGunStats().properties.GunStrength;
    }

    private void Update()
    {
        if (isMyTurn && Input.GetKeyDown(KeyCode.O))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        gunManager.SaveGunRelativePosition();
        gunManager.FireGun();
        shootStrength = gunManager.GetActiveGunStats().properties.GunStrength;
        Debug.Log("shootstrength: " + shootStrength);
        ball.AddForceToBall(GetGunDirection(), shootStrength, true);

        isMyTurn = false;
    }

    void PlayerBallFinalDestination()
    {
        Manageroo.Instance.NextPlayerTurn();
        Debug.Log("ball is land lmao");
    }

    [ContextMenu("Start My Turn")]
    public void BecomePlayerTurn()
    {
        isMyTurn = true;
        gunManager.StartPlayerTurn();
    }

    public Vector3 GetGunDirection(bool flatten = false)
    {
        Vector3 dir = directionPointer.transform.position - ballObject.transform.position;
        dir.Normalize();

        if (flatten) dir.y = 0;

        return dir;
    }
}
