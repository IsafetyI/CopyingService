using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyingServiceLib
{
    public class DelegateCopyingService<TItem> : ICopyingService<TItem>
    {
        public Func<TItem, TItem, TItem> CopyingDelegate 
        { 
            get => copyingDelegate; 
            set
            {
                if (value != null)
                    copyingDelegate = value;
                else
                    throw new ArgumentNullException(nameof(CopyingDelegate));
            }
        }

        public TItem? Copy(TItem? source, TItem? target)
        {
            return CopyingDelegate.Invoke(source, target);
        }

        public object? Copy(object? source, object? target)
        {
            if (source.GetType() != typeof(TItem) || target.GetType() != typeof(TItem))
                throw new ArgumentException();
            var result = Copy((TItem)source, (TItem)target);
            if (result == null)
                throw new Exception();
            return result;
        }

        public DelegateCopyingService(Func<TItem, TItem, TItem> copyingDelegate)
        {
            CopyingDelegate = copyingDelegate;
        }

        private Func<TItem, TItem, TItem> copyingDelegate = null!;
    }
}
