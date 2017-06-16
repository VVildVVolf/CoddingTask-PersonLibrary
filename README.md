# CoddingTask-PersonLibrary

## How to run tests
You can navigate to the Operations.Test project in a command line, and run "dotnet xunit".

## Description of the project

### /Operations/SelectionByCountryAndAge/Average/AllAverageGetter.cs
    Counts All averages of the GrossSalaried by Country and Age. Returns the collection of the keyvalue pair, where 
    the key is the Tuple of the age and the country, and value is the average value.
    Required for the Operations.Utils.AverageCounting.IAverageKeeperFactory to calculate the average value.
    The defauld implementation of the last interface is the Operations.Utils.AverageCounting.AverageKeeperFactory;
    
### /Operations/Utils/AverageSetting/GrossSalaryAverageSetter.cs
    It sets the average value for the given set of persons with the apropriage age and country. Returns the updated
    records. Required for the Operations.SelectionByCountryAndAge.ISelectorByCountryAndAge to make selection.
    The default implementation of the last interface is the Operations.SelectionByCountryAndAge.SelectorByCountryAndAge.cs
    
### /Operations/SelectionByCountryAndAge/Average/Printing/PrinterToConsole.cs
    Prints the result of the AllAverageGetter calculation to the console as a table. It is not test covered because I do not
    have any experience on the testing the user output to the console.
    
### /Operations/SelectionByTwoFields/GrouperByTwoFields.cs
    It groups by any 2 fields of person and get the aggreate function as parameter. Returns the keyValue collection,
    where the key is the Tuple of 2 objects (which are chosen) and value is typed as the return value 
    of the aggregate functions. It is better to use it with the GrouperByTwoFieldsValidationDecorator decorator (the same
    namespace), but the validation is not required. 
    
### /Operations/SelectionByTwoFields/Generic/GrouperByTwoFields.cs
    It has the same behaviour as the previous, but gets the generic IEnumerable as the input collection.
    
### /Operations/Utils/Json/CustomPersonSerializer.cs
    It is the json serializer for the people object. It is not test covered because it is based on the trusted Newtonsoft.
    
### /Operations/Utils/Json/CustomPersonDeserializer.cs
    It is the json deserializer for the people object. It is not test covered because it is based on the trusted Newtonsoft.
    
### /Operations/Aggregation/Aggregator.cs
    It groups the generic enumerable by multiple fields. Returns the tree like object (not the binary tree).
    It is better to use it with the AggregatorValidationDecorator decorator (the same
    namespace), but the validation is not required.
    
### /Operations/Aggregation/Printing/PrinterToConsole.cs
    It prints the result of the previous object's work to the console.  It is not test covered because I do not
    have any experience on the testing the user output to the console. And also I did not have anought time to optimize it,
    so I just tried to manage it work. There are also a lot lof one time used objects, but as it is the used output, it sould 
    not be too big.
