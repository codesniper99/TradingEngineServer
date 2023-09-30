using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingEngineServer.Orders;

namespace TradingEngineServer.Orders
{
    public class Order : IOrderCore
    {
        public Order(IOrderCore orderCore, long price, uint quantity, bool isBuySide)
        {
            // PROPERTIES //
            Price = price;
            IsBuySide = isBuySide;

            // FIELDS /
            _orderCore = orderCore;
            CurrentQuantity = quantity;
        }

        public Order(ModifyOrder modifyOrder) : 
            this(modifyOrder, modifyOrder.Price, modifyOrder.Quantity, modifyOrder.IsBuyside)
        {
            
        }

        public long Price { get; private set; }

        public uint InitialQuantity { get; private set; }
        public uint CurrentQuantity { get; private set; }

        public bool IsBuySide { get; private set; }

        // METHODS // 
        public void IncreaseQuantity(uint quantityDelta)
        {
            CurrentQuantity += quantityDelta;
        }

        public void DecreaseQuantity(uint quantityDelta)
        {
            if(quantityDelta > CurrentQuantity)
            {
                throw new InvalidOperationException($"Quantity Delta > Current Quantity for OrderId = {OrderId}");
    
            }
            CurrentQuantity -= quantityDelta;
        }

        public long OrderId => _orderCore.OrderId;

        public string Username => _orderCore.Username;

        public int SecurityId => _orderCore.SecurityId;


        // FIELDS // 

        private readonly IOrderCore _orderCore;
    }
}
