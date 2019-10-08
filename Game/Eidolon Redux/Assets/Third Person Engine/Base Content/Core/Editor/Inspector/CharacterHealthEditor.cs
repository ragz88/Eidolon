/* ==================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================== */

using System.Linq;
using ThirdPersonEngine.Runtime;
using ThirdPersonEngine.Utility;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.PostProcessing;

namespace ThirdPersonEngine.Editor
{
    [CustomEditor(typeof(CharacterHealth))]
    public sealed class CharacterHealthEditor : RSEditor<CharacterHealth>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent HealthProperties = new GUIContent("Health Properties");
            public readonly static GUIContent Health = new GUIContent("Health", "Health point value.");
            public readonly static GUIContent MaxHealth = new GUIContent("Max Health", "Max health point value.");
            public readonly static GUIContent MinHealth = new GUIContent("Min Health", "Min health point value.");

            public readonly static GUIContent AdditionalsSystems = new GUIContent("Additional Systems");
            public readonly static GUIContent RegenerationProperties = new GUIContent("Regeneration Properties");

            public readonly static GUIContent HealthSounds = new GUIContent("Sounds Effects");
            public readonly static GUIContent TakeDamageSound = new GUIContent("Take Damage", "Sound will be player when player take daamge.");
            public readonly static GUIContent VelocityDamageSound = new GUIContent("Velocity Damage", "Sound will be played when player take damage from velocity speed.");
            public readonly static GUIContent HeartbeatSound = new GUIContent("Heartbeat", "Heartbeat sound will player every rate, if condition [Heartbeat Start From] is met.");
            public readonly static GUIContent HeartbeatRate = new GUIContent("Heartbeat Rate", "Heartbeat sound play rate.");
            public readonly static GUIContent HeartbeatStartFrom = new GUIContent("Heartbeat Start From", "Start play heartbeat sound if player health <= this value.");
            public readonly static GUIContent DeathSound = new GUIContent("Death", "Death sound will player when player die.");

            public readonly static GUIContent VelocityDamageProperties = new GUIContent("Velocity Damage Properties");
            public readonly static GUIContent Rate = new GUIContent("Rate", "Rate (in seconds) of adding health points.\n(V/R - Value per rate).");
            public readonly static GUIContent Value = new GUIContent("Value", "Health point value.");
            public readonly static GUIContent Delay = new GUIContent("Delay", "Delay before start adding health.");

            public readonly static GUIContent HealthCameraEffect = new GUIContent("Camera Effects", "Health camera effect system.");
            public readonly static GUIContent ChromaticAberration = new GUIContent("Chromatic Aberration");
            public readonly static GUIContent Vignette = new GUIContent("Vignette");
            public readonly static GUIContent Profile = new GUIContent("Profile", "Post processing profile assets.");
            public readonly static GUIContent StartPoint = new GUIContent("Start From", "From how many percent of health begin to show the effect.");
            public readonly static GUIContent ResetSmooth = new GUIContent("Reset Speed", "Speed for reserting effect, when health more then start point value.");
            public readonly static GUIContent ChromaticAberrationSpeed = new GUIContent("CA Speed", "Chromatic aberration effect speed.");
            public readonly static GUIContent VignetteSmooth = new GUIContent("Vignette Smooth", "Vignette effect smooth value.");
            public readonly static GUIContent VignetteRange = new GUIContent("Vignette Range", "Vignette intensity range effect.");
        }

        private bool regenerationFoldout;
        private bool fallDamagePropertiesFoldout;
        private bool deathCameraFoldout;
        private bool healthSoundFoldout;
        private bool damageCameraEffect;
        private float maxSpeed = 20.0f;
        private bool editMaxSpeed = false;
        private ReorderableList fdpList;

        public override string GetHeaderName()
        {
            return "Character Health";
        }

        public override void OnInitializeProperties()
        {
            maxSpeed = GetMaxSpeed();

            SerializedProperty fallDamageProperties = serializedObject.FindProperty("velocityDamageProperties");
            fdpList = new ReorderableList(serializedObject, fallDamageProperties, true, true, true, true)
            {
                drawHeaderCallback = (rect) =>
                    {
                        EditorGUI.LabelField(new Rect(rect.x + 15, rect.y + 1, 65, EditorGUIUtility.singleLineHeight), "Damage");
                        EditorGUI.LabelField(new Rect(rect.x + 77.5f, rect.y + 1, 77, EditorGUIUtility.singleLineHeight), "Min Speed");
                        float x = editMaxSpeed ? 80 : 25;
                        if (GUI.Button(new Rect(rect.width - x, rect.y + 1, 67, EditorGUIUtility.singleLineHeight), "Max Speed", EditorStyles.label))
                            editMaxSpeed = !editMaxSpeed;
                        if (editMaxSpeed)
                            maxSpeed = EditorGUI.FloatField(new Rect(rect.width - 9, rect.y + 1.5f, 35, EditorGUIUtility.singleLineHeight - 2), GUIContent.none, maxSpeed);
                    },

                    drawElementCallback = (rect, index, isActive, isFocused) =>
                    {
                        SerializedProperty property = fallDamageProperties.GetArrayElementAtIndex(index);

                        EditorGUI.PropertyField(new Rect(rect.x + 7.5f, rect.y + 1.5f, 35, EditorGUIUtility.singleLineHeight), property.FindPropertyRelative("damage"), GUIContent.none);
                        EditorGUI.LabelField(new Rect(rect.x + 55, rect.y + 1, 50, EditorGUIUtility.singleLineHeight), "->");
                        EditorGUI.PropertyField(new Rect(rect.x + 77.5f, rect.y + 1.5f, 35, EditorGUIUtility.singleLineHeight), property.FindPropertyRelative("minSpeed"), GUIContent.none);
                        float min = property.FindPropertyRelative("minSpeed").floatValue;
                        float max = property.FindPropertyRelative("maxSpeed").floatValue;
                        EditorGUI.MinMaxSlider(new Rect(rect.x + 120, rect.y + 1.5f, rect.width - 175, EditorGUIUtility.singleLineHeight), ref min, ref max, 0, maxSpeed);
                        property.FindPropertyRelative("minSpeed").floatValue = TPEMathf.AllocatePart(min);
                        property.FindPropertyRelative("maxSpeed").floatValue = TPEMathf.AllocatePart(max);
                        EditorGUI.PropertyField(new Rect(rect.width + 5, rect.y + 1.5f, 35, EditorGUIUtility.singleLineHeight), property.FindPropertyRelative("maxSpeed"), GUIContent.none);
                    }
            };
        }

        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.HealthProperties);
            instance.SetHealth(EditorGUILayout.IntSlider(ContentProperties.Health, instance.GetHealth(), 0, instance.GetMaxHealth()));
            instance.SetMaxHealth(EditorGUILayout.IntField(ContentProperties.MaxHealth, instance.GetMaxHealth()));
            instance.SetMinHealth(EditorGUILayout.IntField(ContentProperties.MinHealth, instance.GetMinHealth()));
            GUILayout.Space(3);
            IncreaseIndentLevel();
            BeginFoldoutGroup(ref regenerationFoldout, ContentProperties.RegenerationProperties);
            if (regenerationFoldout)
            {
                HealthRegenirationProperties regenerationSystem = instance.GetRegenerationSystem().GetRegenerationProperties();
                regenerationSystem.SetRate(EditorGUILayout.FloatField(ContentProperties.Rate, regenerationSystem.GetRate()));
                regenerationSystem.SetValue(EditorGUILayout.IntField(ContentProperties.Value, regenerationSystem.GetValue()));
                regenerationSystem.SetDelay(EditorGUILayout.FloatField(ContentProperties.Delay, regenerationSystem.GetDelay()));
                instance.GetRegenerationSystem().SetRegenerationProperties(regenerationSystem);
            }
            EditorGUI.EndDisabledGroup();
            if (regenerationFoldout && !instance.RegenirationIsActive())
            {
                Rect notificationBackgroungRect = GUILayoutUtility.GetLastRect();
                Rect notificationTextRect = GUILayoutUtility.GetLastRect();

                notificationBackgroungRect.y -= 38.5f;
                notificationBackgroungRect.height = 56;

                notificationTextRect.y -= 35;
                notificationTextRect.height = 50;

                Notification("Regeneration Disabled", notificationBackgroungRect, notificationTextRect);
            }
            if (regenerationFoldout)
            {
                string rpToggleName = instance.RegenirationIsActive() ? "Regeniration Enabled" : "Regeneration Disabled";
                instance.RegenirationActive(EditorGUILayout.Toggle(new GUIContent(rpToggleName), instance.RegenirationIsActive()));
            }
            EndFoldoutGroup();

            BeginFoldoutGroup(ref healthSoundFoldout, ContentProperties.HealthSounds);
            if (healthSoundFoldout)
            {
                CharacterHealth.HealthSoundEffects healthSounds = instance.GetHealthSoundEffects();
                healthSounds.SetTakeDamageSound(ObjectField<AudioClip>(ContentProperties.TakeDamageSound, healthSounds.GetTakeDamageSound(), true));
                healthSounds.SetVelocityDamageSound(ObjectField<AudioClip>(ContentProperties.VelocityDamageSound, healthSounds.GetVelocityDamageSound(), true));
                healthSounds.SetHeartbeatSound(ObjectField<AudioClip>(ContentProperties.HeartbeatSound, healthSounds.GetHeartbeatSound(), true));
                healthSounds.SetHeartbeatRate(EditorGUILayout.Slider(ContentProperties.HeartbeatRate, healthSounds.GetHeartbeatRate(), 0.0f, 10.0f));
                healthSounds.SetHeartbeatStartFrom(EditorGUILayout.IntSlider(ContentProperties.HeartbeatStartFrom, healthSounds.GetHeartbeatStartFrom(), instance.GetMinHealth(), instance.GetMaxHealth()));
                healthSounds.SetDeathSound(ObjectField<AudioClip>(ContentProperties.DeathSound, healthSounds.GetDeathSound(), true));
                instance.SetHealthSoundEffects(healthSounds);
            }
            EndFoldoutGroup();

            BeginFoldoutGroup(ref fallDamagePropertiesFoldout, ContentProperties.VelocityDamageProperties);
            if (fallDamagePropertiesFoldout)
            {
                DecreaseIndentLevel();
                fdpList.DoLayoutList();
                IncreaseIndentLevel();
            }
            EndFoldoutGroup();

            BeginFoldoutGroup(ref damageCameraEffect, ContentProperties.HealthCameraEffect);
            if (damageCameraEffect)
            {
                instance.GetHealthCameraEffects().SetProfile((PostProcessingProfile) EditorGUILayout.ObjectField(ContentProperties.Profile, instance.GetHealthCameraEffects().GetProfile(), typeof(PostProcessingProfile), true));
                instance.GetHealthCameraEffects().SetStartPoint(EditorGUILayout.IntSlider(ContentProperties.StartPoint, instance.GetHealthCameraEffects().GetStartPoint(), instance.GetMinHealth(), instance.GetMaxHealth()));
                instance.GetHealthCameraEffects().SetResetSmooth(EditorGUILayout.FloatField(ContentProperties.ResetSmooth, instance.GetHealthCameraEffects().GetResetSmooth()));
                EditorGUI.BeginDisabledGroup(instance.GetHealthCameraEffects().GetProfile() == null);
                instance.GetHealthCameraEffects().SetChromaticAberrationSpeed(EditorGUILayout.FloatField(ContentProperties.ChromaticAberrationSpeed, instance.GetHealthCameraEffects().GetChromaticAberrationSpeed()));
                EditorGUI.EndDisabledGroup();
                EditorGUI.BeginDisabledGroup(instance.GetHealthCameraEffects().GetProfile() == null);
                instance.GetHealthCameraEffects().SetVignetteSmooth(EditorGUILayout.FloatField(ContentProperties.VignetteSmooth, instance.GetHealthCameraEffects().GetVignetteSmooth()));
                float min = instance.GetHealthCameraEffects().GetVignetteMinValue();
                float max = instance.GetHealthCameraEffects().GetVignetteMaxValue();
                MinMaxSlider(ContentProperties.VignetteRange, ref min, ref max, 0.0f, 1.0f);
                instance.GetHealthCameraEffects().SetVignetteMinValue(TPEMathf.AllocatePart(min));
                instance.GetHealthCameraEffects().SetVignetteMaxValue(TPEMathf.AllocatePart(max));

                EditorGUI.EndDisabledGroup();
            }
            EndFoldoutGroup();
            DecreaseIndentLevel();

            EndGroup();
        }

        private float GetMaxSpeed()
        {
            if (instance.GetFallDamageProperties() == null || instance.GetFallDamageProperties().Length == 0)
                return 20;
            float height = instance.GetFallDamageProperties().Max(h => h.GetMaxSpeed());
            return Mathf.Ceil(height);
        }
    }
}