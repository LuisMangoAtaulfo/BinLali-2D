using UnityEngine;

public class JugadoraMovimiento : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;
    public float velocidadSalto = 6f;

    [Header("Limites")]
    public float limiteIzquierdo = -5f;
    public float limiteDerecho = 50f;

    private Rigidbody2D rb;
    private Animator anim;
    private bool estaEnSuelo;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (GetComponent<PersonajeAnimationController>() == null)
            gameObject.AddComponent<PersonajeAnimationController>();
    }

    void Update()
    {
        Mover();
        Saltar();
        AplicarLimites();
    }

    void Mover()
    {
        float inputX = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(inputX * velocidad, rb.linearVelocity.y);

        if (anim != null)
            anim.SetFloat("velocidadX", Mathf.Abs(inputX));

        if (inputX > 0) transform.localScale = new Vector3(2, 2, 1);
        if (inputX < 0) transform.localScale = new Vector3(-2, 2, 1);
    }

    void Saltar()
    {
        if (Input.GetButtonDown("Jump") && estaEnSuelo)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, velocidadSalto);
        }
    }

    void AplicarLimites()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, limiteIzquierdo, limiteDerecho);
        transform.position = pos;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Suelo"))
            estaEnSuelo = true;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Suelo"))
            estaEnSuelo = false;
    }
}