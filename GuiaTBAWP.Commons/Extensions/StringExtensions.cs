﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using GuiaTBAWP.Commons.Helpers;

namespace GuiaTBAWP.Commons.Extensions
{
    public static class StringExtensions
    {
        public static string SanitizeHtml(this string source)
        {
            return HttpUtility.HtmlDecode(source);
        }

        public const string UppercaseAccentedCharacters = "ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖØÙÚÛÜÝÞ";
		public const string LowercaseAccentedCharacters = "ßàáâãäåæçèéêëìíîïðñòóôõöøùúûüýþÿ";
		private static readonly HashSet<RegexReplacement> UnaccentRules = new HashSet<RegexReplacement>();
		private static readonly Regex UrlCleanRegEx = new Regex(@"([,¿+:´""!¡%\.\?\*])|(&quot;)|([\$@=\#&;\\<>\{\}|^~\[\]`\/])", RegexOptions.Compiled);

		private static readonly Regex WordsSpliter =
			new Regex(string.Format(@"([A-Z{0}]+[a-z{1}\d]*)|[_\s]", UppercaseAccentedCharacters, LowercaseAccentedCharacters),
			          RegexOptions.Compiled);

		static StringExtensions()
		{
			AddUnaccent("([ÀÁÂÃÄÅÆ])", "A");
			AddUnaccent("([Ç])", "C");
			AddUnaccent("([ÈÉÊË])", "E");
			AddUnaccent("([ÌÍÎÏ])", "I");
			AddUnaccent("([Ð])", "D");
			AddUnaccent("([Ñ])", "N");
			AddUnaccent("([ÒÓÔÕÖØ])", "O");
			AddUnaccent("([ÙÚÛÜ])", "U");
			AddUnaccent("([Ý])", "Y");
			AddUnaccent("([Þ])", "T");
			AddUnaccent("([ß])", "s");
			AddUnaccent("([àáâãäåæ])", "a");
			AddUnaccent("([ç])", "c");
			AddUnaccent("([èéêë])", "e");
			AddUnaccent("([ìíîï])", "i");
			AddUnaccent("([ð])", "e");
			AddUnaccent("([ñ])", "n");
			AddUnaccent("([òóôõöø])", "o");
			AddUnaccent("([ùúûü])", "u");
			AddUnaccent("([ý])", "y");
			AddUnaccent("([þ])", "t");
			AddUnaccent("([ÿ])", "y");
		}

		private static void AddUnaccent(string rule, string replacement)
		{
			UnaccentRules.Add(new RegexReplacement(new Regex(rule, RegexOptions.Compiled), replacement));
		}

		public static string Titleize(this string word)
		{
			return Regex.Replace(Humanize(Underscore(word)), @"\b([a-z])", match => match.Captures[0].Value.ToUpper());
		}

		public static string Humanize(this string lowercaseAndUnderscoredWord)
		{
			return Capitalize(Regex.Replace(lowercaseAndUnderscoredWord, @"_", " "));
		}

		public static string Pascalize(this string lowercaseAndUnderscoredWord)
		{
			return Regex.Replace(lowercaseAndUnderscoredWord, "(?:^|_)(.)", match => match.Groups[1].Value.ToUpper());
		}

		public static string Camelize(this string lowercaseAndUnderscoredWord)
		{
			return Uncapitalize(Pascalize(lowercaseAndUnderscoredWord));
		}

		public static string Underscore(this string pascalCasedWord)
		{
			return Regex.Replace(
				Regex.Replace(
					Regex.Replace(pascalCasedWord, @"([A-Z]+)([A-Z][a-z])", "$1_$2"), @"([a-z\d])([A-Z])",
					"$1_$2"), @"[-\s]", "_").ToLower();
		}

		public static string Capitalize(this string word)
		{
			return word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower();
		}

		public static string Uncapitalize(this string word)
		{
			return word.Substring(0, 1).ToLower() + word.Substring(1);
		}

		public static string Dasherize(this string underscoredWord)
		{
			return underscoredWord.Replace('_', '-').Replace(' ', '-');
		}

		public static string Unaccent(this string word)
		{
		    return UnaccentRules.Aggregate(word, (current, rule) => rule.Regex.Replace(current, rule.Replacement));
		}

        public static string ToSearchTerm(this string searchWord)
        {
            searchWord = searchWord.Trim();
            searchWord = UrlCleanRegEx.Replace(searchWord, "");
            return Dasherize(Unaccent(searchWord)).ToLowerInvariant();
        }

		/// <summary>
		/// Quita todos los caracteres invalidos de un string para usarlo en una URL
		/// </summary>
		/// <remarks>No debe usarse con un string que ya represente una URL</remarks>
		/// <param name="urlWord">La cadena que se desea limpiar</param>
		/// <returns>Cadena a limpia para usar en una URL</returns>
		public static string ToUrl(this string urlWord)
		{
			urlWord = urlWord.Trim().Replace("..", ".").Replace("./", "/").Replace("-&-", "-").Replace("?.", ".");
			urlWord = UrlCleanRegEx.Replace(urlWord, "");
			return Dasherize(Unaccent(urlWord)).ToLowerInvariant();
		}

		public static IEnumerable<string> SplitWords(this string composedPascalCaseWords)
		{
		    return from Match regex in WordsSpliter.Matches(composedPascalCaseWords) select regex.Value;
		}

        /// <summary>
        /// Parsea un string a int
        /// </summary>
        /// <param name="source">el string</param>
        /// <param name="defaultValue">el valor de default</param>
        /// <returns>
        /// El valor int del string si es posible parsearlo, si no el valor de default.
        /// </returns>
        public static int SafeParse(this string source, int defaultValue)
        {
            if (string.IsNullOrEmpty(source))
            {
                return defaultValue;
            }
            int result;
            return int.TryParse(source, out result) ? result : defaultValue;
        }

        public static bool IsIn(this string source, IEnumerable<string> values)
        {
            return values.Contains(source);
        }

        public static string Sanitize(this string source)
        {
            // porting desde la implementacción en UIHelper
            // Fabio: cuando tenga tiempo para entender lo que hace el IsLegalXmlChar y encuentro documentacción
            // lo implemento con una regEx.
            if (string.IsNullOrEmpty(source))
            {
                return source;
            }

            var buffer = new StringBuilder(source.Length);

            foreach (char c in source.Where(c => IsLegalXmlChar(c)))
            {
                buffer.Append(c);
            }

            return buffer.ToString();
        }

        /// <summary>
        /// Whether a given character is allowed by XML 1.0.
        /// </summary>
        private static bool IsLegalXmlChar(int character)
        {
            return
            (
                     character == 0x9 /* == '\t' == 9   */        ||
                     character == 0xA /* == '\n' == 10  */        ||
                     character == 0xD /* == '\r' == 13  */        ||
                    (character >= 0x20 && character <= 0xD7FF) ||
                    (character >= 0xE000 && character <= 0xFFFD) ||
                    (character >= 0x10000 && character <= 0x10FFFF)
            );
        }

        //TODO: ver referencias
		//public static string GetMd5Hash(this string source)
		//{
		//	if (source == null)
		//	{
		//		return null;
		//	}
		//	MD5 md5 = MD5.Create();
        //    byte[] inputBytes = ASCII.GetBytes(source);
		//	byte[] hash = md5.ComputeHash(inputBytes);
        //
		//	var sb = new StringBuilder(64);
		//	Array.ForEach(hash, x => sb.Append(x.ToString("X2")));
		//	return sb.ToString();
		//}


		#region Nested type: RegexReplacement

		private class RegexReplacement
		{
			public RegexReplacement(Regex regex, string replacement)
			{
				Regex = regex;
				Replacement = replacement;
			}

			public Regex Regex { get; private set; }
			public string Replacement { get; private set; }
		}

		#endregion


        public static string GetMeasureString(double lat1, double lon1, double lat2, double lon2)
        {
            var meters = Math.Round(FromPointToMeters(lat1, lon1, lat2, lon2), 0);
            return meters < 1000 ? string.Concat(meters, "m") : string.Concat(Math.Round(meters / 1000, 1), "km");
        }

        public static double FromPointToMeters(double lat1, double lon1, double lat2, double lon2)
        {
            const double r = 6378.137; // Radius of earth in KM
            var dLat = (lat2 - lat1) * Math.PI / 180;
            var dLon = (lon2 - lon1) * Math.PI / 180;
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
            Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
            Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = r * c;
            return d * 1000; // meters
        }
	}
}
