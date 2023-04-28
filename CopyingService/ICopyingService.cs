using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyingServiceLib
{
    public interface ICopyingService
    {
        public object? Copy(object? source, object? target);
    }

    public interface ICopyingService<TItem> : ICopyingService
    {
        public TItem? Copy(TItem? source, TItem? target);
    }
}
