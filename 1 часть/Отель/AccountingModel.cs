using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelAccounting
{
    public class AccountingModel : ModelBase
    {
        private double price;
        private int nightsCount;
        private double discount;
        private double total;

        public double Price
        {
            get { return price; }
            set
            {
                if (value < 0)
                    throw new ArgumentException();
                price = value;
                Notify(nameof(Price));
                NotifyAndChangeTotal();
            }
        }

        public int NightsCount
        {
            get { return nightsCount; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException();
                nightsCount = value;
                Notify(nameof(NightsCount));
                NotifyAndChangeTotal();
            }
        }

        public double Discount
        {
            get { return discount; }
            set
            {
                discount = value;
                NotifyAndChangeTotal();
                if (total <= 0)
                    throw new ArgumentException();
                Notify(nameof(Discount));
            }
        }

        public double Total
        {
            get { return total; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException();
                total = value;
                discount = (total / (price * nightsCount) - 1) * 100 * -1;
                Notify(nameof(Discount));
                Notify(nameof(Total));
            }
        }

        public void NotifyAndChangeTotal()
        {
            total = price * nightsCount * (1 - discount / 100);
            Notify(nameof(Total));
        }
    }
}