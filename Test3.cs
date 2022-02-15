using System.Net.Mail;
using System.Threading;

namespace InterviewCodeReviewTest
{
	public class Test3
	{
		// This class represents a queue for email sending.
		// There are multiple active queues at any given time and they have activities all the time.
		// Each queue can be handled by multiple threads.
		public class EmailSendQueue
		{
			public int SentCount { get; private set; }
			public int FailedCount { get; private set; }

			// Assign each email to different thread for performance
			public void SendNextEmail()
			{
				var thread = new Thread(SendEmail);
				thread.Start();
			}

			private void SendEmail()
			{

                var client = new SmtpClient
                {
                    // Send email via Smtp and returns Result object...
                    
                    Host = "smtp.gmail.com",
                    Port = 587,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential()
                    {
                        UserName = "",
                        Password = ""
                    }
                };
                MailAddress FromAddress = new MailAddress("qrious@gmail.com", "Qrious");
                MailAddress ToAddress = new MailAddress("customer@gmail.com", "Customer");

                MailMessage Msg = new MailMessage()
                {
                    From = FromAddress,
                    Subject = "Hello Customer",
                    Body = "Thank you"
                };
                Msg.To.Add(ToAddress);


                try
                {
                    client.Send(Msg);
                    UpdateStatistics(Result.Success());
                    SendNextEmail();

                }
                catch(System.Exception ex) {

                    UpdateStatistics(Result.Failed());
                }

            }

            private void UpdateStatistics(Result result)
			{
				lock (typeof(EmailSendQueue))
				{
					if (result.IsSuccessful)
					{
						SentCount++;
					}
					else
					{
						FailedCount++;
					}
				}
			}
		}
	}
}
