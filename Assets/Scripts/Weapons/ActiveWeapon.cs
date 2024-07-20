using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    public static ActiveWeapon Instance { get; private set; }
    [SerializeField] private Pistol _pistol;

    private void Awake()
    {
        Instance = this;
    }
    // private void Update()
    // {
    //     FollowMousePosition();
    // }

    public Pistol GetActiveWeapon()
    {
        return _pistol;
    }

    // public void FollowMousePosition()
    // {
    //     Vector3 mousePos = GameInput.Instance.GetMousePosition();
    //     Vector3 playerPos = Player.Instance.GetPlayerScreenPosition();

    //     if (mousePos.x < playerPos.x)
    //     {
    //         transform.rotation = Quaternion.Euler(0, 180, 0);
    //     }
    //     else
    //     {
    //         transform.rotation = Quaternion.Euler(0, 0, 0);
    //     }
    //  }
}
