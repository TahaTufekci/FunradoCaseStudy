using System.Collections.Generic;
using UnityEngine;

namespace Builders
{
    public class ArrowBuilder : BaseBuilder<Arrow>
    {
        private Material material;
        private Direction direction;
        private List<ArrowSO> arrowScriptableObjects;

        public ArrowBuilder SetDirection(Direction direction)
        {
            this.direction = direction;
            return this;
        }

        public ArrowBuilder SetScriptableObjects(List<ArrowSO> arrowScriptableObjects)
        {
            this.arrowScriptableObjects = arrowScriptableObjects;
            return this;
        }

        public override Arrow Build(Cell cell)
        {
            var matchingArrowSO = arrowScriptableObjects.Find(arrowSO => arrowSO.colorValue == colorValue);

            if (matchingArrowSO != null)
            {
                var arrowObject = Object.Instantiate(prefab, cell.transform);
                var arrow = arrowObject.GetComponent<Arrow>();
                arrow.Initialize(matchingArrowSO.colorValue, matchingArrowSO.material, direction);
                arrow.SetCoordinates(cell.PosX, cell.PosY, cell.PosZ);

                arrowObject.transform.localPosition = new Vector3(0, 0.1f, 0);
                arrow.transform.rotation = RotationMapper.GetRotation(direction);
                arrow.transform.Rotate(0, 180, 0);

                return arrow;
            }

            Debug.LogError("No matching ArrowSO found.");
            return null;
        }
    }
}
