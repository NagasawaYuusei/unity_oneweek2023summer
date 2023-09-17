using System.Diagnostics;

namespace KyawaLib
{
    public class SingletonClass<T> where T : new()
    {
        static T ms_instance;

        /// <summary>
        /// インスタンス取得
        /// </summary>
        static public T instance
        {
            get { return ms_instance; }
        }

        /// <summary>
        /// インスタンス生成
        /// </summary>
        static public T Create()
        {
            if (ms_instance is null)
            {
                ms_instance = new T();
                return ms_instance;
            }
            return default(T);
        }

        /// <summary>
        /// インスタンス削除
        /// </summary>
        public void Destroy()
        {
            OnDestroy();
            ms_instance = default(T);
        }

        /// <summary>
        /// Destroy時に呼び出されます.
        /// </summary>
        protected virtual void OnDestroy()
        {

        }
    }
}