using TMPro;
using UnityEngine;
public class DamageTextManager : Singleton<DamageTextManager>
{
    private Transform _dmgTextCanvas;
    private ObjectPooler _pooler;
    private TextMeshProUGUI _dmgText;
    private int poolSize = 20;

    public ObjectPooler Pooler => _pooler;
    public Transform DmgTextCanvas => _dmgTextCanvas;

    public TextMeshProUGUI DmgText
    {
        get { return _dmgText; }
        set { _dmgText = value; }
    }

    private void Awake()
    {
        _pooler = GetComponent<ObjectPooler>();
    }
}
