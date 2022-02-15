using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace InterviewCodeReviewTest
{
	public class Test1
	{
		// Called by web API and returns list of strongly typed customer address for given status
		// CustomerAddress is populated by external import and could be dirty
		public IEnumerable<Address> GetCustomerNumbers(string status)
		{
			var connection = new SqlConnection("data source=TestServer;initial catalog=CustomerDB;Trusted_Connection=True");
			var cmd = new SqlCommand($"SELECT CustomerAddress FROM dbo.Customer WHERE Status = '{status}'", connection);

			try
			{
				var addressStrings = new List<string>();

                connection.Open();
				var reader = cmd.ExecuteReader();

				while (reader.Read())
				{
					addressStrings.Add(reader.GetString(0));
				}


				return addressStrings
					.Select(StringToAddress)
					.Where(x => x != null)
					.ToList();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private static Address StringToAddress(string addressString)
		{
			return new Address(ref addressString);
		}
	}

	public class Address
	{
        // Some members...
        public string IsValidEmail(string emailaddress)
        {
            //Check if email has @ symbol
            if (emailaddress.Contains("@")) {

                //Remove whitespace
                if (emailaddress.Contains(" ")) {
                    emailaddress = emailaddress.Replace(" ", "");
                }

                //Parse email to check if it has period
                string[] parts = emailaddress.Split(new[] { '@' });
                string username = parts[0];
                string host = parts[1];

                if (!host.Contains("."))
                {
                    return "0";
                }

                return emailaddress;
            }

            return "0";
        }
		public Address(ref string addressString)
		{
            // Assume there are logic here to parse address and return strongly typed object
            if (!IsValidEmail(addressString).Equals("0"))
            {
                addressString = IsValidEmail(addressString);
            }

        }
	}
}
