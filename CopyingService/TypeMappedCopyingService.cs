using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CopyingServiceLib
{
    public class TypeMappedCopyingService : ICopyingService
    {
        public Dictionary<Type, ICopyingService> TypeCopyingServices
        {
            get => typeCopyingServices;
            set => typeCopyingServices = value != null ? value : new Dictionary<Type, ICopyingService>();
        }

        public Dictionary<Type, TargetPropValueProducingDelegate> TypeValueProducingDelegates
        {
            get => typeValueProducingDelegates;
            set => typeValueProducingDelegates = value != null ? value : new Dictionary<Type, TargetPropValueProducingDelegate>();
        }

        public List<Action<object?, object?>> Actions
        {
            get => actions;
            set => actions = value != null ? value : new List<Action<object?, object?>>();
        }

        public ICopyingService DefaultCopyingService
        {
            get => defaultCopyingService;
            set => defaultCopyingService = value != null ? value : new ValueCopyingService();
        }

        public List<PropertyInfo> NotAffectedProps
        {
            get => notAffectedProps;
            set => notAffectedProps = value != null ? value : new List<PropertyInfo>();
        }

        public object? Copy(object? source, object? target)
        {
            if (source == null)
                return source;
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            if (source.GetType() != target.GetType() || source.GetType() == null)
                throw new ArgumentException();
            var propsArr = source.GetType().GetProperties();
            foreach (var prop in propsArr.Where(p => !NotAffectedProps.Contains(p) && p.GetSetMethod() != null))
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
                var targetPropValue = TypeValueProducingDelegates[prop!.PropertyType]?.Invoke(srcPropValue, oldTargetPropValue, prop!);
                if (targetPropValue == null)
                {
                    prop?.SetValue(target, null);
                    continue;
                }
                var targetPropValueCopied = TypeCopyingServices.TryGetValue(prop!.PropertyType, out var copyingService) ? copyingService.Copy(srcPropValue, targetPropValue) : defaultCopyingService.Copy(srcPropValue, targetPropValue);
                prop?.SetValue(target, targetPropValueCopied);
            }
            foreach (var act in Actions)
                act?.Invoke(source, target);
            return target;
        }

        public TypeMappedCopyingService(
            Dictionary<Type, ICopyingService> propertyCopyingServices = null!,
            Dictionary<Type, TargetPropValueProducingDelegate> propertyValueProducingDelegates = null!,
            IEnumerable<Action<object?, object?>> actions = null!,
            ICopyingService defaultCopyingService = null!,
            IEnumerable<PropertyInfo> notAffectedProps = null!)
        {
            TypeCopyingServices = propertyCopyingServices;
            TypeValueProducingDelegates = propertyValueProducingDelegates;
            Actions = actions.ToList();
            DefaultCopyingService = defaultCopyingService;
            NotAffectedProps = notAffectedProps.ToList();
        }

        private Dictionary<Type, ICopyingService> typeCopyingServices = new();

        private Dictionary<Type, TargetPropValueProducingDelegate> typeValueProducingDelegates = new();

        private List<Action<object?, object?>> actions = new();

        private ICopyingService defaultCopyingService = new ValueCopyingService();

        private List<PropertyInfo> notAffectedProps = new();
    }
}