public class DepthStream
{
    private readonly List<string> _subscribers = new();
    private bool _running;

    public void Start()
    {
        if (_running) return;
        _running = true;

        Console.WriteLine("[DepthStream] Started");
        // Real system: initialize depth sensor, configure format, etc.
    }

    public void Stop()
    {
        if (!_running) return;
        _running = false;

        Console.WriteLine("[DepthStream] Stopped");
    }

    public void AddSubscriber(string agentName)
    {
        if (!_subscribers.Contains(agentName))
        {
            _subscribers.Add(agentName);
            Console.WriteLine($"[DepthStream] Agent subscribed: {agentName}");
        }
    }

    public IReadOnlyList<string> Subscribers => _subscribers;
}
