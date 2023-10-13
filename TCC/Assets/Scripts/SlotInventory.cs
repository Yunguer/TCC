using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetoTCC
{
    public class SlotInventory : MonoBehaviour
    {
        private _GameController _GameController;
        private PainelItemInfo painelItemInfo;
        
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
        [SerializeField]
        private int slotID;

        void Start()
        {
            _GameController = FindObjectOfType(typeof(_GameController)) as _GameController;
            painelItemInfo = FindObjectOfType(typeof(PainelItemInfo)) as PainelItemInfo;
        }

        void Update()
        {

        }

        public void UseItem()
        {
            if(slotObject != null)
            {
                painelItemInfo.SlotObject = slotObject;
                painelItemInfo.SlotID = slotID;
                painelItemInfo.LoadItemInfo();
                _GameController.OpenItenInfo();
            }
        }
    }
}
