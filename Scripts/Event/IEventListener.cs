
namespace TEDCore.Event
{
	public interface IEventListener
	{
		EventResult OnEvent(string eventName, object eventData);
	}
}