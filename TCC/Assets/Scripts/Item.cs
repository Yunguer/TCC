using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetoTCC
{
    public class Item : MonoBehaviour
    {
        [SerializeField]
        private int itemID;

        void Start()
        {

        }

        public void UseItem()
        {
            print("Este item " + itemID + " foi utilizado");
        }
    }
}
