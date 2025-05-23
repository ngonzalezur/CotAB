using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
public class CharacterSelect : MonoBehaviour
{
    public Image[] selectionBoxes;
    public BaseUnit[] prefabs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var img in this.selectionBoxes) { 
            img.gameObject.SetActive(false);
        }

        this.SelectOne(0);
    }

    // Update is called once per frame
    public void SelectOne(int index)
    {
        foreach (var img in this.selectionBoxes)
        {
            img.gameObject.SetActive(false);
        }
        this.selectionBoxes[index].gameObject.SetActive(true);
        //CharacterStorage.PlayerOne = this.prefabs[index];
        SeleccionPersonaje.Instance.player1 = this.prefabs[index];
    }
    public void SelectTwo(int index)
    {
        foreach (var img in this.selectionBoxes)
        {
            img.gameObject.SetActive(false);
        }
        this.selectionBoxes[index].gameObject.SetActive(true);
        //CharacterStorage.PlayerOne = this.prefabs[index];
        SeleccionPersonaje.Instance.player2 = this.prefabs[index];
    }
}
