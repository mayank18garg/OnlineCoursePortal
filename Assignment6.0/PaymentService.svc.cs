using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;

namespace Assignment6._0
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Payment" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Payment.svc or Payment.svc.cs at the Solution Explorer and start debugging.
    public class Payment : IPaymentService
    {
        public string ValidatePayment(long creditCardNum, string expiryDate, string cvv)
        {
            int length = creditCardNum.ToString().Length;
            string last4digit = creditCardNum.ToString().Substring(length - 4); //convert credit card num to string and extract last 4 digit

            int cardDigits = Int32.Parse(last4digit); //convert last 4 digit string to int

            string response = "";

            if (length != 16)
            {
                return "Invalid card number length. Please enter a valid card number";
            }

            else if(!(5000 < cardDigits && cardDigits < 7000))
            {
                return "Card Formatted not supported. Please try a different card.";
            }
            else
            {
                var monthCheck = new Regex(@"^(0[1-9]|1[0-2])$");
                var yearCheck = new Regex(@"^20[0-9]{2}$");
                var cvvCheck = new Regex(@"^\d{3}$");

                if (!cvvCheck.IsMatch(cvv)) // <2>check cvv is valid as "999"
                    return "Inavlid cvv. It should be of 3 digits. Please try again.";

                var dateParts = expiryDate.Split('/'); //expiry date in from MM/yyyy            
                if (!monthCheck.IsMatch(dateParts[0]) || !yearCheck.IsMatch(dateParts[1])) // <3 - 6>
                    return "Invalid format. Please try again."; // ^ check date format is valid as "MM/yyyy"

                var year = int.Parse(dateParts[1]);
                var month = int.Parse(dateParts[0]);
                var lastDateOfExpiryMonth = DateTime.DaysInMonth(year, month); //get actual expiry date
                var cardExpiry = new DateTime(year, month, lastDateOfExpiryMonth, 23, 59, 59);
                if(!(cardExpiry > DateTime.Now && cardExpiry < DateTime.Now.AddYears(6)))
                {
                    return "Card Expiry Date Not Valid.Please try again.";
                }

                response = "Payment completed successfully.";
            }

            return response;

        }
    }
}
