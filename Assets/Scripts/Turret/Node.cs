using System;
using UnityEngine;

public class Node : MonoBehaviour
{
    public static Action<Node> OnNodeSelected;
    public static Action OnTurretSold;

    [SerializeField] private GameObject attackRangeSprite;

    private Turret Turret { get; set; }
    private float _rangeSize;
    private Vector3 _rangeOriginalScale;

    void Start()
    {
        _rangeSize = attackRangeSprite.GetComponent<SpriteRenderer>().bounds.size.y;
        _rangeOriginalScale = attackRangeSprite.transform.localScale;
        attackRangeSprite.SetActive(false);
    }

    public void SetTurret(Turret turret)
    {
        Turret = turret;
    }

    public bool IsEmpty()
    {
        return Turret == null;
    }

    public void CloseAttackRangeSprite()
    {
        attackRangeSprite.SetActive(false);
    }

    public void SelectTurret()
    {
        OnNodeSelected?.Invoke(this);
        if (!IsEmpty())
            ShowTurretInfo();
    }

    public void SellTurret()
    {
        if (!IsEmpty())
        {
            Destroy(Turret.gameObject);
            Turret = null;
            attackRangeSprite.SetActive(false);
            OnTurretSold?.Invoke();
        }
    }

    public void ShowTurretInfo()
    {
        // Show turret info
    }
}
