/// 分割ValidTo是NULL的数据
/// db: 2016-10-10 -> null; input: 2016-10-12 -> 2016-12-15
/// result: 2016-10-10 -> 2016-10-11, 2016-10-12 -> 2016-12-15, 2016-12-16 -> null
/// dbItemCommissionData(数据库中存在的数据)
/// InputItemCommissionRate(外部传入的数据)
/// </summary>
public static List<ItemCommissionSetting> GenerateNewItemCommissionRate(List<ItemCommissionSetting> dbItemCommissionData, List<ItemCommissionSetting> inputItemCommissionRate, ItemCommissionSource Source)
        {
            var changeList = new List<ItemCommissionSetting>();
            if (Source == ItemCommissionSource.Detail)
                return changeList;

            if (dbItemCommissionData == null || dbItemCommissionData.Count == 0)
                return inputItemCommissionRate;

            bool splitFlag;
            foreach (var inputCommissionRate in inputItemCommissionRate)
            {
                splitFlag = false;
                foreach (var dbCommission in dbItemCommissionData)
                {
                    if (!string.Equals(dbCommission.ItemNumber, inputCommissionRate.ItemNumber, StringComparison.OrdinalIgnoreCase) ||
                      inputCommissionRate.TransactionNumber == dbCommission.TransactionNumber)
                        continue;

                    if (dbCommission.ValidTo == null)
                    {
                        //if (inputCommissionRate.ValidFrom <= dbCommission.ValidFrom)
                        if (inputCommissionRate.ValidFrom < dbCommission.ValidFrom)
                            continue;

                        splitFlag = true;
                        if (inputCommissionRate.ValidTo == null)
                        {
                            if (inputCommissionRate.ValidFrom == dbCommission.ValidFrom)
                            {
                                /*和数据库中一样，就不用新增*/
                                changeList.Add(new ItemCommissionSetting {
                                    ItemNumber = dbCommission.ItemNumber,
                                    TransactionNumber = dbCommission.TransactionNumber,
                                    ValidFrom = dbCommission.ValidFrom,
                                    ValidTo = dbCommission.ValidTo,
                                    CommissionFee = inputCommissionRate.CommissionFee
                                });
                            } 
                            else
                            /*分成两段*/
                            changeList.AddRange(
                                new List<ItemCommissionSetting>{
					            /*原来的部分*/
					            new ItemCommissionSetting {
					                    ItemNumber = dbCommission.ItemNumber,
					                    TransactionNumber = dbCommission.TransactionNumber,
					                    ValidFrom = dbCommission.ValidFrom,
					                    ValidTo = inputCommissionRate.ValidFrom.AddSeconds(-1),
					                    CommissionFee = dbCommission.CommissionFee
					            }, 
					            /*新增的部分*/
					            new ItemCommissionSetting { 
					                    ItemNumber = dbCommission.ItemNumber,
					                    TransactionNumber = 0,
					                    ValidFrom = inputCommissionRate.ValidFrom,
					                    ValidTo = inputCommissionRate.ValidTo,
					                    CommissionFee = inputCommissionRate.CommissionFee
					            }});
                        }
                        else
                        {
                            if (inputCommissionRate.ValidFrom == dbCommission.ValidFrom)
                            {
                                changeList.AddRange(
                                new List<ItemCommissionSetting> {
                                    /*新增部分*/
                                    new ItemCommissionSetting
                                    {
                                        ItemNumber = inputCommissionRate.ItemNumber,
                                      TransactionNumber = 0,
                                      ValidFrom = inputCommissionRate.ValidFrom,
                                      ValidTo = inputCommissionRate.ValidTo,
                                      CommissionFee = inputCommissionRate.CommissionFee
                                  },
                                  /*原来部分*/
                                  new ItemCommissionSetting
                                  {
                                      ItemNumber = dbCommission.ItemNumber,
                                      TransactionNumber = dbCommission.TransactionNumber,
                                  ValidFrom = inputCommissionRate.ValidTo.Value.AddSeconds(1),
                                  ValidTo = dbCommission.ValidTo,
                                  CommissionFee = dbCommission.CommissionFee
                              }
                          });
                  }
                  else
                  /*分成三段*/
                  changeList.AddRange(
                      new List<ItemCommissionSetting> { 
                /*原来部分*/
                new ItemCommissionSetting{
                        ItemNumber = dbCommission.ItemNumber,
                        TransactionNumber = dbCommission.TransactionNumber,
                        ValidFrom = dbCommission.ValidFrom,
                        ValidTo = inputCommissionRate.ValidFrom.AddSeconds(-1),
                        CommissionFee = dbCommission.CommissionFee
                },
                /*新增部分*/
                new ItemCommissionSetting{
                        ItemNumber = dbCommission.ItemNumber,
                        TransactionNumber = 0,
                        ValidFrom = inputCommissionRate.ValidFrom,
                        ValidTo = inputCommissionRate.ValidTo,
                        CommissionFee = inputCommissionRate.CommissionFee
                },
                /*新增部分*/
                new ItemCommissionSetting{
                        ItemNumber = dbCommission.ItemNumber,
                        TransactionNumber = 0,
                        ValidFrom = inputCommissionRate.ValidTo.Value.AddSeconds(1),
                        ValidTo = dbCommission.ValidTo,
                        CommissionFee = dbCommission.CommissionFee
                }});
              }
          }
      }

        if (!splitFlag)
        {
            changeList.Add(
                 new ItemCommissionSetting
                 {
                     ItemNumber = inputCommissionRate.ItemNumber,
                     TransactionNumber = inputCommissionRate.TransactionNumber,
                     ValidFrom = inputCommissionRate.ValidFrom,
                     ValidTo = inputCommissionRate.ValidTo,
                     CommissionFee = inputCommissionRate.CommissionFee
                 });
        }
    }

    return changeList;
}


public class ItemCommissionSetting : CommissionSetting
    {
        public string IndustryCode { get; set; }

        public string IndustryName { get; set; }

        public ItemCondition Condition { get; set; }

        public string SellerPartNumber { get; set; }

        public decimal PacksOrSets { get; set; }

        public string Manufactory { get; set; }

        public ItemStatus Active { get; set; }

        public int SubcategoryId { get; set; }

        public string SubcategoryName { get; set; }

        public string ItemNumber { get; set; }

        public decimal SellingPrice { get; set; }

        public CommissionStatus Status { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string ProductUrl { get; set; }
    }
    
    public class CommissionSetting
    {
        public int? TransactionNumber { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime? ValidTo { get; set; }

        public decimal? CommissionRate { get; set; }

        public decimal? CommissionFee { get; set; } // available for item level commission only

        public DateTime? LastEditDate { get; set; }

        public string LastEditUser { get; set; }

        // public DateTime CreateDate { get; set; }

        // public string CreateUser { get; set; }
    }
    
    
    -----------------------------------------------------------------------------------------------------------------------------------
    -----------------------------------------------------------------------------------------------------------------------------------
    
    public class TimeLIneBuilder
    {
        /// <summary>
        /// 在上层timline的基础上生成新的TimeLine
        /// </summary>
        public static List<CommissionTimelineEntry> GenerateTimeLine(
            List<CommissionTimelineEntry> subcategoryTimeLine, 
            List<ItemCommissionSetting> tempItemCommission)
        {
            var splitSubcategoryTimeLine = (List<CommissionTimelineEntry>)subcategoryTimeLine.DeepClone<List<CommissionTimelineEntry>>();

            var result = new List<CommissionTimelineEntry>();
            foreach (var itemTime in tempItemCommission.OrderBy(item => item.ValidFrom).ToList())
            {
                result.Clear();
                /*前段*/
                var itemLevelStartDate = itemTime.ValidFrom;
                var leftcategoryTemp = splitSubcategoryTimeLine.FindAll(item => itemLevelStartDate > item.StartDate);
                foreach (var left in leftcategoryTemp)
                {
                    var temp = new
                            CommissionTimelineEntry
                            {
                                CommissionId = left.CommissionId,
                                StartDate = left.StartDate,
                                EndDate = left.EndDate,
                                Type = CommissionLevel.Item == left.Level ? TimelineType.Customized : TimelineType.Inherited,
                                Level = left.Level,
                                CommissionRate = left.CommissionRate,
                                CommissionFee = left.CommissionFee,
                                LastEditDate = left.LastEditDate,
                                LastEditUser = left.LastEditUser
                            };

                    if (left.EndDate == null || left.EndDate > itemLevelStartDate)
                        temp.EndDate = itemLevelStartDate.AddSeconds(-1);

                    result.Add(temp);
                }

                /*中间段*/
                result.Add(new
                CommissionTimelineEntry
                {
                    CommissionId = itemTime.TransactionNumber,
                    StartDate = itemTime.ValidFrom,
                    EndDate = itemTime.ValidTo,
                    Type =  TimelineType.Customized,
                    Level = CommissionLevel.Item,
                    CommissionFee = itemTime.CommissionFee,
                    CommissionRate = itemTime.SellingPrice == 0 ? 0 : itemTime.CommissionFee /itemTime.SellingPrice,
                    LastEditDate = itemTime.LastEditDate,
                    LastEditUser = itemTime.LastEditUser
                });

                if (itemTime.ValidTo == null)
                {
                    splitSubcategoryTimeLine = (List<CommissionTimelineEntry>)result.DeepClone<List<CommissionTimelineEntry>>();
                    continue;
                }

                /*后段*/
                var itemlevelEndDate = itemTime.ValidTo;
                var rightcategoryTemp = subcategoryTimeLine.FindAll(item => item.EndDate == null || item.EndDate > itemlevelEndDate);
                foreach (var right in rightcategoryTemp)
                {
                    var temp = new
                            CommissionTimelineEntry
                            {
                                CommissionId = right.CommissionId,
                                StartDate = right.StartDate,
                                EndDate = right.EndDate,
                                Type = CommissionLevel.Item == right.Level ? TimelineType.Customized : TimelineType.Inherited,
                                Level = right.Level,
                                CommissionRate = right.CommissionRate,
                                CommissionFee = right.CommissionFee,
                                LastEditDate = right.LastEditDate,
                                LastEditUser = right.LastEditUser
                            };
                    if (right.StartDate < itemlevelEndDate)
                        temp.StartDate = itemlevelEndDate.Value.AddSeconds(1);

                    result.Add(temp);
                }

                splitSubcategoryTimeLine = (List<CommissionTimelineEntry>)result.DeepClone<List<CommissionTimelineEntry>>();
            }

            var dtNow = DateTime.Now;
            splitSubcategoryTimeLine.ForEach(time =>
            {
                if (dtNow >= time.StartDate && (time.EndDate == null || dtNow <= time.EndDate))
                {
                    time.IsCurrent = true;
                    time.Status = CommissionStatus.Current;
                }
                else if (dtNow < time.StartDate)
                {
                    time.IsCurrent = false;
                    time.Status = CommissionStatus.Scheduled;
                }
                else
                {
                   time.IsCurrent = false;
                    time.Status = CommissionStatus.Expired; 
                }
            });

            return splitSubcategoryTimeLine.OrderBy(item => item.StartDate).ToList();
        }

        /// <summary>
        /// 获取当前的时间线和前后标识
        /// </summary>
        public static List<CommissionTimelineEntry> GenerateCurrentTimeline(
            List<CommissionTimelineEntry> allTimeLine,
            out bool FutureTimelineOmitted,
            out bool PastTimelineOmitted)
        {
            FutureTimelineOmitted = false; PastTimelineOmitted = false;

            if (allTimeLine == null || allTimeLine.Count == 0)
                return null;

            var currentTimeLine = new List<CommissionTimelineEntry>();
            for (int i = 0; i < allTimeLine.Count; i++)
            {
                if (allTimeLine[i].IsCurrent)
                {
                    if (i >= 1)
                        currentTimeLine.Add(allTimeLine[i - 1]);
                    if (i >= 2)
                        PastTimelineOmitted = true;

                    currentTimeLine.Add(allTimeLine[i]);

                    if (i + 1 < allTimeLine.Count)
                        currentTimeLine.Add(allTimeLine[i + 1]);
                    if (i+2 < allTimeLine.Count)
                        FutureTimelineOmitted = true;
                }
            }
            return currentTimeLine;
        }
    }
    
    ------------------------------------------------------------------
    public class CommissionTimelineEntry
    {

        public int? CommissionId { get; set; }

        public decimal? CommissionRate { get; set; }

        public decimal? CommissionFee { get; set; }

        public TimelineType Type { get; set; }

        public CommissionLevel Level { get; set; }

        public CommissionStatus Status { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsCurrent { get; set; }

        public DateTime? LastEditDate { get; set; }

        public string LastEditUser { get; set; }
    }
    
    ------------------------
    public enum TimelineType
    {
        // Inherit from upper level
        Inherited,

        // Customzied at current level
        Customized
    }

    public enum CommissionStatus
    {
        Scheduled,
        Current,
        Expired
    }

    public enum CommissionLevel
    {
        IndustryStandard,
        Industry,
        Subcategory,
        Item
    }
    
    
    
        
