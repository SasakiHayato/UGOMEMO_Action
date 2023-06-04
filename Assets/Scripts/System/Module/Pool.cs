using System.Collections.Generic;
using UnityEngine;
using PoolSystem.Event;

namespace PoolSystem.Event
{
    /// <summary>
    /// Objectの表示を操作するインターフェース
    /// </summary>
    public interface IPoolActive
    {
        /// <summary>
        /// 表示、非表示のたびに呼ばれる
        /// </summary>
        /// <param name="is_active">[TRUE: 表示][FALSE: 非表示]</param>
        void Active(bool is_active);
    }

    /// <summary>
    /// 使用終了時のコールバックインターフェース
    /// </summary>
    public interface IPoolReleaseCallback
    {
        /// <summary>
        /// コールバックした際の処理
        /// </summary>
        /// <param name="pool_id">リリースするオブジェクトのID</param>
        void ReleaseCallback(int pool_id);
    }
}

namespace PoolSystem
{
    /// <summary>
    /// プールを有効にするためのインターフェース
    /// </summary>
    public interface IPool
    {
        /// <summary>
        /// 初期化.
        /// </summary>
        /// <param name="release_callback">リリースする際のコールバックインターフェース</param>
        /// <param name="pool_id">自身のID</param>
        void Initialize(IPoolReleaseCallback release_callback, int pool_id);

        /// <summary>
        /// 使用開始時の処理.
        /// UnityEngine.OnEnable()
        /// </summary>
        void Entry();

        /// <summary>
        /// 使用終了時の処理.
        /// UnityEngine.OnDisable()
        /// </summary>
        void Release();
    }

    /// <summary>
    /// オブジェクトの再利用化を有効にするための汎用クラス.
    /// ObjectPoolパターン.
    /// </summary>
    /// <typeparam name="PooledObject">IPoolインターフェースを継承したComponentクラス</typeparam>
    public class Pool<PooledObject> : IPoolReleaseCallback, System.IDisposable where PooledObject : Component, IPool
    {
        //========================================================
        // public interface
        //========================================================

        /// <summary>
        /// Pool.Dataを読み込み専用にして外部に公開するためのインターフェース
        /// </summary>
        public interface IReadOnlyPoolData
        {
            /// <summary>
            /// 自身のID
            /// </summary>
            int ID { get; }
            /// <summary>
            /// オブジェクト
            /// </summary>
            PooledObject Object { get; }
        }

        //========================================================
        // private enum
        //========================================================

        private enum MemoryType
        {
            Heap,
            Release,

            Wait,
        }

        //========================================================
        // private class
        //========================================================

        /// <summary>
        /// 各プールオブジェクトのデータクラス
        /// </summary>
        private class Data : IReadOnlyPoolData
        {
            //========================================================
            // property
            //========================================================

            public int ID { get; }
            public PooledObject Object { get; }
            public MemoryType Memory { get; set; }

            //========================================================
            // variable
            //========================================================

            private IPoolActive _activeEvent;

            //========================================================
            // constructor
            //========================================================

            /// <summary>
            /// 初期化
            /// </summary>
            /// <param name="pooled_object"></param>
            /// <param name="pool_id"></param>
            public Data(PooledObject pooled_object, int pool_id)
            {
                ID = pool_id;
                Object = pooled_object;
                Memory = MemoryType.Release;

                _activeEvent = pooled_object as IPoolActive;
            }

            //========================================================
            // public method
            //========================================================

            /// <summary>
            /// 表示、非表示イベント
            /// </summary>
            /// <param name="is_active"></param>
            public void ActiveEvent(bool is_active)
            {
                if (_activeEvent == null) return;
                _activeEvent.Active(is_active);
            }
        }

        //========================================================
        // constant
        //========================================================

        private const int CAPACITY = 100;
        private readonly PooledObject _pooledObject;
        private readonly IList<Data> _poolDataList;
        private readonly Transform _parent;

        //========================================================
        // variable
        //========================================================

        private int _poolID;

        //========================================================
        // constructor
        //========================================================

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="pooled_object"></param>
        /// <param name="capacity"></param>
        public Pool(PooledObject pooled_object, int capacity = CAPACITY)
        {
            _parent = new GameObject("Pool_Stock").transform;
            //_parent.hideFlags = HideFlags.HideInHierarchy;

            List<Data> list = new List<Data>(capacity);
            for (int index = 0; index < capacity; index++)
            {
                var clone = Object.Instantiate(pooled_object);
                clone.name = $"{pooled_object.name}_{_poolID}";
                clone.Initialize(this, _poolID);
                clone.transform.SetParent(_parent);
                list.Add(new Data(clone, _poolID));
                _poolID++;
            }

            _pooledObject = pooled_object;
            _poolDataList = list;
        }

        //========================================================
        // public method
        //========================================================

        /// <summary>
        /// Poolからオブジェクトを取得する為の処理.
        /// 使用する前に独自の関数を呼びたい場合に推奨.
        /// 使用時にPool.Set()にIDを引数として渡す.
        /// </summary>
        /// <param name="is_re_get">取得出来なかった際にオブジェクトを再生成してから再度取得するか否か</param>
        /// <returns></returns>
        public IReadOnlyPoolData Get(bool is_re_get = false)
        {
            for (int index = 0; index < _poolDataList.Count; index++)
            {
                var data = _poolDataList[index];
                if (data.Memory != MemoryType.Release) continue;
                data.Memory = MemoryType.Wait;

                return data;
            }

            if (!is_re_get) return null;

            ReBuild();

            return Get(is_re_get);
        }

        /// <summary>
        /// 使用を有効化するための処理.
        /// Pool.Get()のあとに呼ぶ,
        /// </summary>
        /// <param name="use_id">Pool.Get()で取得したオブジェクトのID</param>
        public void Set(int use_id)
        {
            var data = _poolDataList[use_id];
            data.Memory = MemoryType.Heap;
            data.ActiveEvent(true);
            data.Object.transform.SetParent(null);
            data.Object.Entry();
        }

        /// <summary>
        /// オブジェクトを使用する際の処理.
        /// 使用だけの場合に推奨.
        /// </summary>
        public void Use()
        {
            var data = Get();
            Set(data.ID);
        }

        //========================================================
        // private method
        //========================================================

        /// <summary>
        /// オブジェクトの再生成
        /// </summary>
        private void ReBuild()
        {
            for (int index = 0; index < CAPACITY; index++)
            {
                _poolID++;

                var clone = Object.Instantiate(_pooledObject);
                clone.Initialize(this, _poolID);
                _poolDataList.Add(new Data(clone, _poolID));
            }
        }

        //========================================================
        // IPoolReleaseCallback interface
        //========================================================

        /// <summary>
        /// リリース時の処理
        /// </summary>
        /// <param name="pool_id"></param>
        void IPoolReleaseCallback.ReleaseCallback(int pool_id)
        {
            var data = _poolDataList[pool_id];
            data.Object.Release();
            data.Object.transform.SetParent(_parent);
            data.ActiveEvent(false);
            data.Memory = MemoryType.Release;
        }

        //========================================================
        // Syste,IDisposable interface
        //========================================================

        /// <summary>
        /// メモリー解放時の処理
        /// </summary>
        void System.IDisposable.Dispose()
        {
            foreach (var data in _poolDataList)
            {
                Object.Destroy(data.Object);
            }

            _poolID = 0;

            System.GC.Collect();
        }
    }
}