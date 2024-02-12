using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Weapon")]
public class GunStats : ScriptableObject
{
    [SerializeField] private int gunStrength;
    public int GunStrength { get { return gunStrength; } }

    [SerializeField] float gunHeight;
    public float GunHeight { get {  return gunHeight; } }

    [SerializeField] private AudioClip gunSound;
    public AudioClip GunSound { get {  return gunSound; } }
}
