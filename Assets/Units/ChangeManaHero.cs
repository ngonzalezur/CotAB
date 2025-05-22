using UnityEngine;
using UnityEngine.UI;
public class ChangeManaHero : MonoBehaviour
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
            Debug.Log(unit.MaxStamina);
            fill.fillAmount = (float)unit.MoveCooldown / unit.MaxStamina;
            Debug.Log(fill.fillAmount);
        }
    }
}
