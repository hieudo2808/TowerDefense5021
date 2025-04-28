using UnityEngine;

[CreateAssetMenu(fileName = "Turret Shop Setting")]
public class TurretSetting : ScriptableObject
{
    public GameObject TurretPrefab;
    public int TurretCost;
    public Sprite TurretShopSprite;
}
