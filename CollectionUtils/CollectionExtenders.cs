using System.Collections.Generic;
using System.Collections;

namespace CollectionUtils.Extenders
{
    public static class CollectionExtenders
    {
        public static CollectionCastWrapper<TResult, TIn>
            CastCollection<TResult, TIn>(this ICollection<TIn> c)
        {
            return new CollectionCastWrapper<TResult, TIn>(c);
        }

        public static NonGenericCollectionCastWrapper<TResult>
            CastCollection<TResult>(this ICollection c)
        {
            return new NonGenericCollectionCastWrapper<TResult>(c);
        }

        public static ListCastWrapper<TResult, TIn>
            CastList<TResult, TIn>(this IList<TIn> l)
        {
            return new ListCastWrapper<TResult, TIn>(l);
        }

        public static NonGenericListCastWrapper<TResult>
            CastList<TResult>(this IList l)
        {
            return new NonGenericListCastWrapper<TResult>(l);
        }
    }
}