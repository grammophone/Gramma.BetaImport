using System;
using System.Linq;
using System.Text;
using System.IO;

namespace Gramma.BetaImport
{
	/// <summary>
	/// A filter stream that takes characters from a stream
	/// of beta code and converts to standard (unicode) text.
	/// </summary>
	public class BetaReader : TextReader
	{
		/// <summary>
		/// The source stream of beta code.
		/// </summary>
		private Stream sourceStream;

		/// <summary>
		/// The current line being read translated from beta code.
		/// </summary>
		private string currentLine;

		/// <summary>
		/// The current position within <see cref="currentLine"/>.
		/// </summary>
		private int currentLinePosition;

		/// <summary>
		/// True if the Dispose method has been called.
		/// </summary>
		private bool isDisposed;

		/// <summary>
		/// The flavour of beta converter to use.
		/// </summary>
		private BetaConverter betaConverter;

		/// <summary>
		/// Create.
		/// </summary>
		/// <param name="sourceStream">The source stream of beta code.</param>
		/// <param name="betaConverter">The flavour of beta converter to use.</param>
		public BetaReader(Stream sourceStream, BetaConverter betaConverter)
		{
			if (sourceStream == null) throw new ArgumentNullException("sourceStream");
			if (betaConverter == null) throw new ArgumentNullException("betaConverter");

			this.sourceStream = sourceStream;
			this.betaConverter = betaConverter;

			isDisposed = false;

			currentLine = String.Empty;
			currentLinePosition = 0;
		}

		#region Reader implementation

		#region Properties

		/// <summary>
		/// True if no more characters are available.
		/// </summary>
		public bool IsEOF
		{
			get
			{
				return currentLine == null;
			}
		}

		#endregion

		#region Methods

		#region Private methods

		private void FeedSourceLine()
		{
			string sourceLine = ReadRawLine();

			// Reached the end?
			if (sourceLine == null)
			{
				currentLine = null;
			}
			else
			{
				currentLine = this.betaConverter.Convert(sourceLine);
				currentLinePosition = 0;
			}
		}

		/// <summary>
		/// Read the current raw line.
		/// </summary>
		/// <returns>Returns null if end of file.</returns>
		private string ReadRawLine()
		{
			StringBuilder line = new StringBuilder();

			int r;
			for (r = sourceStream.ReadByte(); r != -1 && r != '\r'; r = sourceStream.ReadByte())
			{
				line.Append((char)r);
			}

			if (r == '\r')
			{
				line.Append(r);
				r = sourceStream.ReadByte();

				if (r == '\n') line.Append(r);
				else if (r != -1) sourceStream.Seek(-1, SeekOrigin.Current);
			}

			if (r == -1 && line.Length == 0) return null;

			return line.ToString();
		}

		private void EnsureFeed()
		{
			if (currentLine != null)
			{
				if (currentLinePosition == currentLine.Length)
				{
					// Reached end of the source line. Feed next one.
					FeedSourceLine();
				}
			}
		}

		#endregion

		public override int Read()
		{
			if (isDisposed) throw new ObjectDisposedException(this.GetType().Name);

			EnsureFeed();

			if (this.IsEOF) return -1;

			return currentLine[currentLinePosition++];
		}

		public override int Peek()
		{
			if (isDisposed) throw new ObjectDisposedException(this.GetType().Name);

			EnsureFeed();

			if (this.IsEOF) return -1;

			return currentLine[currentLinePosition];
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			isDisposed = true;
		}

		#endregion

		#endregion
	}
}
