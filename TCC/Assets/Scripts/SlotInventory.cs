using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetoTCC
{
    public class SlotInventory : MonoBehaviour
    {
        [SerializeField]
        private GameObject slotObject;
        public GameObject SlotObject
        {
            get
            {
                return slotObject;
            }
            set
            {
                slotObject = value;
            }
        }

        void Start()
        {

        }

        void Update()
        {

        }

        public void UseItem()
        {
            if(slotObject != null)
            {
                slotObject.SendMessage("UseItem", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
