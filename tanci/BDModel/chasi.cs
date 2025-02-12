//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace tanci.BDModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class chasi
    {
        public int TovarID { get; set; }
        public string TovarName { get; set; }
        public string TovarDescription { get; set; }
        public int TovarMaster { get; set; }
        public decimal TovarCost { get; set; }
        public decimal TovarDiscountAmount { get; set; }
        public int TovarManufacturer { get; set; }
        public string TovarPhoto { get; set; }
    
        public virtual Manufacturers Manufacturers { get; set; }
        public virtual Master Master { get; set; }

        public string BackgroundColor
        {
            get
            {
                if (TovarDiscountAmount >= 10)
                    return "Blue";
                return "fff";
            }
        }

        public decimal TovarCostDisc
        {
            get
            {
                var TovarCostD = TovarCost * (100 - TovarDiscountAmount) / 100;
                return TovarCostD;
            }
        }

        public string CostDeco
        {
            get
            {
                if (TovarDiscountAmount == 0)
                    return "None";
                return "Strikethrough";
            }
        }

        public string CostHid
        {
            get
            {
                if (TovarDiscountAmount == 0)
                    return "Hidden";
                return "Visible";
            }
        }

        public string CostColor
        {
            get
            {
                if (TovarDiscountAmount == 0)
                    return "Black";
                return "Red";
            }
        }
    }
}
