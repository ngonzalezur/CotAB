using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIFeedback : MonoBehaviour
{
    [System.Serializable]
    public class Ability
    {
        public Image abilityImage;
        public float cooldown;
        public KeyCode key;
        [HideInInspector] public bool isCooldown = false;
    }

    public Ability[] abilities;

    void Start()
    {
        // Inicializar los sprites de las habilidades en 0
        foreach (var ability in abilities)
        {
            ability.abilityImage.fillAmount = 0;
        }
    }

    void Update()
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            if (Input.GetKey(abilities[i].key) && !abilities[i].isCooldown)
            {
                //StartCoroutine(HandleCooldown(i));
            }
        }
    }

    public IEnumerator HandleCooldown(int i)
    {
        abilities[i].isCooldown = true;
        abilities[i].abilityImage.fillAmount = 1;

        float elapsedTime = 0f;

        while (elapsedTime < abilities[i].cooldown)
        {
            elapsedTime += Time.deltaTime;
            abilities[i].abilityImage.fillAmount = 1 - (elapsedTime / abilities[i].cooldown);
            yield return null;
        }

        abilities[i].abilityImage.fillAmount = 0;
        abilities[i].isCooldown = false;
    }
}