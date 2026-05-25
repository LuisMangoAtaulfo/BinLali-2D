using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instancia;

    [Header("Tokens")]
    public int tokensNecesarios = 20;
    private int tokensActuales = 0;

    [Header("Jefe")]
    public GameObject jefe;

    [Header("Pantallas")]
    public GameObject pantallaInicio;
    public GameObject pantallaHistoria;
    public GameObject pantallaSeleccion;
    public GameObject pantallaVictoria;
    public GameObject pantallaDerrota;

    [Header("UI")]
    public TextMeshProUGUI textoTokens;

    [Header("Seleccion")]
    public SeleccionPersonaje seleccionPersonaje;

    private int estadoPantalla = 0;

    void Awake()
    {
        instancia = this;
    }

    void Start()
    {
        pantallaInicio.SetActive(true);
        pantallaHistoria.SetActive(false);
        pantallaSeleccion.SetActive(false);
        pantallaVictoria.SetActive(false);
        pantallaDerrota.SetActive(false);

        if (jefe != null) jefe.SetActive(false);
        Time.timeScale = 0f;
        ActualizarUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (estadoPantalla == 0)
            {
                pantallaInicio.SetActive(false);
                pantallaHistoria.SetActive(true);
                estadoPantalla = 1;
            }
            else if (estadoPantalla == 1)
            {
                pantallaHistoria.SetActive(false);
                pantallaSeleccion.SetActive(true);
                estadoPantalla = 2;
            }
        }

        if (estadoPantalla == 2)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) ElegirPersonaje(1);
            if (Input.GetKeyDown(KeyCode.Alpha2)) ElegirPersonaje(2);
            if (Input.GetKeyDown(KeyCode.Alpha3)) ElegirPersonaje(3);
            if (Input.GetKeyDown(KeyCode.Alpha4)) ElegirPersonaje(4);
        }
    }

    void ElegirPersonaje(int numero)
    {
        if (seleccionPersonaje != null)
            seleccionPersonaje.SeleccionarPersonaje(numero);

        pantallaSeleccion.SetActive(false);
        Time.timeScale = 1f;
        estadoPantalla = 3;
    }

    public void AgregarToken()
    {
        tokensActuales++;
        ActualizarUI();
        if (tokensActuales >= tokensNecesarios)
            ActivarJefe();
    }

    void ActivarJefe()
    {
        if (jefe != null) jefe.SetActive(true);
        if (textoTokens != null) textoTokens.gameObject.SetActive(false);
    }

    void ActualizarUI()
    {
        if (textoTokens != null)
            textoTokens.text = "Tokens: " + tokensActuales;
    }

    public void Victoria()
    {
        pantallaVictoria.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Derrota()
    {
        pantallaDerrota.SetActive(true);
        Time.timeScale = 0f;
    }
}