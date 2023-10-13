using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetoTCC
{
    public class SlotInventory : MonoBehaviour
    {
        private _GameController _GameController;
        
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
            _GameController = FindObjectOfType(typeof(_GameController)) as _GameController;
        }

        void Update()
        {

        }

        public void UseItem()
        {
            if(slotObject != null)
            {
                //slotObject.SendMessage("UseItem", SendMessageOptions.DontRequireReceiver);
                _GameController.OpenItenInfo();
            }
        }
    }
}
