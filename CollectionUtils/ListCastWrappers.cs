using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;

namespace CollectionUtils
{
    public class ListCastWrapper<T, Q> : CollectionCastWrapper<T,Q>, IList<T>, IList
    {
        private IList<Q> realList;

        public ListCastWrapper(IList<Q> l)
            : base(l)
        {
            realList = l;
        }

        #region IList<T> implementation
        public int IndexOf(T item)
        {
            return realList.IndexOf((Q)(object)item);
        }

        public void Insert(int index, T item)
        {
            realList.Insert(index, (Q)(object)item);
        }

        public void RemoveAt(int index)
        {
            realList.RemoveAt(index);
        }

        public T this[int index]
        {
            get
            {
                return (T)(object)realList[index];
            }
            set
            {
                realList[index] = (Q)(object)value;
            }
        }
        #endregion

        #region IList implementation

        public int Add(object value)
        {
            realList.Add((Q)value);
            return realList.Count - 1;
        }

        public int IndexOf(object value)
        {
            return ((IList<Q>)realList).IndexOf((Q)value);
        }

        public void Insert(int index, object value)
        {
            ((IList<Q>)realList).Insert(index, (Q)value);
        }

        public bool IsFixedSize
        {
            get
            {
                if (realList is IList)
                    return ((IList)realList).IsFixedSize;
                else
                {
                    MethodInfo gf = InternalUtils.ResolveGetProp(realList, "IsFixedSize", typeof(bool));
                    if (gf != null)
                        return (bool)gf.Invoke(realList, new object[0]);
                    else
                        return false;
                }
            }
        }

        object IList.this[int index]
        {
            get
            {
                return realList[index];
            }
            set
            {
                realList[index] = (Q)value;
            }
        }
        #endregion
    }

    public class NonGenericListCastWrapper<T> : NonGenericCollectionCastWrapper<T>, IList<T>
    {
        private IList realList;

        public NonGenericListCastWrapper(IList l)
            : base(l)
        {
            realList = l;
        }

        #region IList<T> implementation

        public int IndexOf(T item)
        {
            return realList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            realList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            realList.RemoveAt(index);
        }

        public T this[int index]
        {
            get
            {
                return (T)realList[index];
            }
            set
            {
                realList[index] = value;
            }
        }

        #endregion

        #region ICollection<T> non-readonly overrides

        public override void Add(T item)
        {
            realList.Add(item);
        }

        public override void Clear()
        {
            realList.Clear();
        }

        public override bool Contains(T item)
        {
            return realList.Contains(item);
        }

        public override bool IsReadOnly
        {
            get { return realList.IsReadOnly; }
        }

        public override bool Remove(T item)
        {
            realList.Remove(item);
            return true;
        }

        #endregion
    } 
}
