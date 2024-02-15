using UnityEngine;

/* 
 * Manages the player's guns.
 * Handles the logic of rotating guns, swapping guns. 
 * Also makes sure the gun properly follows and looks at the ball.
 */

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
        ability.PassiveGunAbility(ballObject);
    }

    void Update()
    {
        SetGunLookAtBall();
        SetWeaponOffset();

        if (!isMyTurn) return;

        if (Input.GetKeyDown(KeyCode.Q)) ChangeActiveGun(-1);
        if (Input.GetKeyDown(KeyCode.E)) ChangeActiveGun(1);
    }

    //Get my ball information, send it over to the guns.
    //Probably a shitty solution.
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

    //Set what gun should be the next one. It's either +1 or -1
    public void ChangeActiveGun(int change)
    {
        gunObjects[activeGunIndex].GetComponent<WeaponAbility>().OnWeaponDeselect(); //Tell weapon it was deselected.

        SaveGunRelativePosition();

        activeGunIndex += change;

        if (activeGunIndex > gunObjects.Length - 1)
            activeGunIndex = 0;

        if (activeGunIndex < 0)
            activeGunIndex = gunObjects.Length - 1;

        
        foreach (GameObject gun in gunObjects) //Disable all guns (visibily)
        {
            ToggleGunMeshVisibility(gun, false);
        }

        currentGun = gunObjects[activeGunIndex];
        ToggleGunMeshVisibility(currentGun, true); //Enable visibility of chosen weapon.

        gunObjects[activeGunIndex].GetComponent<WeaponAbility>().OnWeaponSelect(); //Tell weapon it was selected.
    }

    //Fire the current gun based on current gun's 
    public void FireGun()
    {
        WeaponAbility ability = currentGun.GetComponent<WeaponAbility>();
        isMyTurn = false;
        ability.ActiveGunAbility(ballObject);
    }

    //end my turn.
    void FinishPlayerTurn()
    {
        ToggleGunMeshVisibility(currentGun, false);
        currentGun.GetComponent<WeaponAbility>().OnWeaponDeselect();
    }

    //start my turn.
    public void StartPlayerTurn()
    {
        isMyTurn = true;
        ToggleGunMeshVisibility(currentGun, true);
        currentGun.GetComponent<WeaponAbility>().OnWeaponSelect();
    }

    //Get active gun's stats. Mainly used for its shoot power.
    public GunProperties GetActiveGunStats()
    {
        return gunObjects[activeGunIndex].GetComponent<GunProperties>();
    }

    //Just enable or disable a weapon's meshes.
    public void ToggleGunMeshVisibility(GameObject weapon, bool setEnabled)
    {
        foreach(MeshRenderer mesh in weapon.GetComponentsInChildren<MeshRenderer>())
        {
            mesh.enabled = setEnabled;
        }

        gunPointer.GetComponent<MeshRenderer>().enabled = setEnabled;
    }

    //Lock the gun's rotation onto the ball.
    void SetGunLookAtBall()
    {
        gunObjects[activeGunIndex].transform.LookAt(ballObject.transform.position);
    }

    //Save the offset of the gun in relation to the ball.
    public void SaveGunRelativePosition()
    {
        gunOffset = ballObject.transform.position - transform.position;
    }

    //Set the weapon & the gun pointer's offset based on the saved value.
    void SetWeaponOffset()
    {
        transform.position = ballObject.transform.position - gunOffset;
        gunObjects[activeGunIndex].transform.position = transform.position + new Vector3(0, gunObjects[activeGunIndex].GetComponent<GunProperties>().properties.GunHeight, 0);
        gunPointer.transform.position = ballObject.transform.position + new Vector3(gunOffset.x, -gunOffset.y, gunOffset.z) + new Vector3(0, gunObjects[activeGunIndex].GetComponent<GunProperties>().properties.GunHeight, 0);
    }

    //Rotate the gun around the player's ball.
    void RotateGunToTheSide(bool left)
    {
        int dir = left ? 1 : -1;
        transform.RotateAround(ballObject.transform.position, Vector3.up, dir * rotationSpeed * Time.deltaTime);
        SaveGunRelativePosition();
    }
}
