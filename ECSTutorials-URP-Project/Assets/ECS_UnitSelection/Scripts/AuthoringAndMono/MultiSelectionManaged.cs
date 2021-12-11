using Unity.Entities;
using UnityEngine;

namespace TMG.UnitSelection
{
    public class MultiSelectionManaged : MonoBehaviour
    {
        [SerializeField] private float _castDistance;
        
        private UnitSelectionSystem _unitSelectionSystem;

        private void Start()
        {
            _unitSelectionSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<UnitSelectionSystem>();
            _unitSelectionSystem.CastDistance = _castDistance;
        }

        private void OnGUI()
        {
            if (_unitSelectionSystem.IsDragging)
            {
                var rect = SelectionGUI.GetScreenRect(_unitSelectionSystem.MouseStartPos, Input.mousePosition);
                SelectionGUI.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.1f));
                SelectionGUI.DrawScreenRectBorder(rect, 1, Color.blue);
            }
        }
    }    
    
    public static class SelectionGUI
    {
        private static Texture2D _whiteTexture;

        private static Texture2D WhiteTexture
        {
            get
            {
                if (_whiteTexture == null)
                {
                    _whiteTexture = new Texture2D(1, 1);
                    _whiteTexture.SetPixel(0, 0, Color.white);
                    _whiteTexture.Apply();
                }

                return _whiteTexture;
            }
        }

        public static Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
        {
            // Move origin from bottom left to top left
            screenPosition1.y = Screen.height - screenPosition1.y;
            screenPosition2.y = Screen.height - screenPosition2.y;
            // Calculate corners
            var topLeft = Vector3.Min(screenPosition1, screenPosition2);
            var bottomRight = Vector3.Max(screenPosition1, screenPosition2);
            // Create Rect
            return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
        }
        
        public static void DrawScreenRect(Rect rect, Color color)
        {
            GUI.color = color;
            GUI.DrawTexture(rect, WhiteTexture);
        }

        public static void DrawScreenRectBorder(Rect rect, float thickness, Color color)
        {
            //Top
            DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
            // Left
            DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
            // Right
            DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
            // Bottom
            DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
        }
    }
}