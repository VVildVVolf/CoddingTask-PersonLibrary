using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Operations.Aggregation{
    //It is just the POCO, there is nothing to test.
    public class GroupingResult {
        public GroupingResult(object key, IEnumerable<GroupingResult> groupValues, bool isHorisontal = false){
            Key = key;
            GroupValues = groupValues;
            IsHorisontal = isHorisontal;
        }
        public bool IsHorisontal {get; private set;}
        public object Key {get; private set;}
        public IEnumerable<GroupingResult> GroupValues {get; private set;}
    } 
}