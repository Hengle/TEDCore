
namespace TEDCore.Event
{
	public class EventResult
	{
		public readonly bool WasEaten;
		public readonly object Response;

		public EventResult(bool wasEaten, object response)
		{
			WasEaten = wasEaten;
			Response = response;
		}
	}
}