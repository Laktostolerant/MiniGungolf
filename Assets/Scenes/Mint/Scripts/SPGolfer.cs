using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPGolfer : MonoBehaviour
{
    public Transform Pivot;
    public Transform Ball;
    public Transform Pointer;

    private SPGunController[] Weapons;
    private SPGunController CurrentWeapon;
    private int SelectedWeaponIndex;

    // Player Movement.
    // ================
    // If this flag is true, it means the player is able to control inputs such as the rotation of the the weaponry and
    // switching of said weaponry. If it's false, it means the player is at that point in time unable to do the above.
    // It is a public variable so the gun controller itself can tell the player when it's done firing, as there are both
    // continous and single firing weapons, and the player needs to be able to control the fire direction of continous
    // firing weaponry. It is true by default, as the player needs to be able to take the first shot before it's changed.

    [HideInInspector]
    public bool canPlayerMove = true;

    private void Start()
    {
        SPGunController[] TempArray = new SPGunController[Pivot.childCount];

        for(int i = 0; i < Pivot.childCount; i++)
        {
            TempArray[i] = Pivot.GetChild(i).GetComponent<SPGunController>();
        }

        Weapons = TempArray;
    }

    private void Update()
    {
        Pivot.Rotate(new Vector3(0.0f, Input.GetAxis("Horizontal") * 120.0f * Time.deltaTime, 0.0f));
        Pointer.rotation = Quaternion.Euler(90.0f, Pivot.eulerAngles.y, 0.0f);

        if(canPlayerMove)
        {
            if(Input.GetKeyUp(KeyCode.Space))
            {
                Ball.transform.parent = null;
                Weapons[0].Fire(Ball.GetComponent<Rigidbody>());
                canPlayerMove = false;
            }
        }
        else
        {
            transform.position = Ball.position;
        }

        if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.E))
        {
            var direction = (Input.GetKeyUp(KeyCode.Q)) ? -1 : 1;
            SelectedWeaponIndex = (SelectedWeaponIndex + direction + Weapons.Length) % Weapons.Length;
            CurrentWeapon = Weapons[SelectedWeaponIndex];
            foreach (var Weapon in Weapons) Weapon.transform.GetChild(0).gameObject.SetActive(false);
            CurrentWeapon.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
