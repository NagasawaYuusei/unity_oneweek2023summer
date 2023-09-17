using UnityEngine;

namespace KyawaLib
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : Component
    {
        static T ms_instance;

        /// <summary>
        /// インスタンス取得
        /// </summary>
        static public T instance
        {
            get { return ms_instance; }
        }

        void Awake()
        {
            if (ms_instance)
            {
                Destroy(gameObject);
            }
            else
            {
                ms_instance = this as T;
                Init();
            }
        }

        /// <summary>
        /// 初期化（Awakeで呼び出されます）
        /// </summary>
        protected virtual void Init() { }

        /// <summary>
        /// インスタンス生成
        /// </summary>
        static public void Create(GameObject obj = null)
        {
            if (ms_instance is null)
            {
                if (obj == null)
                    obj = new GameObject();

                var comp = obj.AddComponent<T>();

                if (obj == null)
                    obj.name = comp.GetType().Name;
            }
        }

        /// <summary>
        /// インスタンス削除
        /// </summary>
        public void Destroy()
        {
            ms_instance = null;
            Destroy(gameObject);
        }
    }
}