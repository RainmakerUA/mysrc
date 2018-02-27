using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.CSharpTest
{
	public class Morse
	{
		private static readonly IDictionary<char, string> _morseAlphabet = new Dictionary<char, string>
																				{
																					['А'] = ".-",
																					['Б'] = "-...",
																					['В'] = ".--",
																					['Г'] = "--.",
																					['Д'] = "-..",
																					['Е'] = ".",
																					['Ж'] = "...-",
																					['З'] = "--..",
																					['И'] = "..",
																					['Й'] = ".---",
																					['К'] = "-.-",
																					['Л'] = ".-..",
																					['М'] = "--",
																					['Н'] = "-.",
																					['О'] = "---",
																					['П'] = ".--.",
																					['Р'] = ".-.",
																					['С'] = "...",
																					['Т'] = "-",
																					['У'] = "..-",
																					['Ф'] = "..-.",
																					['Х'] = "....",
																					['Ц'] = "-.-.",
																					['Ч'] = "---.",
																					['Ш'] = "----",
																					['Щ'] = "--.-",
																					['Ъ'] = "--.--",
																					['Ы'] = "-.--",
																					['Ь'] = "-..-",
																					['Э'] = "..-..",
																					['Ю'] = "..--",
																					['Я'] = ".-.-",
																					['1'] = ".----",
																					['2'] = "..---",
																					['3'] = "...--",
																					['4'] = "....-",
																					['5'] = ".....",
																					['6'] = "-....",
																					['7'] = "--...",
																					['8'] = "---..",
																					['9'] = "----.",
																					['0'] = "-----",
																					['.'] = "......",
																					[','] = ".-.-.-",
																					[':'] = "---...",
																					[';'] = "-.-.-.",
																					['('] = "-.--.-",
																					//[')'] = "-.--.-",
																					['\''] = ".----.",
																					['\\'] = ".-..-.",
																					['-'] = "-....-",
																					['/'] = "-..-.",
																					['?'] = "..--..",
																					['!'] = "--..--",
																					['@'] = ".--.-.",
																					['='] = "-...-",
																					['\u0020'] = "\u0020"
																				};

		private static readonly IDictionary<string, char> _inverseMorseAlphabet = _morseAlphabet.ToDictionary(kv => kv.Value, kv => kv.Key);

		private readonly string _buffer;
		private readonly bool _withSpace;

		//static Morse()
		//{
		//	_inverseMorseAlphabet = new Dictionary<string, char>();

		//	foreach (var key in _morseAlphabet.Keys)
		//	{
		//		try
		//		{
		//			_inverseMorseAlphabet.Add(_morseAlphabet[key], key);
		//		}
		//		catch (Exception e)
		//		{
		//			Console.WriteLine("{0}: (['{1}'] = \"{2}\") {3}", e.GetType().FullName, key, _morseAlphabet[key], e.Message);
		//		}
		//	}
		//}

		protected string Buffer => _buffer;

		public Morse(string buffer = null, bool withSpace = false)
		{
			_buffer = buffer;
			_withSpace = withSpace;
		}

		public override string ToString()
		{
			return Stringify(this);
		}

		public static string ToCode(string text)
		{
			var str = String.Join("_", text.ToUpperInvariant().Select(c => _morseAlphabet[c]));
			str = str.Replace('.', '+') + (String.IsNullOrEmpty(str) ? "" : "_");
			return str.Replace("_ ", "__").Replace(" _", "__");
		}

		public static Morse operator +(Morse morse)
		{
			return new Morse("." + (morse._buffer ?? String.Empty), morse._withSpace);
		}

		public static Morse operator -(Morse morse)
		{
			return new Morse("-" + (morse._buffer ?? String.Empty), morse._withSpace);
		}
		
		public static string operator +(Morse morseLeft, Morse morseRight)
		{
			return Stringify(morseLeft) + Stringify(+morseRight);
		}
		
		public static string operator +(string stringLeft, Morse morseRight)
		{
			return stringLeft + Stringify(+morseRight);
		}

		public static string operator -(Morse morseLeft, Morse morseRight)
		{
			return Stringify(morseLeft) + Stringify(-morseRight);
		}

		public static string operator -(string stringLeft, Morse morseRight)
		{
			return stringLeft + Stringify(-morseRight);
		}

		private static string Stringify(Morse morse)
		{
			return new String(_inverseMorseAlphabet[morse._buffer], 1) + (morse._withSpace ? "\u0020" : "");
		}
	}

	public class MorseWithSpace : Morse
	{
		
	}
}
