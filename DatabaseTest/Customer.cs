using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace DatabaseTest
{
    class Customer
    {
        public int custID;
        public DateTime created;
        public DateTime lastOrder;
        public int noOfOrders;
        public int basketSum;
        public int basketAvg;
        public int priceSum;
        public int priceAvg;
        public string custStatus;
        public string custName;
        public ArrayList emails;
        public ArrayList addresses;
        public ArrayList phoneNumbers;

        public Customer(int custID, DateTime created, DateTime lastOrder, int noOfOrders, int basketSum, int basketAvg, int priceSum, int priceAvg, string custStatus, string custName)
        {
            this.custID = custID;
            this.created = created;
            this.lastOrder = lastOrder;
            this.noOfOrders = noOfOrders;
            this.basketSum = basketSum;
            this.basketAvg = basketAvg;
            this.priceSum = priceSum;
            this.priceAvg = priceAvg;
            this.custStatus = custStatus;
            this.custName = custName;
        }
        override
        public String ToString()
        {
            string createdString = this.created.ToShortDateString();
            string lastSeenString = this.lastOrder.ToShortDateString();
            string correctedName = (this.custName.Length >= 7 ? this.custName.Substring(0, 7) : this.custName);
            string customer = this.custID + "\t" +  correctedName + "\t\t" + this.custStatus + "\t" + createdString + "\t" + lastSeenString + "\t" + this.noOfOrders + "\t" + this.basketSum + "\t" + this.basketAvg + "\t" + this.priceSum + "\t" + this.priceAvg;
            return customer;
        }
    }
}
