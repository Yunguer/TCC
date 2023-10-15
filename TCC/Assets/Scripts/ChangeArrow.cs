using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetoTCC
{
    public class ChangeArrow : MonoBehaviour
    {
        private _GameController _GameController;
        private SpriteRenderer spriteRenderer;

        void Start()
        {
            _GameController = FindObjectOfType(typeof(_GameController)) as _GameController;
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            spriteRenderer.sprite = _GameController.ArrowImg[_GameController.EquipedArrowID];
        }
    }

}