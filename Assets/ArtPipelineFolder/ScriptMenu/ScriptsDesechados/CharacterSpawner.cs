using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instantiate(CharacterStorage.PlayerOne);
        Instantiate(CharacterStorage.PlayerTwo);
        Destroy(this.gameObject);
    }
}
