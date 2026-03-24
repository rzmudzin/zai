public class LidarStream
{
    private readonly List<string> _subscribers = new();
    private bool _running;

    public void Start()
    {
        if (_running) return;
        _running = true;

        Console.WriteLine("[LidarStream] Started");
        // Real system: initialize LiDAR driver, configure scan rate, etc.
    }

    public void Stop()
    {
        if (!_running) return;
        _running = false;

        Console.WriteLine("[LidarStream] Stopped");
    }

    public void AddSubscriber(string agentName)
    {
        if (!_subscribers.Contains(agentName))
        {
            _subscribers.Add(agentName);
            Console.WriteLine($"[LidarStream] Agent subscribed: {agentName}");
        }
    }

    public IReadOnlyList<string> Subscribers => _subscribers;
}
