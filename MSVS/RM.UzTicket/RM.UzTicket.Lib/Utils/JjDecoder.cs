using System;
using System.Text;
using System.Text.RegularExpressions;

// ReSharper disable StringIndexOfIsCultureSpecific.1

namespace RM.UzTicket.Lib.Utils
{
	internal static class JjDecoder
	{
		public static string Decode(string jjCode)
		{
			var result = new StringBuilder();

			// clean
			jjCode = Regex.Replace(jjCode, @"^\s+|\s+$", String.Empty);

			int startPos;
			int endPos;
			string gv;

			// check palindrome
			if (jjCode.IndexOf("\"\'\\\"+\'+\",") == 0)
			{
				// get jjCode
				startPos = jjCode.IndexOf(@"$$+""\""""+") + 8;
				endPos = jjCode.IndexOf(@"""\"""")())()");

				// get gv
				var startIndex = jjCode.IndexOf(@"""'\""+'+"",") + 9;
				var endIndex = jjCode.IndexOf("=~[]");
				gv = jjCode.Substring(startIndex, endIndex - startIndex);
			}
			else
			{
				// get gv
				gv = jjCode.Substring(0, jjCode.IndexOf("="));

				// get jjCode
				startPos = jjCode.IndexOf(@"""\""""+") + 5;
				endPos = jjCode.IndexOf(@"""\"""")())()");
			}

			if (startPos == endPos)
			{
				return String.Empty;
			}

			var data = jjCode.Substring(startPos, endPos - startPos);
			
			// hex decode string
			var b = new[] {"___+", "__$+", "_$_+", "_$$+", "$__+", "$_$+", "$$_+", "$$$+", "$___+", "$__$+", "$_$_+", "$_$$+", "$$__+", "$$_$+", "$$$_+", "$$$$+"};

			// l o t u
			var strL = @"(![]+"""")[" + gv + "._$_]+";
			var strO = gv + "._$+";
			var strT = gv + ".__+";
			var strU = gv + "._+";

			// 0123456789abcdef
			var strHex = gv + ".";

			// s
			var strS = @"""";
			var gvsig = gv + ".";

			var strQuote = @"\\\""";
			var strSlash = @"\\\\";

			var strLower = @"\\""+";
			var strUpper = strLower + strU;

			var strEnd = @"""+"; // end of s loop

			while (data.Length > 0)
			{
				// l o t u
				if (data.StartsWith(strL))
				{
					Skip(ref data, strL.Length);
					result.Append("l");
					continue;
				}
				if (data.StartsWith(strO))
				{
					Skip(ref data, strO.Length);
					result.Append("o");
					continue;
				}
				if (data.StartsWith(strT))
				{
					Skip(ref data, strT.Length);
					result.Append("t");
					continue;
				}
				if (data.StartsWith(strU))
				{
					Skip(ref data, strU.Length);
					result.Append("u");
					continue;
				}

				// 0123456789abcdef
				if (data.StartsWith(strHex))
				{
					Skip(ref data, strHex.Length);

					for (int i = 0; i < b.Length; i++)
					{
						if (data.StartsWith(b[i]))
						{
							Skip(ref data, b[i].Length);
							result.Append(i.ToString("x"));
							break;
						}
					}

					continue;
				}

				// start of s block
				if (data.StartsWith(strS))
				{
					Skip(ref data, strS.Length);

					// check if "R"
					if (data.StartsWith(strUpper))
					{
						Skip(ref data, strUpper.Length);

						var chStr = String.Empty;
						
						for (int j = 0; j < 2; j++)
						{
							if (data.StartsWith(gvsig))
							{
								Skip(ref data, gvsig.Length);

								for (int k = 0; k < b.Length; k++)
								{
									if (data.StartsWith(b[k]))
									{
										Skip(ref data, b[k].Length);
										chStr += k.ToString("x");
										break;
									}
								}
							}
							else
							{
								break;
							}
						}

						result.Append(fromCharCode(parseInt(chStr, 16)));
						continue;
					}

					if (data.StartsWith(strLower))
					{
						Skip(ref data, strLower.Length);

						var chStr = String.Empty;
						var chLotux = String.Empty;
						var temp = String.Empty;
						var bCheckR1 = 0;

						for (int j = 0; j < 3; j++)
						{
							if (j > 1)
							{
								if (data.StartsWith(strL))
								{
									Skip(ref data, strL.Length);
									chLotux = "l";
									break;
								}
								if (data.StartsWith(strO))
								{
									Skip(ref data, strO.Length);
									chLotux = "o";
									break;
								}
								if (data.StartsWith(strT))
								{
									Skip(ref data, strT.Length);
									chLotux = "t";
									break;
								}
								if (data.StartsWith(strU))
								{
									Skip(ref data, strU.Length);
									chLotux = "u";
									break;
								}
							}

							if (data.StartsWith(gvsig))
							{
								temp = data.Substring(gvsig.Length);

								for (int k = 0; k < 8; k++)
								{
									if (temp.StartsWith(b[k]))
									{
										if (parseInt(chStr + k, 8) > 128)
										{
											bCheckR1 = 1;
											break;
										}

										chStr += k.ToString();
										Skip(ref data, gvsig.Length + b[k].Length);
										break;
									}
								}

								if (bCheckR1 == 1)
								{
									if (data.StartsWith(strHex))
									{
										Skip(ref data, strHex.Length);

										for (int i = 0; i < b.Length; i++)
										{
											if (data.StartsWith(b[i]))
											{
												Skip(ref data, b[i].Length);
												chLotux = i.ToString("x");
												break;
											}
										}

										break;
									}
								}
							}
							else
							{
								break;
							}
						}

						result.Append(fromCharCode(parseInt(chStr, 8)) + chLotux);
						continue;
					}

					var match = 0;
					int n;

					while (true)
					{
						n = data[0];

						if (data.StartsWith(strQuote))
						{
							Skip(ref data, strQuote.Length);
							result.Append(@"""");
							match += 1;
							continue;
						}
						if (data.StartsWith(strSlash))
						{
							Skip(ref data, strSlash.Length);
							result.Append(@"\");
							match += 1;
							continue;
						}
						
						if (data.StartsWith(strEnd))
						{
							if (match == 0)
							{
								// no match in s block
								return String.Empty;
							}

							Skip(ref data, strEnd.Length);
							break;
						}

						if (data.StartsWith(strUpper))
						{
							if (match == 0)
							{
								// no match s block n > 128
								return String.Empty;
							}

							Skip(ref data, strUpper.Length);

							var chStr = String.Empty;
							var chLotux = String.Empty;

							for (int j = 0; j < 10; j++)
							{
								if (j > 1)
								{
									if (data.StartsWith(strL))
									{
										Skip(ref data, strL.Length);
										chLotux = "l";
										break;
									}
									if (data.StartsWith(strO))
									{
										Skip(ref data, strO.Length);
										chLotux = "o";
										break;
									}
									if (data.StartsWith(strT))
									{
										Skip(ref data, strT.Length);
										chLotux = "t";
										break;
									}
									if (data.StartsWith(strU))
									{
										Skip(ref data, strU.Length);
										chLotux = "u";
										break;
									}
								}

								if (data.StartsWith(gvsig))
								{
									Skip(ref data, gvsig.Length);

									for (int k = 0; k < b.Length; k++)
									{
										if (data.StartsWith(b[k]))
										{
											Skip(ref data, b[k].Length);
											chStr += k.ToString("x");
											break;
										}
									}
								}
								else
								{
									break;
								}
							}

							result.Append(fromCharCode(parseInt(chStr, 16)));
							break;
						}
						
						if (data.StartsWith(strLower))
						{
							if (match == 0)
							{
								// no match s block n < 128
								return String.Empty;
							}

							Skip(ref data, strLower.Length);

							var chStr = String.Empty;
							var chLotux = String.Empty;
							var temp = String.Empty;
							var bCheckR1 = 0;

							for (int j = 0; j < 3; j++)
							{
								if (j > 1)
								{
									if (data.StartsWith(strL))
									{
										Skip(ref data, strL.Length);
										chLotux = "l";
										break;
									}
									if (data.StartsWith(strO))
									{
										Skip(ref data, strO.Length);
										chLotux = "o";
										break;
									}
									if (data.StartsWith(strT))
									{
										Skip(ref data, strT.Length);
										chLotux = "t";
										break;
									}
									if (data.StartsWith(strU))
									{
										Skip(ref data, strU.Length);
										chLotux = "u";
										break;
									}
								}

								if (data.StartsWith(gvsig))
								{
									temp = data.Substring(gvsig.Length);

									for (int k = 0; k < 8; k++)
									{
										if (temp.StartsWith(b[k]))
										{
											if (parseInt(chStr + k, 8) > 128)
											{
												bCheckR1 = 1;
												break;
											}

											chStr += k;
											Skip(ref data, gvsig.Length + b[k].Length);
											break;
										}
									}

									if (bCheckR1 == 1)
									{
										if (data.StartsWith(strHex))
										{
											Skip(ref data, strHex.Length);

											for (int i = 0; i < b.Length; i++)
											{
												if (data.StartsWith(b[i]))
												{
													Skip(ref data, b[i].Length);
													chLotux = i.ToString("x");
													break;
												}
											}
										}
									}
								}
								else
								{
									break;
								}
							}

							result.Append(fromCharCode(parseInt(chStr, 8)) + chLotux);
							break;
						}
						if ((0x21 <= n && n <= 0x2f) || (0x3A <= n && n <= 0x40) || (0x5b <= n && n <= 0x60) || (0x7b <= n && n <= 0x7f))
						{
							result.Append(data[0]);
							Skip(ref data, 1);
							match += 1;
						}
					}

					continue;
				}

				// no match
				break;
			}

			return result.ToString();
		}

		private static int parseInt(string a, int b)
		{
			return System.Convert.ToInt32(a, b);
		}

		private static string fromCharCode(int value)
		{
			var character = (char)value;
			return character.ToString();
		}

		private static void Skip(ref string str, int count)
		{
			str = str.Substring(count);
		}
	}
}
