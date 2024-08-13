using UnityEngine;

namespace Builders
{
    public abstract class BaseBuilder<T> where T : CellContent
    {
        protected int colorValue;
        protected GameObject prefab;
        protected Transform parentTransform;

        public virtual BaseBuilder<T> SetColorValue(int colorValue)
        {
            this.colorValue = colorValue;
            return this;
        }

        public virtual BaseBuilder<T> SetPrefab(GameObject prefab)
        {
            this.prefab = prefab;
            return this;
        }

        public abstract T Build(Cell cell);
    }
}
