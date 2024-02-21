using RateLimiter;

var rateLimiter = new RateLimiterThreadSafe(5, TimeSpan.FromSeconds(10));
string userId = "user1"; // Simulating requests from a single user

for (int i = 0; i < 10; i++) // Simulate 10 requests
{
    if (rateLimiter.RequestShouldBeAllowed(userId))
    {
        Console.WriteLine($"Request {i + 1}: Allowed");
    }
    else
    {
        Console.WriteLine($"Request {i + 1}: Denied");
    }

    Thread.Sleep(1000); // Wait 1 second between requests
}