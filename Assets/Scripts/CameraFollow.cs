using UnityEngine;

public class CamaraFollow : MonoBehaviour
{
    public Transform objetivo;
    public float velocidadSuavizado = 5f;
    public Vector2 limiteMin;
    public Vector2 limiteMax;

    void LateUpdate()
    {
        if (objetivo == null) return;

        Vector3 posObjetivo = new Vector3(
            objetivo.position.x,
            transform.position.y,
            transform.position.z
        );

        posObjetivo.x = Mathf.Clamp(posObjetivo.x, limiteMin.x, limiteMax.x);

        transform.position = Vector3.Lerp(
            transform.position,
            posObjetivo,
            velocidadSuavizado * Time.deltaTime
        );
    }
}