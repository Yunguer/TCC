using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetoTCC
{
    public enum WeaponType
    {
        Melee,
        Bow,
        Staff
    }

    public enum DamageType
    {
        Normal,
        Fire,
        Water,
        Earth
    }

    [CreateAssetMenu(menuName = "TCC/ScriptableAssets/Items/Weapon", fileName = "CustomWeapon")]
    public class CustomWeaponData : ScriptableObject
    {
        [SerializeField]
        private string id;
        public string Id => id;

        [SerializeField]
        private WeaponType weaponType;
        public WeaponType WeaponType => weaponType;

        [SerializeField]
        private string name;
        public string Name => name;

        [SerializeField]
        private Sprite inventoryIcon;
        public Sprite InventoryIcon => inventoryIcon;

        [SerializeField]
        private Sprite[] animationIcons;
        public Sprite[] AnimationIcons => animationIcons;

        [SerializeField]
        private int damage;
        public int Damage => damage;

        [SerializeField]
        private DamageType damageType;
        public DamageType DamageType => damageType;

        [SerializeField]
        private int level;
        public int Level => level;

    }

}
