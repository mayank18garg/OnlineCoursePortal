﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Assignment6._0
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPayment" in both code and config file together.
    [ServiceContract]
    public interface IPaymentService
    {
        [OperationContract]
        string ValidatePayment(long creditCardNum, string expiryDate, string cvv);
    }
}
