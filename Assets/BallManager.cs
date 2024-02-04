using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;

public class BallManager : MonoBehaviour
{
    Vector3 previousPosition;
    Transform camRespawningTarget;

    public static BallManager Instance;

    [SerializeField] GameObject ballObject;
    [SerializeField] GameObject gunLocation;
    [SerializeField] GameObject directionPointer;
    [SerializeField] MeshRenderer ballDirectionRenderer;
    [SerializeField] CinemachineFreeLook camera;

    [SerializeField] GameObject[] allGunModels;
    [SerializeField] GunStats[] allGunScripts;
    [SerializeField] LayerMask groundMask;

    Rigidbody rb;

    Vector3 gunOffset;

    int currentGunIndex;
    int shootStrength;
    float rotationSpeed = 100;
    public float gunHeight;

    bool canShoot;
    bool shootCooldown;

    bool isRespawning;

    [SerializeField] GameObject tempWaterBuffer;
    [SerializeField] GameObject bubbles;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        rb = ballObject.GetComponent<Rigidbody>();
        canShoot = true;
        shootStrength = allGunScripts[0].GunStrength;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A)) SideScroller(true);
        if (Input.GetKey(KeyCode.D)) SideScroller(false);

        if (Input.GetKey(KeyCode.W)) gunHeight += 0.01f;
        if (Input.GetKey(KeyCode.S)) gunHeight -= 0.01f;
    }

    private void Update()
    {
        if (canShoot)
        {
            if (Input.GetKeyDown(KeyCode.Q)) SwapWeapon(-1);
            if (Input.GetKeyDown(KeyCode.E)) SwapWeapon(1);
            if (Input.GetKeyDown(KeyCode.O) && !shootCooldown) Shoot();
        }
        else
        {
            if (rb.velocity.magnitude < 0.05f && !shootCooldown) Land();
        }

        if (!isRespawning)
        {
            if (ballObject.transform.position.y > 0)
            {
                gunHeight = Mathf.Clamp(gunHeight, ballObject.transform.position.y + 0.1f, ballObject.transform.position.y + 0.3f);
            }
            else
            {
                isRespawning = true;
                StartCoroutine(RespawnBall());
            }
        }

        AlterObjHeight(gunLocation);
        AlterObjHeight(directionPointer);
        GunDirection();

        if (rb.velocity.magnitude < 0.01f) rb.velocity = Vector3.zero;
    }

    void Shoot()
    {
        previousPosition = ballObject.transform.position;
        SetRelativePosition();

        MeshRenderer[] gunRenderers = allGunModels[currentGunIndex].GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer rend in gunRenderers)
            rend.enabled = false;
        ballDirectionRenderer.enabled = false;


        rb.AddForce((directionPointer.transform.position - ballObject.transform.position) * shootStrength, ForceMode.Impulse);
        canShoot = false;
        StartCoroutine(ShootCooldown());
    }

    void Land()
    {
        ballObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

        MeshRenderer[] gunRenderers = allGunModels[currentGunIndex].GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer rend in gunRenderers)
            rend.enabled = true;
        ballDirectionRenderer.enabled = true;


        canShoot = true;
        gunLocation.transform.position = ballObject.transform.position - gunOffset;
        directionPointer.transform.position = ballObject.transform.position + gunOffset;
    }

    void SetRelativePosition()
    {
        gunOffset = ballObject.transform.position - gunLocation.transform.position;
    }

    void SideScroller(bool left)
    {
        int dir = left ? 1 : -1;

        gunLocation.transform.RotateAround(ballObject.transform.position, Vector3.up, dir * rotationSpeed * Time.deltaTime);
        directionPointer.transform.RotateAround(ballObject.transform.position, Vector3.up, dir * rotationSpeed * Time.deltaTime);
    }

    void AlterObjHeight(GameObject obj)
    {
        obj.transform.position = new Vector3(obj.transform.position.x, gunHeight, obj.transform.position.z);
    }

    void GunDirection()
    {
        Vector3 ballBottom = ballObject.transform.position;
        ballBottom.y -= 0.1f;
        gunLocation.transform.LookAt(ballBottom);
    }

    bool GroundChecker()
    {
        RaycastHit hit;
        if (Physics.Raycast(ballObject.transform.position, Vector3.down, out hit, 0.01f, groundMask))
        {
            if (hit.transform.gameObject.layer == 6)
                return true;
            else
                return false;
        }

        return false;
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

    IEnumerator RespawnBall()
    {
        bubbles.SetActive(true);

        ballObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        Rigidbody rb = ballObject.GetComponent<Rigidbody>();
        rb.angularVelocity = Vector3.zero;
        rb.drag = 20;
        rb.velocity = Vector3.down;

        Vector3 waterSurfaceHit = new Vector3(ballObject.transform.position.x, 0, ballObject.transform.position.z);

        GameObject tempObject = Instantiate(tempWaterBuffer, waterSurfaceHit, Quaternion.identity);
        camera.m_Follow = tempObject.transform;
        camera.LookAt = tempObject.transform;

        yield return new WaitForSeconds(3);
        isRespawning = false;

        bubbles.SetActive(false);

        ballObject.transform.position = previousPosition;
        camera.m_Follow = ballObject.transform;
        camera.m_LookAt = ballObject.transform;
        Destroy(tempObject);

        rb.drag = 1;
    }
}
