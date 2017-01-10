using System.Collections.Generic;

namespace TEDCore.Data
{
	public abstract class SavableData
	{
		public abstract string Name { get; }

		public abstract void Load(Dictionary<string, object> data);

		public abstract void Save(ref Dictionary<string, object> data);
	}
}