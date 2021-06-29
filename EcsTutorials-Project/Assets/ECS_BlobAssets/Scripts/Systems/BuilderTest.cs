using TMG.ChangeFilter;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace TMG.BlobAssets
{
    public class BuilderTest : SystemBase
    {
        private BlobReferenceHolder hodlr;
        
        protected override void OnStartRunning()
        {
            hodlr = GetSingleton<BlobReferenceHolder>();
            
            using var blobBuilder = new BlobBuilder(Allocator.Temp);
            ref var numberBlobAsset = ref blobBuilder.ConstructRoot<NumberBlobAsset>();
            var numberArray = blobBuilder.Allocate(ref numberBlobAsset.NumberArray, 3);
            numberArray[0] = new Number {Value = 1};
            numberArray[1] = new Number {Value = 2};
            numberArray[2] = new Number {Value = 3};

            hodlr.BlobRef = blobBuilder.CreateBlobAssetReference<NumberBlobAsset>(Allocator.Persistent);

            Debug.Log(hodlr.BlobRef.Value.NumberArray.Length);
        }

        protected override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log(hodlr.BlobRef.Value.NumberArray[1].Value.ToString());
            }
        }
    }
}