using System;

namespace Operations.Aggregation{
    public class AggregationException: Exception{
        private AggregationException(string message): base(message){}

        internal static AggregationException DuplicateField(){
            return new AggregationException($"The set contains some duplicate fields.");
        }
        internal static AggregationException EmptyFieldSet(bool isVertical){
            return new AggregationException($"The {(isVertical?"vertical":"horisontal")} direction should contain at leas one field to be grouped.");
        }
        internal static AggregationException UnknownFieldName(string fieldName, Type type){
            return new AggregationException($"the '{fieldName}' is not found in the '{type.FullName}' type.");
        }
    }
}