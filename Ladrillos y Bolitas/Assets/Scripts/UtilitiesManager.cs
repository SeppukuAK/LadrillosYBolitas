using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilitiesManager : MonoBehaviour
{
    public static UtilitiesManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void FadeInFadeOut(SpriteRenderer spriteRenderer, float duration)
    {
        StartCoroutine(FadeInFadeOutCorrutine(spriteRenderer, duration));
    }

    /// <summary>
    /// Corrutina para la animacion de la alerta
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeInFadeOutCorrutine(SpriteRenderer spriteRenderer, float duration)
    {
        Color desiredColor = spriteRenderer.color;
        desiredColor.a = 1.0f;

        StartCoroutine(FadeRoutine(spriteRenderer, duration, desiredColor));//Transparent Color    
        yield return new WaitForSeconds(duration);

        desiredColor.a = 0.0f;

        StartCoroutine(FadeRoutine(spriteRenderer, duration, desiredColor));//Transparent Color        
    }

    /// <summary>
    /// Método que realiza una animación intermitente del alertZone
    /// </summary>
    /// <param name="target">Target.</param>
    /// <param name="duration">Duration.</param>
    /// <param name="color">Transparent color.</param>
    private IEnumerator FadeRoutine(SpriteRenderer target, float duration, Color color)
    {
        if (target == null)
            yield break;

        float alpha = target.color.a;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / duration)
        {
            if (target == null)
                yield break;
            Color newColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(alpha, color.a, t));
            target.color = newColor;
            yield return null;
        }
        target.color = color;
    }
}
