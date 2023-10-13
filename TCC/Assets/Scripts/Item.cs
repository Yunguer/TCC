using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetoTCC
{
    public class Item : MonoBehaviour
    {
        private _GameController _GameController;
        [SerializeField]
        private int itemID;
        public int ItemID => itemID;

        void Start()
        {
            _GameController = FindObjectOfType(typeof(_GameController)) as _GameController;
        }

        public void UseItem()
        {
            print(itemID);
            _GameController.UseItemWeapon(itemID);
        }
    }
}
