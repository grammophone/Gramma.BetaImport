using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grammophone.BetaImport
{
	/// <summary>
	/// A beta code converter to unicode, which handles diacritics by late composition.
	/// </summary>
	public class ComposingDiacriticsBetaConverter : BetaConverter
	{
		#region Protected methods

		protected override void AppendChar(StringBuilder builder, char t,
			bool hasPsile,
			bool hasDaseia,
			bool hasOkseia,
			bool hasBareia,
			bool hasPerispomene,
			bool hasHypogegrammene,
			bool hasProsgegrammene,
			bool hasDialytika)
		{
			builder.Append(t);

			if (hasPsile) builder.Append('\x0313');
			if (hasDaseia) builder.Append('\x0314');
			if (hasOkseia) builder.Append('\x0301');
			if (hasBareia) builder.Append('\x0300');
			if (hasPerispomene) builder.Append('\x0342');
			if (hasDialytika) builder.Append('\x0308');

			if (hasHypogegrammene) builder.Append('\x0345');
			if (hasProsgegrammene) builder.Append('\x1FBE');

		}

		#endregion
	}
}
