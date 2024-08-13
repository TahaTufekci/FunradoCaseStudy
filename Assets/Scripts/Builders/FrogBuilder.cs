using System.Collections.Generic;
using UnityEngine;

namespace Builders
{
    public class FrogBuilder : BaseBuilder<Frog>
    {
        private List<FrogSO> frogScriptableObjects;
        private Direction direction;

        public FrogBuilder SetDirection(Direction direction)
        {
            this.direction = direction;
            return this;
        }

        public FrogBuilder SetScriptableObjects(List<FrogSO> frogScriptableObjects)
        {
            this.frogScriptableObjects = frogScriptableObjects;
            return this;
        }

        public override Frog Build(Cell cell)
        {
            var matchingFrogSO = frogScriptableObjects.Find(frogSO => frogSO.colorValue == colorValue);

            if (matchingFrogSO != null)
            {
                var frogObj = Object.Instantiate(prefab, cell.transform);
                var frog = frogObj.GetComponent<Frog>();
                frog.Initialize(matchingFrogSO.colorValue, direction, matchingFrogSO.texture);
                frog.SetCoordinates(cell.PosX, cell.PosY, cell.PosZ);

                // Set the position to be 0.145f above the parent
                frogObj.transform.localPosition = new Vector3(0, 0.145f, 0);
                var rotation = RotationMapper.GetRotation(direction);
                frog.transform.rotation = rotation;
                return frog;
            }

            Debug.LogError("No matching FrogSO found.");
            return null;
        }
    }
}
