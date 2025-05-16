using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pausa : MonoBehaviour
{
    public GameObject menu;

    public void Pausar()
    {
        menu.SetActive(!menu.activeSelf);

    }
}
