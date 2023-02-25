using _Application.Scripts.Misc;
using UnityEngine;

namespace _Application.Scripts.UI.Windows
{
    public class MatrixZone
    {
        private Vector2 _minPoint;
        private Vector2 _maxPoint;

        private int _rowCount;
        private int _columnCount;
        private float _lengthX;
        private float _lengthY;

        public MatrixZone(RectTransform minPoint, RectTransform maxPoint, RectTransform value, Vector2 spaces,
            RectTransform centre)
        {
            _maxPoint = SwitchToRectTransform(maxPoint, centre);
            _minPoint = SwitchToRectTransform(minPoint, centre);

            _lengthX = _maxPoint.x - _minPoint.x;
            _columnCount = Mathf.RoundToInt(_lengthX / (value.rect.width + spaces.x)) + 1;

            _lengthY = _maxPoint.y - _minPoint.y;
            _rowCount = Mathf.RoundToInt(_lengthY / (value.rect.height + spaces.y)) + 1;
        }

        public Vector2 GetPos(int index)
        {
            Vector2 indices = new(index % _columnCount, index / _columnCount);
            Vector2 normalized = new(indices.x / (_columnCount-1), indices.y / (_rowCount - 1));
            normalized = normalized.With(y: 1f - normalized.y);
            return new Vector2(_minPoint.x + _lengthX * normalized.x,
                _minPoint.y + _lengthY * normalized.y);
        }
        
        private Vector2 SwitchToRectTransform(RectTransform from, RectTransform to)
        {
            Vector2 fromPivotDerivedOffset = new(from.rect.width * from.pivot.x + from.rect.xMin, from.rect.height * from.pivot.y + from.rect.yMin);
            Vector2 screenP = RectTransformUtility.WorldToScreenPoint(null, from.position);
            screenP += fromPivotDerivedOffset;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(to, screenP, null, out Vector2 localPoint);
            Vector2 pivotDerivedOffset = new(to.rect.width * to.pivot.x + to.rect.xMin, to.rect.height * to.pivot.y + to.rect.yMin);
            return to.anchoredPosition + localPoint - pivotDerivedOffset;
        }
    }
}