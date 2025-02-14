using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPrincipal : MonoBehaviour
{
    public string nombreEscenaJuego = "Main Tin"; // Reemplázalo con el nombre exacto de la escena
    public GameObject panelCreditos; // Si usas un panel en vez de cambiar de escena
    public AudioSource musicaFondo;
    private bool musicaActiva = true;

    void Start()
    {
        if (musicaFondo != null && !musicaFondo.isPlaying)
        {
            musicaFondo.Play(); // Reproduce la música al inicio
        }
    }

    public void Jugar()
    {
        SceneManager.LoadScene(nombreEscenaJuego);
    }

    public void MostrarCreditos()
    {
        if (panelCreditos != null)
        {
            panelCreditos.SetActive(!panelCreditos.activeSelf);
        }
    }

    public void MutearMusica()
    {
        if (musicaFondo != null)
        {
            musicaActiva = !musicaActiva;
            musicaFondo.mute = !musicaActiva;
        }
    }

    public void Salir()
    {
        Application.Quit();
    }
}