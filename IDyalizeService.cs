using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace DyalizeService
{
    [ServiceContract(
        Namespace = "http://api.dyalize.serv04.pragmasoft.dk/DyalizeService/v1",
        ProtectionLevel = System.Net.Security.ProtectionLevel.None,
        SessionMode = SessionMode.Allowed)]
    public interface IDyalizeService
    {
        [OperationContract]
        FoodCategoryWithTimeStamp GetFoods();

        [OperationContract]
        bool IsUpdateAvailble(string timestamp);

        //[OperationContract]
        //UpdateAndRemovedItems GetFoodsWithTimeStamp(string timestamp);
    }
}
