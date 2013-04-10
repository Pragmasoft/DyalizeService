using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Web;

namespace DyalizeService
{
    [DataContract]
    public class UpdateAndRemovedItems
    {
            [DataMember]
            public List<string> removedItems { get; set; }

            [DataMember]
            public List<FoodItem> updatedFoodItems { get; set; }
    }
}