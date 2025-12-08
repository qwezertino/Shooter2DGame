using UnityEngine;
using Weapons.Enums;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Main settings")]
    public string weaponName;
    public float attackRate = 0.5f;
    public FireMode fireMode = FireMode.Single;

    [Header("Damage and Range")]
    public int damage = 10;
    public float range = 100f;

    [Header("Visuals")]
    // public Sprite weaponSprite;
    public float muzzleFlashDuration = 0.02f;

    // [Header("Sounds")]
    // public AudioClip shootSound;
    // public AudioClip reloadSound;
}
