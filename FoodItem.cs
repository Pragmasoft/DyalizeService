using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace DyalizeService
{
    [DataContract]
    public class FoodItem
    {
        [DataMember]
        public string foodnavn {get;set;}

        [DataMember]
        public string foodID { get; set; }
        
        [DataMember]
        public string group { get; set; }
        
        [DataMember]
        public string date { get; set; }

        [DataMember]
        public string energivalue { get; set; }

        [DataMember]
        public string energiunit { get; set; }

        [DataMember]
        public string proteinvalue { get; set; }

        [DataMember]
        public string proteinunit { get; set; }

        [DataMember]
        public string proteinmax { get; set; }

        [DataMember]
        public string proteinmin { get; set; }

        [DataMember]
        public string fatvalue { get; set; }

        [DataMember]
        public string fatunit { get; set; }

        [DataMember]
        public string fatmax { get; set; }

        [DataMember]
        public string fatmin { get; set; }

        [DataMember]
        public string fosfatvalue { get; set; }

        [DataMember]
        public string fosfatunit { get; set; }
        
        [DataMember]
        public string fosfatmax { get; set; }

        [DataMember]
        public string fosfatmin { get; set; }

        [DataMember]
        public string kaliumvalue { get; set; }

        [DataMember]
        public string kaliumunit { get; set; }

        [DataMember]
        public string kaliummax { get; set; }

        [DataMember]
        public string kaliummin { get; set; }

        [DataMember]
        public string watervalue { get; set; }

        [DataMember]
        public string waterunit { get; set; }

        [DataMember]
        public string watermax { get; set; }

        [DataMember]
        public string watermin { get; set; }
    }
}