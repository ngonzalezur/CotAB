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
                StartCoroutine(HandleCooldown(abilities[i]));
            }
        }
    }

    IEnumerator HandleCooldown(Ability ability)
    {
        ability.isCooldown = true;
        ability.abilityImage.fillAmount = 1;

        float elapsedTime = 0f;

        while (elapsedTime < ability.cooldown)
        {
            elapsedTime += Time.deltaTime;
            ability.abilityImage.fillAmount = 1 - (elapsedTime / ability.cooldown);
            yield return null;
        }

        ability.abilityImage.fillAmount = 0;
        ability.isCooldown = false;
    }
}