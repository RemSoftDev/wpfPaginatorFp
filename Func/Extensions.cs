using System;
using System.Collections.Generic;

namespace Func
{
    public static class Extensions
    {
        public static TRes
            PipeForward
            <TArg, TRes>(
                this TArg arg,
                Func<TArg, TRes> func)
        {
            return func(arg);
        }

        public static TRes
            Apply
            <TArg, TRes>(
                this Func<TArg, TRes> func,
                TArg arg)
        {
            return func(arg);
        }

        public static Func<TArg2, TRes>
            Curry
            <TArg1, TArg2, TRes>(
                this Func<TArg1, TArg2, TRes> func,
                TArg1 arg1)
        {
            return arg2 => func(
                arg1,
                arg2);
        }

        public static Func<TArg2, Func<TArg3, TRes>>
            Curry
            <TArg1, TArg2, TArg3, TRes>(
                this Func<TArg1, TArg2, TArg3, TRes> func,
                TArg1 arg1)
        {
            return arg2 => arg3 => func(
                arg1,
                arg2,
                arg3);
        }

        public static IEnumerable<T>
            With
            <T>(
                this IEnumerable<T> collection,
                IEnumerable<T> withCollection)
        {
            var enumerable = new List<T>(collection);
            enumerable.AddRange(withCollection);

            return enumerable;
        }

        public static PaginatorState
            With(
                this PaginatorState state,
                params Action<PaginatorState>[] mutators)
        {
            foreach (var item in mutators)
            {
                item(state);
            }

            return new PaginatorState(state);
        }
    }
}
