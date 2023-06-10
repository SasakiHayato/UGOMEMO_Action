using System.Collections.Generic;
using UnityEngine;
using PoolSystem.Event;

namespace PoolSystem.Event
{
    /// <summary>
    /// Object�̕\���𑀍삷��C���^�[�t�F�[�X
    /// </summary>
    public interface IPoolActive
    {
        /// <summary>
        /// �\���A��\���̂��тɌĂ΂��
        /// </summary>
        /// <param name="is_active">[TRUE: �\��][FALSE: ��\��]</param>
        void Active(bool is_active);
    }

    /// <summary>
    /// �g�p�I�����̃R�[���o�b�N�C���^�[�t�F�[�X
    /// </summary>
    public interface IPoolReleaseCallback
    {
        /// <summary>
        /// �R�[���o�b�N�����ۂ̏���
        /// </summary>
        /// <param name="pool_id">�����[�X����I�u�W�F�N�g��ID</param>
        void ReleaseCallback(int pool_id);
    }
}

namespace PoolSystem
{
    /// <summary>
    /// �v�[����L���ɂ��邽�߂̃C���^�[�t�F�[�X
    /// </summary>
    public interface IPool
    {
        /// <summary>
        /// ������.
        /// </summary>
        /// <param name="release_callback">�����[�X����ۂ̃R�[���o�b�N�C���^�[�t�F�[�X</param>
        /// <param name="pool_id">���g��ID</param>
        void Initialize(IPoolReleaseCallback release_callback, int pool_id);

        /// <summary>
        /// �g�p�J�n���̏���.
        /// UnityEngine.OnEnable()
        /// </summary>
        void Entry();

        /// <summary>
        /// �g�p�I�����̏���.
        /// UnityEngine.OnDisable()
        /// </summary>
        void Release();
    }

    /// <summary>
    /// �I�u�W�F�N�g�̍ė��p����L���ɂ��邽�߂̔ėp�N���X.
    /// ObjectPool�p�^�[��.
    /// </summary>
    /// <typeparam name="PooledObject">IPool�C���^�[�t�F�[�X���p������Component�N���X</typeparam>
    public class Pool<PooledObject> : IPoolReleaseCallback, System.IDisposable where PooledObject : Component, IPool
    {
        //========================================================
        // public interface
        //========================================================

        /// <summary>
        /// Pool.Data��ǂݍ��ݐ�p�ɂ��ĊO���Ɍ��J���邽�߂̃C���^�[�t�F�[�X
        /// </summary>
        public interface IReadOnlyPoolData
        {
            /// <summary>
            /// ���g��ID
            /// </summary>
            int ID { get; }
            /// <summary>
            /// �I�u�W�F�N�g
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
        /// �e�v�[���I�u�W�F�N�g�̃f�[�^�N���X
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
            /// ������
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
            /// �\���A��\���C�x���g
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
        /// ������
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
        /// Pool����I�u�W�F�N�g���擾����ׂ̏���.
        /// �g�p����O�ɓƎ��̊֐����Ăт����ꍇ�ɐ���.
        /// �g�p����Pool.Set()��ID�������Ƃ��ēn��.
        /// </summary>
        /// <param name="is_re_get">�擾�o���Ȃ������ۂɃI�u�W�F�N�g���Đ������Ă���ēx�擾���邩�ۂ�</param>
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
        /// �g�p��L�������邽�߂̏���.
        /// Pool.Get()�̂��ƂɌĂ�,
        /// </summary>
        /// <param name="use_id">Pool.Get()�Ŏ擾�����I�u�W�F�N�g��ID</param>
        public void Set(int use_id)
        {
            var data = _poolDataList[use_id];
            data.Memory = MemoryType.Heap;
            data.ActiveEvent(true);
            data.Object.transform.SetParent(null);
            data.Object.Entry();
        }

        /// <summary>
        /// �I�u�W�F�N�g���g�p����ۂ̏���.
        /// �g�p�����̏ꍇ�ɐ���.
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
        /// �I�u�W�F�N�g�̍Đ���
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
        /// �����[�X���̏���
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
        /// �������[������̏���
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