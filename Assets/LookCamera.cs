using UnityEngine;

public class LookCamera : MonoBehaviour
{
    public Camera camera;
    public void Awake()
    {
        camera = Camera.main;
    }
    void Update()
    {
        Vector3 v = camera.transform.position;
        v.y = transform.position.y;
        transform.LookAt(v);
        //transform.Rotate(camera.transform.eulerAngles.x, 0f, 0f);
        transform.eulerAngles = new Vector3(-36.473f, 0f, 0f);
    }
}
