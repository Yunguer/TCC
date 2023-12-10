using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjetoTCC
{
    public class FinishGame : MonoBehaviour
    {
        private _GameController _GameController;
        public static Action OnAnimationEnded;

        private void Start()
        {
            _GameController = FindObjectOfType(typeof(_GameController)) as _GameController;
        }

        public void Interaction()
        {
            _GameController.ChangeState(GameState.COMPLETED);
            _GameController.CompleteGamePainel.SetActive(true);
        }
        public void OnAnimationEnd()
        {
            OnAnimationEnded.Invoke();
        }
    }
}
