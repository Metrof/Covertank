using System;
using System.Collections.Generic;
using System.Linq;

internal class EventBusHelper
{
    private Dictionary<Type, List<Type>> s_CashedSubscriberTypes =
        new Dictionary<Type, List<Type>>();

    public List<Type> GetSubscriberTypes(
        IGlobalSubscriber globalSubscriber)
    {
        Type type = globalSubscriber.GetType();
        if (s_CashedSubscriberTypes.ContainsKey(type))
        {
            return s_CashedSubscriberTypes[type];
        }

        List<Type> subscriberTypes = type
            .GetInterfaces()
            .Where(t => t.GetInterfaces()
                .Contains(typeof(IGlobalSubscriber)))
            .ToList();

        s_CashedSubscriberTypes[type] = subscriberTypes;
        return subscriberTypes;
    }
}
