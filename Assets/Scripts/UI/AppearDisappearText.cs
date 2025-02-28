using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearDisappearText : MonoBehaviour
{
    [SerializeField] private GameObject canvasToHide; // Canvas que se oculta al entrar
    [SerializeField] private GameObject canvasToShow; // Canvas que aparece al entrar

    private void Start()
    {
        // Asegurar que el canvas inicial esté en el estado correcto
        if (canvasToShow != null) canvasToShow.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (canvasToHide != null) canvasToHide.SetActive(false);
            if (canvasToShow != null) canvasToShow.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (canvasToShow != null) canvasToShow.SetActive(false);
        }
    }
}
