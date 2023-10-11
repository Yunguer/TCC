using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace ProjetoTCC
{
    public class Coin : MonoBehaviour
    {
        #region Comunicação Entre Scripts
        private _GameController _GameController;
        #endregion

        [Header("Valor para a Moeda")]
        #region Valor da Moeda
        [SerializeField]
        private int value;
        #endregion

        #region Variavel Collider do Objeto
        private CircleCollider2D circleCollider2D;
        #endregion

        private void Start()
        {
            _GameController = FindObjectOfType(typeof(_GameController)) as _GameController;
            circleCollider2D = gameObject.GetComponent<CircleCollider2D>();
        }

        private void OnEnable()
        {
            CoinAnimationHandler.OnAnimationEnded += InvokeActivateCollider;
        }

        private void OnDisable()
        {
            CoinAnimationHandler.OnAnimationEnded -= InvokeActivateCollider;
        }

        public void Collect()
        {
            _GameController.Gold += value;
            Destroy(this.gameObject);
        }

        public void ActivateCollider()
        {
            if(circleCollider2D != null)
            {
                circleCollider2D.enabled = true;
            }
        }

        public void InvokeActivateCollider()
        {
            Invoke(nameof(ActivateCollider), 1.2f);
        }

    }

}