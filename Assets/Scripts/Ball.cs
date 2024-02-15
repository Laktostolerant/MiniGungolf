using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/* Contains anything related to the set player's ball.
 * This script is individual and manages individual balls, not all balls.
 */

public class Ball : MonoBehaviour
{
    public enum BallState { SHOOTABLE, NOT_SHOOTABLE }; //current state of the ball, could potentially just be a bool but might want to have it be 3 states instead - "SHOOTING" while it's moving? idk.
    public BallState currentBallState;

    Rigidbody rb;

    Vector3 lastSafePosition; //Last position that was safe, to return to incase ball falls out of bounds.

    [SerializeField] CinemachineFreeLook playerCamera;
    [SerializeField] GameObject tempWaterBuffer; //Position that the camera watches until the player respawns, so that it doesnt go underwater together with the ball.
    [SerializeField] GameObject bubbles; //bubble particles underwater, attached to the ball.

    public delegate void BallEventHandler();
    public event BallEventHandler OnPlayerShoot = delegate { }; //Event system that triggers once the player shoots.
    public event BallEventHandler OnBallFinalPosition = delegate { }; //Event system that triggers once the ball lands in its final position after a shot.

    public delegate void BallCollisionEvents(Collision collision); //Event system for tracking collisions on the ball.
    public event BallCollisionEvents OnBallCollision = delegate { };

    void Start()
    {
        lastSafePosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (rb.velocity.magnitude < 0.01f)
        {
            rb.velocity = Vector3.zero;
        }
    }

    //Pretty self explanatory, it adds force to the ball.
    //If the save position bool is set to true it will save this previous position as a safe position.
    public void AddForceToBall(Vector3 direction, float strength, bool savePosition)
    {
        if(savePosition) lastSafePosition = transform.position;
        rb.AddForce(direction * strength, ForceMode.Impulse);
    }

    //Calls the event for all listeners when ball is shot.
    public void BallWasShot()
    {
        OnPlayerShoot();
    }


    //Finish current player's turn.
    [ContextMenu("Finish My Turn")]
    public void GoNextPlayer()
    {
        OnBallFinalPosition();
    }

    //This is a temporary solution since currently the only death is water.
    //Should probably have set subscripts that are called in other worlds?
    //Incase, like, it lands on normal ground out of bounds.
    public void GoRespawnBall() { StartCoroutine(RespawnBall()); }
    IEnumerator RespawnBall()
    {
        currentBallState = BallState.NOT_SHOOTABLE;

        bubbles.SetActive(true);

        transform.rotation = new Quaternion(0, 0, 0, 0);
        rb.angularVelocity = Vector3.zero;
        rb.drag = 20;
        rb.velocity = Vector3.down;

        Vector3 waterSurfaceHitPosition = new Vector3(transform.position.x, 0, transform.position.z);

        GameObject tempObject = Instantiate(tempWaterBuffer, waterSurfaceHitPosition, Quaternion.identity);
        playerCamera.m_Follow = tempObject.transform;
        playerCamera.LookAt = tempObject.transform;

        yield return new WaitForSeconds(3);

        bubbles.SetActive(false);

        transform.position = lastSafePosition;
        playerCamera.m_Follow = transform;
        playerCamera.m_LookAt = transform;
        Destroy(tempObject);

        rb.drag = 1;

        GoNextPlayer();
    }

    //If it lands on a gameobject with the tag out of bounds, it respawns.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("OutOfBounds"))
        {
            Debug.Log("i hit water");
            StartCoroutine(RespawnBall());
        }
    }

    //Get method for other scripts.
    public Rigidbody GetBallRigidbody()
    {
        return rb;
    }

    //Calls the oncollisionenter event for listeners.
    private void OnCollisionEnter(Collision collision)
    {
        OnBallCollision(collision);
    }
}
