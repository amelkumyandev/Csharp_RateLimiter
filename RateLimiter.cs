using System;
using System.Collections.Generic;

namespace RateLimiter
{
    
    public class RateLimiterThreadSafe
    {
        private readonly Dictionary<string, Queue<DateTime>> userRequests = new Dictionary<string, Queue<DateTime>>();
        private readonly int requestLimit;
        private readonly TimeSpan timeSpan;
        private readonly object lockObj = new object();

        public RateLimiterThreadSafe(int limit, TimeSpan period)
        {
            this.requestLimit = limit;
            this.timeSpan = period;
        }

        public bool RequestShouldBeAllowed(string userId)
        {
            lock (lockObj)
            {
                if (!userRequests.ContainsKey(userId))
                {
                    userRequests[userId] = new Queue<DateTime>();
                }

                var now = DateTime.UtcNow;
                var requests = userRequests[userId];

                // Remove timestamps outside the current rate limit window
                while (requests.Count > 0 && now - requests.Peek() > timeSpan)
                {
                    requests.Dequeue();
                }

                // Check current request count against limit
                if (requests.Count < requestLimit)
                {
                    requests.Enqueue(now);
                    return true; // Request allowed
                }

                return false; // Request denied
            }
        }
    }

}
