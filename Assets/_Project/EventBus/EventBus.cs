using System;
using System.Collections.Generic;
using UnityEngine;

public class EventBus 
{
    private EventBusHelper _helper;

    private Dictionary<Type, SubscribersList<IGlobalSubscriber>> s_Subscribers
            = new Dictionary<Type, SubscribersList<IGlobalSubscriber>>();

    public EventBus()
    {
        _helper = new EventBusHelper();
    }

    public void Subscribe(IGlobalSubscriber subscriber)
    {
        List<Type> subscriberTypes = _helper.GetSubscriberTypes(subscriber);
        foreach (Type t in subscriberTypes)
        {
            if (!s_Subscribers.ContainsKey(t))
            {
                s_Subscribers[t] = new SubscribersList<IGlobalSubscriber>();
            }
            s_Subscribers[t].Add(subscriber);
        }
    }
    public void Unsubscribe(IGlobalSubscriber subscriber)
    {
        List<Type> subscriberTypes = _helper.GetSubscriberTypes(subscriber);
        foreach (Type t in subscriberTypes)
        {
            if (s_Subscribers.ContainsKey(t))
                s_Subscribers[t].Remove(subscriber);
        }
    }

    public void RaiseEvent<TSubscriber>(Action<TSubscriber> action)
        where TSubscriber : class, IGlobalSubscriber
    {
        SubscribersList<IGlobalSubscriber> subscribers = s_Subscribers[typeof(TSubscriber)];

        subscribers.Executing = true;
        foreach (IGlobalSubscriber subscriber in subscribers.List)
        {
            try
            {
                action.Invoke(subscriber as TSubscriber);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        subscribers.Executing = false;
        subscribers.Cleanup();
    }
}
