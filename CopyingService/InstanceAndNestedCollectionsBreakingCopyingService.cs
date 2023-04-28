using System.Collections.Immutable;
using System.Reflection;

namespace CopyingServiceLib
{
    public class InstanceAndNestedCollectionsBreakingCopyingService<TItem> : ICopyingService<TItem>
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

        public Dictionary<PropertyInfo, TargetPropValueProducingDelegate> TargetPropertyValueProducers 
        { 
            get => targetPropertyValueProducers; 
            set => targetPropertyValueProducers = value != null ? value : new Dictionary<PropertyInfo, TargetPropValueProducingDelegate>();
        }

        public Dictionary<PropertyInfo, ICopyingService> CollectionsCopyingServices 
        { 
            get => collectionsCopyingServices; 
            set => collectionsCopyingServices = value != null ? value : new Dictionary<PropertyInfo, ICopyingService>();
        }

        public ICopyingService DefaultCollectionCopyingService 
        { 
            get => defaultCollectionCopyingService;
            set => defaultCollectionCopyingService = value != null ? value : new CollectionBreakingCopyingService();
        }

        public TargetPropValueProducingDelegate DefaultTargetPropertyValueProducer 
        { 
            get => defaultTargetPropertyValueProducer;
            set => defaultTargetPropertyValueProducer = value != null ? value : defaultTargetPropertyValueProducerConst;
        }

        public TItem? Copy(TItem? source, TItem? target)
        {
            if (source == null)
                return source;
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            var propsArr = typeof(TItem).GetProperties();
            foreach (var prop in propsArr.Where(p => !NotAffectedProps.Contains(p)))
            {
                var sourceCollectionInst = prop?.GetValue(source);
                var targetCollectionInst = prop?.GetValue(target);
                var newTargetCollectionInst = getProducingDelegate(prop!).Invoke(sourceCollectionInst, targetCollectionInst, prop!);
                var b = (newTargetCollectionInst == targetCollectionInst) && prop.GetSetMethod() == null;
                if (prop.GetSetMethod() != null || (newTargetCollectionInst == targetCollectionInst))
                {
                    if (prop == null)
                        continue;
                    if (!prop.PropertyType.GetInterfaces()
                        .Any(i =>
                        i.IsGenericType &&
                        i.GetGenericTypeDefinition() == typeof(ICollection<>)))
                    {
                        prop?.SetValue(target, prop?.GetValue(source));
                        continue;
                    }
                    object? newTargetPropValue = newTargetCollectionInst;

                    if (CollectionsCopyingServices.ContainsKey(prop!))
                    {
                        var newVal = CollectionsCopyingServices[prop!].Copy(sourceCollectionInst!, newTargetPropValue!);
                        if(!b)
                            prop!.SetValue(target, newVal);
                        continue;
                    }
                    var newVal1 = DefaultCollectionCopyingService.Copy(sourceCollectionInst!, newTargetPropValue!);
                    if (!b)
                        prop!.SetValue(target, newVal1);
                }
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
            if (!source.GetType().IsAssignableTo(typeof(TItem))  || !target.GetType().IsAssignableTo(typeof(TItem)))
                throw new ArgumentException($"source type: {source.GetType()} \n target type: {target.GetType()}\n one of them isn't assignable to \n TItem type: {typeof(TItem)}");
            var result = Copy((TItem)source, (TItem)target);
            if (result == null)
                throw new Exception();
            return result;
        }

        public InstanceAndNestedCollectionsBreakingCopyingService(
            IEnumerable<PropertyInfo> notAffectedProps = null!,
            IEnumerable<Action<TItem, TItem>> actions = null!,
            Dictionary<PropertyInfo, TargetPropValueProducingDelegate> targetPropertyValueProducers = null!,
            TargetPropValueProducingDelegate defaultTargetPropertyValueProducer = null!,
            Dictionary<PropertyInfo, ICopyingService> collectionsCopyingServices = null!,
            ICopyingService defaultCollectionCopyingService = null!)
        {
            NotAffectedProps = notAffectedProps != null ? notAffectedProps.ToList() :  new();
            Actions = actions != null ? actions.ToList() : new();
            TargetPropertyValueProducers = targetPropertyValueProducers;
            DefaultTargetPropertyValueProducer = defaultTargetPropertyValueProducer;
            CollectionsCopyingServices = collectionsCopyingServices;
            DefaultCollectionCopyingService = defaultCollectionCopyingService;
        }

        private List<PropertyInfo> notAffectedProps = new List<PropertyInfo>();

        private List<Action<TItem, TItem>> actions = new List<Action<TItem, TItem>>();

        private Dictionary<PropertyInfo, TargetPropValueProducingDelegate> targetPropertyValueProducers = new Dictionary<PropertyInfo, TargetPropValueProducingDelegate>();

        private Dictionary<PropertyInfo, ICopyingService> collectionsCopyingServices = new Dictionary<PropertyInfo, ICopyingService>();

        private ICopyingService defaultCollectionCopyingService = new CollectionBreakingCopyingService();

        private TargetPropValueProducingDelegate defaultTargetPropertyValueProducer = defaultTargetPropertyValueProducerConst;

        private static readonly TargetPropValueProducingDelegate defaultTargetPropertyValueProducerConst = new TargetPropValueProducingDelegate((sourcePropertyValue, targetPropertyValue, propInfo) =>
        {
            try
            {
                var inst = Activator.CreateInstance(propInfo.PropertyType);
                if (inst != null)
                    return inst;
                else
                    throw new Exception("If TItem class not implements new() you should " +
                        "specify your own tItemProducingFunc in constructor");
            }
            catch (Exception)
            {
                throw new Exception("If TItem class not implements new() you should " +
                    "specify your own tItemProducingFunc in constructor");
            }
        });

        protected TargetPropValueProducingDelegate getProducingDelegate (PropertyInfo? prop)
        {
            if (prop == null)
                return defaultTargetPropertyValueProducer;
            if (TargetPropertyValueProducers?.ContainsKey(prop!) == true)
                return TargetPropertyValueProducers[prop!];
            return defaultTargetPropertyValueProducer;
        }
    }
}
