using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace DyalizeService
{
    [DataContract]
    public class FoodCategory
    {
        [DataMember]
        public string categoryName { get; set; }

        [DataMember]
        public List<FoodItem> foodItems {get;set;}

        public FoodCategory()
        {
            foodItems = new List<FoodItem>();
        }
    }
}