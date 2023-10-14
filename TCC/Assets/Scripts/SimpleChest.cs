using Cainos.LucidEditor;
using System;
using System.Collections;
using UnityEngine;

namespace ProjetoTCC
{
    public class SimpleChest : MonoBehaviour
    {
        #region Variaveis Unity
        public static Action OnAnimationEnded;
        private _GameController _GameController;
        public Animator animator;
        #endregion

        [Header("Triggers Para a Checagem")]
        #region Variaveis para Checagem
        private bool isOpened;
        private bool alreadyOpened = false;
        #endregion

        [Header("Configurações Para o Loot")]
        #region Variaveis para o Sistema de Loot
        [SerializeField]
        private GameObject[] loots;
        [SerializeField]
        private int minQnt;
        [SerializeField]
        private int maxQnt;
        #endregion

        public bool IsOpened
        {
            get { return isOpened; }
            set
            {
                isOpened = value;
                animator.SetBool("IsOpened", isOpened);
            }
        }

        public void Interaction()
        {
            if (!IsOpened && !alreadyOpened)
            {
                int coinsAmount = UnityEngine.Random.Range(minQnt, maxQnt);
                for (int i = 0; i < coinsAmount; i++)
                {
                    int rand = 0;
                    GameObject lootTemp = Instantiate(loots[rand], transform.position, transform.localRotation);
                    lootTemp.GetComponent<Rigidbody2D>().AddForce(new Vector2(UnityEngine.Random.Range(-35, 35), 200));
                }
                alreadyOpened = true;
            }
            IsOpened = true;
            GetComponent<Collider2D>().enabled = false;
        }

        public void OnAnimationEnd()
        {
            OnAnimationEnded.Invoke();
        }

    }

}