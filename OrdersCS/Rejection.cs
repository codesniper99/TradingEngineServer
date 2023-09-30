using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingEngineServer.Orders;

namespace TradingEngineServer.Rejects
{
    public class Rejection : IOrderCore
    {
        public Rejection( IOrderCore rejectedOrder, RejectionReason rejectionReason)
        {
            // PROPERTIES // 
            RejectionReason = rejectionReason;

            // FIELDS //

            _orderCore = rejectedOrder;

        }

       
        // PROPERTIES // 
        public RejectionReason RejectionReason { get; private set; }
        public long OrderId => _orderCore.OrderId;

        public string Username => _orderCore.Username;

        public int SecurityId => _orderCore.SecurityId;

        // FIELDS 
        private readonly IOrderCore _orderCore;

    }
}
