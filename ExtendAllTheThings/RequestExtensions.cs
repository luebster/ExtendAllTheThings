using Microsoft.AspNetCore.Http;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ExtendAllTheThings
{
	public static class RequestExtensions
	{
		// e.g. https
		public static string Protocol(this HttpRequest Request)
		{
			return Request.Scheme;
		}

		// e.g. pavey.azurewebsites.net
		public static string Host(this HttpRequest Request)
		{
			return Request.Host.Host.ToLower();
		}

		// e.g. https://pavey.azurewebsites.net/debug/?x=1&y=2
		public static string UrlHttps(this HttpRequest Request)
		{
			return string.Format("https://{0}{1}", Request.Host, Request.Path);
		}

		// e.g. http://pavey.azurewebsites.net/debug/?x=1&y=2
		public static string UrlHttp(this HttpRequest Request)
		{
			return string.Format("http://{0}{1}", Request.Host, Request.Path);
		}

		// e.g. true if value is formatted as a valid IPv4 address
		public static bool IsValidIPAddress(this string Value)
		{
			return Regex.Match(Value, "\\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\b").Success;
		}

		// e.g. true if testing using an IP address instead of a fully qualified host name
		public static bool IsIPAddress(this HttpRequest Request)
		{
			return Request.Host().IsValidIPAddress();
		}

		// e.g. true if testing using localhost
		public static bool IsLocalHost(this HttpRequest Request)
		{
			return Request.Host().Equals("localhost", System.StringComparison.CurrentCultureIgnoreCase);
		}

		// e.g. dev.www
		public static List<string> SubDomains(this HttpRequest Request)
		{
			// variables
			string[] MyArray = Request.Host().Split(".".ToCharArray());
			List<string> MySubDomains = new List<string>();

			// make sure this is not an ip address
			if (Request.IsIPAddress())
			{
				return MySubDomains;
			}

			// make sure we have all the parts necessary
			if (MyArray == null)
			{
				return MySubDomains;
			}

			// last part is the tld (e.g. .com)
			// second to last part is the domain (e.g. mydomain)
			// the remaining parts are the sub-domain(s)
			if (MyArray.Length > 2)
			{
				for (int i = 0; i <= MyArray.Length - 3; i++)
				{
					MySubDomains.Add(MyArray[i]);
				}
			}

			// return
			return MySubDomains;
		}

		//e.g.www
		public static string SubDomain(this HttpRequest Request)
		{
			if (Request.SubDomains().Count > 0)
			{
				// handle cases where multiple sub-domains (e.g. dev.www)
				return Request.SubDomains().Last();
			}
			else
			{
				// handle cases where no sub-domains
				return string.Empty;
			}
		}

		// e.g. azurewebsites.net
		public static string Domain(this HttpRequest Request)
		{
			// variables
			string[] MyArray = Request.Host.Host.Split(".".ToCharArray());

			// make sure this is not an ip address
			if (Request.IsIPAddress())
			{
				return string.Empty;
			}

			// special case for localhost
			if (Request.IsLocalHost())
			{
				return Request.Host().ToLower();
			}

			// make sure we have all the parts necessary
			if (MyArray == null)
			{
				return string.Empty;
			}

			// make sure we have all the parts necessary
			if (MyArray.Length > 1)
			{
				return string.Format("{0}.{1}", MyArray[MyArray.Length - 2], MyArray[MyArray.Length - 1]);
			}

			// return empty string
			return string.Empty;
		}

		// e.g. https://pavey.azurewebsites.net/
		//public static string BaseUrl(this HttpRequest Request)
		//{
		//	// variables
		//	string Authority = Request.Url.GetLeftPart(UriPartial.Authority).TrimStart('/').TrimEnd('/');
		//	string ApplicationPath = Request.ApplicationPath.TrimStart('/').TrimEnd('/');

		//	// add trailing slashes if necessary
		//	if (Authority.Length > 0)
		//	{
		//		Authority += "/";
		//	}

		//	if (ApplicationPath.Length > 0)
		//	{
		//		ApplicationPath += "/";
		//	}

		//	// return
		//	return string.Format("{0}{1}", Authority, ApplicationPath);
		//}

		// e.g. default, default.aspx
		public static string PageName(this HttpRequest Request, bool IncludeExtension = false, bool IncludeQueryString = false)
		{
			// variables
			string AbsolutePath = Request.Path;
			string PageName = Path.GetFileName(AbsolutePath);
			string Extension = Path.GetExtension(AbsolutePath);
			string QueryString = Request.QueryString.ToString();

			// remove extension
			if (!IncludeExtension && !IncludeQueryString && PageName.IsNotEmptyOrWhiteSpace())
			{
				PageName = PageName.Replace(Extension, string.Empty);
			}

			// include querystring
			if (IncludeQueryString && PageName.IsNotEmptyOrWhiteSpace() && QueryString.IsNotEmptyOrWhiteSpace())
			{
				PageName = string.Format("{0}?{1}", PageName, QueryString);
			}

			// return
			return PageName;
		}
	}
}
