using System.Collections;
using System.Collections.Generic;
using ActionGame.Utils;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        GameInput.Instance.OnPlayerAttack += GameInput_OnPlayerAttack;
        // SortingGroup sortingGroup = GetComponent<SortingGroup>();
        // sortingGroup.sortingOrder = 1;
    }

    private void GameInput_OnPlayerAttack(object sender, System.EventArgs e)
    {
        Debug.Log("Player attack called");
        ActiveWeapon.Instance.Attack();
    }
    public Vector3 GetPlayerScreenPosition()
    {
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }
}
