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

    private void Awake() { Instance = this; } //THIS SHOULD NOT BE AN INSTANCE UNLESS IT'S MY PLAYER. OTHERWISE MAYBE THERE IS ANOTHER SOLUTION IDK

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

    //Shoot the ball! Saves gun's relative position t othe ball.
    void Shoot()
    {
        gunManager.SaveGunRelativePosition();
        gunManager.FireGun();
        shootStrength = gunManager.GetActiveGunStats().properties.GunStrength;
        ball.AddForceToBall(GetGunDirection(), shootStrength, true);

        isMyTurn = false;
    }

    //Tell game that i have finished my turn.
    void PlayerBallFinalDestination()
    {
        Manageroo.Instance.NextPlayerTurn();
        Debug.Log("ball is land lmao");
    }

    //Tell player that it's their turn.
    [ContextMenu("Start My Turn")]
    public void BecomePlayerTurn()
    {
        Debug.Log("my turn");
        isMyTurn = true;
        gunManager.StartPlayerTurn();
    }

    //Get whatever direction the gun is currently pointing in.
    //Flatten means it ignores the Y value.
    public Vector3 GetGunDirection(bool flatten = false)
    {
        Vector3 dir = directionPointer.transform.position - ballObject.transform.position;
        dir.Normalize();

        if (flatten) dir.y = 0;

        return dir;
    }
}
