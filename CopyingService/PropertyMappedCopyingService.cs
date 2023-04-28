using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CopyingServiceLib
{
    public class PropertyMappedCopyingService<TItem> : ICopyingService<TItem>
    {
        public Dictionary<PropertyInfo, ICopyingService> PropertyCopyingServices 
        { 
            get => propertyCopyingServices;
            set => propertyCopyingServices = value != null ? value : new Dictionary<PropertyInfo, ICopyingService>();
        }

        public Dictionary<PropertyInfo, TargetPropValueProducingDelegate> PropertyValueProducingDelegates 
        { 
            get => propertyValueProducingDelegates;
            set => propertyValueProducingDelegates = value != null ? value : new Dictionary<PropertyInfo, TargetPropValueProducingDelegate>();
        }

        public List<Action<TItem, TItem>> Actions
        {
            get => actions;
            set => actions = value != null ? value : new List<Action<TItem, TItem>>();
        }

        public ICopyingService DefaultCopyingService
        {
            get => defaultCopyingService;
            set => defaultCopyingService = value != null ? value : new ValueCopyingService();
        }

        public TItem? Copy(TItem? source, TItem? target)
        {
            if (source == null)
                return source;
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            var propsArr = PropertyValueProducingDelegates.Select(kvp => kvp.Key);
            foreach (var prop in propsArr.Where(p => p.DeclaringType == typeof(TItem) && p.GetSetMethod() != null))
            {
                if (prop == null)
                    continue;
                var srcPropValue = prop?.GetValue(source);
                var oldTargetPropValue = prop?.GetValue(target);
                if (srcPropValue == null)
                {
                    prop?.SetValue(target, null);
                    continue;
                }
                var targetPropValue = PropertyValueProducingDelegates[prop!]?.Invoke(srcPropValue, oldTargetPropValue, prop!);
                if (targetPropValue == null)
                {
                    prop?.SetValue(target, null);
                    continue;
                }
                var targetPropValueCopied = PropertyCopyingServices.TryGetValue(prop!, out var copyingService) ? copyingService.Copy(srcPropValue, targetPropValue) : defaultCopyingService.Copy(srcPropValue, targetPropValue);
                prop?.SetValue(target, targetPropValueCopied);
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
            if (source.GetType() != typeof(TItem) || target.GetType() != typeof(TItem))
                throw new ArgumentException();
            var result = Copy((TItem)source, (TItem)target);
            if (result == null)
                throw new Exception();
            return result;
        }

        public PropertyMappedCopyingService(
            Dictionary<PropertyInfo, ICopyingService> propertyCopyingServices = null!,
            Dictionary<PropertyInfo, TargetPropValueProducingDelegate> propertyValueProducingDelegates = null!,
            List<Action<TItem, TItem>> actions = null!,
            ICopyingService defaultCopyingService = null!)
        {
            PropertyCopyingServices = propertyCopyingServices;
            PropertyValueProducingDelegates = propertyValueProducingDelegates;
            Actions = actions;
            DefaultCopyingService = defaultCopyingService;
        }

        private Dictionary<PropertyInfo, ICopyingService> propertyCopyingServices = new Dictionary<PropertyInfo, ICopyingService>();

        private Dictionary<PropertyInfo, TargetPropValueProducingDelegate> propertyValueProducingDelegates = new Dictionary<PropertyInfo, TargetPropValueProducingDelegate>();

        private List<Action<TItem, TItem>> actions = new List<Action<TItem, TItem>>();

        private ICopyingService defaultCopyingService = new ValueCopyingService();
    }
}