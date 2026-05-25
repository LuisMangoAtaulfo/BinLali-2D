using UnityEngine;

public class SeleccionPersonaje : MonoBehaviour
{
    [Header("Sprites iniciales de cada personaje")]
    public Sprite spriteNatalia;
    public Sprite spriteSusy;
    public Sprite spriteNani;
    public Sprite spriteSopas;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GameObject.FindWithTag("Player").GetComponent<SpriteRenderer>();
    }

    public void SeleccionarPersonaje(int numero)
    {
        switch (numero)
        {
            case 1: spriteRenderer.sprite = spriteNatalia; break;
            case 2: spriteRenderer.sprite = spriteSusy; break;
            case 3: spriteRenderer.sprite = spriteNani; break;
            case 4: spriteRenderer.sprite = spriteSopas; break;
        }
    }
}