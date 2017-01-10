
namespace TEDCore.Utils
{
	public class Destroyable
	{
		public static void Destroy (IDestroyable obj)
		{
			if(obj != null)
			{
				obj.Destroy();
			}
		}
	}
}