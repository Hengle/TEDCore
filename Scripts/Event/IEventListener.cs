
namespace TEDCore.Event
{
	public interface IEventListener
	{
        EventResult OnEvent(int eventId, object eventData);
	}
}