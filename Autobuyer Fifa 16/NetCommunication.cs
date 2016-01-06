using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Autobuyer_Fifa_16 {
	public static class NetCommunication {

		public static int CheckBetaEnd() { 
            Uri URL = new Uri("http://fifa16autobuyer.altervista.org/autodistruzione.html");
			try {
				WebClient webClient = new WebClient();
				Stream stream = webClient.OpenRead(URL);
				StreamReader reader = new StreamReader(stream);
				string request = reader.ReadLine();
				Console.WriteLine(request);
				if (request == "true") return 1;
				else if (request == "false") return 0;
				else return -1;
			}
			catch (WebException ex) {
				if (ex.Response is HttpWebResponse) {
					switch (((HttpWebResponse)ex.Response).StatusCode) {
						case HttpStatusCode.NotFound:
							break;

						default:
							throw ex;
					}
				}
				return -2;
            }
		}

		public static int Login(string email, string password) {
			Uri URL = new Uri("http://fifa16autobuyer.altervista.org/login.php?Email={0}&Password={1}");
			try {
				WebClient webClient = new WebClient();

				string hash;

				MD5 md5Hash = MD5.Create();
				Byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

				StringBuilder sBuilder = new StringBuilder();

				// Loop through each byte of the hashed data 
				// and format each one as a hexadecimal string.
				for (int i = 0; i < data.Length; i++) {
					sBuilder.Append(data[i].ToString("x2"));
				}

				hash = sBuilder.ToString();

				Stream stream = webClient.OpenRead(string.Format(URL.ToString(), email, hash));
				StreamReader reader = new StreamReader(stream);

				string request = reader.ReadLine();

				Console.WriteLine(request);

				if (request == "true")
					return 1;
				else if (request == "false")
					return 0;
				else if (request == "online")
					return 1;					//FORZATO  prima era -1
				else
					return -2;
			}
			catch (WebException ex) {
				if (ex.Response is HttpWebResponse) {
					switch (((HttpWebResponse)ex.Response).StatusCode) {
						case HttpStatusCode.NotFound:
							break;

						default:
							throw ex;
					}
				}
				return -3;
			}
		}

		public static int ChangeAccountOnlineStatus(string email, bool online) {
			Uri URL = new Uri("http://fifa16autobuyer.altervista.org/setOn.php?Email={0}&On={1}");
			try {
				WebClient webClient = new WebClient();
				Stream stream = webClient.OpenRead(string.Format(URL.ToString(), email, online.GetHashCode().ToString()));
				StreamReader reader = new StreamReader(stream);

				string request = reader.ReadLine();

				Console.WriteLine(request);

				if (request == "true")
					return 1;
				else if (request == "false")
					return 0;
				else
					return -1;
			}
			catch (WebException ex) {
				if (ex.Response is HttpWebResponse) {
					switch (((HttpWebResponse)ex.Response).StatusCode) {
						case HttpStatusCode.NotFound:
							break;

						default:
							throw ex;
					}
				}
				return -2;
			}
		}
	}
}
