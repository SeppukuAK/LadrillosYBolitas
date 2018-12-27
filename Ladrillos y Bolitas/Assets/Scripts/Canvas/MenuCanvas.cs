using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

namespace AcesMultiverse
{
    /// <summary>
    /// Clase que controla todos los canvas
    /// </summary>
    public class MenuCanvas : MonoBehaviour
    {
        //Imagen de fundido
        public Image FaderImage;

        //Singleton
        public static MenuCanvas Instance { get; private set; }

        //Tiempo de fundido 
        public float FadeTime = 1f;

        void Start()
        {
            Instance = this;

            if (this.FaderImage)
                this.Fade(0, " ", true);
        }

        /// <summary>
        /// Metodo para iniciar un nivel con opciones
        /// </summary>
        /// <param name="level"></param>
        /// <param name="loading"></param>
        public void PlayLevelOptions(string level, bool loading = true)
        {
            this.Fade(1, level, loading);
        }

        /// <summary>
        /// Metodo para hacer un fundido en la pantalla
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="nameLevel"></param>
        /// <param name="loading"></param>
        private void Fade(int alpha, string nameLevel, bool loading)
        {
            this.FaderImage.gameObject.SetActive(true);
            StartCoroutine(FadeImage(this.FaderImage, this.FadeTime, (alpha == 1) ? Color.black : new Color(0, 0, 0, 0)));//Transparent Color

            if (alpha == 1)
                StartCoroutine(this.PlayLevelCo(this.FadeTime, nameLevel, loading));            
        }

        /// <summary>
        /// Metodo para cargar un nivel pasado un tiempo determinado
        /// </summary>
        /// <param name="time"></param>
        /// <param name="nameLevel"></param>
        /// <param name="loading"></param>
        /// <returns></returns>
        private IEnumerator PlayLevelCo(float time, string nameLevel, bool loading)
        {
            yield return new WaitForSeconds(time);
            SceneManager.LoadScene(nameLevel);
        }

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
    }
}