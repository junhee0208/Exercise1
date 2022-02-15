using System;
using System.Data.SqlClient;

namespace InterviewCodeReviewTest
{
	public class Test2
	{
		// Record customer purchase and update customer reward programme
		public Result UpdateCustomerHistory(Purchase customerPurchase)
		{
			var connPruchase = new SqlConnection("data source=TestPurchaseServer;initial catalog=PurchaseDB;Trusted_Connection=True");
			var connReward = new SqlConnection("data source=TestRewardServer;initial catalog=RewardDB;Trusted_Connection=True");

            string customerId = customerPurchase.CustomerId;
            string productId = customerPurchase.ProductId;
            double amount = customerPurchase.PurchaseAmount;
            double reward = customerPurchase.Reward;


            var cmdPurchase = new SqlCommand("INSERT INTO dbo.Purchase (customerId, productId, totalAmount)" +
                                            " VALUES ('" + customerId + "','" + productId + "'," + amount + ");"); // omitted the columns
            var cmdReward = new SqlCommand("INSERT INTO dbo.Reward (customerId, reward) VALUES ('" + customerId + "'," + reward + ");"); // omitted the columns

			SqlTransaction tranPurchase = null;
			SqlTransaction tranReward = null;

			try
			{
				connPruchase.Open();
				tranPurchase = connPruchase.BeginTransaction();
				cmdPurchase.ExecuteNonQuery();

				connReward.Open();
				tranReward = connReward.BeginTransaction();
				cmdReward.ExecuteNonQuery();

				tranPurchase.Commit();
				tranReward.Commit();

				return Result.Success();
			}
			catch (Exception ex)
			{
				tranPurchase.Rollback();
				tranReward.Rollback();

				return Result.Failed();
			}
		}
	}

    public class Purchase
    {
        // Some members
        public string CustomerId;
        public string ProductId;
        public double PurchaseAmount;
        public double Reward;

        public Purchase(string customerId, string productId, double amount){
            this.CustomerId = customerId;
            this.ProductId = productId;
            this.PurchaseAmount = amount;
            this.Reward = amount * 0.1;
        }
    }

	public class Result
	{
		public bool IsSuccessful { get; private set; }

		public static Result Success()
		{
			return new Result { IsSuccessful = true };
		}

		public static Result Failed()
		{
			return new Result { IsSuccessful = false };
		}
	}
}
