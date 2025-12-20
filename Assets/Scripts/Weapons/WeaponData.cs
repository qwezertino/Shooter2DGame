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

    [Header("Spread Settings")]
    [Tooltip("Minimum spread in degrees")]
    public float minSpread = 0f;
    [Tooltip("Spread increase per shot in degrees")]
    public float spreadIncreasePerShot = 2f;
    [Tooltip("Maximum spread in degrees")]
    public float maxSpread = 10f;
    [Tooltip("Accuracy recovery rate (degrees per second)")]
    public float spreadRecoveryRate = 5f;

    // [Header("Sounds")]
    // public AudioClip shootSound;
    // public AudioClip reloadSound;
}
