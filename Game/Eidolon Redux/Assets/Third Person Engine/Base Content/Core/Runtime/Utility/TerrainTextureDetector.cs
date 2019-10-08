﻿/* ==================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================== */

using UnityEngine;

namespace ThirdPersonEngine.Utility
{
    public struct TerrainTextureDetector
    {
        private TerrainData terrainData;
        private int alphamapWidth;
        private int alphamapHeight;
        private float[, , ] splatmapData;
        private int numTextures;

        /// <summary>
        /// Constructor with TerrainData parameter for detect texture from custom terran.
        /// </summary>
        public TerrainTextureDetector(TerrainData terrainData)
        {
            this.terrainData = terrainData;
            alphamapWidth = this.terrainData.alphamapWidth;
            alphamapHeight = this.terrainData.alphamapHeight;

            splatmapData = this.terrainData.GetAlphamaps(0, 0, alphamapWidth, alphamapHeight);
            numTextures = splatmapData.Length / (alphamapWidth * alphamapHeight);
        }

        /// <summary>
        /// Convert world position to splat map coordinate.
        /// </summary>
        private Vector3 ConvertToSplatMapCoordinate(Vector3 worldPosition)
        {
            Vector3 splatPosition = new Vector3();
            Terrain ter = Terrain.activeTerrain;
            Vector3 terPosition = ter.transform.position;
            splatPosition.x = ((worldPosition.x - terPosition.x) / ter.terrainData.size.x) * ter.terrainData.alphamapWidth;
            splatPosition.z = ((worldPosition.z - terPosition.z) / ter.terrainData.size.z) * ter.terrainData.alphamapHeight;
            return splatPosition;
        }

        /// <summary>
        /// Get active texture id.
        /// </summary>
        public int GetActiveTextureId(Vector3 position)
        {
            Vector3 terrainCord = ConvertToSplatMapCoordinate(position);
            int activeTerrainIndex = 0;
            float largestOpacity = 0.0f;

            for (int i = 0; i < numTextures; i++)
            {
                if (largestOpacity < splatmapData[(int) terrainCord.z, (int) terrainCord.x, i])
                {
                    activeTerrainIndex = i;
                    largestOpacity = splatmapData[(int) terrainCord.z, (int) terrainCord.x, i];
                }
            }
            return activeTerrainIndex;
        }

        /// <summary>
        /// Get active texture instance. 
        /// </summary>
        public Texture2D GetActiveTexture(Vector3 position)
        {
            Vector3 terrainCord = ConvertToSplatMapCoordinate(position);
            int activeTerrainIndex = 0;
            float largestOpacity = 0.0f;

            for (int i = 0; i < numTextures; i++)
            {
                if (largestOpacity < splatmapData[(int) terrainCord.z, (int) terrainCord.x, i])
                {
                    activeTerrainIndex = i;
                    largestOpacity = splatmapData[(int) terrainCord.z, (int) terrainCord.x, i];
                }
            }

#if UNITY_2018_1_OR_NEWER
            return terrainData.terrainLayers[activeTerrainIndex].diffuseTexture ?? null;
#else
            return terrainData.splatPrototypes[activeTerrainIndex].texture ?? null;
#endif
        }

        public TerrainData GetTerrainData()
        {
            return terrainData;
        }

        public void SetTerrainData(TerrainData value)
        {
            terrainData = value;
            alphamapWidth = this.terrainData.alphamapWidth;
            alphamapHeight = this.terrainData.alphamapHeight;

            splatmapData = this.terrainData.GetAlphamaps(0, 0, alphamapWidth, alphamapHeight);
            numTextures = splatmapData.Length / (alphamapWidth * alphamapHeight);
        }

        public int GetAlphamapWidth()
        {
            return alphamapWidth;
        }

        public int GetAlphamapHeight()
        {
            return alphamapHeight;
        }

        public float[,,] GetSplatmapData()
        {
            return splatmapData;
        }

        public int GetNumTextures()
        {
            return numTextures;
        }
    }
}