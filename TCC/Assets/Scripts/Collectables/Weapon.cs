using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace ProjetoTCC
{
    public class Weapon : MonoBehaviour
    {
        #region Comunicação Entre Scripts
        private _GameController _GameController;
        #endregion

        [Header("Objeto para Coletar")]
        #region Objeto a coletar
        [SerializeField]
        private GameObject colectableWeapon;
        #endregion

        #region Variavel Collider do Objeto
        private BoxCollider2D boxCollider2D;
        private bool colected;
        #endregion

        private void Start()
        {
            InvokeActivateCollider();
            _GameController = FindObjectOfType(typeof(_GameController)) as _GameController;
            boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
        }

        public void Collect()
        {
            if(!colected)
            {
                _GameController.ColectItem(colectableWeapon);
                Destroy(this.gameObject);
                colected = true;
            }
        }

        public void ActivateCollider()
        {
            if (boxCollider2D != null)
            {
                boxCollider2D.enabled = true;
            }
        }

        public void InvokeActivateCollider()
        {
            Invoke(nameof(ActivateCollider), 2.2f);
        }
    }

}