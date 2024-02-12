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

    [SerializeField] bool isMyTurn = true;

    private void Start()
    {
        currentGun = gunObjects[0];
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A)) RotateGunToTheSide(true);
        if (Input.GetKey(KeyCode.D)) RotateGunToTheSide(false);

        if (!isMyTurn) return;
         
        WeaponAbility ability = currentGun.GetComponent<WeaponAbility>();
        ability.IdleAbility(ballObject);
    }

    void Update()
    {
        SetGunLookAtBall();
        SetWeaponOffset();

        if (!isMyTurn) return;

        if (Input.GetKeyDown(KeyCode.Q)) ChangeActiveGun(-1);
        if (Input.GetKeyDown(KeyCode.E)) ChangeActiveGun(1);
    }

    public void ConnectBallToPlayer(GameObject myBall)
    {
        ballObject = myBall;
        ball = ballObject.GetComponent<Ball>();
        ball.OnPlayerShoot += FinishPlayerTurn;
        SaveGunRelativePosition();

        foreach(GameObject gun in gunObjects)
        {
            gun.GetComponent<WeaponAbility>().ConnectToMyBall(ball);
        }
    }

    public void ChangeActiveGun(int change)
    {
        gunObjects[activeGunIndex].GetComponent<WeaponAbility>().OnWeaponDeselect();

        SaveGunRelativePosition();

        activeGunIndex += change;

        if (activeGunIndex > gunObjects.Length - 1)
            activeGunIndex = 0;

        if (activeGunIndex < 0)
            activeGunIndex = gunObjects.Length - 1;

        foreach (GameObject gun in gunObjects)
        {
            ToggleGunMeshVisibility(gun, false);
        }

        currentGun = gunObjects[activeGunIndex];
        ToggleGunMeshVisibility(currentGun, true);

        gunObjects[activeGunIndex].GetComponent<WeaponAbility>().OnWeaponSelect();
    }

    public void FireGun()
    {
        WeaponAbility ability = currentGun.GetComponent<WeaponAbility>();
        isMyTurn = false;
        ability.ShootAbility(ballObject);
    }

    void FinishPlayerTurn()
    {
        ToggleGunMeshVisibility(currentGun, false);
        currentGun.GetComponent<WeaponAbility>().OnWeaponDeselect();
    }

    public void StartPlayerTurn()
    {
        isMyTurn = true;
        ToggleGunMeshVisibility(currentGun, true);
        currentGun.GetComponent<WeaponAbility>().OnWeaponSelect();
    }

    public GunProperties GetActiveGunStats()
    {
        return gunObjects[activeGunIndex].GetComponent<GunProperties>();
    }

    public void ToggleGunMeshVisibility(GameObject weapon, bool setEnabled)
    {
        foreach(MeshRenderer mesh in weapon.GetComponentsInChildren<MeshRenderer>())
        {
            mesh.enabled = setEnabled;
        }

        gunPointer.GetComponent<MeshRenderer>().enabled = setEnabled;
    }

    void SetGunLookAtBall()
    {
        gunObjects[activeGunIndex].transform.LookAt(ballObject.transform.position);
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
        SaveGunRelativePosition();
    }
}
