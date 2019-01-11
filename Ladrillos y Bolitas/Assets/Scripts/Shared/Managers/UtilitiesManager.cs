using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clase singleton que proporciona utilidades animaciones de FadeIn/FadeOut
/// </summary>
public class UtilitiesManager : MonoBehaviour
{
    #region PersistentSingleton

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
    #endregion PersistentSingleton

    /// <summary>
    /// Fades the specified image to the target opacity and duration.
    /// </summary>
    /// <param name="target">Target.</param>
    /// <param name="opacity">Opacity.</param>
    /// <param name="duration">Duration.</param>
    public static IEnumerator FadeImage(Image target, float duration, Color color)
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

    #region FadeSprite
    /// <summary>
    /// Empieza a hacer una rutina de FadeIn/FadeOut sobre un SpriteRenderer
    /// </summary>
    /// <param name="spriteRenderer">Sprite al que afecta</param>
    /// <param name="duration"> Duración de la animación</param>
    public void FadeInFadeOut(SpriteRenderer spriteRenderer, float duration)
    {
        StartCoroutine(FadeInFadeOutCorrutine(spriteRenderer, duration));
    }

    /// <summary>
    /// Corrutina que hace FadeIn/FadeOut
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeInFadeOutCorrutine(SpriteRenderer spriteRenderer, float duration)
    {
        Color desiredColor = spriteRenderer.color;
        desiredColor.a = 1.0f;

        //FadeIn
        StartCoroutine(FadeRoutine(spriteRenderer, duration, desiredColor));
        yield return new WaitForSeconds(duration);

        desiredColor.a = 0.0f;

        //FadeOut
        StartCoroutine(FadeRoutine(spriteRenderer, duration, desiredColor));
    }

    /// <summary>
    /// Método que realiza un Fade sobre un spriteRenderer
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

    #endregion FadeSprite

}
