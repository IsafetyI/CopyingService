using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CopyingServiceLib
{
    //схема наследования спорная, надо тестить
    public class CollectionBreakingCopyingService : ICopyingService
    {
        public Func<Type, object, object> DefaultTItemProducingFunc
        {
            get => defaultTItemProducingFunc;
            set => defaultTItemProducingFunc = (value != null) ? value : defaultTItemProducingFunc;
        }

        public Dictionary<Type, Func<Type, object, object>> ItemProducers
        {
            get => itemProducers;
            set => itemProducers = value != null ? value : new Dictionary<Type, Func<Type, object, object>>();
        }

        public List<Action<object, object>> Actions
        {
            get => actions;
            set => actions = (value != null) ? value : new List<Action<object, object>>();
        }

        public ICopyingService ItemCopyingService
        {
            get => itemCopyingService;
            set => itemCopyingService = (value != null) ? value : new ValueCopyingService();
        }

        public virtual object? Copy(object? source, object? target)
        {
            if (source == null)
                return source;
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            if (!source.GetType().GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollection<>)).Any() || 
                !target.GetType().GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollection<>)).Any())
                throw new ArgumentException();
            Type? tItem;
            if ((tItem = source.GetType().GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollection<>)).FirstOrDefault()?.GetGenericArguments().FirstOrDefault()) != 
                target.GetType().GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollection<>)).FirstOrDefault()?.GetGenericArguments().FirstOrDefault())
                throw new Exception();
            if (tItem == null)
                throw new Exception();
            var genericMethod = typeof(CollectionBreakingCopyingService)
                .GetMethods()
                .First(m => m.IsGenericMethod && m.Name == nameof(Copy))
                .MakeGenericMethod(new Type[] { tItem });
            if (genericMethod == null)
                throw new Exception();
            object? result;
            if ((result = genericMethod.Invoke(this, new object[] { source, target })) == null)
                throw new Exception();
            return result;
        }

        public ICollection<TItem>? Copy<TItem>(ICollection<TItem>? source, ICollection<TItem>? target)
        {
            if (source == null)
                return source;
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            if (!source.GetType().IsAssignableTo(typeof(ICollection<TItem>)) || !target.GetType().IsAssignableTo(typeof(ICollection<TItem>)))
                throw new ArgumentException($"source type: {source.GetType()} \n target type: {target.GetType()}\n one of them isn't assignable to \n TItem type: {typeof(ICollection<TItem>)}");
            foreach (var item in target)
            {
                target.Remove(item);
            }
            foreach (var item in source)
            {
                if (item == null)
                {
                    target.Add(item);
                    continue;
                }
                var itemCopyingTarget = getProducingFunc(item.GetType()).Invoke(item.GetType(), item);
                if (itemCopyingTarget == null)
                    throw new Exception();
                var itemCopyingResult = (TItem?)ItemCopyingService.Copy(item, itemCopyingTarget);
                if (itemCopyingResult == null)
                    throw new Exception();
                target.Add(itemCopyingResult);
            }
            foreach (var act in Actions)
            {
                act?.Invoke(source, target);
            }
            return target;
        }

        public CollectionBreakingCopyingService(
            ICopyingService itemCopyingService = null!, 
            IEnumerable<Action<object, object>> actions = null!, 
            Func<Type, object, object> defaultTItemProducingFunc = null!,
            Dictionary<Type, Func<Type, object, object>> itemProducers = null!)
        {
            ItemCopyingService = itemCopyingService;
            Actions = actions?.ToList()!;
            DefaultTItemProducingFunc = defaultTItemProducingFunc;
            ItemProducers = itemProducers;
        }

        private ICopyingService itemCopyingService = new ValueCopyingService();

        private Dictionary<Type, Func<Type, object, object>> itemProducers = new Dictionary<Type, Func<Type, object, object>>();

        protected static readonly Func<Type, object, object> defaultTItemProducingFuncConst = new Func<Type, object, object>((sourceCollectionItemType, sourceCollectionItem) => {
            try
            {
                var inst = Activator.CreateInstance(sourceCollectionItemType);
                if (inst != null)
                    return inst;
                else
                    throw new Exception($"If {sourceCollectionItemType} class not implements new() you should " +
                        "specify your own tItemProducingFunc in constructor");
            }
            catch (Exception)
            {
                throw new Exception($"If {sourceCollectionItemType} class not implements new() you should " +
                    "specify your own tItemProducingFunc in constructor");
            }
        });

        private Func<Type, object, object> defaultTItemProducingFunc = defaultTItemProducingFuncConst;

        private List<Action<object, object>> actions = new List<Action<object, object>>();

        protected Func<Type, object, object> getProducingFunc(Type? type)
        {
            if (type == null)
                return DefaultTItemProducingFunc;
            if (ItemProducers?.ContainsKey(type!) == true)
                return ItemProducers[type!];
            return DefaultTItemProducingFunc;
        }
    }






















    public class CollectionBreakingCopyingService<TGenericCollection, TItem> : CollectionBreakingCopyingService, ICopyingService<TGenericCollection>
        where TGenericCollection : ICollection<TItem>
    {
        public new ICopyingService ItemCopyingService 
        { 
            get => itemCopyingService; 
            set => itemCopyingService = (value != null) ? value : new ValueCopyingService(); 
        }

        public new Func<TItem, TItem> DefaultTItemProducingFunc
        { 
            get => defaultTItemProducingFunc;
            set => defaultTItemProducingFunc = (value != null) ? value : defaultTItemProducingFuncConst;
        }

        public Dictionary<Type, Func<TItem, TItem>> ItemProducers
        {
            get => itemProducers;
            set => itemProducers = (value != null) ? value : itemProducers;
        }

        private Dictionary<Type, Func<TItem, TItem>> itemProducers = new();

        public new List<Action<TGenericCollection, TGenericCollection>> Actions 
        { 
            get => actions; 
            set => actions = (value != null) ? value : new List<Action<TGenericCollection, TGenericCollection>>();
        }

        public override object Copy(object? source, object? target)
        {
            if (source == null || target == null)
                throw new ArgumentNullException();
            if (source.GetType() != typeof(TGenericCollection) || target.GetType() != typeof(TGenericCollection))
                throw new ArgumentException();
            var result = Copy((TGenericCollection)source, (TGenericCollection)target);
            if (result == null)
                throw new Exception();
            return result;
        }

        public TGenericCollection? Copy(TGenericCollection? source, TGenericCollection? target)
        {
            if (source == null || target == null)
                throw new ArgumentNullException();
            if (source.GetType() != typeof(TGenericCollection) || target.GetType() != typeof(TGenericCollection))
                throw new ArgumentException();
            target.Clear();
            foreach (var item in source)
            {
                if (item == null)
                {
                    target.Add(item);
                    continue;
                }
                var itemCopyingTarget = getProducingFunc(item.GetType()).Invoke(item);
                if (itemCopyingTarget == null)
                    throw new Exception();
                var itemCopyingResult = (TItem?)ItemCopyingService.Copy(item, itemCopyingTarget);
                if (itemCopyingResult == null)
                    throw new Exception();
                target.Add(itemCopyingResult);
            }
            foreach (var act in Actions)
            {
                act?.Invoke(source, target);
            }
            return target;
        }

        public CollectionBreakingCopyingService(
            ICopyingService itemCopyingService = null!, 
            IEnumerable<Action<TGenericCollection, TGenericCollection>> actions = null!, 
            Func<TItem, TItem> tItemProducingFunc = null!,
            Dictionary<Type, Func<TItem, TItem>> itemProducers = null!)
        {
            ItemCopyingService = itemCopyingService;
            Actions = actions?.ToList()!;
            DefaultTItemProducingFunc = tItemProducingFunc;
            ItemProducers = itemProducers;
        }

        private ICopyingService itemCopyingService = new ValueCopyingService();
        
        private Func<TItem, TItem> defaultTItemProducingFunc = defaultTItemProducingFuncConst;

        private List<Action<TGenericCollection, TGenericCollection>> actions = new();

        protected static new readonly Func<TItem, TItem> defaultTItemProducingFuncConst = new Func<TItem, TItem>((sourceCollectionItem) => {
            try
            {
                var inst = Activator.CreateInstance(sourceCollectionItem?.GetType());
                if (inst != null)
                    return (TItem)inst;
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

        protected Func<TItem, TItem> getProducingFunc(Type? type)
        {
            if (type == null)
                return DefaultTItemProducingFunc;
            if (ItemProducers?.ContainsKey(type!) == true)
                return ItemProducers[type!];
            return DefaultTItemProducingFunc;
        }
    }
}
