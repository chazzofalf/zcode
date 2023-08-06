using System;

namespace zcode_mac
{
	public class MacUtil
	{
		public MacUtil()
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
			public T Get()
			{
				return value = value != null ? value : _creator();
			}
		}
	}
}

