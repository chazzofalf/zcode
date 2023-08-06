using System;

namespace zcode_common_std
{
	public class Util
	{
		public Util()
		{
		}
		public class LazyVariable<T> where T : class
		{
			private Func<T> _creator;
			private T value;
			private LazyVariable()
			{
			}
			public static LazyVariable<T> Create(Func<T> creator)
			{
				var outx = new LazyVariable<T>();
				outx._creator = creator;
				return outx;
			}
			public T Get => ((Func<T>)(() =>
			{
				return value = value != null ? value : _creator();
			}))();
		}
	}
}

