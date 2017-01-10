using TEDCore;

namespace TEDCore.Startup
{
    public interface IStartupTask : IDestroyable, IUpdate
	{
		bool IsDone { get; }
		bool Init();
	}
}