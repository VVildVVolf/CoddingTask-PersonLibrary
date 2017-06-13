using System;
using System.Collections.Generic;
using CommonEntities;

namespace Operations.SelectionByTwoFields{
    public class GrouperByTwoFieldsValidationDecorator : IGrouperByTwoFields
    {
        public IDictionary<Tuple<object, object>, T> GroupByTwoFields<T>(List<Person> source, string fieldName1, string fieldName2, Func<IEnumerable<Person>, T> aggregation)
        {
            Validate(fieldName1, fieldName2, aggregation);
            return _grouperByTwoFields.GroupByTwoFields(source, fieldName1, fieldName2, aggregation);
        }

        private void Validate<T>(string fieldName1, string fieldName2, Func<IEnumerable<Person>, T> aggregation){
            if (!PersonSelectors.Selectors.Keys.Contains(fieldName1)){
                throw GrouperByTwoFieldsException.InvalidFieldName(fieldName1);
            }
            if (!PersonSelectors.Selectors.Keys.Contains(fieldName2)){
                throw GrouperByTwoFieldsException.InvalidFieldName(fieldName2);
            }
            if (fieldName1 == fieldName2){
                throw GrouperByTwoFieldsException.EqualFields();
            }
        }

        private readonly IGrouperByTwoFields _grouperByTwoFields;
        public GrouperByTwoFieldsValidationDecorator(IGrouperByTwoFields grouperByTwoFields){
            if (grouperByTwoFields == null){
                throw new ArgumentException("The decorated object cannot be null.");
            }
            if (grouperByTwoFields is GrouperByTwoFieldsValidationDecorator){
                throw new ArgumentException("The recursive decoration is detected.");
            }
            _grouperByTwoFields = grouperByTwoFields;
        }
    }
}