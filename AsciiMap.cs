using System;
using System.Text;

namespace Gramma.BetaImport
{
	/// <summary>
	/// Class to map ascii characters to unicode characters.
	/// </summary>
	public class AsciiMap
	{
		private char[] map;

		public AsciiMap()
			: this(256)
		{
		}

		public AsciiMap(int capacity)
		{
			map = new char[capacity];
		}

		public char this[char c]
		{
			get
			{
				int ci = c;
				if (ci >= map.Length) return '\0';

				return map[ci];
			}
			set
			{
				int ci = c;
				if (ci >= map.Length) throw new ArgumentOutOfRangeException();

				map[ci] = value;
			}
		}
	}
}
