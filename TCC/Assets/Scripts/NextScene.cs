using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjetoTCC
{
    public class NextScene : MonoBehaviour
    {
        private PlayerScript playerScript;

        [SerializeField]
        private float x, y, z;
        [SerializeField]
        private Canvas hudCanvas;
        [SerializeField]
        private string destinyScene;
        public static Action OnAnimationEnded;
        private FadeIn_FadeOut fade;

        private void Start()
        {
            fade = FindObjectOfType(typeof(FadeIn_FadeOut)) as FadeIn_FadeOut;
            playerScript = FindObjectOfType(typeof(PlayerScript)) as PlayerScript;
        }

        public void Interaction()
        {
            StartCoroutine("ChangeScene");
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
            playerScript.transform.position = new Vector3(x, y, z);
            OnAnimationEnd();
        }
    }
}
