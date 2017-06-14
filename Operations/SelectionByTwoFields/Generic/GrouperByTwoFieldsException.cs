using System;
using CommonEntities;

namespace Operations.SelectionByTwoFields.Generic {
    public class GrouperByTwoFieldsException : Exception {
        private GrouperByTwoFieldsException(string message) : base(message){}
        
        internal static GrouperByTwoFieldsException InvalidFieldName(Type type, string invalidFieldName){
            var message = $"The '{type.FullName}' class does not contain the '{invalidFieldName}' field.";
            return new GrouperByTwoFieldsException(message);
        }
        internal static GrouperByTwoFieldsException EqualFields(Type type){
            var message = $"The field names of the '{type.FullName}' class cannot be equal.";
            return new GrouperByTwoFieldsException(message);
        }
    }
}