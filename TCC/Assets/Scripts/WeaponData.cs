using System;
using UnityEngine;

namespace ProjetoTCC
{
   public class WeaponData : MonoBehaviour
    {
        [Header("Configura��es Para a Arma")]
        #region Variaveis para as Informa��es da Arma
        [SerializeField]
        private float damage;
        public float Damage
        {
            
            get
            {
                    return damage;
            }
            set
            {
                    damage = value;
            }
        }
        [SerializeField]
        private int damageType;
        public int DamageType
        {

            get
            {
                return damageType;
            }
            set
            {
                damageType = value;
            }
        }

        public static implicit operator WeaponData(int v)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
