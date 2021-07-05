using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace TMG.DynamicBuffers
{
    public class InventorySystem : SystemBase
    {
        private DynamicBuffer<PotionDataBufferElement> _inventoryBuffer;
        private Entity _inventoryEntity;
        private PlayerStatData _playerStatData;
        
        protected override void OnStartRunning()
        {
            _playerStatData = GetSingleton<PlayerStatData>();
            
            _inventoryEntity = EntityManager.CreateEntity();
            _inventoryBuffer = EntityManager.AddBuffer<PotionDataBufferElement>(_inventoryEntity);

            var healthPot1 = new PotionDataBufferElement {Type = PotionType.Health, Value = 24};
            var healthPot2 = new PotionDataBufferElement {Type = PotionType.Health, Value = 25};
            var manaPot1 = new PotionDataBufferElement {Type = PotionType.Mana, Value = 26};
            var strengthPot1 = new PotionDataBufferElement {Type = PotionType.Strength, Value = 27};

            //var ecb = new EntityCommandBuffer(Allocator.Temp);
            
            //ecb.AddBuffer<>()
            
            _inventoryBuffer.Add(healthPot1);
            _inventoryBuffer.Add(healthPot2);
            _inventoryBuffer.Add(manaPot1);
            _inventoryBuffer.Add(strengthPot1);
        }

        protected override void OnUpdate()
        {
            _inventoryBuffer = EntityManager.GetBuffer<PotionDataBufferElement>(_inventoryEntity);
            
            #region InputRegion

            var bufferIndex = -1;
            
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                bufferIndex = 0;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                bufferIndex = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                bufferIndex = 2;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                bufferIndex = 3;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                bufferIndex = 4;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                bufferIndex = 5;
            }
            
            if (bufferIndex == -1 || bufferIndex >= _inventoryBuffer.Length){ return; }

            #endregion
            
            //Debug.Log($"Pot @ index: {bufferIndex} is type: {_inventoryBuffer[bufferIndex].Type.ToString()} with value: {_inventoryBuffer[bufferIndex].Value}");

            var curPotion = _inventoryBuffer[bufferIndex];
            
            switch (curPotion.Type)
            {
                case PotionType.Health:
                    var initialHealth = _playerStatData.Health;
                    Debug.Log($"Player Health = {initialHealth}");
                    _playerStatData.Health = math.min(100, initialHealth + curPotion.Value);
                    SetSingleton(_playerStatData);
                    curPotion.Value -= (_playerStatData.Health - initialHealth);
                    _inventoryBuffer[bufferIndex] = curPotion;
                    Debug.Log($"Pot @ index: {bufferIndex} is type: {_inventoryBuffer[bufferIndex].Type.ToString()} with value: {_inventoryBuffer[bufferIndex].Value}");

                    break;
                case PotionType.Mana:
                    _playerStatData.Mana = math.min(100, _playerStatData.Mana + curPotion.Value);
                    break;
                case PotionType.Strength:
                    _playerStatData.Strength = math.min(100, _playerStatData.Strength + curPotion.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}