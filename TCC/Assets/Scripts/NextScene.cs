using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjetoTCC
{
    public class NextScene : MonoBehaviour
    {
        [SerializeField]
        private Canvas hudCanvas;
        [SerializeField]
        private string destinyScene;
        public static Action OnAnimationEnded;
        private FadeIn_FadeOut fade;

        private void Start()
        {
            fade = FindObjectOfType(typeof(FadeIn_FadeOut)) as FadeIn_FadeOut;    
        }

        public void Interaction()
        {
            StartCoroutine("ChangeScene");
            hudCanvas.worldCamera = Camera.current;
        }
        public void OnAnimationEnd()
        {
            OnAnimationEnded.Invoke();
        }

        IEnumerator ChangeScene()
        {
            fade.FadeIn();
            yield return new WaitWhile(() => fade.Tinted.color.a < 0.99f);
            SceneManager.LoadScene(destinyScene);
            OnAnimationEnd();
        }
    }
}
