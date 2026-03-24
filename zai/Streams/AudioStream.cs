public class AudioStream
{
    private readonly List<string> _subscribers = new();
    private bool _running;

    public void Start()
    {
        if (_running) return;
        _running = true;

        Console.WriteLine("[AudioStream] Started");
        // Real system: open microphone, configure sample rate, etc.
    }

    public void Stop()
    {
        if (!_running) return;
        _running = false;

        Console.WriteLine("[AudioStream] Stopped");
    }

    public void AddSubscriber(string agentName)
    {
        if (!_subscribers.Contains(agentName))
        {
            _subscribers.Add(agentName);
            Console.WriteLine($"[AudioStream] Agent subscribed: {agentName}");
        }
    }

    public IReadOnlyList<string> Subscribers => _subscribers;
}
