using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ball : MonoBehaviour
{
    UnityEvent ballWasRespawned = new UnityEvent();

    public enum BallState { SHOOTABLE, NOT_SHOOTABLE };
    public BallState currentBallState;

    Rigidbody rb;

    Vector3 lastSafePosition;

    [SerializeField] CinemachineFreeLook cam;
    [SerializeField] GameObject tempWaterBuffer;
    [SerializeField] GameObject bubbles;

    public delegate void BallEventHandler();
    public event BallEventHandler OnBallFinalDestination;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (rb.velocity.magnitude < 0.01f)
        {
            rb.velocity = Vector3.zero;
            //Land();
        }
    }

    public void FireBall(Vector3 direction, int strength)
    {
        rb.AddForce(direction * strength, ForceMode.Impulse);
    }

    [ContextMenu("Land Ball")]
    void Land()
    {
        OnBallFinalDestination();
    }



    public void GoRespawnBall() { StartCoroutine(RespawnBall()); }
    IEnumerator RespawnBall()
    {
        currentBallState = BallState.NOT_SHOOTABLE;

        bubbles.SetActive(true);

        transform.rotation = new Quaternion(0, 0, 0, 0);
        rb.angularVelocity = Vector3.zero;
        rb.drag = 20;
        rb.velocity = Vector3.down;

        Vector3 waterSurfaceHit = new Vector3(transform.position.x, 0, transform.position.z);

        GameObject tempObject = Instantiate(tempWaterBuffer, waterSurfaceHit, Quaternion.identity);
        cam.m_Follow = tempObject.transform;
        cam.LookAt = tempObject.transform;

        yield return new WaitForSeconds(3);

        bubbles.SetActive(false);

        transform.position = lastSafePosition;
        cam.m_Follow = transform;
        cam.m_LookAt = transform;
        Destroy(tempObject);

        rb.drag = 1;
    }
}
