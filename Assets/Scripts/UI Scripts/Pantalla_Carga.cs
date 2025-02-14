using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PantallaCarga : MonoBehaviour
{
    public Image splashArt; // Arrastra la imagen del splash desde el Inspector
    public float tiempoEspera = 5f; // Ajusta entre 5 y 10 segundos

    void Start()
    {
        StartCoroutine(CargarMenu());
    }

    IEnumerator CargarMenu()
    {
        yield return new WaitForSeconds(tiempoEspera);
        SceneManager.LoadScene("UIMenu"); // Reemplaza con el nombre exacto de la escena
    }
}