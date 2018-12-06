﻿using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Aspects.Dependencies;
using PostSharp.Constraints.Internals;
using PostSharp.Extensibility;
using PostSharp.Patterns.Model;
using PostSharp.Reflection;
using PostSharp.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace XJK.NotifyProperty
{
    /// <summary>
    /// 订阅 Property 的 PropertyChanged
    /// </summary>
    [AspectRoleDependency(AspectDependencyAction.Order, AspectDependencyPosition.After, "Tracing")]
    [AspectRoleDependency(AspectDependencyAction.Order, AspectDependencyPosition.After, "Threading")]
    [AspectRoleDependency(AspectDependencyAction.Order, AspectDependencyPosition.After, "Validation")]
    [AspectRoleDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, "Caching")]
    [AspectTypeDependency(AspectDependencyAction.Order, AspectDependencyPosition.After, typeof(AggregatableAttribute))]
    [AspectTypeDependency(AspectDependencyAction.Order, AspectDependencyPosition.After, typeof(NotifyPropertyChangedAttribute))]
    [HasConstraint]
    [IntroduceInterface(typeof(INotifyPropertyChanged), OverrideAction = InterfaceOverrideAction.Ignore, AncestorOverrideAction = InterfaceOverrideAction.Ignore)]
    [MulticastAttributeUsage(MulticastTargets.Class, Inheritance = MulticastInheritance.Strict, PersistMetaData = true, AllowMultiple = false, TargetTypeAttributes = MulticastAttributes.UserGenerated)]
    [PSerializable]
    public class NestedNotifyPropertyChangedAttribute : InstanceLevelAspect, INotifyPropertyChanged
    {
        private Dictionary<object, PropertyChangedEventHandler> HandlerDict = new Dictionary<object, PropertyChangedEventHandler>();

        [IntroduceMember(OverrideAction = MemberOverrideAction.Ignore)]
        public event PropertyChangedEventHandler PropertyChanged;
        
        [ImportMember(nameof(OnPropertyChanged), IsRequired = true, Order = ImportMemberOrder.BeforeIntroductions)]
        public Action<PropertyChangedEventArgs> OnPropertyChangedMethod;

        [IntroduceMember(OverrideAction = MemberOverrideAction.Ignore, Visibility = Visibility.Family)]
        public void OnPropertyChanged(PropertyChangedEventArgs arg) => OnPropertyChangedMethod(arg);

        [OnLocationSetValueAdvice, MethodPointcut(nameof(SelectINotifyPropertyChanged))]
        public void OnSetProperty_INotifyPropertyChanged(LocationInterceptionArgs args)
        {
            if (ReferenceEquals(args.Value, args.GetCurrentValue())) return;

            var oldValue = args.GetCurrentValue();
            if (oldValue is INotifyPropertyChanged oldNotifyProperty)
            {
                var handler = HandlerDict[oldValue];
                oldNotifyProperty.PropertyChanged -= handler;
                HandlerDict.Remove(oldValue);
            }

            args.ProceedSetValue();

            if (args.Value is INotifyPropertyChanged notifyProperty)
            {
                var handler = MakeNestedPropertyChangedHandler(args.LocationName);
                notifyProperty.PropertyChanged += handler;
                HandlerDict.Add(args.Value, handler);
            }
        }

        private PropertyChangedEventHandler MakeNestedPropertyChangedHandler(string PropertyName)
        {
            return (sender, e) =>
            {
                OnPropertyChanged(new NestedPropertyChangedEventArgs(PropertyName, e));
            };
        }
        
        // Selector
        private IEnumerable<PropertyInfo> SelectINotifyPropertyChanged(Type type)
        {
            const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public;
            return from property in type.GetProperties(bindingFlags)
                   where property.CanWrite
                        && !IsDefined(property, typeof(IgnoreAutoChangeNotificationAttribute))
                        && !IsDefined(property, typeof(ParentAttribute))
                        && !IsDefined(property, typeof(ReferenceAttribute))
                   select property;
        }
    }
}