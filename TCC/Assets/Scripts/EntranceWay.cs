using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetoTCC
{
    public class EntranceWay : MonoBehaviour
    {
        #region Comunicação Entre Scripts
        private FadeIn_FadeOut fade;
        #endregion

        [Header("Configurações Para o Transporte do Player")]
        #region Variaveis para o Transporte do Player
        [SerializeField]
        private Transform tPlayer;
        public Transform TPlayer
        {
            get
            {
                return tPlayer;
            }
            set
            {
                tPlayer = value;
            }
        }

        [SerializeField]
        private Transform destiny;
        #endregion

        public static Action OnAnimationEnded;
        void Start()
        {
            fade = FindObjectOfType(typeof(FadeIn_FadeOut)) as FadeIn_FadeOut;
        }

        public void Interaction()
        {
            StartCoroutine("TriggerEntrance");
        }

        public void OnAnimationEnd()
        {
            OnAnimationEnded.Invoke();
        }

        IEnumerator TriggerEntrance()
        {
            fade.FadeIn();
            yield return new WaitWhile(() => fade.Tinted.color.a < 0.97f);
            tPlayer.gameObject.SetActive(false);
            tPlayer.position = destiny.position;
            tPlayer.gameObject.SetActive(true);
            fade.FadeOut();
            yield return new WaitForSeconds(0.5f);
            OnAnimationEnd();
        }
    }

}
