using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetoTCC
{
    public class CoinAnimationHandler : MonoBehaviour
    {
        #region Variaveis Para Controle de Animação da Moeda
        public static Action OnAnimationEnded;
        private bool isFirstAnimation = true;
        #endregion

        public void OnAnimationEnd()
        {
            if(isFirstAnimation)
            {
                OnAnimationEnded.Invoke();
                isFirstAnimation = false;
            }
        }
    }
}
