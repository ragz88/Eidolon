/* ==================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================== */

using ThirdPersonEngine.Runtime;
using UnityEditor;
using UnityEngine;

namespace ThirdPersonEngine.Editor
{
    [CustomEditor(typeof(RespawnManager))]
    [CanEditMultipleObjects]
    public class RespawnManagerEditor : RSEditor<RespawnManager>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent BaseProperties = new GUIContent("Base Properties");
            public readonly static GUIContent CharacterTransform = new GUIContent("Character", "Character transform.");
            public readonly static GUIContent RespawnType = new GUIContent("Respawn Type");
            public readonly static GUIContent Key = new GUIContent("Key", "Key for respawn.");
            public readonly static GUIContent Radius = new GUIContent("Radius", "Radius the radius in which the player will be respwaned.");
            public readonly static GUIContent Delay = new GUIContent("Delay", "Delay before respawn.");
            public readonly static GUIContent RespawnHealth = new GUIContent("Respawn Health", "Character health after respawn.");
            public readonly static GUIContent RespawnSound = new GUIContent("Respawn Sound", "Respawn sound will be played after player respawned.");
            public readonly static GUIContent ScreenFadeProperties = new GUIContent("Screen Fade Properties");
            public readonly static GUIContent InFadeColor = new GUIContent("In Fade Color");
            public readonly static GUIContent InFadeSpeed = new GUIContent("In Fade Speed");
            public readonly static GUIContent OutFadeSpeed = new GUIContent("Out Fade Speed");
            public readonly static GUIContent UseScreenFade = new GUIContent("Use Screen Fade");
            public readonly static GUIContent TimeToFade = new GUIContent("Time To Fade");
        }

        private CharacterHealth health;
        private bool screenFadePropertiesFoldout;

        public override void OnInitializeProperties()
        {
            InitializeHealth();
        }

        public virtual void OnSceneGUI()
        {
            const float SCALE_VALUE_HANDLE_SIZE = 1.0f;

            Vector3 position = instance.transform.position;

            Color solidColor = new Color(0.25f, 0.25f, 0.25f, 0.5f);
            Color wireColor = Color.black;
            Color scaleSliderColor = Color.white;

            float radius = instance.GetRadius();

            Handles.color = wireColor;
            Handles.DrawWireDisc(position, Vector3.up, radius * 2);

            Handles.color = solidColor;
            Handles.DrawSolidDisc(position, Vector3.up, radius * 2);

            Handles.color = scaleSliderColor;
            radius = Handles.ScaleSlider(radius, position, Vector3.right, Quaternion.identity, SCALE_VALUE_HANDLE_SIZE, 1);

            instance.SetRadius(radius);
        }

        public override void OnBaseGUI()
        {
            InitializeHealth();
            BeginGroup(ContentProperties.BaseProperties);
            instance.SetCharacter(ObjectField<Transform>(ContentProperties.CharacterTransform, instance.GetCharacter(), true));
            instance.SetRespawnType((RespawnManager.RespawnType) EditorGUILayout.EnumPopup(ContentProperties.RespawnType, instance.GetRespawnType()));
            if (instance.GetRespawnType() == RespawnManager.RespawnType.ByKey)
                instance.SetKey((KeyCode) EditorGUILayout.EnumPopup(ContentProperties.Key, instance.GetKey()));
            instance.SetRadius(EditorGUILayout.FloatField(ContentProperties.Radius, instance.GetRadius()));
            instance.SetDelay(EditorGUILayout.FloatField(ContentProperties.Delay, instance.GetDelay()));
            instance.SetRespawnHealth(EditorGUILayout.IntSlider(ContentProperties.RespawnHealth, instance.GetRespawnHealth(), health ? health.GetMinHealth() : 0, health ? health.GetMaxHealth() : 0));
            instance.SetRespawnSound(ObjectField<AudioClip>(ContentProperties.RespawnSound, instance.GetRespawnSound(), true));

            IncreaseIndentLevel();
            BeginFoldoutGroup(ref screenFadePropertiesFoldout, ContentProperties.ScreenFadeProperties);
            if (screenFadePropertiesFoldout)
            {
                ScreenFadeProperties screenFadeProperties = instance.GetScreenFadeProperties();
                screenFadeProperties.SetInFadeColor(EditorGUILayout.ColorField(ContentProperties.InFadeColor, screenFadeProperties.GetInFadeColor()));
                screenFadeProperties.SetInFadeSpeed(EditorGUILayout.FloatField(ContentProperties.InFadeSpeed, screenFadeProperties.GetInFadeSpeed()));
                screenFadeProperties.SetOutFadeSpeed(EditorGUILayout.FloatField(ContentProperties.OutFadeSpeed, screenFadeProperties.GetOutFadeSpeed()));
                instance.SetScreenFadeProperties(screenFadeProperties);
                instance.SetTimeToFade(EditorGUILayout.FloatField(ContentProperties.TimeToFade, instance.GetTimeToFade()));
                instance.UseScreenFade(EditorGUILayout.Toggle(ContentProperties.UseScreenFade, instance.UseScreenFade()));
            }
            EndFoldoutGroup();
            DecreaseIndentLevel();
            EndGroup();
        }

        private void InitializeHealth()
        {
            health = instance.GetCharacter()?.GetComponent<CharacterHealth>();
        }
    }
}