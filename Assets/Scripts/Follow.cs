using UnityEngine;

public class Follow : MonoBehaviour
{
    // Kamera eğimi sürekli 0 a setlenerek topla beraber dönmesini engelledim.
    void Update()
    {
        transform.eulerAngles = Vector3.zero;
    }
}
