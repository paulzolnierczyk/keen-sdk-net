﻿using Keen.Core;
using Keen.Core.EventCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keen.Net
{
    /// <summary>
    /// <para>This is a simple memory-based cache provider. It has no cache-expiration policy.
    /// To use, pass an instance of this class when constructing KeenClient</para>
    /// <seealso cref="Keen.Core.KeenClient"/>
    /// </summary>
    public class EventCacheMemory : IEventCache
    {
        private Queue<CachedEvent> events = new Queue<CachedEvent>();

        public Task Add(CachedEvent e)
        {
            if (null == e)
                throw new KeenException("Cached events may not be null");

            return Task.Run(() =>
            {
                lock (events)
                    events.Enqueue(e);
            });
        }

        public Task Clear()
        {
            return Task.Run(() =>
            {
                lock (events)
                    events.Clear();
            });
        }

        public Task<CachedEvent> TryTake()
        {
            return Task.Run(() =>
            {
                lock (events)
                    return events.Any() ? events.Dequeue() : null;
            });
        }
    }
}
