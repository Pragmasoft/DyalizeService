using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace DyalizeService
{
    [DataContract]
    public class FoodCategoryWithTimeStamp
    {
       [DataMember]
       public string TimestampOnFoodItems { get; set; }

       [DataMember]
       public List<FoodCategory> updatedFoodItems { get; set; }
    }
}