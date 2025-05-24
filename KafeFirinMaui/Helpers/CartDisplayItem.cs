using SharedClass.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KafeFirinMaui.Helpers
{
    public class CartDisplayItem : INotifyPropertyChanged
    {
        public Products Product { get; set; }
        public int Quantity { get; set; }

        private decimal _discountRate;
        public decimal DiscountRate
        {
            get => _discountRate;
            set
            {
                if (_discountRate != value)
                {
                    _discountRate = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(DiscountedPrice));
                    OnPropertyChanged(nameof(IsDiscounted));
                }
            }
        }

        public decimal OriginalPrice => Product?.Price ?? 0;
        public decimal DiscountedPrice => OriginalPrice * (1 - DiscountRate);
        public bool IsDiscounted => DiscountRate > 0;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
