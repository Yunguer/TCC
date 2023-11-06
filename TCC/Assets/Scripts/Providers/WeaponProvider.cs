using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetoTCC
{
    [CreateAssetMenu(menuName = "TCC/ScriptableAssets/Providers/WeaponProvider", fileName = "WeaponProvider")]
    public class WeaponProvider : ScriptableObject
    {
        [SerializeField]
        private SerializableDictionary<string, CustomWeaponData> meleeWeapons = new SerializableDictionary<string, CustomWeaponData>();

        [SerializeField]
        private SerializableDictionary<string, CustomWeaponData> bowWeapons = new SerializableDictionary<string, CustomWeaponData>();

        [SerializeField]
        private SerializableDictionary<string, CustomWeaponData> staffWeapons = new SerializableDictionary<string, CustomWeaponData>();

        [SerializeField]
        private SerializableDictionary<PlayerType, CustomWeaponData> inicialWeapons = new SerializableDictionary<PlayerType, CustomWeaponData>();

        public CustomWeaponData GetInicialWeaponByCharacterId(PlayerType characterId)
        {
            CustomWeaponData customWeaponData = null;
            inicialWeapons.TryGetValue(characterId, out customWeaponData);

            return customWeaponData;
        }

        public CustomWeaponData GetWeaponById(string id)
        {
            CustomWeaponData customWeaponData = null;
            if(meleeWeapons.ContainsKey(id))
            {
                meleeWeapons.TryGetValue(id, out customWeaponData);
            }
            else if (bowWeapons.ContainsKey(id))
            {
                bowWeapons.TryGetValue(id, out customWeaponData);
            }
            else if (staffWeapons.ContainsKey(id))
            {
                staffWeapons.TryGetValue(id, out customWeaponData);
            }

            return customWeaponData;
        }
    }
}
