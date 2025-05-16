using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ChangeHealthHero2 : MonoBehaviour
{
    public Image fill;
    private BaseUnit unit;

    void Start()
    {
       // unit = GetComponent<BaseUnit>();
       unit = GameObject.FindObjectsByType<BaseUnit>(FindObjectsSortMode.None)
                                     .FirstOrDefault(u => u.isPersistentHero);
    }

    void Update()
    {
        if (unit != null && fill != null)
        {
            fill.fillAmount = (float)unit.Health / unit.MaxHealth;
        }
    }
}

