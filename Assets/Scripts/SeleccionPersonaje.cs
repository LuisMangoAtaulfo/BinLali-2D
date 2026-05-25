using UnityEngine;

public class SeleccionPersonaje : MonoBehaviour
{
    [System.Serializable]
    public class PersonajeData
    {
        [Header("Idle (6 frames)")]
        public Sprite[] spritesIdle;

        [Header("Walk (6 frames)")]
        public Sprite[] spritesWalk;

        [Header("Disparo (1 frame)")]
        public Sprite spriteDisparo;
    }

    [Header("Personajes")]
    public PersonajeData natalia;
    public PersonajeData susy;
    public PersonajeData nani;
    public PersonajeData sopas;

    public static int PersonajeActual { get; private set; } = 0;
    public static SeleccionPersonaje Instancia { get; private set; }

    void Awake()
    {
        Instancia = this;
    }

    public void SeleccionarPersonaje(int numero)
    {
        PersonajeActual = numero;
    }

    public PersonajeData GetPersonajeData(int numero)
    {
        switch (numero)
        {
            case 1: return natalia;
            case 2: return sopas;
            case 3: return susy;
            case 4: return nani;
            default: return susy;
        }
    }
}
