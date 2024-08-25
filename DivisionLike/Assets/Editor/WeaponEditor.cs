using UnityEngine;
using System.Collections;
using UnityEditor;

namespace DivisionLike
{
    [CustomEditor( typeof( Weapon ) )]
    public class WeaponEditor : Editor
    {
        Weapon weapon;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            weapon = (Weapon) target;

            EditorGUILayout.LabelField( "Weapon Helpers" );

            if ( GUILayout.Button( "Save gun equip location." ) )
            {
                Transform weaponT = weapon.transform;
                Vector3 weaponPos = weaponT.localPosition;
                Vector3 weaponRot = weaponT.localEulerAngles;
                weapon.m_WeaponSettings.equipPosition = weaponPos;
                weapon.m_WeaponSettings.equipRotation = weaponRot;
            }

            if ( GUILayout.Button( "Save gun unequip location." ) )
            {
                Transform weaponT = weapon.transform;
                Vector3 weaponPos = weaponT.localPosition;
                Vector3 weaponRot = weaponT.localEulerAngles;
                weapon.m_WeaponSettings.unequipPosition = weaponPos;
                weapon.m_WeaponSettings.unequipRotation = weaponRot;
            }

            EditorGUILayout.LabelField( "Debug Positioning" );

            if ( GUILayout.Button( "Move gun to equip location" ) )
            {
                Transform weaponT = weapon.transform;
                weaponT.localPosition = weapon.m_WeaponSettings.equipPosition;
                Quaternion eulerAngles = Quaternion.Euler( weapon.m_WeaponSettings.equipRotation );
                weaponT.localRotation = eulerAngles;
            }

            if ( GUILayout.Button( "Move gun to unequip location" ) )
            {
                Transform weaponT = weapon.transform;
                weaponT.localPosition = weapon.m_WeaponSettings.unequipPosition;
                Quaternion eulerAngles = Quaternion.Euler( weapon.m_WeaponSettings.unequipRotation );
                weaponT.localRotation = eulerAngles;
            }
        }
    }
}