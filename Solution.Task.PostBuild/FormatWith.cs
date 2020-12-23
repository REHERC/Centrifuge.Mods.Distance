// Original code by James Newton King:
// http://james.newtonking.com/archive/2008/03/29/formatwith-2-0-string-formatting-with-named-variables
// I modified it to not throw errors when non-existent properties are used, thanks for this stackoverflow post, took me some time to find
// https://stackoverflow.com/questions/3773857/escape-curly-brace-in-string-format

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI;

public static class FormatWithExtensions
{
	public static string FormatWith(this string format, object source)
	{
		return FormatWith(format, null, source);
	}

	public static string FormatWith(this string format, IFormatProvider provider, object source)
	{
		if (format == null)
		{
			throw new ArgumentNullException("format");
		}

		Regex r = new Regex(@"(?<start>\{)+(?<property>[\w\.\[\]]+)(?<format>:[^}]+)?(?<end>\})+",
		  RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

		List<object> values = new List<object>();
		string rewrittenFormat = r.Replace(format, delegate (Match m)
		{
			Group startGroup = m.Groups["start"];
			Group propertyGroup = m.Groups["property"];
			Group formatGroup = m.Groups["format"];
			Group endGroup = m.Groups["end"];

			try
			{
				values.Add((propertyGroup.Value == "0")
				  ? source
				  : DataBinder.Eval(source, propertyGroup.Value));

				return new string('{', startGroup.Captures.Count) + (values.Count - 1) + formatGroup.Value
				+ new string('}', endGroup.Captures.Count);
			}
			catch
			{
				return $"{"{{"}{propertyGroup.Value}{"}}"}";
			}
		});

		return string.Format(provider, rewrittenFormat, values.ToArray());
	}
}
