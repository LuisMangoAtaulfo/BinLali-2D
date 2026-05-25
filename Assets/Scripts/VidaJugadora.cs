using UnityEngine;
using UnityEngine.UI;

public class VidaJugadora : MonoBehaviour
{
    [Header("Vida")]
    public int vidaMaxima = 5;
    private int vidaActual;

    [Header("UI")]
    public Slider barraVida;

    private bool invencible = false;
    public float tiempoInvencible = 1.5f;

    void Start()
    {
        vidaActual = vidaMaxima;

        if (barraVida != null)
        {
            barraVida.maxValue = vidaMaxima;
            barraVida.value = vidaMaxima;
        }
    }

    public void RecibirDanio(int danio)
    {
        if (invencible) return;

        vidaActual -= danio;

        if (barraVida != null)
            barraVida.value = vidaActual;

        if (vidaActual <= 0)
            Morir();
        else
            StartCoroutine(TiempoInvencible());
    }

    System.Collections.IEnumerator TiempoInvencible()
    {
        invencible = true;
        yield return new WaitForSeconds(tiempoInvencible);
        invencible = false;
    }

    void Morir()
    {
        if(GameManager.instancia != null)
            GameManager.instancia.Derrota();
        gameObject.SetActive(false);
    }
}