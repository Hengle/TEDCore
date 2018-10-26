
namespace TEDCore.Event
{
	public interface IEventListener
	{
        EventResult OnEvent(int eventName, object eventData);
	}
}