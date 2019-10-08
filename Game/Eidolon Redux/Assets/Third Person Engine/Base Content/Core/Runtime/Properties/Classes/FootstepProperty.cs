/* ==================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================== */

using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ThirdPersonEngine.Runtime
{
    /// <summary>
    /// Properties to distinguish between the surface. 
    /// </summary>
    [Serializable]
    public struct FootstepProperty : IEquatable<FootstepProperty>
    {
        [SerializeField] private PhysicMaterial physicMaterial;
        [SerializeField] private Texture2D texture;
        [SerializeField] private AudioClip[] stepSounds;
        [SerializeField] private AudioClip[] jumpSounds;
        [SerializeField] private AudioClip[] landSounds;

        /// <summary>
        /// Footstep property constructor.
        /// </summary>
        /// <param name="physicMaterial">Surface physic material.</param>
        /// <param name="texture">Surface texture.</param>
        /// <param name="stepSounds">Footstep sounds.</param>
        /// <param name="jumpSounds">Jump footstep sounds.</param>
        /// <param name="landSounds">Land footstep sounds.</param>
        public FootstepProperty(PhysicMaterial physicMaterial, Texture2D texture, AudioClip[] stepSounds, AudioClip[] jumpSounds, AudioClip[] landSounds)
        {
            this.physicMaterial = physicMaterial;
            this.texture = texture;
            this.stepSounds = stepSounds;
            this.jumpSounds = jumpSounds;
            this.landSounds = landSounds;
        }

        /// <summary>
        /// Return surface physic material.
        /// </summary>
        /// <returns></returns>
        public PhysicMaterial GetPhysicMaterial()
        {
            return physicMaterial;
        }

        /// <summary>
        /// Set surface physic material.
        /// </summary>
        /// <param name="value"></param>
        public void SetPhysicMaterial(PhysicMaterial value)
        {
            physicMaterial = value;
        }

        public bool ComparePhysicMaterial(PhysicMaterial physicMaterial)
        {
            return this.physicMaterial == physicMaterial;
        }

        /// <summary>
        /// Return surface texture.
        /// </summary>
        /// <returns></returns>
        public Texture2D GetTexture()
        {
            return texture;
        }

        /// <summary>
        /// Set surface texture.
        /// </summary>
        /// <param name="value"></param>
        public void SetTexture(Texture2D value)
        {
            texture = value;
        }

        public bool CompareTexture(Texture2D texture)
        {
            return this.texture == texture;
        }

        /// <summary>
        /// Return footstep sounds.
        /// </summary>
        /// <returns></returns>
        public AudioClip[] GetStepSounds()
        {
            return stepSounds;
        }

        public AudioClip GetRandomStepSound()
        {
            if (stepSounds.Length == 0)
            {
                return null;
            }
            return stepSounds[Random.Range(0, stepSounds.Length)];
        }

        /// <summary>
        /// Set range footstep sounds.
        /// </summary>
        /// <param name="stepSounds"></param>
        public void SetStepSoundsRange(AudioClip[] stepSounds)
        {
            this.stepSounds = stepSounds;
        }

        /// <summary>
        /// Return footstep sound.
        /// </summary>
        /// <param name="index">Footstep sound index.</param>
        /// <returns></returns>
        public AudioClip GetStepSound(int index)
        {
            return stepSounds[index];
        }

        /// <summary>
        /// Set footstep sound.
        /// </summary>
        /// <param name="index">Footstep sound index.</param>
        /// <param name="stepSound">Footstep sound.</param>
        public void SetStepSound(int index, AudioClip stepSound)
        {
            stepSounds[index] = stepSound;
        }

        /// <summary>
        /// Return jumpstep sounds.
        /// </summary>
        /// <returns></returns>
        public AudioClip[] GetJumpSounds()
        {
            return jumpSounds;
        }

        /// <summary>
        /// Set range jumpstep sounds.
        /// </summary>
        /// <param name="jumpSounds"></param>
        public void SetJumpSoundsRange(AudioClip[] jumpSounds)
        {
            this.jumpSounds = jumpSounds;
        }

        /// <summary>
        /// Return jumpstep sound.
        /// </summary>
        /// <param name="index">Jumpstep sound index.</param>
        /// <returns></returns>
        public AudioClip GetJumpSound(int index)
        {
            return jumpSounds[index];
        }

        /// <summary>
        /// Set jumpstep sound.
        /// </summary>
        /// <param name="index">Jumpstep sound index.</param>
        /// <param name="jumpSound">Jumpstep sound.</param>
        public void SetJumpSound(int index, AudioClip jumpSound)
        {
            jumpSounds[index] = jumpSound;
        }

        /// <summary>
        /// Return landstep sounds.
        /// </summary>
        /// <returns></returns>
        public AudioClip[] GetLandSounds()
        {
            return landSounds;
        }

        /// <summary>
        /// Set range landstep sounds.
        /// </summary>
        /// <param name="landSounds"></param>
        public void SetLandSoundsRange(AudioClip[] landSounds)
        {
            this.landSounds = landSounds;
        }

        /// <summary>
        /// Return land step sound.
        /// </summary>
        /// <param name="index">Landstep sound index.</param>
        /// <returns></returns>
        public AudioClip GetLandSound(int index)
        {
            return landSounds[index];
        }

        /// <summary>
        /// Set landstep sound.
        /// </summary>
        /// <param name="index">Landstep sound index.</param>
        /// <param name="landSound">Landstep sound.</param>
        public void SetLandSound(int index, AudioClip landSound)
        {
            landSounds[index] = landSound;
        }

        /// <summary>
        /// Return footstep sounds array length.
        /// </summary>
        /// <returns></returns>
        public int GetStepSoundsLength()
        {
            return stepSounds.Length;
        }

        /// <summary>
        /// Return jump step sounds array length.
        /// </summary>
        /// <returns></returns>
        public int GetJumpSoundsLength()
        {
            return jumpSounds.Length;
        }

        /// <summary>
        /// Return land step sounds array length.
        /// </summary>
        /// <returns></returns>
        public int GetLandSoundsLength()
        {
            return landSounds.Length;
        }

        public bool IsEmpty()
        {
            return this == FootstepProperty.Empty;
        }

        /// <summary>
        /// Empty Footstep property.
        /// </summary>
        /// <returns></returns>
        public readonly static FootstepProperty Empty = new FootstepProperty(null, null, null, null, null);

        public static bool operator ==(FootstepProperty left, FootstepProperty right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(FootstepProperty left, FootstepProperty right)
        {
            return !Equals(left, right);
        }

        public override bool Equals(object obj)
        {
            return (obj is FootstepProperty metrics) && Equals(metrics);
        }

        public bool Equals(FootstepProperty other)
        {
            return (physicMaterial, texture) == (other.physicMaterial, other.texture);
        }

        public override int GetHashCode()
        {
            return (physicMaterial, texture, stepSounds, jumpSounds, landSounds).GetHashCode();
        }
    }
}