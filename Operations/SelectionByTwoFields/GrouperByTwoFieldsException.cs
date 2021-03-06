using System;
using CommonEntities;

namespace Operations.SelectionByTwoFields{
    public class GrouperByTwoFieldsException : Exception {
        private GrouperByTwoFieldsException(string message) : base(message){}
        
        internal static GrouperByTwoFieldsException InvalidFieldName(string invalidFieldName){
            var message = $"The {nameof(Person)} class does not contain the '{invalidFieldName}' field.";
            return new GrouperByTwoFieldsException(message);
        }
        internal static GrouperByTwoFieldsException EqualFields(){
            var message = $"The field names cannot be equal.";
            return new GrouperByTwoFieldsException(message);
        }
    }
}