using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grammophone.BetaImport
{
	/// <summary>
	/// Class to convert a beta code string to unicode.
	/// </summary>
	public abstract class BetaConverter
	{
		#region Private fields

		private enum ScriptMode
		{
			Greek,
			Latin
		}

		private enum Code
		{
			None,
			PageFormat,
			GreekFont,
			LatinFont,
			AdditionalPunctuation,
			LeftBracket,
			RightBracket,
			AdditionalCharacter,
			MarkupOpen,
			MarkupClose,
			FormattingOpen,
			FormattingClose,
			Quotation,
			QuarterSpace
		}

		private static AsciiMap uCaseMap = InitializeUCaseMap();
		private static AsciiMap lCaseMap = InitializeLCaseMap();
		private static AsciiMap additionalPunctuationMap = InitializeAdditionalPunctuationMap();
		private static AsciiMap leftBracketMap = InitializeLeftBracketMap();
		private static AsciiMap rightBracketMap = InitializeRightBracketMap();
		private static AsciiMap additionalUCharacterMap = InitializeAdditionalUCharacterMap();
		private static AsciiMap additionalLCharacterMap = InitializeAdditionalLCharacterMap();
		private static AsciiMap leftQuotesMap = InitializeLeftQuotesMap();
		private static AsciiMap rightQuotesMap = InitializeRightQuotesMap();

		#endregion

		#region Construction

		//public static readonly BetaConverter Global = new BetaConverter();

		/// <summary>
		/// Create.
		/// </summary>
		public BetaConverter()
		{
		}

		/// <summary>
		/// Initialize upper case map.
		/// </summary>
		private static AsciiMap InitializeUCaseMap()
		{
			AsciiMap uCaseMap = new AsciiMap();

			uCaseMap['A'] = '\x0391';
			uCaseMap['B'] = '\x0392';
			uCaseMap['C'] = '\x039E';
			uCaseMap['D'] = '\x0394';
			uCaseMap['E'] = '\x0395';
			uCaseMap['F'] = '\x03A6';
			uCaseMap['G'] = '\x0393';
			uCaseMap['H'] = '\x0397';
			uCaseMap['I'] = '\x0399';
			uCaseMap['K'] = '\x039A';
			uCaseMap['L'] = '\x039B';
			uCaseMap['M'] = '\x039C';
			uCaseMap['N'] = '\x039D';
			uCaseMap['O'] = '\x039F';
			uCaseMap['P'] = '\x03A0';
			uCaseMap['Q'] = '\x0398';
			uCaseMap['R'] = '\x03A1';
			uCaseMap['S'] = '\x03A3';
			uCaseMap['T'] = '\x03A4';
			uCaseMap['U'] = '\x03A5';
			uCaseMap['V'] = '\x03DC';
			uCaseMap['W'] = '\x03A9';
			uCaseMap['X'] = '\x03A7';
			uCaseMap['Y'] = '\x03A8';
			uCaseMap['Z'] = '\x0396';

			return uCaseMap;
		}

		/// <summary>
		/// Initialize lower case map.
		/// </summary>
		private static AsciiMap InitializeLCaseMap()
		{
			AsciiMap lCaseMap = new AsciiMap();

			lCaseMap['A'] = '\x03B1';
			lCaseMap['B'] = '\x03B2';
			lCaseMap['C'] = '\x03BE';
			lCaseMap['D'] = '\x03B4';
			lCaseMap['E'] = '\x03B5';
			lCaseMap['F'] = '\x03C6';
			lCaseMap['G'] = '\x03B3';
			lCaseMap['H'] = '\x03B7';
			lCaseMap['I'] = '\x03B9';
			lCaseMap['K'] = '\x03BA';
			lCaseMap['L'] = '\x03BB';
			lCaseMap['M'] = '\x03BC';
			lCaseMap['N'] = '\x03BD';
			lCaseMap['O'] = '\x03BF';
			lCaseMap['P'] = '\x03C0';
			lCaseMap['Q'] = '\x03B8';
			lCaseMap['R'] = '\x03C1';
			lCaseMap['S'] = '\x03C3'; // MEDIAL SIGMA. 03C2 IS FINAL SIGMA.
			lCaseMap['T'] = '\x03C4';
			lCaseMap['U'] = '\x03C5';
			lCaseMap['V'] = '\x03DD';
			lCaseMap['W'] = '\x03C9';
			lCaseMap['X'] = '\x03C7';
			lCaseMap['Y'] = '\x03C8';
			lCaseMap['Z'] = '\x03B6';

			return lCaseMap;
		}

		/// <summary>
		/// Creates mappings for the '@' beta escape codes.
		/// </summary>
		private static AsciiMap InitializeAdditionalPunctuationMap()
		{
			var map = new AsciiMap(11);

			map['\x0'] = '\x2020';
			map['\x1'] = '?';
			map['\x2'] = '*';
			map['\x3'] = '/';
			map['\x4'] = '!';
			map['\x5'] = '|';
			map['\x6'] = '=';
			map['\x7'] = '+';
			map['\x8'] = '%';
			map['\x9'] = '&';
			map['\xA'] = ':';

			return map;
		}

		/// <summary>
		/// Creates mappings for '[' beta escape codes.
		/// </summary>
		private static AsciiMap InitializeLeftBracketMap()
		{
			var map = new AsciiMap(9);

			map['\x0'] = '[';
			map['\x1'] = '(';
			map['\x2'] = '\x2329';
			map['\x3'] = '{';
			map['\x4'] = '\x27E6';
			map['\x5'] = '\x2E24';
			map['\x6'] = '\x2E22';
			map['\x7'] = '\x2E22';
			map['\x8'] = '\x2E24';

			return map;
		}

		/// <summary>
		/// Creates mappings for ']' beta escape codes.
		/// </summary>
		private static AsciiMap InitializeRightBracketMap()
		{
			var map = new AsciiMap(9);

			map['\x0'] = ']';
			map['\x1'] = ')';
			map['\x2'] = '\x232A';
			map['\x3'] = '}';
			map['\x4'] = '\x27E7';
			map['\x5'] = '\x2E25';
			map['\x6'] = '\x2E23';
			map['\x7'] = '\x2E25';
			map['\x8'] = '\x2E23';

			return map;
		}

		/// <summary>
		/// Creates mapping for the upper case '#' beta escape codes.
		/// </summary>
		private static AsciiMap InitializeAdditionalUCharacterMap()
		{
			var map = new AsciiMap();

			map['\x00'] = '\x0374';
			map['\x01'] = '\x03DE';
			map['\x02'] = '\x03DA';
			map['\x03'] = '\x03D8';
			map['\x04'] = '\x03DE';
			map['\x05'] = '\x03E0';
			map['\x06'] = '\x2E0F';

			return map;
		}

		/// <summary>
		/// Creates mapping for the upper case '#' beta escape codes.
		/// </summary>
		private static AsciiMap InitializeAdditionalLCharacterMap()
		{
			var map = new AsciiMap();

			map['\x00'] = '\x0374';
			map['\x01'] = '\x03DF';
			map['\x02'] = '\x03DB';
			map['\x03'] = '\x03D9';
			map['\x04'] = '\x03DE';
			map['\x05'] = '\x03E1';
			map['\x06'] = '\x2E0F';

			return map;
		}

		private static AsciiMap InitializeLeftQuotesMap()
		{
			var map = new AsciiMap();
			
			map['\x00'] = '\x201C';
			map['\x01'] = '\x201E';
			map['\x02'] = '\x201C';
			map['\x03'] = '\x2018';
			map['\x04'] = '\x201A';
			map['\x05'] = '\x201B';
			map['\x06'] = '\x00AB';
			map['\x07'] = '\x2039';
			map['\x08'] = '\x201C';

			return map;
		}

		private static AsciiMap InitializeRightQuotesMap()
		{
			var map = new AsciiMap();

			map['\x00'] = '\x201D';
			map['\x01'] = '\x201E';
			map['\x02'] = '\x201C';
			map['\x03'] = '\x2019';
			map['\x04'] = '\x201A';
			map['\x05'] = '\x201B';
			map['\x06'] = '\x00BB';
			map['\x07'] = '\x203A';
			map['\x08'] = '\x201E';

			return map;
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Convert a beta code string to unicode.
		/// </summary>
		/// <param name="betaCode">The beta code string.</param>
		/// <returns>The unicode string.</returns>
		public string Convert(string betaCode)
		{
			if (betaCode == null) throw new ArgumentNullException("betaCode");

			StringBuilder builder = new StringBuilder(betaCode.Length);

			int charIndex;
			char c;

			bool inQuotes = false;

			ScriptMode scriptMode = ScriptMode.Greek;

			for (charIndex = 0; charIndex < betaCode.Length; charIndex++)
			{
				bool hasPsile = false;
				bool hasDaseia = false;
				bool hasOkseia = false;
				bool hasBareia = false;
				bool hasPerispomene = false;
				bool hasYpogegrammene = false;
				bool hasProsgegrammene = false;
				bool hasDialytika = false;

				bool isCapital = false;

				Code code = Code.None;

				char t = '\0';
				int selector = 0;

				for (; charIndex < betaCode.Length; charIndex++)
				{
					c = betaCode[charIndex];

					if (code == Code.None && t != '\0')
					{
						switch (c)
						{
							case '$':
							case '&':
							case '%':
							case '@':
							case '[':
							case ']':
							case '#':
							case '{':
							case '}':
							case '<':
							case '>':
							case '\"':
							case '^':
								goto lastCharacter;
						}
					}

					switch (code)
					{
						case Code.None:
							switch (c)
							{
								case '$':
									scriptMode = ScriptMode.Greek;
									code = Code.GreekFont;
									continue;

								case '&':
									scriptMode = ScriptMode.Latin;
									code = Code.LatinFont;
									continue;

								case '%':
									code = Code.AdditionalPunctuation;
									continue;

								case '@':
									code = Code.PageFormat;
									continue;

								case '[':
									code = Code.LeftBracket;
									continue;

								case ']':
									code = Code.RightBracket;
									continue;

								case '#':
									code = Code.AdditionalCharacter;
									continue;

								case '{':
									code = Code.MarkupOpen;
									continue;

								case '}':
									code = Code.MarkupClose;
									continue;

								case '<':
									code = Code.FormattingOpen;
									continue;

								case '>':
									code = Code.FormattingClose;
									continue;

								case '\"':
									code = Code.Quotation;
									continue;

								case '^':
									code = Code.QuarterSpace;
									continue;

								case '*':
									if (t != '\0')
									{
										charIndex--;
										goto readyCharacter;
									}

									isCapital = true;
									continue;

								case ')':
									hasPsile = true;
									continue;

								case '(':
									hasDaseia = true;
									continue;

								case '/':
									hasOkseia = true;
									continue;

								case '\\':
									hasBareia = true;
									continue;

								case '=':
									hasPerispomene = true;
									continue;

								case '+':
									hasDialytika = true;
									continue;

								case '|':
									hasYpogegrammene = true;
									continue;

								default:
									if (c >= 'A' && c <= 'Z')
									{
										if (t != '\0')
										{
											charIndex--;
											goto readyCharacter;
										}

										switch (scriptMode)
										{
											case ScriptMode.Greek:
												if (isCapital)
													t = uCaseMap[c];
												else
													t = lCaseMap[c];
												break;

											case ScriptMode.Latin:
												t = c;
												break;
										}

										if (charIndex < betaCode.Length - 1)
											continue;
										else
											charIndex++; // To force exiting the loop after subsequent "charindex--".
									}
									else if (c >= '0' && c <= '9')
									{
										if (t != '\0')
										{
											selector = selector * 10 + c - '0';
											continue;
										}
									}
									else if (c >= 'a' && c <= 'z' && scriptMode == ScriptMode.Latin)
									{
										if (t != '\0')
										{
											charIndex--;
											goto readyCharacter;
										}

										t = c;
										continue;
									}
									break;
							}

							break;

						default:
							if (c >= '0' && c <= '9')
							{
								selector = selector * 10 + c - '0';
								continue;
							}

							charIndex--;
							goto readyCharacter;
					}


					if (c >= 0x0080)
					{
						builder.Append("\r\n");

						for (charIndex++; charIndex < betaCode.Length; charIndex++)
						{
							if (betaCode[charIndex] < 0x0080)
							{
								charIndex--;
								break;
							}
						}

						continue;
					}

				lastCharacter:

					if (t != '\0')
					{
						charIndex--;

						// If the last letter was s, make it final s.
						if (t == '\x03C3' && c != '-' && !isCapital) t = '\x03C2';

						break;
					}

					t = c;

					switch (t)
					{
						// Semicolon to upper stop.
						/* Change this from unicode 00B7 specified by TLG to unicode 0387 as entered by a polytonic keyboard. */
						case ':':
							t = '\x0387';
							break;

						case '\'': // Plain ASCII apostrophe. Change it to greek apostrophe.
							t = '\x1FBF'; // This is the '᾿' apostrophe, appropriate for greek.
 							break;
					}

					break;
				}

			readyCharacter:

				switch (code)
				{
					case Code.PageFormat:
						switch (selector)
						{
							case 0:
								builder.Append('\t');
								break;

							case 6:
								builder.Append("\r\n");
								break;
						}
						break;

					case Code.AdditionalPunctuation:
						t = additionalPunctuationMap[(char)selector];

						if (t != '\0') builder.Append(t);
						break;

					case Code.LeftBracket:
						t = leftBracketMap[(char)selector];

						if (t != '\0') builder.Append(t);
						break;

					case Code.RightBracket:
						t = rightBracketMap[(char)selector];

						if (t != '\0') builder.Append(t);
						break;

					case Code.AdditionalCharacter:
						if (isCapital)
							t = additionalUCharacterMap[(char)selector];
						else
							t = additionalLCharacterMap[(char)selector];

						if (t != '\0') builder.Append(t);
						break;

					case Code.Quotation:
						if (inQuotes)
							t = rightQuotesMap[(char)selector];
						else
							t = leftQuotesMap[(char)selector];

						switch (selector)
						{
							case 1:
							case 2:
							case 4:
							case 5:
								break;

							default:
								inQuotes = !inQuotes;
								break;
						}

						if (t != '\0') builder.Append(t);

						break;

					case Code.QuarterSpace:
						for (int i = 0; i < selector / 4; i++) builder.Append(' ');
						break;

					case Code.None:
						if (t == '\x03C3') // If SIGMA...
						{
							switch (selector)
							{
								case 2:
									t = '\x03C2'; // Final sigma is one less than median.
									break;

								case 3:
									if (isCapital)
										t = '\x03F9';
									else
										t = '\x03F2';
									break;
							}
						}

						if (t != '\0')
						{
							AppendChar(builder, t, hasPsile, hasDaseia, hasOkseia, hasBareia,
								hasPerispomene, hasYpogegrammene, hasProsgegrammene, hasDialytika);
						}
						break;
				}

			}

			return builder.ToString();
		}

		#endregion

		#region Protected methods

		protected abstract void AppendChar(StringBuilder builder, char t,
			bool hasPsile,
			bool hasDaseia,
			bool hasOkseia,
			bool hasBareia,
			bool hasPerispomene,
			bool hasHypogegrammene,
			bool hasProsgegrammene,
			bool hasDialytika);

		#endregion
	}
}
