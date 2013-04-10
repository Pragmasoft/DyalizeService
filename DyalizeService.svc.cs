using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.web;


namespace DyalizeService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Single)]
    public class DyalizeService : IDyalizeService
    {
        private static TraceSource _log = new TraceSource("DyalizeService");
        public double updatedTimeFromUmbraco { get; set; }
        public List<FoodCategory> categoriesWithFoodList { get; set; }
        public FoodCategoryWithTimeStamp categoriesWithTimeStamp { get; set; }
       
        //public List<string> deletedItemsWithIDs { get; set; }
        //public List<FoodItem> tempFoodListItems { get; set; }
        //public UpdateAndRemovedItems updatedAndRemovedFoodItems { get; set; }


        public bool IsUpdateAvailble(string timestamp)
        {
            var currentNode = new umbraco.NodeFactory.Node(4523);
            var timeFromUmbraco = currentNode.GetProperty("publisherePaaDato").ToString();

            //Laver tiden fra iOS om til epoch (double)
            double timeStampAsDoubleFromiOS = double.Parse(timestamp);

            //Laver umbraco tiden om til epoch
            timeFromUmbraco = timeFromUmbraco.Replace("T", " ");

            DateTime tempDate = new DateTime();
            tempDate = DateTime.ParseExact(timeFromUmbraco, "yyyy-MM-dd HH:mm:ss", null);

            //"2012-12-14T12:30:00"

            DateTime epoch = new DateTime(1970, 1, 1);
            double timeAsEpochFromUmbraco = tempDate.Subtract(epoch).TotalMilliseconds;

            if (timeStampAsDoubleFromiOS < timeAsEpochFromUmbraco) //Hvis tiden fra iOS er mindre end denne returneres true - betyder der er opdateringer.
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public FoodCategoryWithTimeStamp GetFoods()
        {
            try
            {
                categoriesWithFoodList = new List<FoodCategory>();
                categoriesWithTimeStamp = new FoodCategoryWithTimeStamp();
                
                //4523 er id'et på "Food Items" noden i umbraco. Ligger efter "Data" mappen.
                var currentNode = new umbraco.NodeFactory.Node(4523);
                var categoriesList = currentNode.ChildrenAsList;

                //Får også tiden fra Food Items mappen - den indikere den nye tidspunkt for opdateringer.
                var tempTime = currentNode.GetProperty("publisherePaaDato").ToString();

                //Laver umbraco tiden om til epoch
                tempTime = tempTime.Replace("T", " ");

                //Laver umbraco tiden om til epoch
                DateTime tempDateForConvertering = new DateTime();
                tempDateForConvertering = DateTime.ParseExact(tempTime, "yyyy-MM-dd HH:mm:ss", null);

                DateTime epochTime = new DateTime(1970, 1, 1);
                updatedTimeFromUmbraco = tempDateForConvertering.Subtract(epochTime).TotalMilliseconds;

                foreach (var categoryTempNode in categoriesList)
                {
                    var allFoodItemsFromGivenCategory = categoryTempNode.ChildrenAsList;

                    var foodCategory = new FoodCategory();
                    foodCategory.categoryName = categoryTempNode.Name;

                    categoriesWithFoodList.Add(foodCategory);

                    foreach (var foodTempNode in allFoodItemsFromGivenCategory)
                    {
                        FoodItem foodItem = new FoodItem();

                        string lastEditedTimeFromUmbraco = foodTempNode.UpdateDate.ToString();

                        DateTime tempDate = new DateTime();
                        tempDate = DateTime.ParseExact(lastEditedTimeFromUmbraco, "dd-MM-yyyy HH:mm:ss", null);

                        DateTime epoch = new DateTime(1970, 1, 1);
                        double timeAsEpoch = tempDate.Subtract(epoch).TotalMilliseconds;

                        foodItem.foodnavn = foodTempNode.GetProperty("navn").ToString();
                        foodItem.foodID = foodTempNode.GetProperty("madID").ToString();
                        foodItem.group = foodTempNode.GetProperty("gruppe").ToString();
                        foodItem.date = timeAsEpoch.ToString();
                        foodItem.energivalue = foodTempNode.GetProperty("energiVaerdi").ToString();
                        foodItem.energiunit = foodTempNode.GetProperty("energiEnhed").ToString();
                        foodItem.proteinvalue = foodTempNode.GetProperty("proteinVaerdi").ToString();
                        foodItem.proteinunit = foodTempNode.GetProperty("protienEnhed").ToString();
                        foodItem.proteinmax = foodTempNode.GetProperty("protienMaksimumVaerdi").ToString();
                        foodItem.proteinmin = foodTempNode.GetProperty("protienMinimumVaerdi").ToString();
                        foodItem.fatvalue = foodTempNode.GetProperty("fedtVaerdi").ToString();
                        foodItem.fatunit = foodTempNode.GetProperty("fedtEnhed").ToString();
                        foodItem.fatmax = foodTempNode.GetProperty("fedtMaksimumVaerdi").ToString();
                        foodItem.fatmin = foodTempNode.GetProperty("fedtMinimumVaerdi").ToString();
                        foodItem.fosfatvalue = foodTempNode.GetProperty("fosfatVaerdi").ToString();
                        foodItem.fosfatunit = foodTempNode.GetProperty("fosfatEnhed").ToString();
                        foodItem.fosfatmax = foodTempNode.GetProperty("fosfatMaksimumVaerdi").ToString();
                        foodItem.fosfatmin = foodTempNode.GetProperty("fosfatMinimumVaerdi").ToString();
                        foodItem.kaliumvalue = foodTempNode.GetProperty("kaliumVaerdi").ToString();
                        foodItem.kaliumunit = foodTempNode.GetProperty("kaliumEnhed").ToString();
                        foodItem.kaliummax = foodTempNode.GetProperty("kaliumMaksimumVaerdi").ToString();
                        foodItem.kaliummin = foodTempNode.GetProperty("kaliumMinimumVaerdi").ToString();
                        foodItem.watervalue = foodTempNode.GetProperty("vandVaerdi").ToString();
                        foodItem.waterunit = foodTempNode.GetProperty("vandEnhed").ToString();
                        foodItem.watermax = foodTempNode.GetProperty("vandMaksimumVaerdi").ToString();
                        foodItem.watermin = foodTempNode.GetProperty("vandMinimumVaerdi").ToString();

                        foodCategory.foodItems.Add(foodItem);

                    }
                }
            }
            catch (Exception e)
            {
                var ts = new TraceSource("DyalizeService");
                ts.TraceEvent(TraceEventType.Critical, 0, "Exeception e: " + e.ToString());
            }

            categoriesWithTimeStamp.TimestampOnFoodItems = updatedTimeFromUmbraco.ToString();
            categoriesWithTimeStamp.updatedFoodItems = categoriesWithFoodList;

            return categoriesWithTimeStamp;
        }

        /*    GAMMEL KODE - Gemmes dog hvis det kan bruges senere
        public List<string> deletedItems(string timestamp)
        {
            List<String> tempList = new List<String>();

            double timeStampAsDouble = double.Parse(timestamp);

            var type = RecycleBin.RecycleBinType.Content;
            RecycleBin recycleBin = new RecycleBin(type);

            var allItemsInRecycleBin = recycleBin.Children;

            foreach (var itemInRecycleBin in allItemsInRecycleBin)
            {
                //Får fat i den pågældende node.
                var deletedNode = new umbraco.NodeFactory.Node(itemInRecycleBin.Id);

                //Konvertere tiden om til Epoch så vi kan sammenligne med iOS
                string lastEditedTimeFromUmbraco = deletedNode.UpdateDate.ToString();

                DateTime tempDate = new DateTime();
                tempDate = DateTime.ParseExact(lastEditedTimeFromUmbraco, "dd-MM-yyyy HH:mm:ss", null);

                DateTime epoch = new DateTime(1970, 1, 1);
                double timeAsEpoch = tempDate.Subtract(epoch).TotalMilliseconds;

                if (timeStampAsDouble < timeAsEpoch)
                {
                    var nodeWithID = deletedNode.GetProperty("madID").ToString();

                    tempList.Add(nodeWithID);
                }

            }
            return tempList;
        }

        public UpdateAndRemovedItems GetFoodsWithTimeStamp(string timestamp)
        {
            try
            {
                updatedAndRemovedFoodItems = new UpdateAndRemovedItems();
                tempFoodListItems = new List<FoodItem>();

                deletedItemsWithIDs = deletedItems(timestamp);

                double timeStampAsDouble = double.Parse(timestamp);

                var currentNode = new umbraco.NodeFactory.Node(4523); //Food Items noden
                
                var categoriesList = currentNode.ChildrenAsList;


                foreach (var categoryTempNode in categoriesList)
                {
                    var allFoodItemsFromGivenCategory = categoryTempNode.ChildrenAsList;

                    foreach (var foodTempNode in allFoodItemsFromGivenCategory)
                    {
                        FoodItem foodItem = new FoodItem();

                        string lastEditedTimeFromUmbraco = foodTempNode.UpdateDate.ToString();

                        DateTime tempDate = new DateTime();
                        tempDate = DateTime.ParseExact(lastEditedTimeFromUmbraco, "dd-MM-yyyy HH:mm:ss", null);

                        DateTime epoch = new DateTime(1970, 1, 1);
                        double timeAsEpoch = tempDate.Subtract(epoch).TotalMilliseconds;

                        if (timeStampAsDouble < timeAsEpoch) //Hvis tiden der kommer fra iOS er mindre -> Så skal den opdatere i og med der er ændringer.
                        {
                            foodItem.date = timeAsEpoch.ToString(); //Gemmer tiden fra "Last edited" i epoch tid (tid siden 1970 i millisekonder) er i GMT!
                            foodItem.foodnavn = foodTempNode.GetProperty("navn").ToString();
                            foodItem.foodID = foodTempNode.GetProperty("madID").ToString();
                            foodItem.group = foodTempNode.GetProperty("gruppe").ToString();
                            foodItem.energivalue = foodTempNode.GetProperty("energiVaerdi").ToString();
                            foodItem.energiunit = foodTempNode.GetProperty("energiEnhed").ToString();
                            foodItem.proteinvalue = foodTempNode.GetProperty("proteinVaerdi").ToString();
                            foodItem.proteinunit = foodTempNode.GetProperty("protienEnhed").ToString();
                            foodItem.proteinmax = foodTempNode.GetProperty("protienMaksimumVaerdi").ToString();
                            foodItem.proteinmin = foodTempNode.GetProperty("protienMinimumVaerdi").ToString();
                            foodItem.fatvalue = foodTempNode.GetProperty("fedtVaerdi").ToString();
                            foodItem.fatunit = foodTempNode.GetProperty("fedtEnhed").ToString();
                            foodItem.fatmax = foodTempNode.GetProperty("fedtMaksimumVaerdi").ToString();
                            foodItem.fatmin = foodTempNode.GetProperty("fedtMinimumVaerdi").ToString();
                            foodItem.fosfatvalue = foodTempNode.GetProperty("fosfatVaerdi").ToString();
                            foodItem.fosfatunit = foodTempNode.GetProperty("fosfatEnhed").ToString();
                            foodItem.fosfatmax = foodTempNode.GetProperty("fosfatMaksimumVaerdi").ToString();
                            foodItem.fosfatmin = foodTempNode.GetProperty("fosfatMinimumVaerdi").ToString();
                            foodItem.kaliumvalue = foodTempNode.GetProperty("kaliumVaerdi").ToString();
                            foodItem.kaliumunit = foodTempNode.GetProperty("kaliumEnhed").ToString();
                            foodItem.kaliummax = foodTempNode.GetProperty("kaliumMaksimumVaerdi").ToString();
                            foodItem.kaliummin = foodTempNode.GetProperty("kaliumMinimumVaerdi").ToString();
                            foodItem.watervalue = foodTempNode.GetProperty("vandVaerdi").ToString();
                            foodItem.waterunit = foodTempNode.GetProperty("vandEnhed").ToString();
                            foodItem.watermax = foodTempNode.GetProperty("vandMaksimumVaerdi").ToString();
                            foodItem.watermin = foodTempNode.GetProperty("vandMinimumVaerdi").ToString();

                            tempFoodListItems.Add(foodItem);
                        }
                    }
                }

            }
            catch (Exception e) 
            {
                _log.TraceEvent(TraceEventType.Error, 0, "Got error in GetFoodsWithTimeStamp. Exception: " + e.ToString());
            }

            updatedAndRemovedFoodItems.removedItems = deletedItemsWithIDs;
            updatedAndRemovedFoodItems.updatedFoodItems = tempFoodListItems;

            return updatedAndRemovedFoodItems;
        }*/
    }
}
