using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grammophone.BetaImport
{
	/// <summary>
	/// A beta converter to unicode using precombined diacritics.
	/// </summary>
	public class PrecombinedDiacriticsBetaConverter : BetaConverter
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
			// First, attmpt to map into the standard modern greek unicode range.
			if (!(hasPsile || hasDaseia || hasBareia || hasHypogegrammene || hasProsgegrammene || hasPerispomene))
			{
				switch (t)
				{
					case 'Α':
						if (hasOkseia) t = 'Ά';
						break;

					case 'Ε':
						if (hasOkseia) t = 'Έ';
						break;

					case 'Η':
						if (hasOkseia) t = 'Ή';
						break;

					case 'Ι':
						if (hasOkseia && !hasDialytika) t = 'Ί';
						else if (!hasOkseia && hasDialytika) t = 'Ϊ';
						break;

					case 'Ο':
						if (hasOkseia) t = 'Ό';
						break;

					case 'Υ':
						if (hasOkseia && !hasDialytika) t = 'Ύ';
						else if (!hasOkseia && hasDialytika) t = 'Ϋ';
						break;

					case 'Ω':
						if (hasOkseia) t = 'Ώ';
						break;

					case 'α':
						if (hasOkseia) t = 'ά';
						break;

					case 'ε':
						if (hasOkseia) t = 'έ';
						break;

					case 'η':
						if (hasOkseia) t = 'ή';
						break;

					case 'ι':
						if (hasOkseia && !hasDialytika) t = 'ί';
						else if (!hasOkseia && hasDialytika) t = 'ϊ';
						else if (hasOkseia && hasDialytika) t = 'ΐ';
						break;

					case 'ο':
						if (hasOkseia) t = 'ό';
						break;

					case 'υ':
						if (hasOkseia && !hasDialytika) t = 'ύ';
						else if (!hasOkseia && hasDialytika) t = 'ϋ';
						else if (hasOkseia && hasDialytika) t = 'ΰ';
						break;

					case 'ω':
						if (hasOkseia) t = 'ώ';
						break;

				}

				builder.Append(t);
				return;
			}

			// Then, if there is at least one pneuma, the unicode values follow a computable matrix.
			if (hasPsile || hasDaseia)
			{
				char diacriticsOffset = '\u0000';

				if (hasDaseia) diacriticsOffset += '\u0001';
				if (hasBareia) diacriticsOffset += '\u0002';
				if (hasOkseia) diacriticsOffset += '\u0004';
				if (hasPerispomene) diacriticsOffset += '\u0006';

				if (!(hasHypogegrammene || hasProsgegrammene))
				{

					switch (t)
					{
						case 'Ρ':
							t = '\u1FEB';
							break;

						case 'Α':
							t = 'Ἀ';
							break;

						case 'Ε':
							t = 'Ἐ';
							break;

						case 'Η':
							t = 'Ἠ';
							break;

						case 'Ι':
							t = 'Ἰ';
							break;

						case 'Ο':
							t = 'Ὀ';
							break;

						case 'Υ':
							t = '\u1F58';
							break;

						case 'Ω':
							t = 'Ὠ';
							break;

						case 'ρ':
							t = 'ῤ';
							break;

						case 'α':
							t = 'ἀ';
							break;

						case 'ε':
							t = 'ἐ';
							break;

						case 'η':
							t = 'ἠ';
							break;

						case 'ι':
							t = 'ἰ';
							break;

						case 'ο':
							t = 'ὀ';
							break;

						case 'υ':
							t = 'ὐ';
							break;

						case 'ω':
							t = 'ὠ';
							break;

					}
				}
				else // There is Hypogegrammene
				{
					switch (t)
					{
						case 'Α':
							t = 'ᾈ';
							break;

						case 'Η':
							t = 'ᾘ';
							break;

						case 'Ω':
							t = 'ᾨ';
							break;

						case 'α':
							t = 'ᾀ';
							break;

						case 'η':
							t = 'ᾐ';
							break;

						case 'ω':
							t = 'ᾠ';
							break;

					}

				}

				builder.Append((char)(t + diacriticsOffset));

				return;
			}

			// Again, if there is a Hypogrgrammene, we can follow a matrix.
			if (hasHypogegrammene || hasProsgegrammene)
			{
				char diacriticsOffset = '\u0000';

				if (!hasOkseia && !hasBareia)
				{
					if (hasPerispomene) diacriticsOffset += '\u0005';
					else diacriticsOffset += '\u0001';
				}
				else
				{
					if (hasOkseia) diacriticsOffset += '\u0002';
				}

				switch (t)
				{
					case 'α':
						t = 'ᾲ';
						break;

					case 'η':
						t = 'ῂ';
						break;

					case 'ω':
						t = 'ῲ';
						break;

					case 'Α':
						t = '\u1FBB';
						break;

					case 'Η':
						t = '\u1FCB';
						break;

					case 'Ω':
						t = '\u1FFB';
						break;
				}

				builder.Append((char)(t + diacriticsOffset));

				return;
			}

			// Now, if we have perispomene, we are sure that it isn't accompanied with pneuma or hypogegrammene.
			if (hasPerispomene)
			{
				switch (t)
				{
					case 'α':
						t = 'ᾶ';
						break;

					case 'η':
						t = 'ῆ';
						break;

					case 'ι':
						t = 'ῖ';
						break;

					case 'υ':
						t = 'ῦ';
						break;

					case 'ω':
						t = 'ῶ';
						break;
				}

				char diacriticsOffset = '\x0000';

				if (hasDialytika) diacriticsOffset += '\x0001';

				builder.Append((char)(t + diacriticsOffset));

				return;
			}

			// We are left with a single bareia check.

			if (hasBareia)
			{
				switch (t)
				{
					case 'Α':
						t = 'Ὰ';
						break;

					case 'Ε':
						t = 'Ὲ';
						break;

					case 'Η':
						t = 'Ὴ';
						break;

					case 'Ι':
						t = 'Ὶ';
						break;

					case 'Ο':
						t = 'Ὸ';
						break;

					case 'Υ':
						t = 'Ὺ';
						break;

					case 'Ω':
						t = 'Ὼ';
						break;

					case 'α':
						t = 'ὰ';
						break;

					case 'ε':
						t = 'ὲ';
						break;

					case 'η':
						t = 'ὴ';
						break;

					case 'ι':
						if (hasDialytika) t = '\u1FD2';
						else t = 'ὶ';
						break;

					case 'ο':
						if (hasDialytika) t = '\u1FE2';
						else t = 'ὸ';
						break;

					case 'υ':
						t = 'ὺ';
						break;

					case 'ω':
						t = 'ὼ';
						break;

				}

				builder.Append(t);

				return;
			}

			// If the character t is not caught from the above logic, fall back to composition.

			builder.Append(t);

			if (hasPsile) builder.Append('\u0313');
			if (hasDaseia) builder.Append('\u0314');
			if (hasOkseia) builder.Append('\u0301');
			if (hasBareia) builder.Append('\u0300');
			if (hasPerispomene) builder.Append('\u0342');
			if (hasDialytika) builder.Append('\u0308');

			if (hasHypogegrammene) builder.Append('\u0345');
			if (hasProsgegrammene) builder.Append('\u1FBE');


		}

		#endregion
	}
}
