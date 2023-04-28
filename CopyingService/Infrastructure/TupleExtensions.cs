namespace CopyingServiceLib.Infrastructure
{
    public static class TupleExtensions
    {
        public static EditableTuple<T1, T2> ToEditable<T1, T2>(this Tuple<T1, T2> tuple)
        {
            return new EditableTuple<T1, T2> 
            { 
                Item1 = tuple.Item1, 
                Item2 = tuple.Item2 
            };
        }

        public static EditableTuple<T1, T2, T3> ToEditable<T1, T2, T3>(this Tuple<T1, T2, T3> tuple)
        {
            return new EditableTuple<T1, T2, T3> 
            { 
                Item1 = tuple.Item1, 
                Item2 = tuple.Item2, 
                Item3 = tuple.Item3 
            };
        }

        public static EditableTuple<T1, T2, T3, T4> ToEditable<T1, T2, T3, T4>(this Tuple<T1, T2, T3, T4> tuple)
        {
            return new EditableTuple<T1, T2, T3, T4> 
            { 
                Item1 = tuple.Item1, 
                Item2 = tuple.Item2, 
                Item3 = tuple.Item3, 
                Item4 = tuple.Item4 
            };
        }

        public static EditableTuple<T1, T2, T3, T4, T5> ToEditable<T1, T2, T3, T4, T5>(this Tuple<T1, T2, T3, T4, T5> tuple)
        {
            return new EditableTuple<T1, T2, T3, T4, T5> 
            { 
                Item1 = tuple.Item1, 
                Item2 = tuple.Item2, 
                Item3 = tuple.Item3, 
                Item4 = tuple.Item4, 
                Item5 = tuple.Item5 
            };
        }

        public static EditableTuple<T1, T2, T3, T4, T5, T6> ToEditable<T1, T2, T3, T4, T5, T6>(this Tuple<T1, T2, T3, T4, T5, T6> tuple)
        {
            return new EditableTuple<T1, T2, T3, T4, T5, T6> 
            { 
                Item1 = tuple.Item1, 
                Item2 = tuple.Item2, 
                Item3 = tuple.Item3, 
                Item4 = tuple.Item4, 
                Item5 = tuple.Item5, 
                Item6 = tuple.Item6 
            };
        }

        public static EditableTuple<T1, T2, T3, T4, T5, T6, T7> ToEditable<T1, T2, T3, T4, T5, T6, T7>(this Tuple<T1, T2, T3, T4, T5, T6, T7> tuple)
        {
            return new EditableTuple<T1, T2, T3, T4, T5, T6, T7> 
            { 
                Item1 = tuple.Item1, 
                Item2 = tuple.Item2, 
                Item3 = tuple.Item3, 
                Item4 = tuple.Item4, 
                Item5 = tuple.Item5, 
                Item6 = tuple.Item6, 
                Item7 = tuple.Item7 
            };
        }

        //public static EditableTuple<T1, T2, T3, T4, T5, T6, T7, T8> ToEditable<T1, T2, T3, T4, T5, T6, T7, T8>(this Tuple<T1, T2, T3, T4, T5, T6, T7, T8> tuple)
        //{
        //    return new EditableTuple<T1, T2, T3, T4, T5, T6, T7, T8> 
        //    { 
        //        Item1 = tuple.Item1, 
        //        Item2 = tuple.Item2, 
        //        Item3 = tuple.Item3, 
        //        Item4 = tuple.Item4, 
        //        Item5 = tuple.Item5, 
        //        Item6 = tuple.Item6, 
        //        Item7 = tuple.Item7, 
        //        Item8 = tuple.Rest 
        //    };
        //}
    }
}
