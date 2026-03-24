public class VideoStream
{
    public string Resolution { get; }
    public int Fps { get; }

    private readonly List<string> _subscribers = new();
    private bool _running;

    public VideoStream(string resolution, int fps)
    {
        Resolution = resolution;
        Fps = fps;
    }

    public void Start()
    {
        if (_running) return;
        _running = true;

        Console.WriteLine($"[VideoStream] Started {Resolution} @ {Fps}fps");
        // In a real system: open camera, configure pipeline, etc.
    }

    public void Stop()
    {
        if (!_running) return;
        _running = false;

        Console.WriteLine("[VideoStream] Stopped");
    }

    public void AddSubscriber(string agentName)
    {
        if (!_subscribers.Contains(agentName))
        {
            _subscribers.Add(agentName);
            Console.WriteLine($"[VideoStream] Agent subscribed: {agentName}");
        }
    }

    public IReadOnlyList<string> Subscribers => _subscribers;
}
