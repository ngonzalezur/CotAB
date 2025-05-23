using UnityEngine;
using UnityEngine.UI;

public class ChangeManaHeroe : MonoBehaviour
{
    public Image fill;
    [SerializeField] private BaseUnit unit;

    void Start()
    {
        
    }

    void Update()
    {
        if (unit != null && fill != null)
        {
            fill.fillAmount = (float)unit.MoveCooldown / unit.MaxStamina;
        }
    }
}
