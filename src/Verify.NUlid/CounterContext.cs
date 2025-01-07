class CounterContext
{
  static AsyncLocal<CounterContext?> local = new();

  ConcurrentDictionary<Ulid, int> cache = [];
  int current;

  public int Next(Ulid input) =>
      cache.GetOrAdd(input, _ => Interlocked.Increment(ref current));

  public static void Init() =>
      VerifierSettings.OnVerify(Start, Stop);

  public static CounterContext Current =>
      local.Value ?? throw new("No current context");

  static void Start() =>
      local.Value = new();

  static void Stop() =>
      local.Value = null;
}