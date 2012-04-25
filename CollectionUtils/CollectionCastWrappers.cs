using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;

namespace CollectionUtils
{
    internal static class InternalUtils
    {
        public static MethodInfo ResolveGetProp(object o, string p, Type rtype)
        {
            PropertyInfo pi = o.GetType().GetProperty(
                        p, BindingFlags.Instance | BindingFlags.Public,
                        null, rtype, new Type[0], null);
            if (pi != null && pi.CanRead && pi.GetGetMethod() != null)
                return pi.GetGetMethod();
            else
                return null;
        }
    }
        
    public class CollectionCastWrapper<T, Q> : ICollection<T>, ICollection
    {
        private ICollection<Q> realCollection;

        public CollectionCastWrapper(ICollection<Q> l)
        {
            realCollection = l;
        }

        #region ICollection<T> implementation

        public void Add(T item)
        {
            realCollection.Add((Q)(object)item);
        }

        public void Clear()
        {
            realCollection.Clear();
        }

        public bool Contains(T item)
        {
            return realCollection.Contains((Q)(object)item);
        }

        private void DoCopyTo(Array array, int arrayIndex, string idxName)
        {
            if (realCollection is ICollection)
            {
                ((ICollection)realCollection).CopyTo(array, arrayIndex);
            }
            if (array == null)
                throw new ArgumentNullException("array");
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(
                    idxName, arrayIndex, idxName + " must be 0 or greater.");
            if (realCollection.Count > array.GetLength(0) - arrayIndex)
                throw new ArgumentException(
                    "The number of elements in the source s greater than the available " +
                    "space from arrayIndex to the end of the destination array.");
            int i = arrayIndex;
            foreach (Q q in realCollection)
            {
                array.SetValue(q, i);
                i++;
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            DoCopyTo(array, arrayIndex, "arrayIndex");
        }

        public int Count
        {
            get { return realCollection.Count; }
        }

        public bool IsReadOnly
        {
            get { return realCollection.IsReadOnly; }
        }

        public bool Remove(T item)
        {
            return realCollection.Remove((Q)(object)item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return realCollection.Cast<T>().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return realCollection.GetEnumerator();
        }
        #endregion

        #region ICollection implementation

        public bool Contains(object value)
        {
            return realCollection.Contains((Q)value);
        }

        public void Remove(object value)
        {
            realCollection.Remove((Q)value);
        }

        public void CopyTo(Array array, int index)
        {
            DoCopyTo(array, index, "index");
        }

        public bool IsSynchronized
        {
            get
            {
                if (realCollection is ICollection)
                    return ((ICollection)realCollection).IsSynchronized;
                else
                {
                    MethodInfo gf = InternalUtils.ResolveGetProp(realCollection, "IsSynchronized", typeof(bool));
                    if (gf != null)
                        return (bool)gf.Invoke(realCollection, new object[0]);
                    else
                        return false;
                }
            }
        }

        public object SyncRoot
        {
            get
            {
                if (realCollection is ICollection)
                    return ((ICollection)realCollection).SyncRoot;
                else
                {
                    MethodInfo gf = InternalUtils.ResolveGetProp(realCollection, "SyncRoot", typeof(object));
                    if (gf != null)
                        return gf.Invoke(realCollection, new object[0]);
                    else
                        return realCollection;
                }
            }
        }
        #endregion
    }

    public class NonGenericCollectionCastWrapper<T> : ICollection<T>
    {
        private ICollection realCollection;

        public NonGenericCollectionCastWrapper(ICollection l)
        {
            realCollection = l;
        }

        private void ReadOnlyErr()
        {
            throw new NotSupportedException("Collection is read-only.");
        }

        public virtual void Add(T item)
        {
            ReadOnlyErr();
        }

        public virtual void Clear()
        {
            ReadOnlyErr();
        }

        public virtual bool Contains(T item)
        {
            return realCollection.Cast<T>().Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            realCollection.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return realCollection.Count; }
        }

        public virtual bool IsReadOnly
        {
            get { return true; }
        }

        public virtual bool Remove(T item)
        {
            ReadOnlyErr();
            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return realCollection.Cast<T>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return realCollection.GetEnumerator();
        }
    }
}
