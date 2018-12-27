using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace AcesMultiverse
{
    /// <summary>
    /// Clase que inicia las cartelas del principio del juego
    /// </summary>
    public class InitCanvas : MonoBehaviour
    {
        [Header("Logos")]
        public List<Sprite> Cartelas;
        public Image CartelaImage;

        [Header("Times")]
        public float TimeCartelas;

        IEnumerator Start()
        {
            foreach (Sprite cartela in this.Cartelas)
            {
                this.CartelaImage.sprite = cartela;
                yield return new WaitForSeconds(this.TimeCartelas);
            }
        }
    }
}