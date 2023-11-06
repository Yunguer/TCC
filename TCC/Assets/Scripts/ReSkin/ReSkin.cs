using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjetoTCC
{
    public class ReSkin : MonoBehaviour
    {
        #region Comunicação Entre Scripts
        private _GameController _GameController;
        #endregion

        [Header("Configuração para Manter Informações Entre Cenas")]
        #region Variaveis para Manter Informações Entre Cenas
        [SerializeField]
        private bool isPlayer;
        #endregion

        [Header("Configuração para a ReSkin")]
        #region Variaveis para a ReSkin
        private SpriteRenderer sRender;
        private Sprite[] sprites;
        [SerializeField]
        private string spriteSheetName;
        private string loadedSpriteSheetName;
        private Dictionary<string, Sprite> spriteSheet;
        #endregion;

        void Start()
        {
            _GameController = FindObjectOfType(typeof(_GameController)) as _GameController;

            if (isPlayer)
            {
                spriteSheetName = _GameController.SpriteSheetName[_GameController.CharacterID].name;
            }

            sRender = GetComponent<SpriteRenderer>();
            loadSpriteSheet();
        }


        void LateUpdate()
        {
            if (isPlayer)
            {
                if (_GameController.CharacterID != _GameController.CurrentCharacterID && _GameController.CharacterID < _GameController.SpriteSheetName.Length)
                {
                    spriteSheetName = _GameController.SpriteSheetName[_GameController.CharacterID].name;
                    _GameController.CurrentCharacterID = _GameController.CharacterID;
                }

                if (spriteSheetName != _GameController.SpriteSheetName[_GameController.CharacterID].name)
                {
                    spriteSheetName = _GameController.SpriteSheetName[_GameController.CharacterID].name;
                }

                _GameController.ValidateWeapon();
            }

            if (loadedSpriteSheetName != spriteSheetName)
            {
                loadSpriteSheet();
            }

            sRender.sprite = spriteSheet[sRender.sprite.name];
        }

        private void loadSpriteSheet()
        {
            sprites = Resources.LoadAll<Sprite>(spriteSheetName);
            spriteSheet = sprites.ToDictionary(x => x.name, x => x);
            loadedSpriteSheetName = spriteSheetName;
        }
    }

}
