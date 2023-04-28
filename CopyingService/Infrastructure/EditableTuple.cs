using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CopyingServiceLib.Infrastructure
{
    public class EditableTuple
    {
        public static EditableTuple<T1, T2> Create<T1, T2> (T1 item1, T2 item2)
        {
            return new EditableTuple<T1, T2> 
            { 
                Item1 = item1, 
                Item2 = item2 
            };
        }

        public static EditableTuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
        {
            return new EditableTuple<T1, T2, T3> 
            { 
                Item1 = item1, 
                Item2 = item2, 
                Item3 = item3 
            };
        }

        public static EditableTuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            return new EditableTuple<T1, T2, T3, T4> 
            { 
                Item1 = item1, 
                Item2 = item2, 
                Item3 = item3, 
                Item4 = item4 
            };
        }

        public static EditableTuple<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
        {
            return new EditableTuple<T1, T2, T3, T4, T5> 
            { 
                Item1 = item1, 
                Item2 = item2, 
                Item3 = item3, 
                Item4 = item4, 
                Item5 = item5 
            };
        }

        public static EditableTuple<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
        {
            return new EditableTuple<T1, T2, T3, T4, T5, T6> 
            { 
                Item1 = item1, 
                Item2 = item2, 
                Item3 = item3, 
                Item4 = item4, 
                Item5 = item5, 
                Item6 = item6 
            };
        }

        public static EditableTuple<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
        {
            return new EditableTuple<T1, T2, T3, T4, T5, T6, T7> 
            { 
                Item1 = item1, 
                Item2 = item2, 
                Item3 = item3, 
                Item4 = item4, 
                Item5 = item5, 
                Item6 = item6, 
                Item7 = item7 
            };
        }

        //public static EditableTuple<T1, T2, T3, T4, T5, T6, T7, T8> Create<T1, T2, T3, T4, T5, T6, T7, T8>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8)
        //{
        //    return new EditableTuple<T1, T2, T3, T4, T5, T6, T7, T8> 
        //    { 
        //        Item1 = item1, 
        //        Item2 = item2,
        //        Item3 = item3, 
        //        Item4 = item4, 
        //        Item5 = item5, 
        //        Item6 = item6, 
        //        Item7 = item7, 
        //        Item8 = item8 
        //    };
        //}
    }

    public class EditableTuple<T1, T2> : EditableTuple
    {
        public T1? Item1 { get; set; }

        public T2? Item2 { get; set; }

        public object? this[int index] =>
            index switch
            {
                0 => Item1,
                1 => Item2,

                _ => throw new IndexOutOfRangeException(),
            };

        public Tuple<T1, T2> ToTuple()
        {
            return new Tuple<T1, T2>(Item1, Item2);
        }
    }

    public class EditableTuple<T1, T2, T3> : EditableTuple
    {
        public T1? Item1 { get; set; }

        public T2? Item2 { get; set; }

        public T3? Item3 { get; set; }

        public object? this[int index] =>
            index switch
            {
                0 => Item1,
                1 => Item2,
                2 => Item3,

                _ => throw new IndexOutOfRangeException(),
            };

        public Tuple<T1, T2, T3> ToTuple()
        {
            return new Tuple<T1, T2, T3>(Item1, Item2, Item3);
        }
    }

    public class EditableTuple<T1, T2, T3, T4> : EditableTuple
    {
        public T1? Item1 { get; set; }

        public T2? Item2 { get; set; }

        public T3? Item3 { get; set; }

        public T4? Item4 { get; set; }

        public object? this[int index] =>
            index switch
            {
                0 => Item1,
                1 => Item2,
                2 => Item3,
                3 => Item4,

                _ => throw new IndexOutOfRangeException(),
            };

        public Tuple<T1, T2, T3, T4> ToTuple()
        {
            return new Tuple<T1, T2, T3, T4>(Item1, Item2, Item3, Item4);
        }
    }

    public class EditableTuple<T1, T2, T3, T4, T5> : EditableTuple
    {
        public T1? Item1 { get; set; }

        public T2? Item2 { get; set; }

        public T3? Item3 { get; set; }

        public T4? Item4 { get; set; }

        public T5? Item5 { get; set; }

        public object? this[int index] =>
            index switch
            {
                0 => Item1,
                1 => Item2,
                2 => Item3,
                3 => Item4,
                4 => Item5,

                _ => throw new IndexOutOfRangeException(),
            };

        public Tuple<T1, T2, T3, T4, T5> ToTuple()
        {
            return new Tuple<T1, T2, T3, T4, T5>(Item1, Item2, Item3, Item4, Item5);
        }
    }

    public class EditableTuple<T1, T2, T3, T4, T5, T6> : EditableTuple
    {
        public T1? Item1 { get; set; }

        public T2? Item2 { get; set; }

        public T3? Item3 { get; set; }

        public T4? Item4 { get; set; }

        public T5? Item5 { get; set; }

        public T6? Item6 { get; set; }

        public object? this[int index] =>
            index switch
            {
                0 => Item1,
                1 => Item2,
                2 => Item3,
                3 => Item4,
                4 => Item5,
                5 => Item6,

                _ => throw new IndexOutOfRangeException(),
            };

        public Tuple<T1, T2, T3, T4, T5, T6> ToTuple()
        {
            return new Tuple<T1, T2, T3, T4, T5, T6>(Item1, Item2, Item3, Item4, Item5, Item6);
        }
    }

    public class EditableTuple<T1, T2, T3, T4, T5, T6, T7> : EditableTuple
    {
        public T1? Item1 { get; set; }

        public T2? Item2 { get; set; }
        
        public T3? Item3 { get; set; }
        
        public T4? Item4 { get; set; }
        
        public T5? Item5 { get; set; }
        
        public T6? Item6 { get; set; }
        
        public T7? Item7 { get; set; }

        public object? this[int index] =>
            index switch
            {
                0 => Item1,
                1 => Item2,
                2 => Item3,
                3 => Item4,
                4 => Item5,
                5 => Item6,
                6 => Item7,

                _ => throw new IndexOutOfRangeException(),
            };

        public Tuple<T1, T2, T3, T4, T5, T6, T7> ToTuple()
        {
            return new Tuple<T1, T2, T3, T4, T5, T6, T7>(Item1, Item2, Item3, Item4, Item5, Item6, Item7);
        }
    }

    //public class EditableTuple<T1, T2, T3, T4, T5, T6, T7, T8> : EditableTuple
    //    where T8 : notnull 
    //{
    //    public T1? Item1 { get; set; }
        
    //    public T2? Item2 { get; set; }
        
    //    public T3? Item3 { get; set; }
        
    //    public T4? Item4 { get; set; }
        
    //    public T5? Item5 { get; set; }
        
    //    public T6? Item6 { get; set; }
        
    //    public T7? Item7 { get; set; }
        
    //    public T8 Item8 { get; set; }

    //    public Tuple<T1, T2, T3, T4, T5, T6, T7, T8> ToTuple()
    //    {
    //        return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8>(Item1, Item2, Item3, Item4, Item5, Item6, Item7, Item8);
    //    }
    //}
}
