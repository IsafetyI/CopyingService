using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CopyingServiceLib
{
    public class InstanceBreakingCopyingService<TItem> : ICopyingService<TItem>
    {
        public List<PropertyInfo> NotAffectedProps
        {
            get => notAffectedProps;
            set => notAffectedProps = value != null ? value : new List<PropertyInfo>();
        }

        public List<Action<TItem, TItem>> Actions
        {
            get => actions;
            set => actions = value != null ? value : new List<Action<TItem, TItem>>();
        }

        public InstanceBreakingCopyingService(
            IEnumerable<PropertyInfo> notAffectedProps = null!, 
            IEnumerable<Action<TItem, TItem>> actions = null!)
        {
            NotAffectedProps = notAffectedProps?.ToList()!;
            Actions = actions?.ToList()!;
        }

        public TItem? Copy(TItem? source, TItem? target)
        {
            if (source == null)
                return source;
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            var propsArr = typeof(TItem).GetProperties();
            foreach (var prop in propsArr.Where(p => !NotAffectedProps.Contains(p) && p.GetSetMethod() != null))
            {
                prop?.SetValue(target, prop?.GetValue(source));
            }
            foreach (var act in Actions)
            {
                act?.Invoke(source, target);
            }
            return target;
        }

        public object? Copy(object? source, object? target)
        {
            if (source == null)
                return source;
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            if (!source.GetType().IsAssignableTo(typeof(TItem)) || !target.GetType().IsAssignableTo(typeof(TItem)))
                throw new ArgumentException($"source type: {source.GetType()} \n target type: {target.GetType()}\n one of them isn't assignable to \n TItem type: {typeof(TItem)}");
            var result = Copy((TItem)source, (TItem)target);
            if (result == null)
                throw new Exception();
            return result;
        }

        private List<PropertyInfo> notAffectedProps = new List<PropertyInfo>();

        private List<Action<TItem, TItem>> actions = new List<Action<TItem, TItem>>();
    }
}
