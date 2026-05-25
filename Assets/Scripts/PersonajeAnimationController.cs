using UnityEngine;

[DefaultExecutionOrder(100)]
[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public class PersonajeAnimationController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void LateUpdate()
    {
        if (SeleccionPersonaje.Instancia == null) return;
        if (SeleccionPersonaje.PersonajeActual == 0) return;

        var data = SeleccionPersonaje.Instancia.GetPersonajeData(SeleccionPersonaje.PersonajeActual);
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Idle"))
        {
            var sprites = data.spritesIdle;
            if (sprites == null || sprites.Length == 0) return;
            int frame = Mathf.FloorToInt(stateInfo.normalizedTime * sprites.Length) % sprites.Length;
            spriteRenderer.sprite = sprites[frame];
        }
        else if (stateInfo.IsName("Walk"))
        {
            var sprites = data.spritesWalk;
            if (sprites == null || sprites.Length == 0) return;
            int frame = Mathf.FloorToInt(stateInfo.normalizedTime * sprites.Length) % sprites.Length;
            spriteRenderer.sprite = sprites[frame];
        }
        else if (stateInfo.IsName("Disparo"))
        {
            if (data.spriteDisparo != null)
                spriteRenderer.sprite = data.spriteDisparo;
        }
    }
}
