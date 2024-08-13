using System.Collections.Generic;
using UnityEngine;

namespace Builders
{
    public class BerryBuilder : BaseBuilder<Berry>
    {
        private List<BerrySO> berryScriptableObjects;

        public BerryBuilder SetScriptableObjects(List<BerrySO> berryScriptableObjects)
        {
            this.berryScriptableObjects = berryScriptableObjects;
            return this;
        }

        public override Berry Build(Cell cell)
        {
            var matchingBerrySO = berryScriptableObjects.Find(berrySO => berrySO.colorValue == colorValue);

            if (matchingBerrySO != null)
            {
                var berryObject = Object.Instantiate(prefab, cell.transform);
                var berry = berryObject.GetComponent<Berry>();
                berry.Initialize(matchingBerrySO.colorValue, matchingBerrySO.texture);
                berry.SetCoordinates(cell.PosX, cell.PosY, cell.PosZ);

                berryObject.transform.localPosition = new Vector3(0, 0.28f, 0);
                return berry;
            }

            Debug.LogError("No matching BerrySO found.");
            return null;
        }
    }
}
