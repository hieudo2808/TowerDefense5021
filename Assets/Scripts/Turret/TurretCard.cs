using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurretCard : MonoBehaviour
{
    public static Action<TurretSetting> OnPlaceTurret;

    [SerializeField]
    private Image turretImage;
    [SerializeField]
    private TextMeshProUGUI turretCost;

    public TurretSetting TurretLoaded { get; set; }

    public void SetupTurretButton(TurretSetting turretSetting)
    {
        TurretLoaded = turretSetting;
        turretImage.sprite = turretSetting.TurretShopSprite;
        turretCost.text = turretSetting.TurretCost.ToString();
    }

    public void PlaceTurret()
    {
        if (CurrencySystem.Instance.TotalCoins >= TurretLoaded.TurretCost)
        {
            CurrencySystem.Instance.RemoveCoins(TurretLoaded.TurretCost);
            UIManager.Instance.CloseTurretShopPanel();
            OnPlaceTurret?.Invoke(TurretLoaded);
        }
    }    
}
