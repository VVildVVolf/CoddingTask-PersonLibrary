using System.Collections.Generic;
using CommonEntities;

namespace Operations.Utils.AverageSetting{
    public interface IGrossSalaryAverageSetter {

        // because there is the List<Persion> in the task.
        // This method should return the updated Persons.
        IEnumerable<Person> SetAverage(List<Person> source, int age, string country, int value);
    }
}