using UnityEngine;
using TMPro;

public class JefeFinal : MonoBehaviour
{
    [Header("Vida")]
    public int vidaMaxima = 20;
    private int vidaActual;

    [Header("Movimiento")]
    public float amplitudMovimiento = 2f;
    public float velocidadMovimiento = 1.5f;
    public float velocidadHorizontal = 2f;
    private Vector3 posicionInicial;
    private float direccionH = 1f;
    public float limiteIzquierdo = 3f;
    public float limiteDerecho = 8f;

    [Header("Ataque")]
    public GameObject proyectilPrefab;
    public float tiempoEntreAtaques = 1.5f;
    private float timerAtaque;
    public int cantidadProyectiles = 3;

    [Header("UI")]
    public UnityEngine.UI.Slider barraVida;

    private bool muerto = false;

    void Start()
    {
        vidaActual = vidaMaxima;
        posicionInicial = transform.position;
        timerAtaque = tiempoEntreAtaques;

        if (barraVida != null)
        {
            barraVida.maxValue = vidaMaxima;
            barraVida.value = vidaMaxima;
        }
    }

    void Update()
    {
        if (muerto) return;
        Volar();
        Atacar();
    }

    void Volar()
    {
        // Movimiento vertical senoidal
        float y = posicionInicial.y + Mathf.Sin(Time.time * velocidadMovimiento) * amplitudMovimiento;

        // Movimiento horizontal de lado a lado
        transform.position += new Vector3(direccionH * velocidadHorizontal * Time.deltaTime, 0, 0);

        if (transform.position.x >= limiteDerecho) direccionH = -1f;
        if (transform.position.x <= limiteIzquierdo) direccionH = 1f;

        transform.position = new Vector3(transform.position.x, y, 0);
    }

    void Atacar()
    {
        timerAtaque -= Time.deltaTime;
        if (timerAtaque <= 0)
        {
            timerAtaque = tiempoEntreAtaques;
            LanzarRafaga();
        }
    }

    void LanzarRafaga()
    {
        if (proyectilPrefab == null) return;

        GameObject jugadora = GameObject.FindWithTag("Player");
        if (jugadora == null) return;

        // Dispara varios proyectiles en abanico
        for (int i = 0; i < cantidadProyectiles; i++)
        {
            Vector2 direccionBase = (jugadora.transform.position - transform.position).normalized;
            float angulo = (i - cantidadProyectiles / 2) * 15f;
            Vector2 direccionFinal = RotarVector(direccionBase, angulo);

            GameObject proyectil = Instantiate(proyectilPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = proyectil.GetComponent<Rigidbody2D>();
            rb.linearVelocity = direccionFinal * 5f;
            Destroy(proyectil, 4f);
        }
    }

    Vector2 RotarVector(Vector2 v, float grados)
    {
        float rad = grados * Mathf.Deg2Rad;
        return new Vector2(
            v.x * Mathf.Cos(rad) - v.y * Mathf.Sin(rad),
            v.x * Mathf.Sin(rad) + v.y * Mathf.Cos(rad)
        );
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Token"))
        {
            Destroy(col.gameObject);
            RecibirDanio(1);
        }
    }

    void RecibirDanio(int danio)
    {
        vidaActual -= danio;

        if (barraVida != null)
            barraVida.value = vidaActual;

        if (vidaActual <= 0 && !muerto)
            Morir();
    }

    void Morir()
    {
        muerto = true;
        if (GameManager.instancia != null)
            GameManager.instancia.Victoria();
        Destroy(gameObject, 0.5f);
    }
}