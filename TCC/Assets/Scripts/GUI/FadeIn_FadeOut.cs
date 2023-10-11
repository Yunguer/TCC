using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetoTCC
{
    public class FadeIn_FadeOut : MonoBehaviour
    {
        [Header("Configurações Para a Animação FadeIn/FadeOut")]
        #region Variaveis para animação de FadeIn FadeOut
        [SerializeField]
        private GameObject tintedPainel;
        [SerializeField]
        private Image tinted;
        public Image Tinted => tinted;
        [SerializeField]
        private Color[] transitionColor;
        [SerializeField]
        private float step;
        #endregion

        private void Start()
        {
            FadeOut();
        }

        public void FadeIn()
        {
            tintedPainel.SetActive(true);
            StartCoroutine(nameof(FadeI));
        }

        public void FadeOut()
        {
            StartCoroutine(nameof(FadeO));
        }

        IEnumerator FadeI()
        {
            for(float i = 0; i <= 1; i+= step)
            {
                tinted.color = Color.Lerp(transitionColor[0], transitionColor[1], i);
                yield return new WaitForEndOfFrame();
            }
        }

        IEnumerator FadeO()
        {
            yield return new WaitForSeconds(0.5f);
            for (float i = 0; i <= 1; i += step)
            {
                tinted.color = Color.Lerp(transitionColor[1], transitionColor[0], i);
                yield return new WaitForEndOfFrame();
            }

            tintedPainel.SetActive(false);
        }
    }
}
