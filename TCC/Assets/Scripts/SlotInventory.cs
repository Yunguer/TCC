using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetoTCC
{
    public class SlotInventory : MonoBehaviour
    {
        private _GameController _GameController;
        private PainelItemInfo painelItemInfo;
        
        [SerializeField]
        private Image slotIcon;

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

        private void OnEnable()
        {
            PainelItemInfo.OnItemClicked += UseItem;
            slotIcon.enabled = false;
        }

        private void OnDisable()
        {
            PainelItemInfo.OnItemClicked -= UseItem;
        }

        public void UseItem()
        {
            if(slotIcon != null)
            {
                painelItemInfo.SlotID = slotID;
                painelItemInfo.LoadItemInfo();
                _GameController.OpenItenInfo();
            }
        }

        public void SetIcon(Sprite iconSprite)
        {
            slotIcon.enabled = true;
            slotIcon.sprite = iconSprite;
        }



        public void SetEnableIcon(bool enable)
        {
            slotIcon.enabled = enable;
        }
    }
}
