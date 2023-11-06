using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetoTCC
{
    public class Item : MonoBehaviour
    {
        private _GameController _GameController;
        [SerializeField]
        private string itemID;
        public string ItemID
        {
            get
            {
                return itemID;
            }
            set
            {
                itemID = value;
            }
        }

        void Start()
        {
            _GameController = FindObjectOfType(typeof(_GameController)) as _GameController;
        }

        public void UseItem()
        {
            _GameController.UseItemWeapon(itemID);
        }
    }
}
