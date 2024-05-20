using UnityEngine;

public class ME_Lightning_Projectile : ME_Projectile
{
    public LineRenderer lineRenderer;

    // how many points in line
    private int points = 16;
    private float amplitude = 0.4f;
    private float range = 6f;
    private float frequency = 128f;

    // Start is called before the first frame update
    void Start()
    {
        // relative scaling
        lineRenderer.startWidth = 0.1f * transform.localScale.y;
        lineRenderer.endWidth = 0.1f * transform.localScale.y;
        StartCoroutine(DealContinuousAOEDamage());
    }

    // Update is called once per frame
    void Update()
    {
        Draw();
        RotateProjectile();
    }

    void Draw()
    {
        lineRenderer.positionCount = points;
        float tau = 2 * Mathf.PI;

        for (int i = 0; i < points; i++)
        {
            float percent = (float)i / (points-1);
            float x = Mathf.Lerp(0, range, percent);
            float y = SineCalculation(amplitude, tau, frequency, x) +
            SineCalculation(amplitude / 2, tau, frequency / 2, x) +
            SineCalculation(amplitude / 2, tau, frequency / 3, x);

            lineRenderer.SetPosition(i, new Vector2(x, y));
        }
    }

    private float SineCalculation(float A, float tau, float f, float progress)
    {
        return A * (Mathf.Sin(tau * f * Time.time * progress));
    }
}
